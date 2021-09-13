using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;
using ARX;
using UnityEngine.UI;

/// <summary>
/// Gets a list of child objects holding ElementContainers, sets each of their Ability variables
/// </summary>
public class RPGStage_AbilityScrollView : RPGStage_UIElement
{
    /// <summary>
    /// Object that controls the page navigation and item numbering of the drawn slides
    /// and inventory items.
    /// </summary>
    [SerializeField]
    private ARX_PageList mo_pageList = new ARX_PageList();

    /// <summary>
    /// An array of attributes by which the drawn abilities are chosen
    /// </summary>
    public string[] mastr_targetAttributes = new string[0];

    /// <summary>
    /// Enum detailing how the attributes will be used to search for which abilities to show
    /// </summary>
    public enum SELECTIONCRITERIA {ALL_ATTRIBUTES, ANY_ATTRIBUTE }

    [Tooltip("All Attributes = All attributes must be present.\nAny Attribute = At least one of the attributes must be present.")]
    public SELECTIONCRITERIA me_selectionCriteria = SELECTIONCRITERIA.ALL_ATTRIBUTES;

 

    /// <summary>
    /// A list of the child objects that hold Ability Slide components.
    /// </summary>
    private RPGStage_AbilitySlide[] moa_slides = null;

    private void Start()
    {
        moa_slides = GetComponentsInChildren<RPGStage_AbilitySlide>(true);    
    }

    /// <summary>
    /// Searches and returns a list of abilities to be drawn based on the 
    /// target attributes and the search criteria enum
    /// </summary>
    /// <returns></returns>
    List<FEN_Ability> GetAbilitiesToDraw()
    {
        //If the target unit is null, 
        //return a new, empty array
        FEN_Unit oTargetUnit = RootElement.Unit;
        
        if (oTargetUnit == null)
            return new List<FEN_Ability>();

        List<FEN_Ability> oaBuf = new List<FEN_Ability>();
        
        //If there is no criteria on what should be drawn, 
        //Draw everything
        if (mastr_targetAttributes.Length == 0)
            return oTargetUnit.AbilityList.AsList;

        //Draw all abilities that bear any of the target attribute
        if(me_selectionCriteria == SELECTIONCRITERIA.ANY_ATTRIBUTE)
        {
            oaBuf = oTargetUnit.GetAbilitiesByAnyAttribute(mastr_targetAttributes);
        }
        //Draw all abilities that bear all of the target attributes
        else if (me_selectionCriteria == SELECTIONCRITERIA.ALL_ATTRIBUTES)
        {
            oaBuf = oTargetUnit.GetAbilitiesByAttribute(mastr_targetAttributes);
        }

        return oaBuf;
    }
    
    public override void I_DrawElement(RPGStage_ElementContainer elementContainer)
    {
        mo_pageList.mn_itemsPerPage = moa_slides.Length;

        List<FEN_Ability> oaAbilitiesToDraw = GetAbilitiesToDraw();
        Vector2 vecMinMaxActive = mo_pageList.GetActiveInventoryMinAndMax(oaAbilitiesToDraw.Count);
        Vector2 vecMinMaxInactive = mo_pageList.GetInactiveInventoryMinAndMax(oaAbilitiesToDraw.Count);

        Vector2 vecMinMaxSlides = mo_pageList.GetActiveSlides(oaAbilitiesToDraw.Count, moa_slides.Length);

    }
}
