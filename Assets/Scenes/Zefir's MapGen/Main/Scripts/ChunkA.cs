using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;
using TRM = TerrainResourceManager;

public class ChunkA : MonoBehaviour, IChunk
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public void BuildMesh(Vector3[] perlinData)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = MeshAPI.ResizePerlinVerticesDown(perlinData);
        mesh.triangles = MeshAPI.CalculateTrianglesFlat(TerrainConfig.chunkSize);
        mesh.RecalculateNormals();
        SetMesh(mesh);
    }

    public void BuildMeshCallback(AsyncGPUReadbackRequest request)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = MeshAPI.ResizePerlinVerticesDown(request.GetData<Vector3>().ToArray());
        mesh.triangles = MeshAPI.CalculateTrianglesFlat(TerrainConfig.chunkSize);
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
        [Inject] readonly TRM.TopologyWorker.Pool _topologyWorkerPool;

        public ChunkA Create(GameObject gameObject)
        {
            //ChunkA chunk = _container.InstantiateComponentOnNewGameObject<ChunkA>("Chunk");
            ChunkA chunk = _container.InstantiateComponent<ChunkA>(gameObject);
            chunk.meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
            chunk.meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
            chunk.meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");

            return chunk;
        }
    }
}

