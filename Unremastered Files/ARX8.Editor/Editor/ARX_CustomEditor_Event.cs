using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;
using UnityEngine.Events;
using System;


public class ARXEventViewer : ARX_IEnumerableViewer<ARX_EventHandlerEntry>
{
    public override bool V_DrawNewItemButton(RectGuide oGuide, bool bIsTopDrawn)
    {
        return false;
    }

    public override void I_ClickNewItem(List<ARX_EventHandlerEntry> moa_list)
    {

    }

    public override void I_DrawName(RectGuide oGuide, ARX_EventHandlerEntry oItem, int nIndex, List<ARX_EventHandlerEntry> oList)
    {
        oItem.mstr_processName = GUI.TextField(oGuide.GetNextRect(150, 16), oItem.mstr_processName);
    }

    public override void I_DrawUnique(RectGuide oGuide, ARX_EventHandlerEntry oItem, int nIndex, List<ARX_EventHandlerEntry> oList)
    {

    }

    public override bool I_Equal(ARX_EventHandlerEntry one, ARX_EventHandlerEntry two)
    {
        return one == two;
    }

    public override void I_ReactToDeleteButtonClickedForItem(ARX_EventHandlerEntry oItem, int nIndex)
    {

    }
}

[CustomEditor(typeof(ARX_Event))]
[InitializeOnLoad]
public class ARX_CustomEditor_Event : Editor
{

    ARXEventViewer mo_viewer;

    int GetHeight(ARX_EventHandlerEntry ev)
    {
        return 16 * 5;
    }

    Rect GetRect
    {
        get
        {
            int nHeight = 600;

            foreach (ARX_EventHandlerEntry ev in GetTarget.EventHandlerSeries)
            {
                nHeight += GetHeight(ev);
            }


            return GUILayoutUtility.GetRect(600, nHeight);
        }
    }

    private void OnDestroy()
    {
        Save();
    }

    void Save()
    {
        EditorUtility.SetDirty(GetTarget);
        //AssetDatabase.SaveAssets();
    }

    ARX_Event GetTarget { get { return (ARX_Event)target; } }

    List<DataPair> mo_dataPairList = new List<DataPair>();
    DataStringViewer mo_dataStringViewer = null;

    private static Vector2 offset = new Vector2(0, 2);

    static ARX_CustomEditor_Event()
    {
        EditorApplication.projectWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    static bool ContainsInstanceID(int nID)
    {
        foreach (int n in Selection.instanceIDs)
            if (n == nID)
                return true;
        return false;
    }

    private static void HandleHierarchyWindowItemOnGUI(string guid, Rect selectionRect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        Type oType = AssetDatabase.GetMainAssetTypeAtPath(path);

        if (oType == typeof(ARX.ARX_Event))
        {
            if (Event.current.type == EventType.Repaint)
            {
                ARX_Event oEvent = AssetDatabase.LoadAssetAtPath<ARX_Event>(path);

                Color highlightColor = Color.green;

                if (oEvent.LoadStatus == ARX_Event.LOADSTATUS.UNLOADED)
                    highlightColor = Color.red;
                else if (oEvent.LoadStatus == ARX_Event.LOADSTATUS.LOADEDTWICE)
                    highlightColor = Color.yellow;

                highlightColor = highlightColor.GetColorWithAlpha(0.07F);

                GUI.backgroundColor = highlightColor;

                //doing this three times because once is kind of transparent.
                GUI.Box(selectionRect, "");
                GUI.Box(selectionRect, "");
                GUI.Box(selectionRect, "");
                GUI.backgroundColor = Color.white;
                EditorApplication.RepaintHierarchyWindow();
            }

        }


    }

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();


        if (mo_viewer == null)
            mo_viewer = new ARXEventViewer();
        if (mo_dataStringViewer == null)
            mo_dataStringViewer = new DataStringViewer();

        

        RectGuide oGuide = new RectGuide(GetRect, 16);

        if (GUI.Button(oGuide.GetNextRect(100, 30), "Create output"))
        {
            CreateGameEventFile();
        }
        oGuide.NewLine();



        //GUI.Label(oGuide.GetNextRect(150,16),  "ARX Event: " + GetTarget.name);
        //oGuide.NewLine(2);

        mo_viewer.V_Draw(oGuide, GetTarget.EventHandlerSeries);

        //List the subscribers
        oGuide.NewLine();


        GUI.Label(oGuide.GetNextRect(300, 16), "Message: " + GetTarget.name);
        oGuide.NewLine(2);



        //Set Event Order list
        GUI.Label(oGuide.GetNextRect(300, 16), "Set Event Order List");
        oGuide.NewLine();
        GetTarget.mo_eventOrderList = (ARX_EventOrder)EditorGUI.ObjectField(oGuide.GetNextRect(200, 16), GetTarget.mo_eventOrderList, typeof(ARX_EventOrder), true);

        oGuide.NewLine(2);

        //Premade Messages
        ARX_DataStringAsset oDatAsset = null;
        GUI.Label(oGuide.GetNextRect(300, 16), "Fire Premade Message");
        oGuide.NewLine();
        oDatAsset = (ARX_DataStringAsset)EditorGUI.ObjectField(oGuide.GetNextRect(200, 16), oDatAsset, typeof(ARX_DataStringAsset), true);
        if (oDatAsset != null)
        {
            GetTarget.FireEvent(oDatAsset.mo_dataString.Clone);
        }
        oGuide.NewLine(2);


        mo_dataStringViewer.mstr_newItemButtonLabel = "New\nMessage";
        mo_dataStringViewer.V_Draw(oGuide, mo_dataPairList);

        oGuide.NewLine(4);

        if (GUI.Button(oGuide.GetNextRect(100, 30), "Fire Message"))
        {
            DataString dat = new DataString(null);
            dat.mstr_data = DataPair.CombineDataPairsIntoString(mo_dataPairList);
            GetTarget.FireEvent(dat);
        }

        oGuide.NewLine(2);

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
    
    void CreateGameEventFile()
    {
        string strUsing = "using ARX;\nusing UnityEngine;\n\n";
        string strNamespace = "namespace ARX{\n\n";
        string strVariableOutput = "\n#region ARX Game Events\n";
        string strInitializeFunctionContent = "";
        Dictionary<string, List<ARX_Event>> oaRegionDictionary = new Dictionary<string, List<ARX_Event>>();

        #region Instantiate Event String
        const string strInstantiateEvent = @"public static void InstantiateEvent(out ARX_Event oEvent, string strID)
        {
                oEvent = FEN.Loading.LoadEvent(strID);

                if (oEvent == null)
                {
                    oEvent = ScriptableObject.CreateInstance<ARX_Event>();
                }
                oEvent.SetLoadStatus();
            }
            ";
        #endregion Instantiate Event String

        //Get all defined ARX_Events
        List<ARX_Event> allEventsInEditor = EditorHelper.FindAssetsByType<ARX_Event>();

        Debug.Log("Found " + allEventsInEditor.Count + " arx events defined in the editor");

        //Remove the non-assets
        //Arrange all events by their region
        for (int i = 0; i < allEventsInEditor.Count; i++)
        {
            ARX_Event oEvent = allEventsInEditor[i];

            //If the region does not have a list, add a new lise
            if (oaRegionDictionary.ContainsKey(oEvent.mstr_eventRegion) == false)
            {
                oaRegionDictionary[oEvent.mstr_eventRegion] = new List<ARX_Event>();
            }
            oaRegionDictionary[oEvent.mstr_eventRegion].Add(oEvent);
        }

        //For each region
        foreach (string strRegion in oaRegionDictionary.Keys)
        {
            //Add open region to strInstantiateEvent
            strInitializeFunctionContent += "#region " + strRegion + "\n";

            List<ARX_Event> oaList = oaRegionDictionary[strRegion];
            //For each event in the region
            foreach(ARX_Event oaEvent in oaList)
            {
                //Add an output string to the Variable Output
                ARX_VariableSpecs oStringSpec = new ARX_VariableSpecs();
                #region Variable Spec for Variable String
                oStringSpec.me_security = ARX.VarGen.SecurityScope.Public;
                oStringSpec.mstr_name = oaEvent.name;
                oStringSpec.mstr_variableType = "ARX_Event";
                oStringSpec.mb_isStatic = true;
                oStringSpec.me_output = ARX.VarGen.VariableGenOutputType.Variable;
                oStringSpec.mb_isLiteral = true;
                #endregion
                strVariableOutput += oStringSpec.GetOutput() + "\n";

                //Add a function call string to the Function Content output
                string str = "InstantiateEvent(out " + oaEvent.name + ", nameof(" + oaEvent.name + "));\n";
                strInitializeFunctionContent += str;
            }
            //Add close region to strInstantiateEvent
            strInitializeFunctionContent += "#endregion " + strRegion + "\n";
        }

        strVariableOutput += "#endregion";

        string strFullInstantiateFunction = "public static void Initialize(){\n" + strInitializeFunctionContent + "\n}";

        //Create output
        ARX_VariableSpecs oGameEventsClass = new ARX_VariableSpecs();
        oGameEventsClass.mb_isStatic = true;
        oGameEventsClass.mb_isPartial = true;
        oGameEventsClass.me_security = ARX.VarGen.SecurityScope.Public;
        oGameEventsClass.mstr_name = "GameEvents";
        oGameEventsClass.me_output = ARX.VarGen.VariableGenOutputType.Class;
        oGameEventsClass.mstr_content += strVariableOutput + "\n\n";
        oGameEventsClass.mstr_content += strInstantiateEvent + "\n\n";
        oGameEventsClass.mstr_content += strFullInstantiateFunction;

        GetTarget.mstr_gameEventFile = strUsing + strNamespace + oGameEventsClass.GetOutput() + "\n}";

    }

}
