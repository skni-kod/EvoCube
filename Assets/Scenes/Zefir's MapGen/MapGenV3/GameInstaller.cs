using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] GameObject meshTopologyGeneratorPrefab;
    [SerializeField] GameObject meshTopologyWorkerPrefab;
    /*
    public override void InstallBindings()
    {
        Container.Bind<TerrainController>().AsSingle();

        Container.Bind<IMeshTopologyGenerator>()
            .FromInstance(meshTopologyGeneratorPrefab.GetComponent<MeshTopologyGenerator>())
            .AsSingle();

        Container.BindMemoryPool<Chunk, Chunk.Pool>()
            .WithInitialSize(10)
            .FromComponentInNewPrefab(chunkPrefab)
            .UnderTransformGroup("ChunksMemoryPool");

        Container.BindMemoryPool<MeshTopologyWorker, MeshTopologyWorker.Pool>()
            .WithInitialSize(4)
            .FromComponentInNewPrefab(meshTopologyWorkerPrefab)
            .UnderTransformGroup("TopologyWorkersPool");
    }
    */
}