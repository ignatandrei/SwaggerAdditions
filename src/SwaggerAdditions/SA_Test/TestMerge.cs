using System.Text.Json;

namespace SA_Test;
[FeatureDescription(
@"MergeInfo")] //feature description
[Label("Story-Info")]
[TestClass]
public partial class TestMergeInfo: FeatureFixture
{
    //[TestMethod]
    //public async Task ReadMergeInfoUpsert()
    //{
    //    var file = @"Data/simple.json";
    //    var doc = await OpenAPIReader.Read(file);
    //    Assert.IsNotNull(doc);
    //    await doc.MergeInfo(@"Data/mergeInfo1");
    //    Assert.AreEqual("newTest", doc.Info.Title);
    //    Assert.AreEqual("2.0", doc.Info.Version);
    //    Assert.AreEqual("newDescription", doc.Info.Description);
    //}

    [Scenario]
    [Label("Upsert  - see comment")]
    //[ScenarioCategory(Categories.Security)]
    public async Task MergeJSONUpsertInfo()
    {
        await Runner.RunScenarioAsync(
            given => Original_Swagger_Is_File(@"Data/simple.json"),
            then => Swagger_Info_Have_The_Following("testToBeDeleted", null, "1.0"),
            give => Merge_With_Info_From_File(@"Data/mergeInfo1"),
            then => Swagger_Info_Have_The_Following("newTest", "newDescription", "2.0")
        );
    }
    OpenApiDocumentAdditional? doc = null;

    private async Task Merge_With_Info_From_File(string file)
    {
        ArgumentNullException.ThrowIfNull(doc);
        var data= await doc.MergeInfo(file);
        StepExecution.Current.Comment("merged with "+data);
    }
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
        StepExecution.Current.Comment("Info" + JsonSerializer.Serialize(doc.Info));
    }
    [Scenario]
    [Label("AddNotReplace  - see comment")]
    //[ScenarioCategory(Categories.Security)]
    public async Task MergeJSONAddNotReplaceInfo()
    {
        await Runner.RunScenarioAsync(
            given => Original_Swagger_Is_File(@"Data/simple.json"),
            then => Swagger_Info_Have_The_Following("testToBeDeleted", null, "1.0"),
            give => Merge_With_Info_From_File(@"Data/mergeInfo2"),
            then => Swagger_Info_Have_The_Following("testToBeDeleted", "newDescription", "1.0")
        );
    }



    //[TestMethod]     
    //public async Task ReadMergeInfoAddNotReplace()
    //{
    //    var file = @"Data/simple.json";
    //    var doc = await OpenAPIReader.Read(file);
    //    Assert.IsNotNull(doc);
    //    await doc.MergeInfo(@"Data/mergeInfo2");
    //    Assert.AreEqual("testToBeDeleted", doc.Info.Title);
    //    Assert.AreEqual("1.0", doc.Info.Version);
    //    Assert.AreEqual("newDescription",doc.Info.Description);
    //}
}
