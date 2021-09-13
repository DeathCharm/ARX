using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FEN;
using ARX;

/// <summary>
/// Another debug script used as Scratch Paper
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_Scratch2 : MonoBehaviour
{
    [TextArea(2,5)]
    public string strPosition = "";

    private void Update()
    {
        strPosition = transform.position.ToString();
    }

}
