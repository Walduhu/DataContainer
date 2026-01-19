using System.Text.Json.Serialization;

namespace DataContainerApp.Models.Request;

public class RequestedField
{
    [JsonPropertyName("dataId")]
    public string DataId { get; set; } = string.Empty;

    [JsonPropertyName("purposes")]
    public List<PurposeRequest> Purposes { get; set; } = new();
}

