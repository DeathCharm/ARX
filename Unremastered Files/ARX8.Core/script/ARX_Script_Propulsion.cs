using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves an object towards a target position each frame.
/// This script is in need of expansion/rework
/// </summary>
public class ARX_Script_Propulsion : MonoBehaviour
{

    public enum DRAGTYPE { SLERP=0, LERP=1, FLAT = 2 }
    public DRAGTYPE me_dragType = DRAGTYPE.LERP;
    
    public bool mb_rotate = true;
    public Vector3 mvec_targetPosition;
    public Vector3 GetTargetPosition
    {
        get
        {
            //if (mo_target != null)
                //mvec_targetPosition = mo_target.transform.position; 
            return mvec_targetPosition;
        }
    }
    

    public float mnf_drag = 0.2F;



    public void Update()
    {
            DragTarget();
        
    }
    
    void DragTarget()
    {
        Vector3 vecNewPosition = new Vector3();
        Vector3 vecNewRotation = new Vector3();

        switch (me_dragType)
        {
            case DRAGTYPE.FLAT:
                vecNewPosition = Vector3.MoveTowards(GetTargetPosition, transform.position, mnf_drag * 0.1F);
                if (mb_rotate)
                {
                    //vecNewRotation = Vector3.RotateTowards(mo_target.transform.eulerAngles, transform.eulerAngles, 0.1F, 0.1F);
                }
                break;
            case DRAGTYPE.SLERP:
                vecNewPosition = Vector3.Slerp(GetTargetPosition, transform.position, Time.deltaTime * mnf_drag);
                if (mb_rotate)
                {
                    //vecNewRotation = Vector3.Slerp(mo_target.transform.eulerAngles, transform.eulerAngles, 0.1F);
                }
                break;
            case DRAGTYPE.LERP:
                vecNewPosition = Vector3.Lerp(GetTargetPosition, transform.position, Time.deltaTime * mnf_drag);
                if (mb_rotate)
                {
                    //vecNewRotation = Vector3.Lerp(mo_target.transform.eulerAngles, transform.eulerAngles, 0.1F);
                }
                break;
        }
        transform.position = vecNewPosition;
        transform.eulerAngles = vecNewRotation;
    }

}
