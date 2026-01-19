using DataContainerApp.Models.Request;
using DataContainerApp.Models.Response;
using DataContainerApp.Models.Profile;
using DataContainerApp.Services;
using DataContainerApp.Storage;

Console.WriteLine("===================");
Console.WriteLine("   DataContainer");
Console.WriteLine("===================");

ProcessRequests();

Console.WriteLine("\nDataContainer beendet.");
Console.ReadKey();

void ProcessRequests()
{
    var requestFiles = FilePaths.GetRequestFiles();

    if (requestFiles.Length == 0)
    {
        Console.WriteLine("\nKeine Anfragen gefunden.");
        return;
    }

    var requests = requestFiles
        .Select(path => (Path: path, Request: JsonService.Load<DataRequest>(path)))
        .ToList();

    while (requests.Count > 0)
    {
        if (requests.Count == 1)
        {
            Console.WriteLine($"\nDie App {requests[0].Request.RequestingApp.AppName} verlangt Ihre persönlichen Daten.\n");
            Console.ReadKey();
        }
        else
            Console.WriteLine($"\n{requests.Count} Apps verlangen Ihre persönlichen Daten.\n");

        int choice;
        (string Path, DataRequest Request) selected;

        if (requests.Count == 1)
        {
            choice = 1;
            selected = requests[0];
        }
        else
        {
            for (int i = 0; i < requests.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {requests[i].Request.RequestingApp.AppName}");
            }

            Console.Write("\nWelche Anfrage möchten Sie bearbeiten?\nAuswahl: ");

            if (!int.TryParse(Console.ReadLine(), out choice) ||
                choice < 1 || choice > requests.Count)
            {
                Console.WriteLine("Ungültige Auswahl.");
                continue;
            }

            selected = requests[choice - 1];
            Console.WriteLine($"Bearbeite Anfrage von {selected.Request.RequestingApp.AppName}.\n");
        }

        var requestPath = selected.Path;
        var request = selected.Request;

        Console.WriteLine($"\nWie wollen Sie mit der App {request.RequestingApp.AppName} verfahren?\nIch möchte...\n");

        string[] options = new[]
        {
        " ...alle angeforderten Daten übermitteln",
        " ...alle zwingend erforderlichen Daten übermitteln",
        " ...nur rechtlich zwingende Daten übermitteln",
        " ...nur vertraglich notwendige Daten übermitteln",
        " ...gesetzlich verpflichtende und Anmeldedaten übermitteln, aber keine Werbung oder Komfortfunktionen",
        " ...alle angeforderten Daten übermitteln, aber anonym bleiben",
        " ...alle angeforderten Daten übermitteln und Werbung erhalten, aber nicht personalisiert",
        " ...alle angeforderten Daten übermitteln und Werbung erhalten, aber anonym bleiben",
        " ...nur Daten übermitteln, die mir das Leben leichter machen",
        "...alle angeforderten Daten übermitteln, aber keine Zahlungsdaten",
        "...nur vertraglich notwendige Zahlungsdaten übertragen",
        "...auf Reisen Daten sparen, aber nicht auf Komfort verzichten",
        "...nur Werbung erhalten",
        "...keine Daten an die App übermitteln, da ich ihr nicht traue"
    };

        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1:D1} {options[i]}");
        }

        Console.Write("Auswahl: ");

        var choiceProf = Console.ReadLine();
        bool noDataSelected = choiceProf == "14";

        ConsentProfile? consentProfile = null;

        if (!noDataSelected)
        {
            consentProfile = JsonService.Load<ConsentProfile>(
                choiceProf switch
                {
                    "1" => "profile_green.json",
                    "2" => "profile_orange.json",
                    "3" => "profile_legalOnly.json",
                    "4" => "profile_contractOnly.json",
                    "5" => "profile_highSecurity.json",
                    "6" => "profile_noIdentity.json",
                    "7" => "profile_marketingLight.json",
                    "8" => "profile_marketingAnon.json",
                    "9" => "profile_convenience.json",
                    "10" => "profile_noPayment.json",
                    "11" => "profile_paymentOnly.json",
                    "12" => "profile_travelMode.json",
                    "13" => "profile_black.json",
                    _ => "profile_red.json"
                }
            );
        }

        if (noDataSelected)
        {
            if (!HandleNoDataTransfer(request, choice - 1, requests))
                continue;

            continue;
        }

        Console.WriteLine("\nWelches Benutzerprofil möchten Sie verwenden?");
        Console.WriteLine("1 - Standardprofil (echte Daten)");
        Console.WriteLine("2 - Alternatives Profil (z. B. für Werbung)");
        // Console.WriteLine("3 - Neues Profil anlegen");
        Console.Write("Auswahl: ");

        var choiceData = Console.ReadLine();

        string userDataPath = choiceData switch
        {
            "2" => "UserDataSpam.json",
            // "3" => $"userData_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            _ => "UserData.json"
        };

        /*
        UserDataStore.Load(userDataPath);

        Console.WriteLine("\nMöchten Sie Ihre persönlichen Daten eingeben oder ändern? (j/n)");

        var choiceEdit = Console.ReadLine();

        if (choiceEdit == "j")
        {
            EnterOrUpdateUserData();
            UserDataStore.Save();
            Console.WriteLine("\nDaten wurden gespeichert.\n");
        }
        */

        UserDataStore.Load(userDataPath);
        var userData = UserDataStore.GetUserData();

        var responseBuilder = new ResponseBuilder();
        var response = consentProfile != null
            ? responseBuilder.Build(request, consentProfile, userData)
            : null;

        if (response == null || response.ProvidedFields.Count == 0)
        {
            if (!HandleNoDataTransfer(request, choice - 1, requests))
                continue;

            continue;
        }

        var responsePath = FilePaths.GetResponsePath(requestPath);
        JsonService.Save(responsePath, response);

        Console.WriteLine("\n" + BuildExplanation(response));
        Console.WriteLine($"\nBearbeitung der Anfrage von {request.RequestingApp.AppName} abgeschlossen.\n" +
            $"---------------------------------------------------------");
        Console.ReadKey();
        requests.RemoveAt(choice - 1);

    }

    Console.WriteLine("\nAlle Anfragen wurden bearbeitet.");
}


// Hilfsmethoden

bool HandleNoDataTransfer(DataRequest request, int index, List<(string Path, DataRequest Request)> requests)
{
    var mandatoryPurposes = GetMandatoryPurposeNames(request);

    if (!ConfirmMandatoryRejection(
            request.RequestingApp.AppName,
            mandatoryPurposes))
    {
        Console.WriteLine("\nBitte wählen Sie ein anderes Profil.");
        return false;
    }

    Console.WriteLine(
        $"\nEs wurden keine Daten an {request.RequestingApp.AppName} übertragen.\n" +
        $"---------------------------------------------------------");
    Console.ReadKey();

    requests.RemoveAt(index);
    return true;
}


string BuildExplanation(DataResponse response)
{
    var pids = response.ProvidedFields
        .SelectMany(d => d.Purposes ?? new List<PurposeReference>())
        .Select(p => p.Pid)
        .Distinct()
        .ToList();

    var fids = response.ProvidedFields
        .SelectMany(d => d.Purposes ?? new List<PurposeReference>())
        .Select(p => p.Fid)
        .Distinct()
        .ToList();

    var purposes = pids
        .Where(DataPurposeDictionary.PurposeNames.ContainsKey)
        .Select(pid => DataPurposeDictionary.PurposeNames[pid])
        .ToList();

    var functions = fids
        .Where(DataPurposeDictionary.FunctionNames.ContainsKey)
        .Select(fid => DataPurposeDictionary.FunctionNames[fid])
        .ToList();

    return
        $"Es wurden personenbezogene Daten zur {JoinNatural(purposes)} " +
        $"übertragen, um {JoinNatural(functions)} zu ermöglichen.";
}

List<string> GetMandatoryPurposeNames(DataRequest request)
{
    return request.RequestedFields
        .SelectMany(f => f.Purposes != null
            ? f.Purposes.Select(p => new PurposeReference
            {
                Pid = p.Pid,
                Fid = p.Fid,
                isMandatory = p.Mandatory
            })
            : Enumerable.Empty<PurposeReference>())
        .Where(p => p.isMandatory)
        .Select(p => p.Pid)
        .Distinct()
        .Where(DataPurposeDictionary.PurposeNames.ContainsKey)
        .Select(pid => DataPurposeDictionary.PurposeNames[pid])
        .ToList();
}

bool ConfirmMandatoryRejection(string appName, List<string> mandatoryPurposes)
{
    if (mandatoryPurposes.Count == 0)
        return true;

    Console.WriteLine(
        $"\nUm die App {appName} nutzen zu können, müssen Daten zu folgendem Zweck weitergegeben werden:");
    Console.WriteLine($"- {JoinNatural(mandatoryPurposes)}");

    Console.Write("\nMöchten Sie wirklich ohne Datenweitergabe fortfahren? (j/n)\nAuswahl: ");

    return Console.ReadLine()?.Trim().ToLower() == "j";
}

string JoinNatural(List<string> items)
{
    if (items.Count == 0) return "";
    if (items.Count == 1) return items[0];
    if (items.Count == 2) return $"{items[0]} und {items[1]}";

    return string.Join(", ", items.Take(items.Count - 1)) +
           " und " + items.Last();
}

/*
void EnterOrUpdateUserData()
{
    Console.WriteLine("\n--- Persönliche Daten ---");

    foreach (var entry in DataDictionary.Names)
    {
        var dataId = entry.Key;
        var displayName = entry.Value;

        Console.WriteLine($"\n{displayName}:");

        if (UserDataStore.TryGetValue(dataId, out var existingValue))
        {
            Console.WriteLine($"Aktueller Wert: {existingValue}");
        }

        Console.Write("Ändern / neu eingeben? (j/n): ");
        if (Console.ReadLine()?.ToLower() != "j")
            continue;

        Console.Write($"Bitte {displayName} eingeben: ");
        var input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            UserDataStore.Set(dataId, input);
        }
    }
}
*/