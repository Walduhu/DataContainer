namespace DataContainerApp.Models.Response;

public class ProvidedField
{
    public string DataId { get; set; } = string.Empty;

    public object Value { get; set; } = string.Empty;

    public List<PurposeReference> Purposes { get; set; } = new();
}
