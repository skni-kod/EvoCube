using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCube : MonoBehaviour
{
    public Vector3 id;

    public void GenerateMeshesFromData(Vert[] data)
    {
        //Extract the positions, normals and indexes.
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> index = new List<int>();

        int chunkVolume = (int)Mathf.Pow(TerrainMap.instance.mapGenSettings.chunkSize, 3);
        int meshBufferSize = chunkVolume * 5 * 3; // there is 5 triangles each with 3 points

        int idx = 0;
        for (int i = 0; i < meshBufferSize; i++)
        {
            //If the marching cubes generated a vert for this index
            //then the position w value will be 1, not -1.
            if (data[i].position.w != -1)
            {
                positions.Add(data[i].position);
                normals.Add(data[i].normal);
                index.Add(idx++);
            }

            int maxTriangles = 65000 / 3;

            if (idx >= maxTriangles)
            {
                GameObject gameObject = Object.Instantiate(TerrainMap.instance.chunkMeshPrefab);
                gameObject.transform.parent = transform;
                gameObject.GetComponent<ChunkMesh>().Build(positions.ToArray(), normals.ToArray(), index.ToArray());


                
                idx = 0;
                positions.Clear();
                normals.Clear();
                index.Clear();
            }
        }
        if (idx != 0)
        {
            GameObject gameObject = Object.Instantiate(TerrainMap.instance.chunkMeshPrefab);
            gameObject.transform.parent = transform;
            gameObject.GetComponent<ChunkMesh>().Build(positions.ToArray(), normals.ToArray(), index.ToArray());
        }
    }
}
