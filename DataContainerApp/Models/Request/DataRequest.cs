using System.Text.Json.Serialization;

namespace DataContainerApp.Models.Request;

public class DataRequest
{
    [JsonPropertyName("requestId")]
    public required string RequestId { get; set; }

    [JsonPropertyName("requestingApp")]
    public required RequestingApp RequestingApp { get; set; }

    [JsonPropertyName("requestedFields")]
    public required List<RequestedField> RequestedFields { get; set; } = new List<RequestedField>();
}

