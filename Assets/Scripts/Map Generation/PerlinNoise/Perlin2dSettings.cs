using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Perlin2dSettings : ScriptableObject
{
    [SerializeField] public float frequency = 0.02f;
    [SerializeField] public float lacunarity = 2f;
    [SerializeField] public float gain = 0.5f;
    [SerializeField] public int seed = 0;
    [SerializeField] public int octaves = 4;
    [SerializeField] public float resolution = 1f;
    [SerializeField] public float idk = 1;
    [SerializeField] public int type = 0;
}
