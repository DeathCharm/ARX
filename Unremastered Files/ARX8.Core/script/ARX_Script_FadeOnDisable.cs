using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Records the current color of the attached Image, TMPText, Text or Light on start
/// and sets the attached object back to that color when on Disable.
/// </summary>
public class ARX_Script_FadeOnDisable : MonoBehaviour
{
    /// <summary>
    /// The color the recorded
    /// </summary>
    Color mo_startColor;

    /// <summary>
    /// Records the current color of the attached Image, TMPText, Text or Light
    /// </summary>
    public void RecordCurrentColor()
    {
        if (GetComponent<Image>())
        {
            //guiTexture.color=colors[2];
            mo_startColor = GetComponent<Image>().color;
        }
        else if (GetComponent<TMPro.TextMeshProUGUI>())
        {
            mo_startColor = GetComponent<TMPro.TextMeshProUGUI>().color;
        }
        else if (GetComponent<Text>())
        {
            mo_startColor = GetComponent<Text>().color;
        }
        else if (GetComponent<Renderer>())
        {
            //renderer.material.color = colors[2];
        }
        else if (GetComponent<Light>())
        {
            //light.color=colors[2];	
            mo_startColor = GetComponent<Light>().color;
        }

    }

    private void Start()
    {
        RecordCurrentColor();
    }

    private void OnDisable()
    {
        if (GetComponent<Image>())
        {
            //guiTexture.color=colors[2];
            GetComponent<Image>().color = mo_startColor;
        }
        else if (GetComponent<TMPro.TextMeshProUGUI>())
        {
            GetComponent<TMPro.TextMeshProUGUI>().color = mo_startColor;
        }
        else if (GetComponent<Text>())
        {
            GetComponent<Text>().color = mo_startColor;
        }
        else if (GetComponent<Renderer>())
        {
            //renderer.material.color = colors[2];
        }
        else if (GetComponent<Light>())
        {
            //light.color=colors[2];	
            GetComponent<Light>().color = mo_startColor;
        }
    }
}
