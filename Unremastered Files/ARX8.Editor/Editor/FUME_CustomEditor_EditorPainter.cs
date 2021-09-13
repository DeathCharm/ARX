using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using ARX;

[CustomEditor(typeof(ARX_Script_PaintObjectToScene))]
[ExecuteInEditMode]
    public class FUME_Editor_EditorPainter : Editor
    {

    public ARX_Script_PaintObjectToScene GetTarget
    {
        get
        {
            return (ARX_Script_PaintObjectToScene)target;
        }
    }

    public Rect GetRect { get
        {
            return GUILayoutUtility.GetRect(325, 310);
        } }

    public void OnSceneGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Backslash)
        {
            PaintObject(GetTarget.mo_prefabToInstantiate);
        }
    }

    public override void OnInspectorGUI()
    {
        Rect rectGui = GetRect;
        

        RectGuide rectGuide = new RectGuide(rectGui, 16, false);
        
        rectGuide.MoveBoundingRect(15,75);
        //serializedObject.Update();

        //Instructions
        base.OnInspectorGUI();
        DrawEditorPainter(rectGuide, KeyCode.Backslash);

        //serializedObject.ApplyModifiedProperties();
    
}
    
    public void DrawEditorPainter(RectGuide rectGuide, KeyCode ePaintKey)
    {
        GUI.Label(rectGuide.GetNextRect(150, 16), "Editor Paint");
        rectGuide.NewLine();

        EditorGUI.TextArea(rectGuide.GetNextRect(300, 58), "Instructions: Add an EditorPainter script to \nscene, select it, then activate the Scene View \nWITH RIGHT CLICK and press the '\\' key to\n paint to the scene.");
        rectGuide.NewLine(4);

        DrawPrefabStats(rectGuide);

    }
    
    public bool IsKeyPressed(KeyCode eCode)
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == eCode)
            return true;
        return false;
    }

    void DrawPrefabStats(RectGuide rectGuide)
    {
        //Set the stats of the prefab tha will be spawned

        //Random Tilt Range
        GetTarget.randomTiltRange = ARX.EditorDraw.DrawScalingSection(rectGuide, 100, "Random Tilt", GetTarget.randomTiltRange);
        rectGuide.NewLine();

        //Min and Max Random Scale
        GetTarget.minScaleRange = ARX.EditorDraw.DrawScalingSection(rectGuide, 100, "Minimum Scale", GetTarget.minScaleRange);
        rectGuide.NewLine();
        GetTarget.maxScaleRange = ARX.EditorDraw.DrawScalingSection(rectGuide, 100, "Maximum Scale", GetTarget.maxScaleRange);
        rectGuide.NewLine(2);

        //Normalize rotation?
        GetTarget.bNormalizeRotation = GUI.Toggle(rectGuide.GetNextRect(16, 16), GetTarget.bNormalizeRotation, "");
        GUI.Label(rectGuide.GetNextRect(134, 16), "Normalize Rotation?");
        rectGuide.NewLine();

        //Starting Rotation if normalized
        if (GetTarget.bNormalizeRotation)
        {
            GetTarget.normalRotation = ARX.EditorDraw.DrawScalingSection(rectGuide, 150, "Normal Rotation", GetTarget.normalRotation);
            rectGuide.NewLine();
        }
        else
            rectGuide.NewLine();

        //Prefab to Instantiate
        GUI.Label(rectGuide.GetNextRect(150, 16), "Prefab to Instantiate");
        GetTarget.mo_prefabToInstantiate = (Transform)EditorGUI.ObjectField(rectGuide.GetNextRect(150, 16),
            GetTarget.mo_prefabToInstantiate, typeof(Transform), true);
        rectGuide.NewLine();


        //Gameobject to parent
        GUI.Label(rectGuide.GetNextRect(150, 16), "Parent");
        GetTarget.mo_parentObject = (Transform)EditorGUI.ObjectField(rectGuide.GetNextRect(150, 16),
            GetTarget.mo_parentObject, typeof(Transform), true);
        rectGuide.NewLine();

        //Position Offset when instantiated
        GUI.Label(rectGuide.GetNextRect(150, 16), "Vertical Offset");
        GetTarget.offset = EditorGUI.FloatField(rectGuide.GetNextRect(150, 16), GetTarget.offset);
        rectGuide.NewLine();
    }

    public Vector2 DrawPaintPallete(RectGuide rectGuide, Vector2 vecScrollPosition)
    {
        float nfRectHeight = 1000;

        Rect rectOuter = rectGuide.GetNextRect(500, 300);
        Rect rectInner = new Rect(0, 0, 500, nfRectHeight);

        ARX.EditorDraw.DrawQuad(rectOuter, Color.gray);

        vecScrollPosition = GUI.BeginScrollView(rectOuter, vecScrollPosition,
            rectInner);


        GUI.Label(rectGuide.GetNextRect(100, 16), "Hello! The paint pallete section isn't implemented yet.");

        GUI.EndScrollView();

        return vecScrollPosition;

    }

    public void PaintObject(Transform obj = null)
    {
        //Get event and cast a ray to determine world point clicked
        Event e = Event.current;

        //Vector2 vecMouseScreenPosition = GUIUtility.GUIToScreenPoint(e.mousePosition);

        //EditorWindow editorWindow = UnityEditor.EditorWindow.GetWindow<SceneView>();
        //vecMouseScreenPosition -= editorWindow.position.position;
        //Debug.Log("Vec Pos: " + editorWindow.position.position);



        //Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(vecMouseScreenPosition);

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        //Debug.Log("Direction: " + ray.direction + " Mouse Position: " + vecMouseScreenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 10);

        if (obj == null && GetTarget.mo_prefabToInstantiate == null)
        {
            Debug.LogError("You forgot to assign a prefab to PaintWindow/Prefab!");
            return;
        }

        RaycastHit hit = new RaycastHit();

        //If the ray hit
        if (Physics.Raycast(ray, out hit))
        {
            //Instantiate Prefab
            if (obj == null)
            {
                obj = (Transform)GameObject.Instantiate(
                GetTarget.mo_prefabToInstantiate,
                hit.point + new Vector3(0, GetTarget.offset, 0),
                Quaternion.LookRotation(hit.point.normalized, hit.normal));
            }
            else
            {
                obj = GameObject.Instantiate(obj).transform;
                obj.transform.position = hit.point + new Vector3(0, GetTarget.offset, 0);
                Debug.Log("Impact at " + hit.point);
                obj.transform.rotation = Quaternion.LookRotation(hit.point.normalized, hit.normal);
            }


            //If Rotation is to be random
            if (GetTarget.bNormalizeRotation == false)
            {
                GetTarget.GrantRandomRotation(obj);
                GetTarget.GrantRandomScale(obj);
            }
            //Else if rotation is to be partially random
            else
            {
                obj.eulerAngles = GetTarget.normalRotation;
                GetTarget.GrantRandomRotation(obj);
                GetTarget.GrantRandomScale(obj);
            }

            Vector3 vec = obj.transform.eulerAngles;
            if (GetTarget.mb_overrideRotationX)
            {
                vec.x = GetTarget.mvec_overrideRotation.x;
            }
            if (GetTarget.mb_overrideRotationY)
            {
                vec.y = GetTarget.mvec_overrideRotation.y;
            }
            if (GetTarget.mb_overrideRotationZ)
            {
                vec.z = GetTarget.mvec_overrideRotation.z;
            }
            obj.transform.eulerAngles = vec;

            //Set the parent of the instantiated prefab
            if (GetTarget.mo_parentObject != null)
                obj.SetParent(GetTarget.mo_parentObject);
        }
    }


}

