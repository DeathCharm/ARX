using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Highlights this object's name in the Unity Editor Hierarchy View
/// </summary>
public class ARX_Script_EditorHighlighter : MonoBehaviour
{
    //Intentionally sparse
    //This script is executed by its Editor Script

    public bool mb_highlightOn = true;
    public Color mo_color= new Color(0,50,0,0.15F);
}
