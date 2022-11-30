using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller
{
    public GameObject spider;
    public override void InstallBindings()
    {
        //  Container.Bind<IEnemy>().To<Enemy>().;
        Container.BindFactory<Spider, Spider.Factory>().FromComponentInNewPrefab(spider);
        Container.Bind<SpawnerEnemy>().AsSingle();
    }
}