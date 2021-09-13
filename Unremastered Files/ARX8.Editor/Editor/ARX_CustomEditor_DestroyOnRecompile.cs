using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ARX_CustomEditor_DestroyOnRecompile))]
public class ARX_CustomEditor_DestroyOnRecompile : Editor
{

    Rect GetRect
    {
        get
        {
            return GUILayoutUtility.GetRect(300, 50);
        }
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    public static void DestroyAllOnRecompile()
    {
        ARX_DestroyOnRecompile[] oaReloaded = FindObjectsOfType<ARX_DestroyOnRecompile>();
        foreach (ARX_DestroyOnRecompile o in oaReloaded)
            GameObject.DestroyImmediate(o.gameObject);
    }

    public override void OnInspectorGUI()
    {
        GUI.Label(GetRect, "CAUTION: Any object with this script will be deleted upon \n script recompilation.");
    }

}

