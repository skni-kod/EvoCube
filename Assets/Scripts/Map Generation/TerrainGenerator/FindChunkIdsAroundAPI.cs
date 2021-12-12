using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public static class FindChunkIdsAroundAPI
{

    public static List<Vector3> FindChunksIdsAroundSquare(Vector3 focusPoint, int range)
    {
        int size = LowPolyTerrain.instance.chunk_size;
        List<Vector3> foundChunks = new List<Vector3>();
        Vector3 center = new Vector3(
                                     Mathf.FloorToInt(focusPoint.x / size) * size,
                                     Mathf.FloorToInt(focusPoint.y / size) * size,
                                     Mathf.FloorToInt(focusPoint.z / size) * size
                                     );

        for (int x = -range;x<range;x++)
        {
            for (int y = -range; y < range; y++)
            {
                for (int z = -range; z < range; z++)
                {
                    Vector3 id = new Vector3(x, y, z);
                    if (!LowPolyTerrain.instance.chunks.ContainsKey(id))
                        foundChunks.Add(id);
                }
            }
        }

        return foundChunks;
    }


}
