using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;

//[CustomEditor(typeof(ARX_AttributeList))]
//public class ARX_CustomEditor_AttributeList : Editor
//{
//    string mstr_output = "";
//    StringViewer mo_viewer;

//    Rect GetRect { get { return GUILayoutUtility.GetRect(300, 800); } }

//    ARX_AttributeList GetTarget { get { return (ARX_AttributeList)target; } }

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        if (mo_viewer == null)
//            mo_viewer = new StringViewer();

//        RectGuide oGuide = new RectGuide(GetRect);
//        mo_viewer.V_Draw(oGuide, GetTarget.Attributes);

//        oGuide.NewLine(3);

//        mstr_output = FEN.AutoCodeCalculations.GetAttributeListFIleOutput(GetTarget.Attributes);
//        GUI.TextArea(oGuide.GetNextRect(300, 300), mstr_output);

//    }


//}
