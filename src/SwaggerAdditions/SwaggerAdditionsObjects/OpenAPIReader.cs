namespace SwaggerAdditionsObjects;

public class OpenAPIReader
{
    public static async Task<OpenApiDocumentAdditional> Read(string openApiPath)
    {
       string content = string.Empty;
        if (IsHttp(openApiPath))
        {
            content = await GetHttpContent(openApiPath);
        }
        else
        {
            content = File.ReadAllText(openApiPath);
        }
    
        return new( content);
    }
    private static async Task<string> GetHttpContent(string openApiPath)
    {
        var httpMessageHandler = new HttpClientHandler();
        httpMessageHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        httpMessageHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        using var http = new HttpClient(httpMessageHandler);
        var content = await http.GetStringAsync(openApiPath);
        return content;
    }
    private static bool IsYaml(string path) => path.EndsWith("yaml") || path.EndsWith("yml");
    private static bool IsHttp(string path) => path.StartsWith("http://") || path.StartsWith("https://");
}
