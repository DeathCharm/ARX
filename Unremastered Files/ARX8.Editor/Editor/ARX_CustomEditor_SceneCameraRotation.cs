using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace ARX
{

    [CustomEditor(typeof(ARX_Script_SceneCameraRotation))]
    public class ARX_CustomEditor_SceneCameraRotation : Editor
    {

        ARX_Script_SceneCameraRotation GetTarget
        {
            get
            {
                return (ARX_Script_SceneCameraRotation)target;
            }
        }
        
        public void MoveSceneViewCamera()
        {
            if (SceneView.lastActiveSceneView == null)
                return;

            Camera cam = SceneView.lastActiveSceneView.camera;

            if (cam == null)
                return;

            Debug.Log("Changing scene view to " + GetTarget.mvec_sceneCameraRotation);
            SceneView.lastActiveSceneView.rotation = Quaternion.Euler(GetTarget.mvec_sceneCameraRotation);
        }
        private void OnSceneGUI()
        {
            if (GetTarget.mb_rotateCamera == true)
            {
                MoveSceneViewCamera();
                GetTarget.mb_rotateCamera = false;
            }

            
        }
        

    }
}
