using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opens the contained link in a new browser window when the attached button is clicked.
/// </summary>
public class ARX_LinkButton : MonoBehaviour
{
    public string mstr_hyperlink = "www.VelvetAlabaster.blogspot.com";

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button == null)
            return;
        button.onClick.AddListener(() => ClickLink());
    }

    public void ClickLink()
    {
        Application.OpenURL(mstr_hyperlink);
    }
}
