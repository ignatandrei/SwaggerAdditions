
namespace SA_Test;
[FeatureDescription(@"Read JSON from file")] 
[Label("Story-Read")]
[TestClass]
public partial class TestReadSwagger: FeatureFixture
{
    [Scenario]
    [Label("ReadJson")]
    //[ScenarioCategory(Categories.Security)]
    public async Task Successful_ReadJSON_FromNetCoreStart() 
    {
        await Runner.RunScenarioAsync(
            given =>Original_Swagger_Is_File(@"Data/simple.json"),
            then => Swagger_Info_Have_The_Following("testToBeDeleted",null, "1.0")
        );  
    }
    OpenApiDocumentAdditional? doc = null;
    private async Task Original_Swagger_Is_File(string file)
    {
        doc = await OpenAPIReader.Read(file);
        Assert.IsNotNull(doc);
    }
    private async Task Swagger_Info_Have_The_Following(string title, string? desc, string version)
    {
        await Task.Yield();
        ArgumentNullException.ThrowIfNull(doc);
        Assert.AreEqual(title, doc.Info.Title);
        Assert.AreEqual(version, doc.Info.Version);
        Assert.AreEqual(desc, doc.Info.Description);
    }

    //[TestMethod]
    //public async Task ReadSimple()
    //{
    //    var file = @"Data/simple.json";
    //    var doc = await OpenAPIReader.Read(file);
    //    Assert.IsNotNull(doc);
    //    Assert.AreEqual(1, doc.Paths.Count);
    //    Assert.AreEqual("testToBeDeleted", doc.Info.Title);
    //    Assert.AreEqual("1.0", doc.Info.Version);
    //    Assert.IsTrue(string.IsNullOrWhiteSpace(doc.Info.Description));
    //}
    
}