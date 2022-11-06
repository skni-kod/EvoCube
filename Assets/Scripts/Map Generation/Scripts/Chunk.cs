using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace EvoCube.MapGeneration
{
    public class Chunk : MonoBehaviour, IChunk
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

        public Mesh GetMesh()
        {
            return meshFilter.sharedMesh;
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

        public class Factory : PlaceholderFactory<GameObject, Chunk>
        {
        }

        public class ChunkFactory : IFactory<GameObject, Chunk>
        {
            [Inject] readonly DiContainer _container;
            [Inject] readonly TerrainResourceManager.TopologyWorker.Pool _topologyWorkerPool;

            public Chunk Create(GameObject gameObject)
            {
                //ChunkA chunk = _container.InstantiateComponentOnNewGameObject<ChunkA>("Chunk");
                Chunk chunk = _container.InstantiateComponent<Chunk>(gameObject);
                chunk.meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
                chunk.meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
                chunk.meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");

                return chunk;
            }
        }
    }

}