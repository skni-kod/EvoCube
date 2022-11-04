using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChunkA : MonoBehaviour, IChunk
{
    public class Factory : PlaceholderFactory<GameObject, ChunkA>
    {
    }

    public class ChunkFactory : IFactory<GameObject, ChunkA>
    {
        [Inject] readonly DiContainer _container;

        public ChunkA Create(GameObject gameObject)
        {
            gameObject.AddComponent(typeof(Mesh));
            gameObject.AddComponent(typeof(Mesh));
            return _container.InstantiateComponent<ChunkA>(gameObject);
        }
    }
}

