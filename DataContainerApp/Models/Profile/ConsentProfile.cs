using System.Text.Json.Serialization;

namespace DataContainerApp.Models.Profile;

public class ConsentProfile
{
    [JsonPropertyName("profileType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ProfileType ProfileType { get; set; }

    [JsonPropertyName("grants")]
    public required List<PurposeGrant> Grants { get; set; }

}

