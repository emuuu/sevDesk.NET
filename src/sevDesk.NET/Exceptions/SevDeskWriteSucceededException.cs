namespace sevDesk.NET.Exceptions;

/// <summary>
/// Exception thrown when a write to sevDesk <b>succeeded</b> but a later step of the same call
/// failed — reading the response body, parsing the identifier out of it, or re-reading the created
/// object.
/// </summary>
/// <remarks>
/// <para>
/// The <c>Factory</c> endpoints are wrapped by convenience methods that post, parse the identifier
/// from the answer and then re-read the full object. Only the post is a write. Once the API has
/// answered it with a success status, the document exists in sevDesk and <b>a retry would create a
/// second one</b>. Everything that fails from that point on is reported through this type, so a
/// caller can tell the two outcomes apart:
/// </para>
/// <list type="bullet">
/// <item><description>
/// Any other <see cref="SevDeskException"/> (or a transport error such as
/// <see cref="HttpRequestException"/>) — the write did not reach sevDesk. Retrying is safe.
/// </description></item>
/// <item><description>
/// <see cref="SevDeskWriteSucceededException"/> — the write reached sevDesk and the document exists.
/// Do not retry. Use <see cref="ObjectId"/> to continue.
/// </description></item>
/// </list>
/// <para>
/// <see cref="ObjectId"/> is <see langword="null"/> when the post succeeded but its answer could not
/// be read or parsed. That is the worst case: the document exists and its identifier is unknown.
/// It still must not be written again — look it up first, for example through the matching
/// <c>ListAsync</c> filtered by the document number.
/// </para>
/// <para>
/// This type is also thrown when the follow-up read was cancelled through the caller's
/// <see cref="CancellationToken"/>. The <see cref="Exception.InnerException"/> is then an
/// <see cref="OperationCanceledException"/>. Cancellation before or during the post itself
/// propagates unwrapped, because nothing was written in that case.
/// </para>
/// </remarks>
public class SevDeskWriteSucceededException : SevDeskApiException
{
    /// <summary>
    /// Gets the sevDesk object type that was written, e.g. <c>"Invoice"</c> or <c>"CreditNote"</c>.
    /// </summary>
    public string ObjectName { get; }

    /// <summary>
    /// Gets the identifier of the written object, or <see langword="null"/> when the write succeeded
    /// but its identifier could not be determined.
    /// </summary>
    public int? ObjectId { get; }

    /// <summary>
    /// Gets a value indicating whether <see cref="ObjectId"/> carries an identifier. When this is
    /// <see langword="false"/> the object exists in sevDesk but has to be looked up before any
    /// further write.
    /// </summary>
    public bool IsObjectIdKnown => ObjectId.HasValue;

    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskWriteSucceededException"/>.
    /// </summary>
    /// <param name="objectName">The sevDesk object type that was written.</param>
    /// <param name="objectId">
    /// The identifier of the written object, or <see langword="null"/> when it could not be determined.
    /// </param>
    /// <param name="rawResponse">
    /// The raw body of the write response, or <see langword="null"/> when it could not be read.
    /// Inherited as <see cref="SevDeskException.RawResponse"/>; it holds the answer to the write,
    /// not the answer to the failed follow-up step.
    /// </param>
    /// <param name="innerException">The failure that occurred after the write had succeeded.</param>
    public SevDeskWriteSucceededException(
        string objectName,
        int? objectId,
        string? rawResponse,
        Exception innerException)
        : base(
            BuildMessage(objectName, objectId),
            (innerException as SevDeskException)?.StatusCode,
            rawResponse,
            innerException)
    {
        ObjectName = objectName;
        ObjectId = objectId;
    }

    private static string BuildMessage(string objectName, int? objectId) =>
        objectId is int id
            ? $"The {objectName} was written to sevDesk with id {id}, but a follow-up step of the same call failed. Do not send the write again; read the object by its id instead."
            : $"The {objectName} was written to sevDesk, but its id could not be determined. Do not send the write again; look the object up first.";
}
