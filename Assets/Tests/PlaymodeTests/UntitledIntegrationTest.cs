using Zenject;
using System.Collections;
using UnityEngine.TestTools;
using NUnit.Framework;

public class UntitledIntegrationTest : ZenjectIntegrationTestFixture
{
    [UnityTest]
    public IEnumerator RunTest1()
    {
        // Setup initial state by creating game objects from scratch, loading prefabs/scenes, etc

        PreInstall();

        GameplayInstaller.Install(Container);
        // Call Container.Bind methods

        PostInstall();

        Assert.IsTrue(EvoCube.MapGeneration.PerlinAPI.initialised);



        // Add test assertions for expected state
        // Using Container.Resolve or [Inject] fields
        yield break;
    }
}