namespace DataContainerApp.Storage;

public static class FilePaths
{
    public static string BaseDirectory =>
        AppContext.BaseDirectory;

    public static string[] GetRequestFiles()
    {
        return Directory.GetFiles(
            BaseDirectory,
            "request*.json",
            SearchOption.TopDirectoryOnly
        );
    }

    public static string GetResponsePath(string requestPath)
    {
        var fileName = Path.GetFileName(requestPath);

        // requestXYZ.json → responseXYZ.json
        var responseFileName = fileName.Replace("request", "response");

        return Path.Combine(BaseDirectory, responseFileName);
    }
}
