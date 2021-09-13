using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FEN;
using ARX;


public abstract class RPGStage_UIElement : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Text mo_textTitle;

    [SerializeField]
    private Text mo_textDescription;
    
    [SerializeField]
    private Text mo_textInfo;

    [SerializeField]
    private Slider mo_statSlider;

    [SerializeField]
    private Image mo_icon;

    /// <summary>
    /// The Root Element owned by this UI element's parent. Contains the reference to the RPGElement
    /// this UI element displays.
    /// </summary>
    private RPGStage_ElementContainer mo_rootElement = null;

    #endregion

    #region Setters and Getters

    public string Title {
        get {
            if (mo_textTitle == null)
                return "";
            return mo_textTitle.text;
        }
        set {

            if (mo_textTitle == null)
                return;
            mo_textTitle.text = value;
        }

    }

    public string Description
    {
        get
        {
            if (mo_textDescription == null)
                return "";
            return mo_textDescription.text;
        }
        set
        {
            if (mo_textDescription == null)
                return;
            mo_textDescription.text = value;
        }
    }

    public string Info
    {
        get
        {
            if (mo_textInfo == null)
                return "";
            return mo_textInfo.text;
        }
        set
        {
            if (mo_textInfo == null)
                return;
            mo_textInfo.text = value;
        }
    }

    public Sprite Icon
    {
        get
        {
            if (mo_icon == null)
                return null;
            return mo_icon.sprite;
        }
        set
        {
            if (mo_icon == null)
                return;
            mo_icon.sprite = value;
        }
    }

    public Slider StatSlider
    {
        get
        {
            if (mo_statSlider == null)
                return null;
            return mo_statSlider;
        }
    }

    public RPGStage_ElementContainer RootElement { get
        {
            if (mo_rootElement == null)
                mo_rootElement = GetComponentInParent<RPGStage_ElementContainer>();
            if(mo_rootElement == null)
            {
                Debug.LogError(name + " does not have a Root Element in its parent");
            }
            return mo_rootElement;
        }
    }

    #endregion

    #region Abstracts

    /// <summary>
    /// Draw this element.
    /// </summary>
    /// <param name="elementContainer"></param>
    public abstract void I_DrawElement(RPGStage_ElementContainer elementContainer);


    /// <summary>
    /// WHen this element can't be drawn due to a missing reference in the element container.
    /// </summary>
    /// <param name="elementContainer"></param>
    public virtual void I_DrawBlank()
    {
        Title = "";
        Description = "";
        Info = "";
        Icon = null;
    }
    #endregion
    
    private void FixedUpdate()
    {
        I_DrawElement(RootElement);
    }

}
