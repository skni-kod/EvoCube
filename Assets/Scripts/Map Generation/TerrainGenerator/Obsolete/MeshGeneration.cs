using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



//[RequireComponent(typeof(MeshFilter))]

public class MeshGenerator : ScriptableObject
{

    public Vector3[] vertices;
    public int[] triangles;
    public Color[] colors;


    public Vector3 static_offset = new Vector3(0, 0, 0);
    public Vector3 chunk_offset = new Vector3(0, 0, 0);

    public TerrainSettings tSett;

    Vector4 CalculateNoiseHeigth(int x, int z)
    {
        float heigth = 0;
        Color color = new Color(0f, 0f, 0f);

        float kraters_and_mountains_check = Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / 220, (z + chunk_offset.z + static_offset.z) / 220);
        float second_check = -Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / 80, (z + chunk_offset.z + static_offset.z) / 90);
        second_check += Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / 30, (z + chunk_offset.z + static_offset.z) / 20);
        float total_check = (kraters_and_mountains_check * 0.7f) + (second_check * 0.3f);


        if (total_check < 0.125f)
        {
            float multiplier = Mathf.Pow(total_check * 8f, 6);
            color = tSett.gradient.Evaluate(multiplier);
            heigth += Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.small_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.small_noise_detail) * 0.2f;
            heigth -= Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.medium_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.medium_noise_detail) * tSett.medium_noise_precentage * multiplier;
            heigth += Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.big_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.big_noise_detail) * tSett.big_noise_precentage * multiplier;
        }
        else if (total_check > 0.5f)
        {

            float multiplier = (total_check - 0.5f) * 2f;
            color = tSett.orangeDirt;
            heigth += Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.small_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.small_noise_detail) * tSett.small_noise_precentage;
            heigth -= Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.medium_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.medium_noise_detail) * tSett.medium_noise_precentage;
            heigth += Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.big_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.big_noise_detail) * (tSett.big_noise_precentage * (1f + multiplier * 4f));

        }
        else
        {
            heigth += Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.small_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.small_noise_detail) * tSett.small_noise_precentage;
            color = tSett.orangeDirt;
            heigth -= Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.medium_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.medium_noise_detail) * tSett.medium_noise_precentage;
            heigth += Mathf.PerlinNoise((x + chunk_offset.x + static_offset.x) / tSett.big_noise_detail, (z + chunk_offset.z + static_offset.z) / tSett.big_noise_detail) * tSett.big_noise_precentage;
        }


        return new Vector4(heigth, color.r, color.g, color.b);
    }

    public void CreateMesh()
    {
        vertices = new Vector3[(tSett.size + 1) * (tSett.size + 1) * 36];
        triangles = new int[(tSett.size * tSett.size) * 6];
        colors = new Color[vertices.Length];

        Vector4 perlinData;
        Vector3 heigth;
        Color color;

        for (int i = 0, z = 0; z <= tSett.size; z++)
        {
            for (int x = 0; x <= tSett.size; x++)
            {
                perlinData = CalculateNoiseHeigth(x, z);
                heigth = new Vector3(x, perlinData.x * tSett.total_noise_depth, z);
                color = new Color(perlinData.y, perlinData.z, perlinData.w);
                vertices[i] = heigth;
                vertices[i + 1] = heigth;
                vertices[i + 2] = heigth;
                vertices[i + 3] = heigth;
                vertices[i + 4] = heigth;
                vertices[i + 5] = heigth;
                colors[i] = color;
                colors[i + 1] = color;
                colors[i + 2] = color;
                colors[i + 3] = color;
                colors[i + 4] = color;
                colors[i + 5] = color;

                i += 6;
            }
        }


        int vert = 0;
        int tris = 0;
        for (int z = 0; z < tSett.size; z++)
        {
            for (int x = 0; x < tSett.size; x++)
            {

                triangles[tris + 0] = vert + 1;
                triangles[tris + 1] = vert + ((tSett.size + 1) * 6) + 6 + 5;
                triangles[tris + 2] = vert + 6 + 3;
                triangles[tris + 3] = vert + 2;
                triangles[tris + 4] = vert + ((tSett.size + 1) * 6);
                triangles[tris + 5] = vert + ((tSett.size + 1) * 6) + 6 + 4;
                vert += 6;
                tris += 6;
            }
            vert += 6;
        }
    }

    void EditSingleVerticePosition(Vector3 change_in_pos, int vertice_index)
    {
        vertices[vertice_index].x += change_in_pos.x;
        vertices[vertice_index].y += change_in_pos.y;
        vertices[vertice_index].z += change_in_pos.z;
    }

    void EditPackofVerticesPositions(Vector3 change_in_pos, int vertice_index)
    {
        int modulo = vertice_index % 6;
        //jak sie wam chce to mozecie to jakoś poprawić
        if (modulo == 0)
        {
            EditSingleVerticePosition(change_in_pos, vertice_index);
            EditSingleVerticePosition(change_in_pos, vertice_index + 1);
            EditSingleVerticePosition(change_in_pos, vertice_index + 2);
            EditSingleVerticePosition(change_in_pos, vertice_index + 3);
            EditSingleVerticePosition(change_in_pos, vertice_index + 4);
            EditSingleVerticePosition(change_in_pos, vertice_index + 5);
        }
        else if (modulo == 1)
        {
            EditSingleVerticePosition(change_in_pos, vertice_index);
            EditSingleVerticePosition(change_in_pos, vertice_index + 1);
            EditSingleVerticePosition(change_in_pos, vertice_index + 2);
            EditSingleVerticePosition(change_in_pos, vertice_index + 3);
            EditSingleVerticePosition(change_in_pos, vertice_index + 4);
            EditSingleVerticePosition(change_in_pos, vertice_index - 1);
        }
        else if (modulo == 2)
        {
            EditSingleVerticePosition(change_in_pos, vertice_index);
            EditSingleVerticePosition(change_in_pos, vertice_index + 1);
            EditSingleVerticePosition(change_in_pos, vertice_index + 2);
            EditSingleVerticePosition(change_in_pos, vertice_index + 3);
            EditSingleVerticePosition(change_in_pos, vertice_index - 2);
            EditSingleVerticePosition(change_in_pos, vertice_index - 1);
        }
        else if (modulo == 3)
        {
            EditSingleVerticePosition(change_in_pos, vertice_index);
            EditSingleVerticePosition(change_in_pos, vertice_index + 1);
            EditSingleVerticePosition(change_in_pos, vertice_index + 2);
            EditSingleVerticePosition(change_in_pos, vertice_index - 3);
            EditSingleVerticePosition(change_in_pos, vertice_index - 2);
            EditSingleVerticePosition(change_in_pos, vertice_index - 1);
        }
        else if (modulo == 4)
        {
            EditSingleVerticePosition(change_in_pos, vertice_index);
            EditSingleVerticePosition(change_in_pos, vertice_index + 1);
            EditSingleVerticePosition(change_in_pos, vertice_index - 4);
            EditSingleVerticePosition(change_in_pos, vertice_index - 3);
            EditSingleVerticePosition(change_in_pos, vertice_index - 2);
            EditSingleVerticePosition(change_in_pos, vertice_index - 1);
        }
        else if (modulo == 5)
        {
            EditSingleVerticePosition(change_in_pos, vertice_index);
            EditSingleVerticePosition(change_in_pos, vertice_index - 5);
            EditSingleVerticePosition(change_in_pos, vertice_index - 4);
            EditSingleVerticePosition(change_in_pos, vertice_index - 3);
            EditSingleVerticePosition(change_in_pos, vertice_index - 2);
            EditSingleVerticePosition(change_in_pos, vertice_index - 1);
        }

    }

    public void RaiseTriangle(int triangleIndex, Vector3 change_in_pos)
    {
        EditPackofVerticesPositions(change_in_pos, triangles[triangleIndex * 3 + 0]);
        EditPackofVerticesPositions(change_in_pos, triangles[triangleIndex * 3 + 1]);
        EditPackofVerticesPositions(change_in_pos, triangles[triangleIndex * 3 + 2]);
    }

    public void EditTriangleColor(int triangleIndex)
    {
        colors[triangles[triangleIndex * 3 + 0]] = new Color(0, 0, 0);
        colors[triangles[triangleIndex * 3 + 1]] = new Color(0, 0, 0);
        colors[triangles[triangleIndex * 3 + 2]] = new Color(0, 0, 0);
    }


}