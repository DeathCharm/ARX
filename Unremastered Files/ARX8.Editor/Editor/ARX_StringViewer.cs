using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using ARX;

public class StringViewer : ARX_IEnumerableViewer<string>
{
    public override bool V_DrawNewItemButton(RectGuide oGuide, bool bIsTopDrawn)
    {
        oGuide.NewLine();
        return base.V_DrawNewItemButton(oGuide, bIsTopDrawn);
    }
    public override void I_ClickNewItem(List<string> oList)
    {
        oList.Add("New Item");

    }
    
    public override void I_DrawName(RectGuide oGuide, string oItem, int nIndex, List<string> oList)
    {

    }

    public override void I_ReactToDeleteButtonClickedForItem(string oItem, int nIndex)
    {

    }


    public override void I_DrawUnique(RectGuide oGuide, string oItem, int nIndex, List<string> oList)
    {
        oList[nIndex] = GUI.TextField(oGuide.GetNextRect(mn_slideWidth, 16), oList[nIndex]);
    }

    public override bool I_Equal(string one, string two)
    {
        return one == two;
    }

    public override bool V_DrawDeleteButton(RectGuide oGuide)
    {
        return base.V_DrawDeleteButton(oGuide);
    }

}

