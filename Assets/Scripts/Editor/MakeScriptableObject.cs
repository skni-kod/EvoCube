using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject
{
    [MenuItem("Assets/Create/Perlin2dSettings")]
    public static void CreateMyAsset()
    {
        Perlin2dSettings asset = ScriptableObject.CreateInstance<Perlin2dSettings>();

        AssetDatabase.CreateAsset(asset, "Assets/Perlin2dSettings.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }


}
