using System.Text.Json.Serialization;

namespace DataContainerApp.Models.Request;

public class PurposeRequest
{
    [JsonPropertyName("pid")]
    public string Pid { get; set; } = string.Empty;

    [JsonPropertyName("fid")]
    public string Fid { get; set; } = string.Empty;

    [JsonPropertyName("mandatory")]
    public bool Mandatory { get; set; }
}

