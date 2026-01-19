using System.Text.Json.Serialization;

namespace DataContainerApp.Models.Response
{
    public class PurposeReference
    {
        public string Pid { get; set; } = string.Empty;

        public string Fid { get; set; } = string.Empty;

        [JsonIgnore]
        public bool isMandatory { get; set; }
    }
}
