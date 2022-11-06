using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EvoCube.MapGeneration;

public class GameplayInstaller : MonoInstaller
{
    public static void Install(DiContainer container)
    {
        container.BindFactory<GameObject, Chunk, Chunk.Factory>().FromFactory<Chunk.ChunkFactory>();

        container.BindInterfacesAndSelfTo<EvoCube.MapGeneration.PerlinAPI>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("PerlinAPI")
        .AsSingle().NonLazy();

        container.BindInterfacesAndSelfTo<TerrainV3>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("Terrain")
        .AsSingle().NonLazy();


        container.BindMemoryPool<TerrainResourceManager.TopologyWorker, TerrainResourceManager.TopologyWorker.Pool>().WithInitialSize(10);

    }

    public override void InstallBindings()
    {

        Install(Container);
    }
}
