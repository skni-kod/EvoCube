using UnityEngine;
using Zenject;

public class MovmentInstaller : MonoInstaller
{
    [SerializeField]
    public GameObject wygladGraczaPrefab;
    public override void InstallBindings()
    {
      
        Container.BindInterfacesAndSelfTo<Movment>().AsSingle().NonLazy();
     
      //  Container.Bind<Playermovment>().FromNewComponentOnNewGameObject().WithGameObjectName("player").AsSingle().NonLazy();
        Container.Bind<Playermovment>().FromNewComponentOnNewPrefab(wygladGraczaPrefab).WithGameObjectName("Player").AsSingle().NonLazy();
    }
}