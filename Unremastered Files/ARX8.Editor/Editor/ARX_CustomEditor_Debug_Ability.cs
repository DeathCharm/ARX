using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;

[CustomEditor(typeof(ARX_Script_Debug_Ability))]
public class ARX_CustomEditor_Debug_Ability : Editor
{
    ARX_Script_Debug_Ability mo_ability = null;

    ARX_Script_Debug_Ability Ability
    {
        get
        {
            if (mo_ability == null)
                mo_ability = (ARX_Script_Debug_Ability)target;
            return mo_ability;
        }
    }

    ARX_StatBox_Quad Stats
    {
        get
        {
            return Ability.mo_stats;
        }
    }
    

    Rect GetRect
    {
        get
        {
            float STATBOXHEIGHT = 400;
            float nfTotalHeight = 120 + Stats.Count * 16;
            return GUILayoutUtility.GetRect(STATBOXHEIGHT, nfTotalHeight);
        }
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RectGuide oGuide = new RectGuide(GetRect, 16);
        EditorDraw.DrawStatBox(oGuide, Stats, 400, false);


        //EditorGUI.TextArea(oGuide.GetNextRect(150), mstr_serialize)
        //oGuide.NewLine();
        //if(GUILayout.Button("Serialize"))
        //{
        //    mstr_serialize = Ability.GetSerializedString;
        //}
        
    }

}
