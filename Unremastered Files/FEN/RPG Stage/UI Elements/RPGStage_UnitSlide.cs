using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;
using ARX;


public class RPGStage_UnitSlide : RPGStage_UIElement
{
    public override void I_DrawElement(RPGStage_ElementContainer elementContainer)
    {
        if (RootElement.Unit == null)
        {
            I_DrawBlank();
            return;
        }

        FEN_Unit ability = RootElement.Unit;

        Title = ability.name;
        Icon = ability.mo_sprite;
    }
}
