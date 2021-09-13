using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ARX
{
    //[CustomEditor(typeof(ARX_Script_Actor))]
    /// <summary>
    /// Just a throwaway class meant to be copied when making another Custom Editor
    /// </summary>
    public class ARX_CustomEditor : UnityEditor.Editor
    {
        Rect GetRect
        {
            get
            {
                return GUILayoutUtility.GetRect(300, 200);
            }
        }


        public override void OnInspectorGUI()
        {

        }

    }
}

