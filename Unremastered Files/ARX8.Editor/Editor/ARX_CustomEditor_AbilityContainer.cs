using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using ARX;

[CustomEditor(typeof(ARX_Script_Debug_AbilityContainer))]
public class ARX_CustomEditor_AbilityContainer : Editor
{
    ARX_Script_Debug_AbilityContainer mo_container = null;

    ARX_Script_Debug_AbilityContainer AbilityContainer
    {
        get
        {
            if (mo_container == null)
                mo_container = (ARX_Script_Debug_AbilityContainer)target;
            return mo_container;
        }
    }

    List<ARX_Script_Debug_Ability> Abilities
    {
        get
        {
            return AbilityContainer.AbilityList;
        }
    }

    ARX_Script_Debug_Ability mo_chosenAbility = null;

    float STATBOXHEIGHT = 300, ABILITYLISTHEIGHT = 400;

    Rect GetRect
    {
        get
        {
            float nfTotalHeight = ABILITYLISTHEIGHT;
            if (mo_chosenAbility != null)
                nfTotalHeight += STATBOXHEIGHT;

            return GUILayoutUtility.GetRect(300, nfTotalHeight);
        }
    }

    void OpenMasterListWindow()
    {

    }

    public override void OnInspectorGUI()
    {
        RectGuide oGuide = new RectGuide(GetRect, 16);

        ARX.EditorDraw.DrawAbilityList(oGuide, mo_chosenAbility, AbilityContainer);
        if (GUI.Button(oGuide.GetNextRect(150, 32), "Add From Master List"))
        {
            OpenMasterListWindow();
        }
        oGuide.NewLine(2);

        if (mo_chosenAbility != null)
        {
            GUI.Label(oGuide.GetNextRect(150, 16), "Ability: " + mo_chosenAbility.mstr_name);
            oGuide.NewLine();
            ARX.EditorDraw.DrawStatBox(oGuide, mo_chosenAbility.mo_stats, 300);
        }
    }

}

