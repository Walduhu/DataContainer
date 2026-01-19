namespace DataContainerApp.Models.Profile;

public enum ProfileType
{
    Green,          // alles erlauben
    Orange,         // alle Pflichtdaten
    Red,            // nichts erlauben
    Black,          // nur Nicht-Pflichtdaten
    LegalOnly,      // nur rechtlich zwingend
    ContractOnly,   // nur vertraglich zwingend
    HighSecurity,   // Sicherheit ja, Komfort nein
    NoIdentity,     // keine Identitätsverknüpfung
    MarketingLight, // Werbung, aber keine Profilbildung
    MarketingAnon,  // Werbung, aber ohne Identitätsverknüpfung
    Convenience,    // nur vertraglich erforderlich und Komfort
    NoPayment,      // keine Zahlungsdaten
    PaymentOnly,    // Zahlungsdaten nur für diese Sitzung
    TravelMode      // nur Sicherheit und Komfort, datensparsam
}