using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ARX
{
    public class SavedPositionViewer : ARX_IEnumerableViewer<ARX_Script_SavedPositions.SavedPosition>
    {
        public ARX_Script_SavedPositions mo_script;

        public override bool V_DrawDeleteButton(RectGuide oGuide)
        {
            bool b = GUI.Button(oGuide.GetNextRect(100, 32), "Delete");
            oGuide.NewLine(2);
            return b;
        }

        public override void I_ReactToDeleteButtonClickedForItem(ARX_Script_SavedPositions.SavedPosition oItem, int nIndex)
        {
            moa_data.Remove(oItem);
        }

        public override void I_DrawName(RectGuide oGuide, ARX_Script_SavedPositions.SavedPosition oItem, int nIndex, List<ARX_Script_SavedPositions.SavedPosition> oList)
        {
            GUI.Label(oGuide.GetNextRect(40, 16), "Name");
            oItem.name = GUI.TextField(oGuide.GetNextRect(230), oItem.name);
            oGuide.NewLine();
        }

        public override Rect V_DrawBackground(ARX_Script_SavedPositions.SavedPosition oItem, RectGuide oGuide, int nIndex)
        {
            #region Draw Background
            Color oColor;
            if (nIndex % 2 == 0)
                oColor = new Color(0.18F, 0.18F, 0.22F);
            else
                oColor = new Color(0.15F, 0.15F, 0.22F);
            Rect backgroundRect = oGuide.PeekNextRect(350, 220);
            backgroundRect.x -= 40;
            EditorGUI.DrawRect(backgroundRect, oColor);
            #endregion

            return backgroundRect;
        }

        public override void I_DrawUnique(RectGuide oGuide, ARX_Script_SavedPositions.SavedPosition oItem, int nIndex, List<ARX_Script_SavedPositions.SavedPosition> oList)
        {
            #region Variables
            const int nButtonHeight = 20;
            const int nButtonWidth_Small = 100;
            const int nButtonWidth_Large = 150;
            const int nField_Large = 230;

            Rect oTopSection = oGuide.GetNextRect(260, 32);
            oGuide.NewLine(3);
            Rect oBottomSection = oGuide.GetNextRect(260, 40);
            RectGuide oTopGuide = new RectGuide(oTopSection);
            RectGuide oBottomGuide = new RectGuide(oBottomSection, nButtonHeight);
            
            #endregion

            #region Top Section
            GUI.Label(oTopGuide.GetNextRect(50), "Position");
            oTopGuide.MoveLastRect(16);
            oItem.targetPosition = EditorGUI.Vector3Field(oTopGuide.GetNextRect(nField_Large, 32), "", oItem.targetPosition);
            oTopGuide.NewLine();

            GUI.Label(oTopGuide.GetNextRect(50), "Rotation");
            oTopGuide.MoveLastRect(16);
            oItem.eulerAngles = EditorGUI.Vector3Field(oTopGuide.GetNextRect(nField_Large, 32), "", oItem.eulerAngles);
            oTopGuide.NewLine();

            GUI.Label(oTopGuide.GetNextRect(50), "Scale");
            oTopGuide.MoveLastRect(16);
            oItem.targetScale = EditorGUI.Vector3Field(oTopGuide.GetNextRect(nField_Large, 32), "", oItem.targetScale);
            #endregion

            #region Bottom Section

            #region GUIContents
            GUIContent moveContent = new GUIContent();
            moveContent.text = "Move";
            moveContent.tooltip = "Move object to position " + oItem.targetPosition;

            GUIContent rotateContent = new GUIContent();
            rotateContent.text = "Rotate";
            rotateContent.tooltip = "Rotate object to " + oItem.eulerAngles;

            GUIContent scaleContent = new GUIContent();
            scaleContent.text = "Scale";
            scaleContent.tooltip = "Scale object to " + oItem.targetScale;

            GUIContent recordPosContent = new GUIContent();
            recordPosContent.text = "Record Pos";
            recordPosContent.tooltip = "Record this object's current position as " + oItem.name;

            GUIContent recordRotContent = new GUIContent();
            recordRotContent.text = "Record Rotation";
            recordRotContent.tooltip = "Record this object's current rotation as " + oItem.name;

            GUIContent recordScaleContent = new GUIContent();
            recordScaleContent.text = "Record Scale";
            recordScaleContent.tooltip = "Record this object's current scale as " + oItem.name;

            GUIContent moveToSceneCamPos = new GUIContent();
            moveToSceneCamPos.text = "Move to SceneCam";
            moveToSceneCamPos.tooltip = "Moves this object's position to the Scene Camera's position";

            GUIContent moveToSceneCamRot = new GUIContent();
            moveToSceneCamRot.text = "Rotate to SceneCam";
            moveToSceneCamRot.tooltip = "Sets this object's rotation to the Scene Caemra's rotation";
            #endregion

            #region Draw
            Camera oSceneCamera = null;

            if(SceneView.currentDrawingSceneView != null)
                oSceneCamera = SceneView.currentDrawingSceneView.camera;

            oBottomGuide.MoveBoundingRect(0, 10);
            //Move and Rotate and Scale
            if (GUI.Button(oBottomGuide.GetNextRect(nButtonWidth_Small, nButtonHeight), moveContent))
            {
                mo_script.MoveToPosition(oItem);
            }
            if (GUI.Button(oBottomGuide.GetNextRect(nButtonWidth_Small, nButtonHeight), rotateContent))
            {
                mo_script.RotateTo(oItem);
            }
            if (GUI.Button(oBottomGuide.GetNextRect(nButtonWidth_Small, nButtonHeight), scaleContent))
            {
                mo_script.ScaleTo(oItem);
            }
            oBottomGuide.NewLine();
            
            //Record Current Position and Rotation and Scale
            //Record Position
            if (GUI.Button(oBottomGuide.GetNextRect(nButtonWidth_Small, nButtonHeight), recordPosContent))
            {
                oItem.SetPosition(mo_script.gameObject.transform.position);
            }
            //Record Rotation
            if (GUI.Button(oBottomGuide.GetNextRect(nButtonWidth_Small, nButtonHeight), recordRotContent))
            {
                oItem.SetRotation(mo_script.gameObject.transform.eulerAngles);
            }
            //Record Scale
            if (GUI.Button(oBottomGuide.GetNextRect(nButtonWidth_Small, nButtonHeight), recordScaleContent))
            {
                oItem.SetScale(mo_script.gameObject.transform.localScale);
            }
            oBottomGuide.NewLine(2);

            //Move to Scene Position and rotation
            //Move to Scene Camera Position
            if (GUI.Button(oBottomGuide.GetNextRect(nButtonWidth_Large, nButtonHeight), moveToSceneCamPos))
            {
                mo_script.MoveToPosition(oSceneCamera.transform.position);
            }
            //Move to Scene Camera Rotation
            if (GUI.Button(oBottomGuide.GetNextRect(nButtonWidth_Large, nButtonHeight), moveToSceneCamRot))
            {
                mo_script.RotateTo(oSceneCamera.transform.eulerAngles);
            }
            oBottomGuide.NewLine();

            #endregion

            oGuide.NewLine(7);
            #endregion
        }

        public override void I_ClickNewItem(List<ARX_Script_SavedPositions.SavedPosition> moa_list)
        {
            moa_list.Add(new ARX_Script_SavedPositions.SavedPosition());
        }

        public override bool I_Equal(ARX_Script_SavedPositions.SavedPosition one, ARX_Script_SavedPositions.SavedPosition two)
        {
            return one == two;
        }

        public override bool V_DrawNewItemButton(RectGuide oGuide, bool bIsTopDrawn)
        {
            oGuide.NewLine();
            return GUI.Button(oGuide.GetNextRect(150, 32), "New Position");
        }
    }

    [CustomEditor(typeof(ARX_Script_SavedPositions))]
    public class ARX_CustomEditor_SavedPositions : Editor
    {

        public Rect GetRect
        {
            get
            {
                int nHeight = 190 + GetTarget.moa_savedPositions.Count * 228;
                Rect oRect = GUILayoutUtility.GetRect(300, nHeight);
                return oRect;
            }
        }

        public ARX_Script_SavedPositions GetTarget
        {
            get
            {
                return (ARX_Script_SavedPositions)target;
            }
        }

        private void OnDisable()
        {
            if(GetTarget != null)
                EditorUtility.SetDirty(GetTarget);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            RectGuide oGuide = new RectGuide(GetRect);

            #region GUI Contents
            GUIContent saveCurrent = new GUIContent();
            saveCurrent.text = "Save Current Position";
            saveCurrent.tooltip = "Create a new saved position with this object's current position";

            GUIContent saveAsVisible = new GUIContent();
            saveAsVisible.text = "Save Position as \"visible\"";
            saveCurrent.tooltip = "Create a new saved position named \"visible\" with this object's current position";

            GUIContent saveAsHidden = new GUIContent();
            saveAsHidden.text = "Save Position as \"hidden\"";
            saveCurrent.tooltip = "Create a new saved position name \"hidden\" with this object's current position";
            #endregion
            
            #region Draw Buttons
            if (GUI.Button(oGuide.GetNextRect(150, 32), saveCurrent))
            {
                GetTarget.SaveCurrentPosition();
            }

            oGuide.NewLine(2);

            if (GUI.Button(oGuide.GetNextRect(150, 32), saveAsVisible))
            {
                GetTarget.SaveCurrentPosition("visible");
            }

            if (GUI.Button(oGuide.GetNextRect(150, 32), saveAsHidden))
            {
                GetTarget.SaveCurrentPosition("hidden");
            }

            oGuide.NewLine(3);
            #endregion

            #region Draw Viewer
            SavedPositionViewer view = new SavedPositionViewer();
            view.mo_script = GetTarget;
            view.V_Draw(oGuide, GetTarget.moa_savedPositions);

            oGuide.NewLine(2);
            #endregion
            

        }

    }
}
