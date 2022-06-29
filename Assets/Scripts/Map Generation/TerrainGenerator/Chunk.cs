using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Chunk : MonoBehaviour
{
    public Vector3 id;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    [Inject]
    private IMeshCreatingService _meshCreatingService;

    [Inject]
    public void Construct(MeshCreatingService meshCreatingService)
    {
        _meshCreatingService = meshCreatingService;
        Debug.Log("pog");
    }

    public void BuildInit(Vector3 id, Vector3[] perlinData)
    {
        this.id = id;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();

        /*mesh.vertices = _meshCreatingService.ResizePerlinVerticesDown(perlinData);
        mesh.triangles = _meshCreatingService.CalculateTrianglesFlat(LowPolyTerrain2D.instance.chunk_size);*/

        StaticMeshService mm = new StaticMeshService();
        mesh.vertices = mm.ResizePerlinVerticesDown(perlinData);
        mesh.triangles = mm.CalculateTrianglesFlat(LowPolyTerrain2D.instance.chunk_size);
        
        
        
        mesh.RecalculateNormals();
        meshRenderer.material = MaterialsAPI.GetMaterialByName("sand");
        meshFilter.mesh = mesh;
        collider.sharedMesh = mesh;
    }

    private void OnDestroy()
    {
        MyDestroy();
    }
    private void MyDestroy()
    {
        Destroy(meshFilter.mesh);
        Destroy(meshFilter.sharedMesh);
        Destroy(GetComponent<MeshCollider>().sharedMesh);
        Destroy(GetComponent<MeshCollider>());
        Destroy(this);
    }

}
