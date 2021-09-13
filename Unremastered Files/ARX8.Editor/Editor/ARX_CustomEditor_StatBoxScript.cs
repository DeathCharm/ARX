using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using ARX;

[CustomEditor(typeof(ARX_Script_StatBox))]
    public class ARX_CustomEditor_StatBoxScript : Editor
    {
    
    public ARX_StatBox_Quad StatBox
    {
        get
        {
                ARX_Script_StatBox script = (ARX_Script_StatBox)target;
                return script.mo_stats;
        }
    }

    Rect GetRect
    {
        get
        {
            return GUILayoutUtility.GetRect(300, StatBox.Count * 16 + 96);
        }
    }

    public override void OnInspectorGUI()
    {
        RectGuide oGuide = new RectGuide(GetRect, 16);
        ARX.EditorDraw.DrawStatBox(oGuide, StatBox, 300, false);
    }

    private void OnDestroy()
    {

        Debug.Log("OnDestroy Statboxscript editor");
    }

    void Close()
    {
        Debug.Log("Saving Assets");
        AssetDatabase.SaveAssets();
    }
}

