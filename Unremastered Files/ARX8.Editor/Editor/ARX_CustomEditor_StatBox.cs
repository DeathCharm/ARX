using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using ARX;

[CustomEditor(typeof(ARX_StatBox_Quad))]
    public class ARX_CustomEditor_StatBox : Editor
    {

    public string mstr_serialized = "";

    private void OnDisable()
    {
        EditorUtility.SetDirty(GetTarget);
        AssetDatabase.SaveAssets();
    }
    

    Rect GetRect
    {
        get
        {
            int nHeight = GetTarget.Count * 16 + 496;
            return GUILayoutUtility.GetRect(300, nHeight);
        }
    }

    ARX_StatBox_Quad GetTarget
    {
        get
        {
            return (ARX_StatBox_Quad)target;
        }
    }

    void LoadSerialization()
    {
        try
        {
            ARX_StatBox_Quad statBox = ARX_File.DeserializeObject<ARX_StatBox_Quad>(mstr_serialized);
            GetTarget.SetStatBoxes(statBox.AsList);
        }
        catch
        {

        }
    }

    public override void OnInspectorGUI()
    {
        RectGuide oGuide = new RectGuide(GetRect, 16);
        ARX.EditorDraw.DrawStatBox(oGuide, GetTarget, 300, false);

        oGuide.NewLine();


        if (GUI.Button(oGuide.GetNextRect(150), "Serialize"))
            mstr_serialized = GetTarget.GetSerializedString();

        if (GUI.Button(oGuide.GetNextRect(150), "Load"))
            LoadSerialization();

        oGuide.NewLine();
        GUI.TextArea(oGuide.GetNextRect(300, 300), mstr_serialized);
        //if (GUI.changed)
        //{
        //    EditorUtility.SetDirty(GetTarget);
        //}
    }
}
