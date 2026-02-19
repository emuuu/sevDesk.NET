using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using sevDesk.NET;

namespace sevDesk.NET.Docs.Generator;

public class RestApiDocGenerator
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private Dictionary<string, string> _xmlDocs = new();
    private Dictionary<string, Dictionary<string, string>> _xmlParamDocs = new();
    private readonly HashSet<Type> _collectedEnums = [];
    private readonly HashSet<Type> _collectedModelTypes = [];
    private readonly NullabilityInfoContext _nullabilityCtx = new();

    public async Task GenerateAsync(string outputPath)
    {
        var assembly = typeof(ISevDeskClient).Assembly;
        var xmlPath = FindXmlDocPath(assembly);
        (_xmlDocs, _xmlParamDocs) = xmlPath is not null
            ? LoadXmlDocs(xmlPath)
            : (new Dictionary<string, string>(), new Dictionary<string, Dictionary<string, string>>());

        var clientInterfaces = assembly.GetExportedTypes()
            .Where(t => t.IsInterface
                        && t.Namespace == "sevDesk.NET.Clients"
                        && t.Name.StartsWith('I')
                        && t.Name.EndsWith("Client"))
            .OrderBy(t => t.Name)
            .ToList();

        var clients = new List<ClientDoc>();

        foreach (var iface in clientInterfaces)
        {
            var client = new ClientDoc
            {
                InterfaceName = iface.Name,
                Description = GetXmlSummary(iface),
                Methods = []
            };

            foreach (var method in iface.GetMethods().Where(m => !m.IsSpecialName))
            {
                var methodDoc = new MethodDoc
                {
                    Name = method.Name,
                    Description = GetXmlMemberSummary(iface, method.Name, "M"),
                    ReturnType = FormatReturnType(method.ReturnType),
                    Parameters = ExtractParameters(method)
                };
                client.Methods.Add(methodDoc);
            }

            clients.Add(client);
        }

        var enums = _collectedEnums
            .OrderBy(e => e.Name)
            .Select(BuildEnumDoc)
            .ToList();

        var modelTypes = _collectedModelTypes
            .OrderBy(t => t.Name)
            .Select(BuildModelTypeDoc)
            .ToList();

        var output = new RestApiDocsRoot
        {
            Clients = clients,
            Enums = enums,
            ModelTypes = modelTypes
        };

        var json = JsonSerializer.Serialize(output, JsonOptions);
        await File.WriteAllTextAsync(outputPath, json);
        Console.WriteLine($"  Generated {clients.Count} clients, {enums.Count} enums, {modelTypes.Count} model types -> {Path.GetFileName(outputPath)}");
    }

    private List<ParamDocEntry> ExtractParameters(MethodInfo method)
    {
        var result = new List<ParamDocEntry>();
        var methodParams = method.GetParameters();

        foreach (var param in methodParams)
        {
            var paramType = param.ParameterType;

            // Skip CancellationToken
            if (paramType == typeof(CancellationToken))
                continue;

            var isNullable = IsNullableParameter(param);
            var paramDescription = GetXmlParamSummary(method.DeclaringType!, method.Name, param.Name!);

            // Special handling for PaginationParameters - flatten
            if (paramType == typeof(PaginationParameters) || UnwrapNullable(paramType) == typeof(PaginationParameters))
            {
                if (string.IsNullOrEmpty(paramDescription))
                    paramDescription = "Pagination parameters (limit and offset).";

                var entry = new ParamDocEntry
                {
                    Name = param.Name!,
                    Type = "PaginationParameters" + (isNullable ? "?" : ""),
                    Required = !param.HasDefaultValue && !isNullable,
                    Description = paramDescription,
                    Properties = ExtractProperties(typeof(PaginationParameters))
                };
                result.Add(entry);
                continue;
            }

            // Special handling for Stream (binary upload)
            if (paramType == typeof(Stream))
            {
                result.Add(new ParamDocEntry
                {
                    Name = param.Name!,
                    Type = "Stream (binary)",
                    Required = true,
                    Description = paramDescription
                });
                continue;
            }

            // Special handling for byte[] (binary)
            if (paramType == typeof(byte[]))
            {
                result.Add(new ParamDocEntry
                {
                    Name = param.Name!,
                    Type = "byte[] (binary)",
                    Required = !param.HasDefaultValue && !isNullable,
                    Description = paramDescription
                });
                continue;
            }

            if (IsSevDeskModelType(paramType))
            {
                if (string.IsNullOrEmpty(paramDescription))
                    paramDescription = GetXmlSummary(paramType);

                var entry = new ParamDocEntry
                {
                    Name = param.Name!,
                    Type = FormatTypeName(paramType) + (isNullable ? "?" : ""),
                    Required = !param.HasDefaultValue && !isNullable,
                    Description = paramDescription,
                    Properties = ExtractProperties(paramType)
                };
                result.Add(entry);
            }
            else
            {
                var entry = new ParamDocEntry
                {
                    Name = param.Name!,
                    Type = FormatTypeName(paramType) + (isNullable && !paramType.Name.Contains("Nullable") ? "?" : ""),
                    Required = !param.HasDefaultValue && !isNullable,
                    Description = paramDescription,
                    Default = param.HasDefaultValue ? FormatDefaultValue(param.DefaultValue) : null
                };

                CollectEnumsFromType(paramType);
                result.Add(entry);
            }
        }

        // Collect return type for model types
        var returnType = UnwrapTaskType(method.ReturnType);
        if (returnType is not null)
        {
            // Unwrap SevDeskListResponse<T>
            var unwrappedReturn = UnwrapListResponse(returnType) ?? returnType;
            if (IsSevDeskModelType(unwrappedReturn))
            {
                CollectModelType(unwrappedReturn);
            }

            CollectEnumsFromType(returnType);
        }

        return result;
    }

    private List<ParamDocEntry> ExtractProperties(Type type, int depth = 0)
    {
        if (depth > 3) return [];

        object? instance = null;
        try
        {
            instance = type.IsValueType ? Activator.CreateInstance(type)
                : type.GetConstructor(Type.EmptyTypes) is not null ? Activator.CreateInstance(type)
                : null;
        }
        catch { /* proceed without defaults */ }

        return ExtractPropertiesCore(type, instance, depth);
    }

    private List<ParamDocEntry> ExtractPropertiesCore(Type type, object? instance, int depth)
    {
        var result = new List<ParamDocEntry>();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetMethod?.IsPublic == true);

        foreach (var prop in properties)
        {
            var propType = prop.PropertyType;
            var isNullable = IsNullableProperty(prop);
            var defaultValue = GetPropertyDefault(instance, prop);

            CollectEnumsFromType(propType);

            var entry = new ParamDocEntry
            {
                Name = prop.Name,
                Type = FormatPropertyTypeName(prop),
                Required = !isNullable && defaultValue is null or "null",
                Description = GetXmlMemberSummary(type, prop.Name, "P"),
                Default = defaultValue
            };

            // Recurse into nested sevDesk model types
            var unwrapped = UnwrapNullable(propType);
            if (IsSevDeskModelType(unwrapped) && !unwrapped.IsEnum)
            {
                entry.Properties = ExtractProperties(unwrapped, depth + 1);
            }

            result.Add(entry);
        }

        return result;
    }

    private void CollectModelType(Type type)
    {
        if (!_collectedModelTypes.Add(type)) return;

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetMethod?.IsPublic == true);

        foreach (var prop in properties)
        {
            CollectEnumsFromType(prop.PropertyType);

            var unwrapped = UnwrapCollectionType(UnwrapNullable(prop.PropertyType));
            if (IsSevDeskModelType(unwrapped) && !unwrapped.IsEnum)
            {
                CollectModelType(unwrapped);
            }
        }
    }

    private ModelTypeDoc BuildModelTypeDoc(Type type)
    {
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetMethod?.IsPublic == true)
            .Select(p =>
            {
                var unwrapped = UnwrapCollectionType(UnwrapNullable(p.PropertyType));
                string? nestedType = IsSevDeskModelType(unwrapped) && !unwrapped.IsEnum
                    ? unwrapped.Name
                    : null;

                return new ModelPropertyDoc
                {
                    Name = p.Name,
                    Type = FormatPropertyTypeName(p),
                    Description = GetXmlMemberSummary(type, p.Name, "P"),
                    NestedType = nestedType
                };
            })
            .ToList();

        return new ModelTypeDoc
        {
            Name = type.Name,
            Description = GetXmlSummary(type),
            Properties = props
        };
    }

    private EnumDoc BuildEnumDoc(Type enumType)
    {
        var values = new List<EnumValueDoc>();
        foreach (var name in Enum.GetNames(enumType))
        {
            var field = enumType.GetField(name)!;
            var enumMember = field.GetCustomAttribute<EnumMemberAttribute>();
            var rawValue = Convert.ToInt64(Enum.Parse(enumType, name));
            values.Add(new EnumValueDoc
            {
                Name = name,
                SerializedValue = enumMember?.Value ?? rawValue.ToString()
            });
        }

        return new EnumDoc
        {
            Name = enumType.Name,
            Description = GetXmlSummary(enumType),
            IsFlags = enumType.GetCustomAttribute<FlagsAttribute>() is not null,
            Values = values
        };
    }

    // ── XML doc helpers ──

    private string GetXmlSummary(Type type)
    {
        var key = $"T:{type.FullName}";
        return _xmlDocs.GetValueOrDefault(key, "");
    }

    private string GetXmlMemberSummary(Type type, string memberName, string prefix)
    {
        var key = $"{prefix}:{type.FullName}.{memberName}";
        if (_xmlDocs.TryGetValue(key, out var doc))
            return doc;

        if (prefix == "M")
        {
            foreach (var kvp in _xmlDocs)
            {
                if (kvp.Key.StartsWith($"M:{type.FullName}.{memberName}"))
                    return kvp.Value;
            }
        }

        return "";
    }

    private string GetXmlParamSummary(Type type, string methodName, string paramName)
    {
        var key = $"M:{type.FullName}.{methodName}";
        if (_xmlParamDocs.TryGetValue(key, out var paramDocs) && paramDocs.TryGetValue(paramName, out var desc))
            return desc;

        foreach (var kvp in _xmlParamDocs)
        {
            if (kvp.Key.StartsWith($"M:{type.FullName}.{methodName}") && kvp.Value.TryGetValue(paramName, out desc))
                return desc;
        }

        return "";
    }

    // ── Type formatting ──

    private static string FormatTypeName(Type type)
    {
        if (type == typeof(void)) return "void";
        if (type == typeof(string)) return "string";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(int)) return "int";
        if (type == typeof(long)) return "long";
        if (type == typeof(double)) return "double";
        if (type == typeof(float)) return "float";
        if (type == typeof(decimal)) return "decimal";
        if (type == typeof(object)) return "object";
        if (type == typeof(DateTime)) return "DateTime";
        if (type == typeof(DateOnly)) return "DateOnly";
        if (type == typeof(byte[])) return "byte[]";
        if (type == typeof(Stream)) return "Stream";

        var nullableUnderlying = Nullable.GetUnderlyingType(type);
        if (nullableUnderlying is not null)
            return $"{FormatTypeName(nullableUnderlying)}?";

        if (type.IsGenericType)
        {
            var name = type.Name[..type.Name.IndexOf('`')];
            var args = string.Join(", ", type.GetGenericArguments().Select(FormatTypeName));
            return $"{name}<{args}>";
        }

        if (type == typeof(Task)) return "Task";

        return type.Name;
    }

    private string FormatPropertyTypeName(PropertyInfo prop)
    {
        var typeName = FormatTypeName(prop.PropertyType);
        if (!typeName.EndsWith('?') && IsNullableProperty(prop))
            typeName += "?";
        return typeName;
    }

    private static string FormatReturnType(Type type)
    {
        var inner = UnwrapTaskType(type);
        if (inner is null) return FormatTypeName(type);

        // Unwrap SevDeskListResponse<T> for display
        if (inner.IsGenericType && inner.GetGenericTypeDefinition() == typeof(SevDeskListResponse<>))
        {
            var itemType = inner.GetGenericArguments()[0];
            return $"SevDeskListResponse<{FormatTypeName(itemType)}>";
        }

        return FormatTypeName(inner);
    }

    private static string? FormatDefaultValue(object? value)
    {
        return value switch
        {
            null => "null",
            string s => $"\"{s}\"",
            bool b => b ? "true" : "false",
            int i => i.ToString(),
            double d => d.ToString("G"),
            Enum e => e.ToString(),
            _ => value.ToString()
        };
    }

    private string? GetPropertyDefault(object? instance, PropertyInfo prop)
    {
        if (instance is null) return null;
        try
        {
            var value = prop.GetValue(instance);
            var propType = UnwrapNullable(prop.PropertyType);

            if (value is null)
                return IsNullableProperty(prop) ? "null" : null;

            return value switch
            {
                string s => $"\"{s}\"",
                bool b => b ? "true" : "false",
                int i => i.ToString(),
                double d => d.ToString("G"),
                Enum e when propType.GetCustomAttribute<FlagsAttribute>() is not null
                    => Convert.ToInt64(e) == 0 ? "None" : e.ToString(),
                Enum e => e.ToString(),
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }

    // ── Nullability helpers ──

    private bool IsNullableProperty(PropertyInfo prop)
    {
        if (Nullable.GetUnderlyingType(prop.PropertyType) is not null)
            return true;

        try
        {
            var info = _nullabilityCtx.Create(prop);
            return info.ReadState == NullabilityState.Nullable;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsNullableParameter(ParameterInfo param)
    {
        if (Nullable.GetUnderlyingType(param.ParameterType) is not null)
            return true;

        if (param.HasDefaultValue && param.DefaultValue is null)
            return true;

        return false;
    }

    // ── Type analysis helpers ──

    private static bool IsSevDeskModelType(Type type)
    {
        var unwrapped = UnwrapNullable(type);
        return unwrapped.Assembly == typeof(ISevDeskClient).Assembly
               && (unwrapped.Namespace?.StartsWith("sevDesk.NET") == true)
               && !unwrapped.IsInterface
               && unwrapped != typeof(SevDeskOptions)
               && unwrapped != typeof(SevDeskAuthHandler)
               && unwrapped != typeof(SevDeskClient);
    }

    private static Type? UnwrapTaskType(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            return type.GetGenericArguments()[0];
        return null;
    }

    private static Type? UnwrapListResponse(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SevDeskListResponse<>))
            return type.GetGenericArguments()[0];
        return null;
    }

    private static Type UnwrapNullable(Type type)
    {
        return Nullable.GetUnderlyingType(type) ?? type;
    }

    private static Type UnwrapCollectionType(Type type)
    {
        if (type.IsGenericType)
        {
            var genDef = type.GetGenericTypeDefinition();
            if (genDef == typeof(List<>) || genDef == typeof(IList<>) ||
                genDef == typeof(IEnumerable<>) || genDef == typeof(ICollection<>) ||
                genDef == typeof(IReadOnlyList<>))
            {
                return type.GetGenericArguments()[0];
            }
        }
        return type;
    }

    private void CollectEnumsFromType(Type type)
    {
        var unwrapped = UnwrapNullable(type);
        if (unwrapped.IsEnum && unwrapped.Assembly == typeof(ISevDeskClient).Assembly)
        {
            _collectedEnums.Add(unwrapped);
        }

        if (type.IsGenericType)
        {
            foreach (var arg in type.GetGenericArguments())
                CollectEnumsFromType(arg);
        }
    }

    // ── XML doc loading ──

    private static string? FindXmlDocPath(Assembly assembly)
    {
        var dllPath = assembly.Location;
        if (string.IsNullOrEmpty(dllPath)) return null;
        var xmlPath = Path.ChangeExtension(dllPath, ".xml");
        return File.Exists(xmlPath) ? xmlPath : null;
    }

    private static (Dictionary<string, string> summaries, Dictionary<string, Dictionary<string, string>> paramDocs) LoadXmlDocs(string xmlPath)
    {
        var summaries = new Dictionary<string, string>();
        var paramDocs = new Dictionary<string, Dictionary<string, string>>();
        try
        {
            var doc = XDocument.Load(xmlPath);
            foreach (var member in doc.Descendants("member"))
            {
                var name = member.Attribute("name")?.Value;
                if (name is null) continue;

                var summary = member.Element("summary")?.Value.Trim();
                if (!string.IsNullOrEmpty(summary))
                {
                    summary = string.Join(" ", summary.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries));
                    summaries[name] = summary;
                }

                foreach (var paramEl in member.Elements("param"))
                {
                    var paramName = paramEl.Attribute("name")?.Value;
                    var paramDesc = paramEl.Value.Trim();
                    if (paramName is not null && !string.IsNullOrEmpty(paramDesc))
                    {
                        paramDesc = string.Join(" ", paramDesc.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries));
                        if (!paramDocs.ContainsKey(name))
                            paramDocs[name] = new Dictionary<string, string>();
                        paramDocs[name][paramName] = paramDesc;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Warning: Could not parse XML docs: {ex.Message}");
        }
        return (summaries, paramDocs);
    }
}

// ── Output models ──

public class RestApiDocsRoot
{
    public List<ClientDoc> Clients { get; set; } = [];
    public List<EnumDoc> Enums { get; set; } = [];
    public List<ModelTypeDoc> ModelTypes { get; set; } = [];
}

public class ClientDoc
{
    public string InterfaceName { get; set; } = "";
    public string Description { get; set; } = "";
    public List<MethodDoc> Methods { get; set; } = [];
}

public class MethodDoc
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ReturnType { get; set; } = "";
    public List<ParamDocEntry> Parameters { get; set; } = [];
}

public class ParamDocEntry
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public bool Required { get; set; }
    public string Description { get; set; } = "";
    public string? Default { get; set; }
    public List<ParamDocEntry>? Properties { get; set; }
}

public class EnumDoc
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsFlags { get; set; }
    public List<EnumValueDoc> Values { get; set; } = [];
}

public class EnumValueDoc
{
    public string Name { get; set; } = "";
    public string SerializedValue { get; set; } = "";
}

public class ModelTypeDoc
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<ModelPropertyDoc> Properties { get; set; } = [];
}

public class ModelPropertyDoc
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string Description { get; set; } = "";
    public string? NestedType { get; set; }
}
