using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ARX;

namespace ARX
{
    #region Viewer Class for Camera Lists
    /// <summary>
    /// Viewer Class for Camera Lists
    /// </summary>
    public class CameraDepthListViewer : ARX_IEnumerableViewer<Camera>
    {
        public override bool V_DrawNewItemButton(RectGuide oGuide, bool bIsTopDrawn)
        {
            //Intentionally blank
            return false;
        }

        public override void I_ReactToDeleteButtonClickedForItem(Camera oItem, int nIndex)
        {

        }

        public override void I_DrawName(RectGuide oGuide, Camera oItem, int nIndex, List<Camera> oList)
        {
            GUI.Label(oGuide.GetNextRect(100, 16), oItem.name);
        }

        public override void I_ClickNewItem(List<Camera> moa_list)
        {

        }

        public override void I_DrawUnique(RectGuide oGuide, Camera oItem, int nIndex, List<Camera> oList)
        {
            //Depth
            oItem.depth = EditorGUI.FloatField(oGuide.GetNextRect(50,16),  oItem.depth);
        }

        public override bool I_Equal(Camera one, Camera two)
        {
            return one == two;
        }
    }
    #endregion

    [CustomEditor(typeof(ARX_Script_CameraDepthList))]
    class ARX_CustomEditor_CameraDepthList : Editor
    {
        /// <summary>
        /// Returns the rect in which the cmaera list is drawn.
        /// </summary>
        Rect GetEditorRect
        {
            get
            {
                float nfHeight = GetTarget.GetSortedCameraList().Count * 16;
                if (nfHeight < 100)
                    nfHeight = 100;
                return GUILayoutUtility.GetRect(300, nfHeight);
            }
        }

        /// <summary>
        /// Return the Depth List script that this editor points to
        /// </summary>
        ARX_Script_CameraDepthList GetTarget { get { return (ARX_Script_CameraDepthList)target; } }


        public override void OnInspectorGUI()
        {
            CameraDepthListViewer oViewer = new CameraDepthListViewer();
            RectGuide oGuide = new RectGuide(GetEditorRect, 16);

            oViewer.V_Draw(oGuide,  GetTarget.GetSortedCameraList());
        }
    }
}
