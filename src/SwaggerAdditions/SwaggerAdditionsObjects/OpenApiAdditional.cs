using Microsoft.OpenApi.Interfaces;
using System.Linq;
namespace SwaggerAdditionsObjects;
public class OpenApiDocumentAdditional
{
    private readonly OpenApiDocument document;
    private string OriginalContent;

    public OpenApiDocumentAdditional(string content)
    {
        OriginalContent = content;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var reader = new OpenApiStreamReader();
        document = reader.Read(stream, out var diagnostic);
    }

    public OpenApiInfo Info => document.Info;
    public OpenApiPaths Paths => document.Paths;

    public async Task<string> MergeInfo(string folder)
    {
        var file = Path.Combine(folder, "info.json");
        var content = await File.ReadAllTextAsync(file);
        var doc = JsonDocument.Parse(content);
        var merge = doc.RootElement.GetProperty("HowToMerge").GetString();
        ArgumentNullException.ThrowIfNull(merge);
        MergeData mergeData = Enum.Parse<MergeData>(merge, true);
        if (mergeData == MergeData.None)
            return content;
        var data = doc.RootElement.GetProperty("data");
        ArgumentNullException.ThrowIfNull(data);
        var kvCol = data.EnumerateObject().ToArray();
        foreach (var item in kvCol)
        {
            switch (item.Name)
            {
                case "title":
                    switch (mergeData)
                    {
                        case MergeData.Upsert:
                            this.Info.Title = item.Value.GetString()??"";
                            break;
                        case MergeData.AddNotReplace:
                            if (string.IsNullOrWhiteSpace(this.Info.Title))
                                this.Info.Title = item.Value.GetString() ?? "";
                            break;
                        default:
                            throw new ArgumentException("Invalid mergeData"+mergeData);
                    }
                    break;
                case "version":
                    switch (mergeData)
                    {
                        case MergeData.Upsert:
                            this.Info.Version = item.Value.GetString() ?? "";
                            break;
                        case MergeData.AddNotReplace:
                            if (string.IsNullOrWhiteSpace(this.Info.Version))
                                this.Info.Version = item.Value.GetString() ?? "";
                            break;
                        default:
                            throw new ArgumentException("Invalid mergeData" + mergeData);
                    }
                    break;
                case "description":
                    switch (mergeData)
                    {
                        case MergeData.Upsert:
                            this.Info.Description = item.Value.GetString() ?? "";
                            break;
                        case MergeData.AddNotReplace:
                            if (string.IsNullOrWhiteSpace(this.Info.Description))
                                this.Info.Description = item.Value.GetString() ?? "";
                            break;
                        default:
                            throw new ArgumentException("Invalid mergeData" + mergeData);
                    }
                    break;
            }
        }
        return content;
    }
}
