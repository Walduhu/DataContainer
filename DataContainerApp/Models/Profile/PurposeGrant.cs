using System.Text.Json.Serialization;

namespace DataContainerApp.Models.Profile;

public class PurposeGrant
{
    [JsonPropertyName("pid")]
    public required string Pid { get; set; }

    [JsonPropertyName("fid")]
    public required string Fid { get; set; }

    [JsonPropertyName("granted")]
    public bool Granted { get; set; }
}
