using Zenject;
using NUnit.Framework;
using static TerrainResourceManager;
using UnityEngine;
using EvoCube.MapGeneration;

[TestFixture]
public class UntitledUnitTest : ZenjectUnitTestFixture
{
    [SetUp]
    public void Install()
    {
        Container.BindFactory<GameObject, Chunk, Chunk.Factory>().FromFactory<Chunk.ChunkFactory>();
        Container.BindMemoryPool<TopologyWorker, TopologyWorker.Pool>().WithInitialSize(10);
    }

    [Test]
    public void RunTest1()
    {

    }
}