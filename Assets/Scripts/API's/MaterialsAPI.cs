using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MaterialsAPI
{
    public static Material GetMaterialByName(string name)
    {
        return Resources.Load<Material>($"Materials/{name}");
    }
}
