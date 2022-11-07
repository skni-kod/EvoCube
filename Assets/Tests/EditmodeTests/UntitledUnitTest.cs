using Zenject;
using NUnit.Framework;
using UnityEngine;
using EvoCube.MapGeneration;

[TestFixture]
public class UntitledUnitTest : ZenjectUnitTestFixture
{
    [SetUp]
    public void Install()
    {
        GameplayInstaller.Install(Container);
    }

    [Test]
    public void RunTest1()
    {
        EvoCube.MapGeneration.PerlinAPI perlin = Container.Resolve<EvoCube.MapGeneration.PerlinAPI>();

        Assert.IsTrue(perlin.initialised);
    }
}