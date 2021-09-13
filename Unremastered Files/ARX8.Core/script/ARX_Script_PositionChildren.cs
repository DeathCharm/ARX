using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Positions the children of this object in a line
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_PositionChildren : MonoBehaviour
{
    public Vector3 mvec_positionOffset;
    public int mn_perRow = 1;
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int nRow = i % mn_perRow;
            int nColumn = i / mn_perRow;
            int nStack = 0;
            transform.GetChild(i).localPosition = new Vector3(mvec_positionOffset.x * nRow, mvec_positionOffset.y * nStack, mvec_positionOffset.z * nColumn);
        }
    }
}
