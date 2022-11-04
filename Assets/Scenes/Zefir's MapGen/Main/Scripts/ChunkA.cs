using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChunkA : MonoBehaviour, IChunk
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public void BuildMesh(Vector3[] perlinData)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = MeshAPI.ResizePerlinVerticesDown(perlinData);
        mesh.triangles = MeshAPI.CalculateTrianglesFlat(64);
        mesh.RecalculateNormals();
        
        SetMesh(mesh);

    }

    public void SetMesh(Mesh mesh)
    {
        if (meshFilter != null)
        {
            RemoveMesh();
            mesh.name = "ChunkMesh";
            meshFilter.sharedMesh = mesh;
        }
    }

    public void RemoveMesh()
    {
        Mesh meshObj = meshFilter.sharedMesh;
        meshFilter.sharedMesh = null;
        Destroy(meshObj);
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
            chunk.meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");
            
            MeshTopologyWorker worker = new MeshTopologyWorker();
            worker.Init();
            worker.chunk = chunk;
            worker.Generate(new Vector3(0, 0, 0));


            return chunk;
        }
    }
}

