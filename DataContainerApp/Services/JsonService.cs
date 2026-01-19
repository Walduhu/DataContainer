using System.Text.Json;

namespace DataContainerApp.Services;

public static class JsonService
{
    public static T Load<T>(string path)
    {
        var json = File.ReadAllText(path);
        var result = JsonSerializer.Deserialize<T>(json);
        if (result is null)
            throw new InvalidOperationException($"Deserialization of type {typeof(T).Name} from file '{path}' returned null.");
        return result;
    }

    public static void Save<T>(string path, T data)
        => File.WriteAllText(path,
            JsonSerializer.Serialize(data, new JsonSerializerOptions { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
}

