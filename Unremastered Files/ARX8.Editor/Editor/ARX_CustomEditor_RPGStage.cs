using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ARX_Script_RPGStage)), CanEditMultipleObjects]
public class ARX_CustomEditor_RPGStage : Editor
{
    
    ARX_Script_RPGStage GetTarget { get { return (ARX_Script_RPGStage)target; } }
    
    private void OnSceneGUI()
    {
        foreach (ARX_Script_RPGStage.StageAnchor oPos in GetTarget.GetGizmosToDraw)
            DrawSavedPositionGizmo(oPos);
    }

    public void DrawSavedPositionGizmo(ARX_Script_RPGStage.StageAnchor oPos)
    {
        EditorGUI.BeginChangeCheck();

        //Draw on the stage
        Vector3 vecChange = Handles.DoPositionHandle(GetTarget.GetPositionOnStage(oPos), Quaternion.identity);
        Handles.Label(GetTarget.GetPositionOnStage(oPos), oPos.mstr_name);

        //Draw on the canvas
        Vector3 vecCanvasWorldPosition = GetTarget.GetPositionOnCanvas(oPos);
        Handles.Label(vecCanvasWorldPosition, oPos.mstr_name);

        if (EditorGUI.EndChangeCheck())
        {
            oPos.SetStagePositionByWorldPosition(vecChange, GetTarget);
        }
        
    }

}
