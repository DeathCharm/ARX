using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;
using ARX;

public class RPGStage_StatBar : RPGStage_UIElement
{
    public override void I_DrawElement(RPGStage_ElementContainer elementContainer)
    {
        StatSlider.minValue = 0;
        StatSlider.maxValue = elementContainer.Stat.Max;
        StatSlider.value = elementContainer.Stat.Current;
    }

    public override void I_DrawBlank()
    {
        StatSlider.minValue = 0;
        StatSlider.maxValue = 69;//Tee hee.
        StatSlider.value = 42;
    }
}
