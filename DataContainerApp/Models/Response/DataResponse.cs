namespace DataContainerApp.Models.Response;

public class DataResponse
{
    public string ResponseId { get; set; } = string.Empty;

    public string RequestId { get; set; } = string.Empty;

    public List<ProvidedField> ProvidedFields { get; set; } = new();
}
