# DataContainer

<p>
  Aufgabe war es, eine Anwendung zu entwickeln, die weniger technikaffinen Menschen die Freigabe ihrer persönlichen Daten an verschiedene Apps erleichtert.
  Dies können z.B. Apps von Online-Shops, Supermärkten, Krankenkassen oder Bezahldiensten sein.
</p>
  <p>
  Dieses Repo enthält nicht die fertige App und hat kein GUI, sondern soll nur die Arbeitsweise eines Datencontainers aufzeigen, der Datenanfragen anderer Apps bearbeitet. 
</p>

# Konzeption - 1. Schritt
<p>
  Zunächst galt es zu ermitteln, welche allgemeinen Daten eine natürliche oder rechtliche Person haben kann und die Semantik hinter den Bezeichnungen der Daten zu klären.
  Außerdem sollten die Gründe für die Datenweitergabe aus User-Sicht eingegrenzt werden.
  Die folgende Tabelle wurde dabei ausgearbeitet: 
</p>
<img width="992" height="413" alt="image" src="https://github.com/user-attachments/assets/389287e2-4992-4972-a65c-0569549582d6" />

  <p>Den Daten wurde in der hier gezeigten Reihenfolge je eine ID zugeordnet (001 bis 016).</p>

# Konzeption - 2. Schritt
<p>
  Als nächstes war es wichtig, Zwecke und Verwendungsarten dieser Daten aus Sicht der anfragenden Apps zu klären.
  Da Zwecke vielfältig und unübersichtlich sind und sich teilweise überschneiden, wurde entschieden, den Daten je zwei Arten von Zwecken zuzuordnen: Primärzwecke und Funktionszwecke.
  Primärzwecke sagen, warum die Verarbeitung eines Datums überhaupt existiert, und Funktionszwecke, wofür ein Datum verwendet wird. 
  Den Zwecken wurden IDs zugeordnet (PID = Primärzweck-ID, FID = Funktionszweck-ID).
  Es wurde folgende Tabelle ausgearbeitet:
</p>

<img width="991" height="317" alt="image" src="https://github.com/user-attachments/assets/a542049c-4f32-4c8d-ab18-a2a260093596" />


# Konzeption - 3. Schritt
<p>
  In einem dritten Schritt wurden alle PIDs und FIDs miteinander permutiert, um alle so möglichen Zweckkombinationen auszuloten und ferner festzustellen, welche Kombinationen realistisch sind und welche nicht.
  Dabei entstand folgende Tabelle:
  </p>
  <img width="556" height="637" alt="image" src="https://github.com/user-attachments/assets/5e6e806a-bb85-459d-8294-9265f91c7e40" />
<p>
  Rot sind dabei unrealistische Szenarien und grün typische Szenarien.
  Typische gegenseitige Ausschlusskriterien sind: Gesetz und Nutzungseinstellungen, Komfort und Zahlungsabwicklung, Werbung und Zugriffskontrolle sowie Werbung und Zahlungsabwicklung.
</p>

# Implementierung
<p>
  Hierzu wurde folgender Plan zur Funktionsweise der App ausgearbeitet:
  Der Datenaustausch erfolgt per json-Dateien. Dabei erhält die Datencontainer-App eine Anfrage (request.json). Diese kann z.B. so aussehen:
</p>
```json
  {
    "requestId": "req-001",
    "requestingApp": {
        "appId": "app-123",
        "appName": "Otto"
    },
    "requestedFields": [
        {
            "dataId": "001",
            "purposes": [
                {
                    "pid": "002",
                    "fid": "001",
                    "mandatory": true
                },
                {
                    "pid": "005",
                    "fid": "002",
                    "mandatory": false
                }
            ]
        },
        {
            "dataId": "002",
            "purposes": [
                {
                    "pid": "002",
                    "fid": "003",
                    "mandatory": true
                },
                {
                    "pid": "005",
                    "fid": "002",
                    "mandatory": false
                }
            ]
        },
        {
            "dataId": "003",
            "purposes": [
                {
                    "pid": "002",
                    "fid": "002",
                    "mandatory": true
                },
                {
                    "pid": "002",
                    "fid": "003",
                    "mandatory": true
                },
                {
                    "pid": "005",
                    "fid": "002",
                    "mandatory": false
                }
            ]
        },
        {
            "dataId": "004",
            "purposes": [
                {
                    "pid": "002",
                    "fid": "002",
                    "mandatory": true
                },
                {
                    "pid": "002",
                    "fid": "003",
                    "mandatory": true
                },
                {
                    "pid": "005",
                    "fid": "002",
                    "mandatory": false
                }
            ]
        },
        {
            "dataId": "005",
            "purposes": [
                {
                    "pid": "002",
                    "fid": "002",
                    "mandatory": true
                },
                {
                    "pid": "003",
                    "fid": "003",
                    "mandatory": false
                },
                {
                    "pid": "005",
                    "fid": "002",
                    "mandatory": false
                }
            ]
        },
        {
            "dataId": "010",
            "purposes": [
                {
                    "pid": "002",
                    "fid": "004",
                    "mandatory": true
                }
            ]
        },
        {
            "dataId": "011",
            "purposes": [
                {
                    "pid": "003",
                    "fid": "003",
                    "mandatory": false
                }
            ]
        }
    ]
}
```
<p>
  Im Folgenden werden mithilfe von einem vordefinierten Profil (profile.json) die angefragten Daten gefiltert.
  Ein Profil genehmigt dann z.B. nur bestimmte Arten von PID-FID-Kombinationen und könnte so aussehen:
</p>
```json
{
  "profileType": "Orange",
  "grants": [
    {
      "pid": "001",
      "fid": "*",
      "granted": true
    },
    {
      "pid": "002",
      "fid": "*",
      "granted": true
    },
    {
      "pid": "005",
      "fid": "*",
      "granted": false
    }
  ]
}
```
<p>
In diesem Profil werden alle PID-FID-Kombinationen einer Anfrage genehmigt, die gesetzliche und vertragliche Primärzwecke enthalten. Primärzwecke für Werbung werden dagegen abgelehnt. Funktionszwecke werden per Wildcards alle genehmigt.
Die Anzahl an vorhandenen Profilen ist unbegrenzt und je nach Anfrage kann ein anderes Profil vom User ausgewählt werden. So wird jedes Mal entschieden, wie auf eine Datenanfrage reagiert wird.
<br>
Mithilfe einer request.json und einer profile.json generiert die Datencontainer-App eine Antwort an die anfragende App (response.json). Unter Einbeziehung von Dummy-Daten und der beiden Beispiele oben sähe diese dann z.B. so aus:
</p>
```json
{
  "responseId": "resp-97be00fb-b494-4e9a-a927-dea51017f351",
  "requestId": "req-001",
  "providedFields": [
    {
      "dataId": "001",
      "value": "Max Mustermann",
      "purposes": [
        {
          "pid": "002",
          "fid": "001"
        }
      ]
    },
    {
      "dataId": "002",
      "value": "12.09.1989",
      "purposes": [
        {
          "pid": "002",
          "fid": "003"
        }
      ]
    },
    {
      "dataId": "003",
      "value": "max.mustermann@example.com",
      "purposes": [
        {
          "pid": "002",
          "fid": "002"
        },
        {
          "pid": "002",
          "fid": "003"
        }
      ]
    },
    {
      "dataId": "004",
      "value": "Hauptstr. 1, 12345 Berlin",
      "purposes": [
        {
          "pid": "002",
          "fid": "002"
        },
        {
          "pid": "002",
          "fid": "003"
        }
      ]
    },
    {
      "dataId": "005",
      "value": "0160987654321",
      "purposes": [
        {
          "pid": "002",
          "fid": "002"
        }
      ]
    },
    {
      "dataId": "010",
      "value": "DE12100500001234567890",
      "purposes": [
        {
          "pid": "002",
          "fid": "004"
        }
      ]
    }
  ]
}
```
# Hinweis: 
<p>
  Damit die Datencontainer-App funktioniert, müssen sich alle request.json- und profile.json-Dateien im Basisverzeichnis befinden, also dort, wo die exe-Datei des Projekts erzeugt wird, z.B. 
  <p>C:\Users\...\DataContainer\DataContainerApp\bin\Debug\net10.0</p>
  Hier werden auch die erzeugten response.json-Dateien gespeichert.
</p>
