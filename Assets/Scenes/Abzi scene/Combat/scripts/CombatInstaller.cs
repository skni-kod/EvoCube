using UnityEngine;
using Zenject;

public class CombatInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ShootingSystem>().AsSingle();
        Container.Bind<PlayerCombatSystem>().AsSingle();
    }
}