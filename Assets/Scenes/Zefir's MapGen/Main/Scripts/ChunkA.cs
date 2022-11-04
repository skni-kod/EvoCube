using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChunkA : MonoBehaviour, IChunk
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public void SetMesh(Mesh mesh)
    {
        if (meshFilter != null)
        {
            RemoveMesh();
            meshFilter.sharedMesh = mesh;
        }
    }

    public Mesh RemoveMesh()
    {
        Mesh meshObj = meshFilter.sharedMesh;
        meshFilter.sharedMesh = null;
        return meshObj;
    }

    public class Factory : PlaceholderFactory<GameObject, ChunkA>
    {
    }

    public class ChunkFactory : IFactory<GameObject, ChunkA>
    {
        [Inject] readonly DiContainer _container;

        public ChunkA Create(GameObject gameObject)
        {
            ChunkA chunk = _container.InstantiateComponent<ChunkA>(gameObject);
            chunk.meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
            chunk.meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));



            return chunk;
        }
    }
}

