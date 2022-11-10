using UnityEngine;
using Zenject;

public class Menager : MonoInstaller
{
    [SerializeField]
    public GameObject wygladGraczaPrefab;
    public override void InstallBindings()
    {
      
        Container.BindInterfacesAndSelfTo<Movment>().AsSingle().NonLazy();
     
      //  Container.Bind<Playermovment>().FromNewComponentOnNewGameObject().WithGameObjectName("player").AsSingle().NonLazy();
        Container.Bind<Playermovment>().FromNewComponentOnNewPrefab(wygladGraczaPrefab).WithGameObjectName("player").AsSingle().NonLazy();
    }
}