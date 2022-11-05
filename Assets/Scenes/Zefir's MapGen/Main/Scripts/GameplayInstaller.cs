using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<GameObject, ChunkA, ChunkA.Factory>().FromFactory<ChunkA.ChunkFactory>();


        Container.BindInterfacesAndSelfTo<TerrainV3>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("Terrain")
        .AsSingle().NonLazy();


        Container.BindMemoryPool<TerrainResourceManager.TopologyWorker, TerrainResourceManager.TopologyWorker.Pool>().WithInitialSize(10);


    }
}
