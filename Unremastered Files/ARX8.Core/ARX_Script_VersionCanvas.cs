using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Prints the current version number of the project to a Text UI element
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_VersionCanvas : MonoBehaviour
{
    public Text mo_versionText;

    private void Start()
    {
        if (mo_versionText == null)
            mo_versionText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mo_versionText != null)
            mo_versionText.text = "V " + Application.version;
    }
}
