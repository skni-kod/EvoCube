using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EvoCube.MapGeneration;
using EvoCube.Player;

public class AbziInstaller : MonoInstaller
{
    public GameObject spider;
    public GameObject bullet;

    public override void InstallBindings()
    {
        GameplayInstaller.Install(Container);//mapa
       // GameplayInstaller.InstallPlayerSystems(Container);//gracz
        InstallPlayerSystem(Container);//moj gracz
        InstallWeaponSystem(Container);
        InstallEnemySystem(Container);

    }
    public  void InstallPlayerSystem(DiContainer Container)
    {
        Container.BindInterfacesAndSelfTo<Movment>().AsSingle().NonLazy();

         Container.Bind<Playermovment>().FromNewComponentOnNewGameObject().WithGameObjectName("player").AsSingle().NonLazy();
    }
    public void InstallWeaponSystem(DiContainer Container)
    {
        Container.BindInterfacesAndSelfTo<Bullet>().AsCached();
        Container.BindFactory<GameObject,Vector3,Bullet, Bullet.Factory>().FromComponentInNewPrefab(bullet);
        Container.BindInterfacesAndSelfTo<Gun>().AsCached().NonLazy();
        Container.BindInstance(bullet);

        //    Container.BindInstance(Bullet.Factory);
        //Container.Bind<Gun>().AsCached();



    }
    public  void InstallEnemySystem(DiContainer Container)
    {
        Container.BindFactory<Spider, Spider.Factory>().FromComponentInNewPrefab(spider);
        Container.Bind<SpawnerEnemy>().AsSingle();
    }
}
