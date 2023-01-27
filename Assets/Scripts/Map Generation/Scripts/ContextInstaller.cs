using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ContextInstaller : MonoInstaller
{
    //[SerializeField] GameObject initializerPrefab;

    public override void InstallBindings()
    {


        Container.BindInterfacesAndSelfTo<CoreDirector>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("CoreDirector")
        .AsSingle().NonLazy();

        //Guard.AgainstNull(initializerPrefab, $"CoreInstaller: initializerPrefab");

        Container.Bind<DirectorsCamera>().FromNewComponentOnNewGameObject().AsTransient();

        Container.BindInterfacesAndSelfTo<UiDirector>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("UIDirector")
        .AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<CameraDirector>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("CameraDirector")
        .AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<LightDirector>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("LigthDirector")
        .AsSingle().NonLazy();


        /*Container.Bind<Initializer>()
        .FromComponentInNewPrefab(initializerPrefab)
        .WithGameObjectName("Initializer")
        .AsSingle().NonLazy();*/



        List<Scene> scenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            scenes.Add(SceneManager.GetSceneAt(i));
        }
    }
}
