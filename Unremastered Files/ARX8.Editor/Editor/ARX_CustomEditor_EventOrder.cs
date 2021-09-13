using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;

[CustomEditor(typeof(ARX_EventOrder))]
public class ARX_CustomEditor_EventOrder : Editor {

    EventOrderViewer mo_viewer = null;

    ARX_EventOrder GetTarget
    {
        get
        {
            return (ARX_EventOrder)target;
        }
    }

    Rect GetRect
    {
        get
        {
            return GUILayoutUtility.GetRect(300, 700);
        }
    }


    void Close()
    {
        AssetDatabase.SaveAssets();
    }

    public override void OnInspectorGUI()
    {
        if (mo_viewer == null)
            mo_viewer = new EventOrderViewer(GetTarget);

        RectGuide oGuide = new RectGuide(GetRect, 16);

        //Title
        GUI.Label(oGuide.GetNextRect(150, 16), "Event Order");
        oGuide.NewLine();


        //Name
        GUI.Label(oGuide.GetNextRect(150, 16), "Event Name");
        GetTarget.name = GUI.TextField(oGuide.GetNextRect(150, 16), GetTarget.name);
        oGuide.NewLine(2);
        
        
        //Process Viewer
        GUI.Label(oGuide.GetNextRect(150, 16), "Process Order");
        oGuide.NewLine();
        mo_viewer.V_Draw(oGuide, GetTarget.moa_processNames);
    }



}
