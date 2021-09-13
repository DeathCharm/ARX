using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ARX
{
    public class ARX_EditorWindow : EditorWindow
    {

        /// <summary>
        /// A preview game object created when this window is opened and destroyed when it is closed.
        /// </summary>
        public static GameObject mo_preview = null;

        /// <summary>
        /// Shows the window when called. Can be called from the unity hierarchy.
        /// </summary>
        //[MenuItem("GameObject/ARX/Example Editor Window", false, 0)]
        public static void ShowWindow()
        {
            EditorWindow oWindow = EditorWindow.GetWindowWithRect<ARX_EditorWindow>(GetRect);
            oWindow.titleContent = new GUIContent("Example Window");
            InitializePreview();
        }

        /// <summary>
        /// Returns a rect that will serve as the Window's dimensions and position
        /// </summary>
        public static Rect GetRect
        {
            get
            {
                return new Rect(0, 0, 800, 800);
            }
        }

        /// <summary>
        /// Creates a preview game object when this window is opened.
        /// </summary>
        public static void InitializePreview()
        {
            if (mo_preview == null)
                mo_preview = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        private void OnDisable()
        {
            DestroyImmediate(mo_preview);
        }

        private void OnFocus()
        {
            InitializePreview();
        }

        private void OnGUI()
        {

        }

    }
}