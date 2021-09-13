using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARX;

/// <summary>
/// Shows a currently active ARX_Process.
/// This system is scheduled for a rework.
/// </summary>
public class ARX_Script_ProcessViewer : MonoBehaviour
{
    public GameObject mo_processButtonBounds, mo_root;

    public Text mo_eventRecordText;

    public string mstr_switchToProcess = "";
    public bool mb_switch = false;
    public bool mb_remake = true;

    ARX_Process mo_focusedProcess;

    [SerializeField]
    ARX_Process mo_process = null;
    public ARX_Process Process
    {
        get
        {
            if (mo_process == null)
                mo_process = Main.MainProcess;
            return mo_process;
        }
        set
        {
            mo_process = value;
        }
    }

    static List<ARX_Script_ProcessViewer> _Viewers;
    public static List<ARX_Script_ProcessViewer> Viewers
    {
        get
        {
            if (_Viewers == null)
                _Viewers = new List<ARX_Script_ProcessViewer>();
            return _Viewers;

        }
    }

    public static void ShowViewer(string strProcessID)
    {
        if (Viewers != null)
        {
            foreach (ARX_Script_ProcessViewer view in Viewers)
            {
                if (view.Process.name == strProcessID)
                {
                    view.gameObject.SetActive(true);
                    return;
                }
                else
                    view.gameObject.SetActive(false);

                Debug.LogError(strProcessID + " was not found in the View canvas.");
            }
        }
    }

    void SwitchProcess()
    {
        
        Process = Main.MainProcess;
        Process = Process.GetProcess(mstr_switchToProcess);

    }

    public static GameObject CreateViewer(ARX_Process oProcess)
    {
        GameObject obj = FEN.Loading.Load("processviewer");
        ARX_Script_ProcessViewer oScript = obj.GetComponent<ARX_Script_ProcessViewer>();
        oScript.Process = oProcess;
        return obj;
    }
    
    List<Button> moa_buttons;

    private void Start()
    {
        moa_buttons = new List<Button>(mo_processButtonBounds.GetComponentsInChildren<Button>());
        Viewers.Add(this);
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }

    private void OnDestroy()
    {
        Viewers.Remove(this);
    }
    
    public void FixedUpdate()
    {
        if (Process == null)
            return;

        if (mb_switch)
        {
            SwitchProcess();
            mb_switch = false;
            return;
        }

}
    
    void DuplicateButton()
    {
        GameObject obj = Instantiate( moa_buttons[0].gameObject);
        obj.transform.SetParent(mo_processButtonBounds.transform);

        Button b = obj.GetComponent<Button>();
        if (b == null)
            b = obj.AddComponent<Button>();

        moa_buttons.Add(b);

        RectTransform rectTransform = mo_processButtonBounds.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + 16);
    }

    void ClickProcess(int nProcessUniqueID)
    {
        Debug.Log("Clicked process " + nProcessUniqueID);

        mo_focusedProcess = ARX_Process.GetProcessByUniqueID(nProcessUniqueID);
    }

    void CreateButton(ARX_Process process, int nButtonIndex)
    {
        //Debug.Log("Creating a button for process " + process.name + " at button index " + nButtonIndex);

        int nTab = process.ParentCount;

        string str = "";
        for (int i = 0; i < nTab; i++)
        {
            str += "\t";
        }
        str += process.name;

        while (nButtonIndex >= moa_buttons.Count)
            DuplicateButton();

        int nbuf = nButtonIndex;

        moa_buttons[nButtonIndex].GetComponentInChildren<Text>().text = str;
        moa_buttons[nButtonIndex].gameObject.SetActive(true);
        moa_buttons[nButtonIndex].onClick.AddListener(()=> ClickProcess(nbuf));

    }

    int RecurseProcess(ARX_Process process, int i)
    {
        //Create a button for process
        CreateButton(process, i);

        i++;
        //For each of process's children, recurse
        foreach (ARX_Process child in process.QueuedProcesses)
        {
            i = RecurseProcess(child, i);
        }

        return i;
    }

    private void OnEnable()
    {
        RemakeViewer();
    }

    int RemakeViewer()
    {
        moa_buttons = new List<Button>(mo_processButtonBounds.GetComponentsInChildren<Button>());

        if (Process == null)
            return 0;

        if (moa_buttons == null)
            moa_buttons = new List<Button>();

        foreach (Button b in moa_buttons)
            b.onClick.RemoveAllListeners();

        return RecurseProcess(Process, 0);

        //For each process in Process's children
    }

    void DeactivateButtonsStartingFrom(int nIndex)
    {
        for (; nIndex < moa_buttons.Count; nIndex++)
        {
            moa_buttons[nIndex].gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (Process.IsDirty || mb_remake)
        {
            mb_remake = false;
            Process.IsDirty = false;
            int nTotalProcesses = RemakeViewer();

            DeactivateButtonsStartingFrom(nTotalProcesses);
        }

        if (Input.GetKeyDown(KeyCode.F11))
            mo_root.SetActive(!mo_root.activeSelf);

        if (mo_focusedProcess != null && mo_eventRecordText != null)
        {
            mo_eventRecordText.text = mo_focusedProcess.EventRecord.ToString();
        }
        

    }
}
