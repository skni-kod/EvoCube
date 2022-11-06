using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EvoCube.MapGeneration;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<GameObject, Chunk, Chunk.Factory>().FromFactory<Chunk.ChunkFactory>();

        Container.BindInterfacesAndSelfTo<PerlinAPI>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("PerlinAPI")
        .AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<TerrainV3>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("Terrain")
        .AsSingle().NonLazy();


        Container.BindMemoryPool<TerrainResourceManager.TopologyWorker, TerrainResourceManager.TopologyWorker.Pool>().WithInitialSize(10);


    }
}
