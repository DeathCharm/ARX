using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;


//[CustomEditor(typeof(ARX_Asset_Algorithm))]
//public class ARX_CustomEditor_Algorithm : Editor
//{
//    public AlgorithmViewer mo_viewer;
//    Rect GetRect
//    {
//        get
//        {
//            if (mo_viewer == null)
//                return GUILayoutUtility.GetRect(350, 200);
//            else
//                return GUILayoutUtility.GetRect(300, mo_viewer.GetHeight(GetTarget.AlgorithmPieces));
//        }
//    }

//    public ARX_Asset_Algorithm GetTarget { get { return (ARX_Asset_Algorithm)target; } }

    
//    string[] astrChoices = new string[0];
//    string[] GetAlgorithmClasses
//    {
//        get
//        {
//            if (astrChoices == null || astrChoices.Length == 0)
//            {
//                List<Type> buf = TypeEnumerator.GetTypes<ARX_AlgorithmAction>();
//                List<string> astrReturn = new List<string>();
//                astrReturn.Add(" - Choose Action");
//                foreach (Type t in buf)
//                    astrReturn.Add(t.ToString());
//                astrChoices = astrReturn.ToArray();
//            }
//            return astrChoices;
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        RectGuide oGuide = new RectGuide(GetRect, 16);

//        if (mo_viewer == null)
//            mo_viewer = new AlgorithmViewer((ARX_Asset_Algorithm)target);



//        //Choose a base Algorithm Piece

//        int n = 0;

//        n = EditorGUI.Popup(oGuide.GetNextRect(150, 16), 0, GetAlgorithmClasses);
//        oGuide.NewLine();

//        if (n > 0)
//        {
//            GetTarget.Add(GetAlgorithmClasses[n]);
//        }

//        mo_viewer.Draw(oGuide, GetTarget.AlgorithmPieces);

//        if (GUI.Button(oGuide.GetNextRect(100, 40), "Run Algorithms"))
//            GetTarget.RunAlgorithm();
//        EditorUtility.SetDirty(GetTarget);
//    }

//}

