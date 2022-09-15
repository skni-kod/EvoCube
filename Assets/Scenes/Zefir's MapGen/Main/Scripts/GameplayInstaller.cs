using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TerrainV3>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("Terrain")
        .AsSingle().NonLazy();
    }
}
