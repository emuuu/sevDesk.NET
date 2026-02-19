using sevDesk.NET.Docs.Models;

namespace sevDesk.NET.Docs.Services;

public interface IRestApiDocService
{
    Task InitializeAsync();
    List<ClientDoc> GetAllClients();
    ClientDoc? GetClient(string interfaceName);
    MethodDoc? GetMethod(string interfaceName, string methodName);
    List<ParamDoc> GetMethodParams(string interfaceName, string methodName);
    List<EnumDoc> GetAllEnums();
    List<ModelTypeDoc> GetAllModelTypes();
}
