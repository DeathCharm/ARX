using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for FUM Editor Painter. Paint objects onto the scene in the Unity Editor. This class is intentionally blank, as it is only used for
/// its custom editor.
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_PaintObjectToScene : MonoBehaviour {

    #region Variables
    [HideInInspector]
    public float offset;

    [HideInInspector]
    public Transform mo_prefabToInstantiate;

    [HideInInspector]
    public Transform mo_parentObject;

    [HideInInspector]
    public Vector3 randomTiltRange;

    [HideInInspector]
    public Vector3 minScaleRange = Vector3.one, maxScaleRange = Vector3.one;

    [HideInInspector]
    public bool bNormalizeRotation = false;

    [HideInInspector]
    public Vector3 normalRotation;

    [HideInInspector]
    public System.Random rand = new System.Random();

    public bool mb_overrideRotationX, mb_overrideRotationY, mb_overrideRotationZ;
    public Vector3 mvec_overrideRotation;
    #endregion

    #region Functions
    public void GrantRandomScale(Transform obj)
    {
        rand = new System.Random((int)System.DateTime.Now.Ticks);
        Vector3 bufMin, bufMax;

        if (minScaleRange == Vector3.zero)
            bufMin = new Vector3(1, 1, 1);
        else
            bufMin = minScaleRange * 100;

        if (maxScaleRange == Vector3.zero)
            bufMax = new Vector3(1, 1, 1);
        else
            bufMax = maxScaleRange * 100;

        int x = rand.Next((int)bufMin.x, (int)bufMax.x);
        int y = rand.Next((int)bufMin.y, (int)bufMax.y);
        int z = rand.Next((int)bufMin.z, (int)bufMax.z);

        float fx = (float)x / 100F;
        float fy = (float)y / 100F;
        float fz = (float)z / 100F;


        obj.localScale = new Vector3(fx, fy, fz);
    }
    
    public void GrantRandomRotation(Transform obj)
    {
        rand = new System.Random((int)System.DateTime.Now.Ticks);
        float x = 0;
        float y = 0;
        float z = 0;

        if (randomTiltRange.x != 0)
            x = (float)(rand.Next() % (int)randomTiltRange.x);
        if (randomTiltRange.y != 0)
            y = (float)(rand.Next() % (int)randomTiltRange.y);
        if (randomTiltRange.z != 0)
            z = (float)(rand.Next() % (int)randomTiltRange.z);
        obj.Rotate(new Vector3(x, y, z));
    }
    #endregion
}
