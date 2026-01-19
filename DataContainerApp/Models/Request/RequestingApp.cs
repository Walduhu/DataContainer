using System.Text.Json.Serialization;

namespace DataContainerApp.Models.Request;

public class RequestingApp
{
    [JsonPropertyName("appId")]
    public required string AppId { get; set; }

    [JsonPropertyName("appName")]
    public required string AppName { get; set; }
}
