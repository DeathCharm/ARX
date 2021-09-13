using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;

public class ARX_EventViewer : ARX_IEnumerableViewer<ARX_Event>
{
    
    public override bool V_DrawNewItemButton(RectGuide oGuide, bool bIsTopDrawn)
    {
        //Add Event to Callback List
        if(bIsTopDrawn)
        {
            Rect rectSelectEvent = oGuide.GetNextRect(mn_slideWidth);
            ARX_Event oItem = EditorGUI.ObjectField(rectSelectEvent, "", null, typeof(ARX_Event), false) as ARX_Event;
            if (oItem != null && moa_data.Contains(oItem) == false)
                moa_data.Add(oItem);
            oGuide.NewLine();
        }
        return false;
    }

    public override void I_DrawUnique(RectGuide oGuide, ARX_Event oItem, int nIndex, List<ARX_Event> oList)
    {
        EditorGUI.ObjectField(oGuide.GetNextRect(mn_slideWidth), "", oItem, typeof(ARX_Event), false);
    }

    public override void I_ClickNewItem(List<ARX_Event> moa_list)
    {

    }

    public override void I_DrawName(RectGuide oGuide, ARX_Event oItem, int nIndex, List<ARX_Event> oList)
    {

    }

    public override bool I_Equal(ARX_Event one, ARX_Event two)
    {
        return one == two;
    }

    public override void I_ReactToDeleteButtonClickedForItem(ARX_Event oItem, int nIndex)
    {
        moa_data.Remove(oItem);
    }
}
