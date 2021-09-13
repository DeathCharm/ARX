using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script automatically creates a tab browser from a given
/// gameobject with button children and a gameobject with ui pages
/// to activate on button press.
/// </summary>
public class ARX_Script_TabBrowser : MonoBehaviour
{

    #region Variables

    /// <summary>
    /// The root of the list of buttons.
    /// This gameObject's children will each have a TabButton
    /// script added to them on Start.
    /// </summary>
    public GameObject mo_buttonRoot;

    /// <summary>
    /// The root of page objects.
    /// This object's children will be activated or deactivated when
    /// tabButtons are pressed.
    /// </summary>
    public GameObject mo_pageRoot;
    #endregion

    #region Functions

    /// <summary>
    /// For each child of the button root, add a Button script and
    /// set its onListener
    /// </summary>
    private void Start()
    {
        if (mo_buttonRoot == null)
            return;

        for (int i = 0; i < mo_buttonRoot.transform.childCount; i++)
        {
            Transform trans = mo_buttonRoot.transform.GetChild(i);
            ARX_Script_TabButton oButton = trans.gameObject.GetComponent<ARX_Script_TabButton>();

            if (oButton == null)
            {
                oButton = trans.gameObject.AddComponent<ARX_Script_TabButton>();
            }

            oButton.mo_browser = this;
            oButton.Initiailze();
        }
        
    }

    /// <summary>
    /// Activates the child of the pageRoot object.
    /// An nPage value of 3 will activate mo_pageRoot.transform.GetChild(2)
    /// </summary>
    /// <param name="nPage"></param>
    /// <param name="bActive"></param>
    void ActivatePage(int nPage, bool bActive)
    {
        Transform trans = mo_pageRoot.transform.GetChild(nPage);
        if (trans == null)
        {
            Debug.LogError("No page for " + nPage + " on item " + name);
            return;
        }

        trans.gameObject.SetActive(bActive);

    }

    /// <summary>
    /// This function is called by the TabButton script.
    /// It activates the given tab while deactivating all other tabs.
    /// </summary>
    /// <param name="tab"></param>
    public void ActivatePushedButton(ARX_Script_TabButton tab)
    {
        for (int i = 0; i < mo_buttonRoot.transform.childCount; i++)
        {
            Transform oTrans = mo_buttonRoot.transform.GetChild(i);
            ARX_Script_TabButton oTab = oTrans.GetComponent<ARX_Script_TabButton>();
            if (oTab == tab)
            {
                ActivatePage(i, true);
            }
            else
            {
                ActivatePage(i, false);
            }

        }
    }
    #endregion

}
