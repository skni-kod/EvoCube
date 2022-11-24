using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace EvoCube.MapGeneration
{
    public class Chunk : MonoBehaviour, IChunk
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        public Vector3 Id { get; set; }

        public void BuildMesh(Vector3[] perlinData)
        {
            Mesh mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.vertices = MeshAPI.ResizePerlinVerticesDown(perlinData);
            mesh.triangles = MeshAPI.CalculateTrianglesFlat(TerrainConfig.chunkSize);
            mesh.RecalculateNormals();
            SetMesh(mesh);
        }


        public void BuildMeshCallback(AsyncGPUReadbackRequest request)
        {
            Mesh mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.vertices = MeshAPI.ResizePerlinVerticesDown(request.GetData<Vector3>().ToArray());
            mesh.triangles = MeshAPI.CalculateTrianglesFlat(TerrainConfig.chunkSize);
            mesh.RecalculateNormals();
            SetMesh(mesh);
        }

        public Mesh GetMesh()
        {
            return _meshFilter.sharedMesh;
        }

        public void SetMesh(Mesh mesh)
        {
            if (_meshFilter != null)
            {
                RemoveMesh();
                mesh.name = "ChunkMesh";
                _meshFilter.sharedMesh = mesh;
            }
        }

        public void RemoveMesh()
        {
            Mesh meshObj = _meshFilter.sharedMesh;
            _meshFilter.sharedMesh = null;
            Destroy(meshObj);
        }

        public class Factory : PlaceholderFactory<GameObject, Chunk>
        {
        }

        public class ChunkFactory : IFactory<GameObject, Chunk>
        {
            [Inject] readonly DiContainer _container;
            [Inject] readonly TopologyWorker.Pool _topologyWorkerPool;

            public Chunk Create(GameObject gameObject)
            {
                //ChunkA chunk = _container.InstantiateComponentOnNewGameObject<ChunkA>("Chunk");
                Chunk chunk = _container.InstantiateComponent<Chunk>(gameObject);
                chunk._meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
                chunk._meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
                chunk._meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");

                return chunk;
            }
        }
    }

}