using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the changing of wireframe object's "_Color" and "_Thickness" material Properties
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_IndividualWireframe : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The thickness of the wireframe lines
    /// </summary>
    public float mnf_thickness = 5F;

    /// <summary>
    /// This object's renderer
    /// </summary>
    Renderer mo_renderer;

    /// <summary>
    /// This object's renderer's materialPropertyBlock
    /// </summary>
    MaterialPropertyBlock mo_propertyBlock;
    #endregion

    #region Get
    /// <summary>
    /// Returns this object's attached renderer
    /// </summary>
    Renderer GetRenderer
    {
        get
        {
            if (mo_renderer == null)
                mo_renderer = GetComponent<Renderer>();
            return mo_renderer;
        }
    }

    /// <summary>
    /// Returns this object's attached renderer's MaterialPropertyBlock
    /// </summary>
    MaterialPropertyBlock GetPropertyBlock
    {
        get
        {
            if (mo_propertyBlock == null)
                mo_propertyBlock = new MaterialPropertyBlock();
            return mo_propertyBlock;
        }
    }

    /// <summary>
    /// Returns a random float between 0 and 1
    /// </summary>
    float RandomColorValue
    {
        get
        {
            return UnityEngine.Random.Range(0, 1.0F);
        }
    }
    #endregion
    
    // Update is called once per frame
    void Update()
    {
        // Get the current value of the material properties in the renderer.
        GetRenderer.GetPropertyBlock(GetPropertyBlock);
        // Assign our new value.
        GetPropertyBlock.SetFloat("_Thickness", mnf_thickness);
        // Apply the edited values to the renderer.
        GetRenderer.SetPropertyBlock(GetPropertyBlock);
    }
}
