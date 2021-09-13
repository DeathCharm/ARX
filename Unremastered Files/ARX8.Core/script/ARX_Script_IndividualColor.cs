using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the changing of an object's color by changing it's renderer's "_Color" materialPropertyBlock
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_IndividualColor : MonoBehaviour {

    #region Variables

    /// <summary>
    /// The current color of the material block.
    /// This is the color shown during Edit Mode.
    /// When Play mode starts, this color will be set to 
    /// the value of mo_setOnStartColor
    /// </summary>
    [Tooltip("This is the color shown during Edit Mode.When Play mode starts, this color will be set to the value of mo_setOnStartColor")]
    public Color mo_color;

    /// <summary>
    /// The first color the material block will be set to when Play Mode Activates.
    /// </summary>
    [Tooltip("The first color the material block will be set to when Play Mode Activates.")]
    public Color mo_setOnStartColor;

    /// <summary>
    /// Set to a random color?
    /// </summary>
    public bool mb_makeRandomColor = false;

    /// <summary>
    /// Change the color to setOnStartColor when Play Mode starts?
    /// </summary>
    public bool mb_setColorOnStart = false;

    /// <summary>
    /// The default Alpha value of the random colors
    /// </summary>
    public float mnf_defaultAlpha = 0.5F;

    /// <summary>
    /// The renderer component attached to this object
    /// </summary>
    Renderer mo_renderer;

    /// <summary>
    /// The renderer's property block pointing to the "_Color" property
    /// </summary>
    MaterialPropertyBlock mo_propertyBlock;
    #endregion

    #region Get

    /// <summary>
    /// Returns the renderer component.
    /// </summary>
    Renderer GetRenderer { get
        {
            if(mo_renderer == null)
                mo_renderer = GetComponent<Renderer>();
            return mo_renderer;
        } }

    /// <summary>
    /// Returns the MaterialPropertyBlock component
    /// </summary>
    MaterialPropertyBlock GetPropertyBlock { get
        {
            if(mo_propertyBlock == null)
                mo_propertyBlock = new MaterialPropertyBlock();
            return mo_propertyBlock;
        } }

    /// <summary>
    /// Returns a random float Value between 0 and 0.5
    /// </summary>
    float RandomValueFromZeroToPointFive
    {
        get
        {
            return UnityEngine.Random.Range(0, 0.5F);
        }
    }
    #endregion
    
    #region Functions

    /// <summary>
    /// Set's the mo_color variable to a random color.
    /// The material's color will change on the next frame.
    /// </summary>
    public void SetToRandomColor()
    {
        mo_color = new Color(RandomValueFromZeroToPointFive, RandomValueFromZeroToPointFive, RandomValueFromZeroToPointFive, mnf_defaultAlpha);
    }
	
    #endregion
    
    #region Unity Overrides

    // Use this for initialization
    void Start () {
        if (Application.isPlaying == false)
            return;

        if (mb_setColorOnStart)
        {
            mo_color = mo_setOnStartColor;
        }
	}

// Update is called once per frame
	void Update () {
        if (mb_makeRandomColor)
        {
            SetToRandomColor();
            mb_makeRandomColor = false;
        }
        // Get the current value of the material properties in the renderer.
        GetRenderer.GetPropertyBlock(GetPropertyBlock);
        // Assign our new value.
        GetPropertyBlock.SetColor("_Color", mo_color);
        // Apply the edited values to the renderer.
        GetRenderer.SetPropertyBlock(GetPropertyBlock);
    }

    #endregion
}
