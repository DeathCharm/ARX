using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script used in combination with the TabBrowser script.
/// When a UI.Button on this object is pressed,
/// the TabBrowser script held by this object's parent will run
/// to deactivate a list of other gameobjects.
/// </summary>
public class ARX_Script_TabButton : MonoBehaviour
{
    /// <summary>
    /// The browser script this button will call on click.
    /// </summary>
    [HideInInspector]
    public ARX_Script_TabBrowser mo_browser;

    /// <summary>
    /// Set this button's listener to PushButton
    /// </summary>
    public void Initiailze()
    {
        Button button = GetComponent<Button>();

        if (button == null)
            button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(() => PushButton());
    }

    /// <summary>
    /// Activates this object's parent browser to make it deactivate a list of other objects.
    /// </summary>
    public void PushButton()
    {
        mo_browser.ActivatePushedButton(this);
    }
}
