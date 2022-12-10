using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EvoCube.MapGeneration;
using EvoCube.Player;

public class AbziInstaller : MonoInstaller
{
    public GameObject spider;

    public override void InstallBindings()
    {
        GameplayInstaller.Install(Container);//mapa
       // GameplayInstaller.InstallPlayerSystems(Container);//gracz
        InstallPlayerSystem(Container);//moj gracz
        InstallEnemySystem(Container);
    }
    public  void InstallPlayerSystem(DiContainer Container)
    {
        Container.BindInterfacesAndSelfTo<Movment>().AsSingle().NonLazy();

         Container.Bind<Playermovment>().FromNewComponentOnNewGameObject().WithGameObjectName("player").AsSingle().NonLazy();
    }
    public  void InstallEnemySystem(DiContainer Container)
    {
        Container.BindFactory<Spider, Spider.Factory>().FromComponentInNewPrefab(spider);
        Container.Bind<SpawnerEnemy>().AsSingle();
    }
}
