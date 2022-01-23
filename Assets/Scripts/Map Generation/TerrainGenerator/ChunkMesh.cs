using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh : MonoBehaviour
{
    MeshFilter _filter;
    MeshRenderer _renderer;
    MeshCollider _collider;

    public void Build(Vector3[] positions, Vector3[] normals, int[] index)
    {
        _filter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<MeshCollider>();

        Mesh mesh = (Mesh)Instantiate(_filter.sharedMesh);
        mesh.Clear();
        
        mesh.vertices = positions;
        mesh.normals = normals;
        mesh.triangles = index;
        //mesh.SetTriangles(index, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        //mesh.bounds = new Bounds(new Vector3(0, TerrainMap.instance.mapGenSettings.chunkSize / 2, 0), new Vector3(TerrainMap.instance.mapGenSettings.chunkSize, TerrainMap.instance.mapGenSettings.chunkSize, TerrainMap.instance.mapGenSettings.chunkSize));

        //mesh.MarkModified();
        //mesh.RecalculateTangents();


        _filter.mesh = mesh;
        _renderer.material = MaterialsAPI.GetMaterialByName("sand");
        //_collider.sharedMesh = mesh;

    }
}
