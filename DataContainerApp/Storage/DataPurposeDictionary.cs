namespace DataContainerApp.Storage;

public static class DataPurposeDictionary
{
    // Datenfelder
    public static readonly Dictionary<string, string> Names = new()
    {
        ["001"] = "Echtname",
        ["002"] = "Geburtsdatum",
        ["003"] = "E-Mail-Adresse",
        ["004"] = "Adresse",
        ["005"] = "Mobilnummer",
        ["006"] = "Festnetznummer",
        ["007"] = "Krankenversicherungsnummer",
        ["008"] = "Steuer-ID",
        ["009"] = "Sozialversicherungsnummer",
        ["010"] = "IBAN",
        ["011"] = "Biometrische Daten",
        ["012"] = "Hell-/Dunkelmodus",
        ["013"] = "Cookie-Präferenzen",
        ["014"] = "Sprache",
        ["015"] = "Schriftgröße",
        ["016"] = "Kreditkarte"
    };

    // Primärzwecke
    public static readonly Dictionary<string, string> PurposeNames = new()
    {
        ["001"] = "Gesetzliche Verpflichtung",
        ["002"] = "Vertragserfüllung",
        ["003"] = "Sicherheit",
        ["004"] = "Komfort / Personalisierung",
        ["005"] = "Werbung / Marketing"
    };

    // Funktionszwecke
    public static readonly Dictionary<string, string> FunctionNames = new()
    {
        ["001"] = "Identitätszuordnung",
        ["002"] = "Kommunikationsfähigkeit",
        ["003"] = "Zugriffskontrolle",
        ["004"] = "Zahlungsabwicklung",
        ["005"] = "Nutzungseinstellungen"
    };
}

