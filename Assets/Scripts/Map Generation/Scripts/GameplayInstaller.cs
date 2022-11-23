using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EvoCube.MapGeneration;

public class GameplayInstaller : MonoInstaller
{
    
    public static void Install(DiContainer container)
    {

        container.Bind<Perlin2dSettings>().FromScriptableObjectResource("ScriptableObjects/PerlinSettigns/TestingNewMapGen").AsSingle();
        container.BindFactory<GameObject, Chunk, Chunk.Factory>().FromFactory<Chunk.ChunkFactory>();

        container.BindInterfacesAndSelfTo<EvoCube.MapGeneration.PerlinAPI>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("PerlinAPI")
        .AsSingle().NonLazy();

        container.BindInterfacesAndSelfTo<TerrainV3>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("Terrain")
        .AsSingle().NonLazy();


        container.BindMemoryPool<TopologyWorker, TopologyWorker.Pool>().WithInitialSize(10).FromNewComponentOnNewGameObject().UnderTransformGroup("Topology Workers");

    }

    public override void InstallBindings()
    {

        Install(Container);
    }
}
