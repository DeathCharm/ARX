using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;

public class RPGStage_AbilitySlide : RPGStage_UIElement
{
    /// <summary>
    /// If this ability is not null, it will be drawn instead of the root element's
    /// ability
    /// </summary>
    [HideInInspector]
    public FEN_Ability mo_overwrittenAbiity = null;

    public override void I_DrawElement(RPGStage_ElementContainer elementContainer)
    {
        if (RootElement.Ability == null)
        {
            I_DrawBlank();
            return;
        }

        FEN_Ability oAbilityToDraw = null;

        //
        if (mo_overwrittenAbiity != null)
            oAbilityToDraw = mo_overwrittenAbiity;
        else
            oAbilityToDraw = RootElement.Ability;

        Title = oAbilityToDraw.name;
        Description = oAbilityToDraw.Description;
        Info = oAbilityToDraw.mstr_iconTooltip;
        Icon = oAbilityToDraw.mo_iconSprite;
    }
    
}
