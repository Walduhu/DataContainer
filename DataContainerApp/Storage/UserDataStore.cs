using System.Text.Json;

namespace DataContainerApp.Storage;

public static class UserDataStore
{
    private static Dictionary<string, object> _data = new();
    private static string _currentPath = "userData.json";

    public static string CurrentProfilePath => _currentPath;

    public static void Load(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Pfad darf nicht leer sein.");

        _currentPath = path;

        if (!File.Exists(_currentPath))
        {
            _data = new();
            return;
        }

        var json = File.ReadAllText(_currentPath);
        _data = JsonSerializer.Deserialize<Dictionary<string, object>>(json)
                ?? new();
    }

    public static void Set(string dataId, object value)
    {
        _data[dataId] = value;
    }

    public static bool TryGetValue(string dataId, out object? value)
        => _data.TryGetValue(dataId, out value);

    public static UserData GetUserData()
        => new UserData { Data = new Dictionary<string, object>(_data) };

    public static void Save()
    {
        var json = JsonSerializer.Serialize(_data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_currentPath, json);
    }
}
