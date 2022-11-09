using UnityEngine;
using Zenject;

public class Menager : MonoInstaller
{
    public override void InstallBindings()
    {
      
        Container.BindInterfacesAndSelfTo<Movment>().AsSingle().NonLazy();
     
        Container.Bind<Playermovment>().FromNewComponentOnNewGameObject().WithGameObjectName("player").AsSingle().NonLazy();

    }
}