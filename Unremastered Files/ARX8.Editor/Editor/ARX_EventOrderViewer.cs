using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using ARX;

public class EventOrderViewer : StringViewer
{
    ARX_EventOrder mo_eventOrder = null;
    public EventOrderViewer(ARX_EventOrder order)
    {
        mo_eventOrder = order;
    }

    public override void I_DrawUnique(RectGuide oGuide, string oItem, int nIndex, List<string> oList)
    {
        base.I_DrawUnique(oGuide, oItem, nIndex, oList);
    }
}