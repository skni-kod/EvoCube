using Zenject;
using System.Collections;
using UnityEngine.TestTools;
using NUnit.Framework;

public class MeshGenerationTests : ZenjectIntegrationTestFixture
{
    [UnityTest]
    public IEnumerator InitialisationTest()
    {
        // Setup initial state by creating game objects from scratch, loading prefabs/scenes, etc
        PreInstall();
        GameplayInstaller.Install(Container);
        // Call Container.Bind methods
        PostInstall();
        EvoCube.MapGeneration.PerlinAPI _perlinApi = Container.Resolve<EvoCube.MapGeneration.PerlinAPI>();
        Assert.IsTrue(_perlinApi.initialised);
        // Add test assertions for expected state
        // Using Container.Resolve or [Inject] fields
        yield break;
    }



}