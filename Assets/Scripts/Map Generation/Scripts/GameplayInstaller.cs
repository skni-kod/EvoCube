using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EvoCube.MapGeneration;
using EvoCube.Player;

public class GameplayInstaller : MonoInstaller
{

    public static void Install(DiContainer Container)
    {

        Container.Bind<Perlin2dSettings>().FromScriptableObjectResource("ScriptableObjects/PerlinSettigns/TestingNewMapGen").AsSingle();
        Container.BindFactory<GameObject, Chunk, Chunk.Factory>().FromFactory<Chunk.ChunkFactory>();

        Container.BindInterfacesAndSelfTo<EvoCube.MapGeneration.PerlinAPI>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("PerlinAPI")
        .AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<TerrainV3>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("Terrain")
        .AsSingle().NonLazy();


        Container.BindMemoryPool<TopologyWorker, TopologyWorker.Pool>()
        .WithInitialSize(10).FromNewComponentOnNewGameObject()
        .UnderTransformGroup("Topology Workers");

    }

    public static void InstallPlayerSystems(DiContainer Container)
    {
        Container.Bind<Player>().FromNewComponentOnNewGameObject()
        .WithGameObjectName("Player")
        .AsSingle().NonLazy();

        Container.Bind<FirstPersonCamera>().FromNewComponentOnNewGameObject()
        .WithGameObjectName("FirstPersonCameraController").UnderTransformGroup("PlayerSystems")
        .AsSingle().NonLazy();

        Container.Bind<SimpleFlyingMovement>().FromNewComponentOnNewGameObject()
        .WithGameObjectName("SimpleFlyingMovementController").UnderTransformGroup("PlayerSystems")
        .AsSingle().NonLazy();
    }

    public override void InstallBindings()
    {

        Install(Container);
        InstallPlayerSystems(Container);
    }
}
