[TestClass]
public class LightBddIntegration
{
    [AssemblyInitialize]
    public static void Setup(TestContext testContext) { LightBddScope.Initialize(); }
    [AssemblyCleanup]
    public static void Cleanup() { LightBddScope.Cleanup(); }
}