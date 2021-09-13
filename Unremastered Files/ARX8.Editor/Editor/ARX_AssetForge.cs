using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;
using FEN;

public class UnitForge : ARX_AssetForge<FEN_Unit>
{
    public UnitForge(EditorWindow window) : base(window)
    {

    }

    public override string I_GetLeftFileOutput()
    {
        return GetUnitProcessOutput(mo_highlightedAsset);
    }
    

    public override string I_GetRightFileOutput()
    {
        return "";
    }



    #region Get Ability Process
    string GetSubscriptions(FEN_Unit ability)
    {
        string strReturn = "";
        const string strPrefix = "\tFEN.GameEvents.";
        const string strPostfix = ".Subscribe(ReactTo";
        const string strSuffix = ", this);";

        foreach (ARX_Event e in ability.GetCallbacks)
        {
            strReturn += strPrefix + e.name + strPostfix + e.name.FirstCharToUpper() + strSuffix + "\n";
        }
        return strReturn;
    }

    string GetCallbacks(FEN_Unit ability)
    {
        string strReturn = "";
        const string strPrefix = "void ReactTo";
        const string strSuffix = "(DataString dat)\n{\n\n}";

        foreach (ARX_Event e in ability.GetCallbacks)
        {
            strReturn += strPrefix + e.name.FirstCharToUpper() + strSuffix + "\n\n";
        }
        strReturn += "\n";
        return strReturn;
    }

    string GetBase(ARX_StatQuad stat, string strSuffix = "")
    {
        return "return mo_targetAbility.Stats.GetStat(\"" + stat.ID + "\").Base" + strSuffix + ";";
    }

    string GetCurrent(ARX_StatQuad stat, string strSuffix = "")
    {
        return "return mo_targetAbility.Stats.GetStat(\"" + stat.ID + "\").Current" + strSuffix + ";";
    }

    string GetMax(ARX_StatQuad stat, string strSuffix = "")
    {
        return "return mo_targetAbility.Stats.GetStat(\"" + stat.ID + "\").Max" + strSuffix + ";";
    }

    string GetIntAccessors(ARX_StatQuad stat, ARX_VariableSpecs specs)
    {
        string strReturn = "";
        specs.me_primitiveType = ARX.VarGen.PrimitiveVariable.Int;

        specs.mstr_name = "GetBase" + stat.ID.FirstCharToUpper() + "Int";
        specs.mstr_content = GetBase(stat, "Int");
        strReturn += specs.GetOutput();

        specs.mstr_name = "GetCurrent" + stat.ID.FirstCharToUpper() + "Int";
        specs.mstr_content = GetCurrent(stat, "Int");
        strReturn += specs.GetOutput();

        specs.mstr_name = "GetMax" + stat.ID.FirstCharToUpper() + "Int";
        specs.mstr_content = GetMax(stat, "Int");
        strReturn += specs.GetOutput();

        return strReturn;
    }

    string GetFloatAccessors(ARX_StatQuad stat, ARX_VariableSpecs specs)
    {
        string strReturn = "";
        specs.me_primitiveType = ARX.VarGen.PrimitiveVariable.Float;

        specs.mstr_name = "GetBase" + stat.ID.FirstCharToUpper();
        specs.mstr_content = GetBase(stat);
        strReturn += specs.GetOutput();

        specs.mstr_name = "GetCurrent" + stat.ID.FirstCharToUpper();
        specs.mstr_content = GetCurrent(stat);
        strReturn += specs.GetOutput();

        specs.mstr_name = "GetMax" + stat.ID.FirstCharToUpper();
        specs.mstr_content = GetMax(stat);
        strReturn += specs.GetOutput();

        return strReturn;
    }

    string GetAccessorBaseCurrentAndMax(ARX_StatQuad stat)
    {
        string strReturn = "";
        ARX_VariableSpecs specs = new ARX_VariableSpecs();
        specs.me_security = ARX.VarGen.SecurityScope.Public;
        specs.me_output = ARX.VarGen.VariableGenOutputType.Accessor;
        specs.mb_encloseInBrackets = true;

        strReturn += GetIntAccessors(stat, specs);
        strReturn += GetFloatAccessors(stat, specs);

        return strReturn;
    }

    string GetAccessors(FEN_Unit ab)
    {
        if (ab.Stats == null)
            return "";

        string strReturn = "#region Accessors\n";

        foreach (ARX_StatQuad stat in ab.Stats.AsList)
        {
            strReturn += GetAccessorBaseCurrentAndMax(stat);
        }

        strReturn += "\n#endregion\n";
        return strReturn;
    }
    
    #endregion

    string GetUnitProcessOutput(FEN_Unit unit)
    {
        string strName = "FEN_UnitAI_" + unit.name.TrimWhitespace().FirstCharToUpper();
        string str =
@"using FEN;
using ARX;
using UnityEngine;

//Auto-generated Unit AI Process Definition by FenAssetForge
namespace FEN{

public class " + strName + " : FEN_UnitAI\n{\n\n" +
        "public " + strName + "(FEN_Unit owner, CardIDs.UNITID eID):base(owner, CardIDs.UNITID." + unit.me_unitID.ToString() + "){\n\n}\n\n";

        string strSubscriptions = GetSubscriptions(unit);

        string strInitialize =
            @"public override void OnInitialized()" +
        "\n{\n" + strSubscriptions + "\n}";


        str += strInitialize + "\n\n" + GetCallbacks(unit) + GetAccessors(unit);

        str += "\n}\n}\n";
        return str;
    }
    
    Vector2 mvec_attributeScroll, mvec_callbackScroll;

    public override void DrawArrays(FEN_Unit toBeDrawn)
    {
        //Main content areas of the Attribute List and Callback List
        Rect rectExternalAttributes = GUILayoutUtility.GetRect(125, 250);
        Rect rectExternalCallbacks = rectExternalAttributes;
        rectExternalCallbacks.x += 200;

        EditorGUI.DrawRect(rectExternalCallbacks, Colors.EditorGray2);

        #region Draw Attributes
        {
            int nAttributeHeight = 140 + toBeDrawn.mastr_attributes.Count * 16;
            Rect rectInternalAttributes = new Rect(0, 0, rectExternalAttributes.width, nAttributeHeight);
            RectGuide oGuideAttributes = new RectGuide(rectInternalAttributes);
            StringViewer oAttributeViewer = new StringViewer();
            oAttributeViewer.mstr_title = "Attributes";
            oAttributeViewer.mn_slideWidth = 125;
            mvec_attributeScroll = GUI.BeginScrollView(rectExternalAttributes, mvec_attributeScroll, rectInternalAttributes);
            oAttributeViewer.V_Draw(oGuideAttributes, toBeDrawn.mastr_attributes);
            GUI.EndScrollView();
        }
        #endregion

        ////Add Event to Callback List
        //{
        //    Rect rectSelectEvent = rectExternalCallbacks;
        //    rectSelectEvent.height = 20;
        //    rectSelectEvent.width = 150;
        //    rectSelectEvent.y -= 20;
        //    ARX_Event oItem = EditorGUI.ObjectField(rectSelectEvent, "", null, typeof(ARX_Event), false) as ARX_Event;
        //    if (oItem != null && toBeDrawn.GetCallbacks.Contains(oItem) == false)
        //        toBeDrawn.GetCallbacks.Add(oItem);
        //}

        #region Draw Callbacks
        {
            int nCallbackHeight = 140 + toBeDrawn.GetCallbacks.Count * 16;
            Rect rectInternalCallbacks = new Rect(0, 0, rectExternalCallbacks.width, nCallbackHeight);
            RectGuide oGuideAttributes = new RectGuide(rectInternalCallbacks);
            ARX_EventViewer oCallBackViewer = new ARX_EventViewer();
            oCallBackViewer.mstr_title = "Callbacks";
            oCallBackViewer.mn_slideWidth = 135;
            mvec_callbackScroll = GUI.BeginScrollView(rectExternalCallbacks, mvec_attributeScroll, rectInternalCallbacks);
            oCallBackViewer.V_Draw(oGuideAttributes, toBeDrawn.GetCallbacks);
            GUI.EndScrollView();
        }
        #endregion
    }
}

public class AbilityForge : ARX_AssetForge<FEN_Ability>
{
    public AbilityForge(EditorWindow window) : base(window)
    {

    }

    #region Get Ability Process
    string GetSubscriptions(FEN_Ability ability)
    {
        string strReturn = "";
        const string strPrefix = "\tFEN.GameEvents.";
        const string strPostfix = ".Subscribe(ReactTo";
        const string strSuffix = ", this);";

        foreach (ARX_Event e in ability.GetCallbacks)
        {
            strReturn += strPrefix + e.name + strPostfix + e.name.FirstCharToUpper() + strSuffix + "\n";
        }
        return strReturn;
    }

    string GetCallbacks(FEN_Ability ability)
    {
        string strReturn = "";
        const string strPrefix = "void ReactTo";
        const string strSuffix = "(DataString dat)\n{\n\n}";

        foreach (ARX_Event e in ability.GetCallbacks)
        {
            strReturn += strPrefix + e.name.FirstCharToUpper() + strSuffix + "\n\n";
        }
        strReturn += "\n";
        return strReturn;
    }

    string GetBase(ARX_StatQuad stat, string strSuffix="")
    {
        return "return mo_targetAbility.Stats.GetStat(\"" + stat.ID + "\").Base" + strSuffix + ";";
    }

    string GetCurrent(ARX_StatQuad stat, string strSuffix="")
    {
        return "return mo_targetAbility.Stats.GetStat(\"" + stat.ID + "\").Current" + strSuffix + ";";
    }

    string GetMax(ARX_StatQuad stat, string strSuffix="")
    {
        return "return mo_targetAbility.Stats.GetStat(\"" + stat.ID + "\").Max" + strSuffix + ";";
    }

    string GetIntAccessors(ARX_StatQuad stat, ARX_VariableSpecs specs)
    {
        string strReturn = "";
        specs.me_primitiveType = ARX.VarGen.PrimitiveVariable.Int;

        specs.mstr_name = "GetBase" + stat.ID.FirstCharToUpper() + "Int";
        specs.mstr_content = GetBase(stat, "Int");
        strReturn += specs.GetOutput();

        specs.mstr_name = "GetCurrent" + stat.ID.FirstCharToUpper() + "Int";
        specs.mstr_content = GetCurrent(stat, "Int");
        strReturn += specs.GetOutput();

        specs.mstr_name = "GetMax" + stat.ID.FirstCharToUpper() + "Int";
        specs.mstr_content = GetMax(stat, "Int");
        strReturn += specs.GetOutput();

        return strReturn;
    }

    string GetFloatAccessors(ARX_StatQuad stat, ARX_VariableSpecs specs)
    {
        string strReturn = "";
        specs.me_primitiveType = ARX.VarGen.PrimitiveVariable.Float;

        specs.mstr_name = "GetBase" + stat.ID.FirstCharToUpper();
        specs.mstr_content = GetBase(stat);
        strReturn += specs.GetOutput();

        specs.mstr_name = "GetCurrent" + stat.ID.FirstCharToUpper();
        specs.mstr_content = GetCurrent(stat);
        strReturn += specs.GetOutput();

        specs.mstr_name = "GetMax" + stat.ID.FirstCharToUpper();
        specs.mstr_content = GetMax(stat);
        strReturn += specs.GetOutput();

        return strReturn;
    }

    string GetAccessorBaseCurrentAndMax(ARX_StatQuad stat)
    {
        string strReturn = "";
        ARX_VariableSpecs specs = new ARX_VariableSpecs();
        specs.me_security = ARX.VarGen.SecurityScope.Public;
        specs.me_output = ARX.VarGen.VariableGenOutputType.Accessor;
        specs.mb_encloseInBrackets = true;

        strReturn += GetIntAccessors(stat, specs);
        strReturn += GetFloatAccessors(stat, specs);
        
        return strReturn;
    }

    string GetAccessors(FEN_Ability ab)
    {
        if (ab.Stats == null)
            return "";

        string strReturn = "#region Accessors\n";

        foreach(ARX_StatQuad stat in ab.Stats.AsList)
        {
            strReturn += GetAccessorBaseCurrentAndMax(stat);
        }

        strReturn += "\n#endregion\n";
        return strReturn;
    }

    string GetAbilityProcessOutput(FEN_Ability ability)
    {
        string strName = "FEN_AbilityProcess_" + ability.name.TrimWhitespace().FirstCharToUpper();
        string str = 
@"using FEN;
using ARX;
using UnityEngine;

//Auto-generated Ability Process Definition by FenAssetForge
namespace FEN{

public class " + strName + " : FEN_AbilityProcess\n{\n\n" +
        "public " + strName + "(FEN_Ability ability, FEN_Unit owner, CardIDs.CARDID eID):base(ability, owner, CardIDs.CARDID." + ability.me_abilityID.ToString() + "){\n\n}\n\n";

        string strSubscriptions = GetSubscriptions(ability);

        string strInitialize =
            @"public override void OnInitialized()" +
        "\n{\n" + strSubscriptions + "\n}";


        str += strInitialize + "\n\n" + GetCallbacks(ability) + GetAccessors(ability);

        str += "\n}\n}\n";
        return str;
    }
    #endregion

    #region Get Load Ability AI

    string GetAbilityCases()
    {
        return FEN.AutoCodeCalculations.GetAbilityCases();
    }

    string GetLoadFenAbilityProcessOutput()
    {
        return FEN.AutoCodeCalculations.GetLoadFenAbilityProcessOutput();
    }
    #endregion

    #region Abstracts (sort of)

    Vector2 mvec_attributeScroll, mvec_callbackScroll;

    public override void DrawArrays(FEN_Ability toBeDrawn)
    {
        //Main content areas of the Attribute List and Callback List
        Rect rectExternalAttributes = GUILayoutUtility.GetRect(125, 250);
        Rect rectExternalCallbacks = rectExternalAttributes;
        rectExternalCallbacks.x += 200;

        EditorGUI.DrawRect(rectExternalCallbacks, Colors.EditorGray2);

        #region Draw Attributes
        {
            int nAttributeHeight = 140 + toBeDrawn.mastr_attributes.Count * 16;
            Rect rectInternalAttributes = new Rect(0, 0, rectExternalAttributes.width, nAttributeHeight);
            RectGuide oGuideAttributes = new RectGuide(rectInternalAttributes);
            StringViewer oAttributeViewer = new StringViewer();
            oAttributeViewer.mstr_title = "Attributes";
            oAttributeViewer.mn_slideWidth = 125;
            mvec_attributeScroll = GUI.BeginScrollView(rectExternalAttributes, mvec_attributeScroll, rectInternalAttributes);
            oAttributeViewer.V_Draw(oGuideAttributes, toBeDrawn.mastr_attributes);
            GUI.EndScrollView();
        }
        #endregion

        ////Add Event to Callback List
        //{
        //    Rect rectSelectEvent = rectExternalCallbacks;
        //    rectSelectEvent.height = 20;
        //    rectSelectEvent.width = 150;
        //    rectSelectEvent.y -= 20;
        //    ARX_Event oItem = EditorGUI.ObjectField(rectSelectEvent, "", null, typeof(ARX_Event), false) as ARX_Event;
        //    if (oItem != null && toBeDrawn.GetCallbacks.Contains(oItem) == false)
        //        toBeDrawn.GetCallbacks.Add(oItem);
        //}

        #region Draw Callbacks
        {
            int nCallbackHeight = 140 + toBeDrawn.GetCallbacks.Count * 16;
            Rect rectInternalCallbacks = new Rect(0, 0, rectExternalCallbacks.width, nCallbackHeight);
            RectGuide oGuideAttributes = new RectGuide(rectInternalCallbacks);
            ARX_EventViewer oCallBackViewer = new ARX_EventViewer();
            oCallBackViewer.mstr_title = "Callbacks";
            oCallBackViewer.mn_slideWidth = 135;
            mvec_callbackScroll = GUI.BeginScrollView(rectExternalCallbacks, mvec_attributeScroll, rectInternalCallbacks);
            oCallBackViewer.V_Draw(oGuideAttributes, toBeDrawn.GetCallbacks);
            GUI.EndScrollView();
        }
        #endregion
    }

    public override string I_GetLeftFileOutput()
    {
        if (mo_highlightedAsset == null)
            return "";
        return GetAbilityProcessOutput(mo_highlightedAsset);
    }

    public override string I_GetRightFileOutput()
    {
        return GetLoadFenAbilityProcessOutput();
    }

    public override void I_ClickAssetEntry(FEN_Ability asset)
    {
        //If the asset's name is the same as a defined CardID, set its abilityID to that cardID
        System.Array ar = System.Enum.GetValues(typeof(CardIDs.CARDID));
        foreach(CardIDs.CARDID eID in ar)
        {
            if(eID != asset.me_abilityID && eID.ToString().ToUpper() == asset.name.ToUpper())
            {
                Debug.Log("Ability " + asset.name + " has the same name as a defined CardID. FEN_Ability AssetForge etting it's me_abilityID to that CardID(" + eID.ToString() + ").");
                asset.me_abilityID = eID;
                break;
            }
        }

    }
    #endregion
}

public abstract class ARX_AssetForge<TAsset> where TAsset : ScriptableObject
{
    #region Variables
    protected Vector2 mvec_internalAssetListScroll;
    public TAsset mo_highlightedAsset;
    protected EditorWindow mo_window;
    protected string mstr_fileOutput = "File Output";

    public List<TAsset> moa_assets;
    #endregion


    #region Constructors
    public ARX_AssetForge(EditorWindow oWindow)
    {
        mo_window = oWindow;
        OnEnable();
    }
    #endregion


    #region Abstracts

    public abstract void DrawArrays(TAsset toBeDrawn);
    
    public virtual void I_CreateNewAsset()
    {
        TAsset newAsset = EditorHelper.CreateAsset<TAsset>("New " + typeof(TAsset).ToString());
    }

    public virtual void I_CopyNewAsset()
    {
        if (mo_highlightedAsset == null)
            return;

        string strPath = AssetDatabase.GetAssetPath(mo_highlightedAsset);
        const int nAddedLength = 7;//Seven additional characters have to be removed since "/.asset" is seven characters
        strPath = strPath.Remove(strPath.Length - (mo_highlightedAsset.name.Length + nAddedLength));

        TAsset newAsset = EditorHelper.CreateAsset<TAsset>(mo_highlightedAsset.name + "-Copy", false, strPath);
        mo_highlightedAsset.HardCopyTo(newAsset);
    }

    public virtual void I_ClickAssetEntry(TAsset asset)
    {

    }

    public abstract string I_GetLeftFileOutput();

    public abstract string I_GetRightFileOutput();

    #endregion


    #region Helper

    public void OnEnable()
    {
        GetAssetsInEditor();
    }

    protected List<TAsset> GetAssetsInEditor()
    {
        moa_assets = new List<TAsset>(EditorHelper.FindAssetsByType<TAsset>());
        return moa_assets;
    }
    #endregion


    #region Draw

    Editor editor;

    public virtual void V_DrawStatArea(Rect rect)
    {
        EditorGUI.DrawRect(rect, ARX.Colors.EditorGray1);
        GUI.BeginGroup(rect);

        if (mo_highlightedAsset != null)
        {
            editor.DrawDefaultInspector();
            DrawArrays(mo_highlightedAsset);
        }
        GUI.EndGroup();
    }

    /// <summary>
    /// Draw the 
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="oGuide"></param>
    public virtual void V_DrawAssetListEntry(TAsset asset, RectGuide oGuide)
    {
        if (asset == null)
            return;

        Rect buttonRect = oGuide.PeekNextRect(oGuide.BoundingRect.width, 16);

        //If this asset's entry is clicked
        if (EditorHelper.IsClickedInEditor(buttonRect))
        {
            GUI.FocusControl("freakingNothing");
            //If the asset clicked is different from the highlighted one
            if (mo_highlightedAsset != asset)
            {
                editor = null;
                mo_highlightedAsset = asset;
                editor = Editor.CreateEditor(mo_highlightedAsset);
                I_ClickAssetEntry(mo_highlightedAsset);
            }
            
            mo_window.Repaint();
        }

        GUI.Label(oGuide.GetNextRect(100, 16), asset.name);
        
        if (asset == mo_highlightedAsset)
            EditorGUI.DrawRect(buttonRect, new Color(0, 1, 0, 0.15F));
        else
        {
            if (EditorHelper.IsHoveredInEditor(buttonRect))
                EditorGUI.DrawRect(buttonRect, new Color(1, 1, 1, 0.15F));
        }

        oGuide.NewLine();
    }

    public virtual void V_DrawListOfAssetsButtons(Rect rect)
    {
        RectGuide oGuide = new RectGuide(rect);


        if (GUI.Button(oGuide.GetNextRect(100, 36), "New Asset"))
            I_CreateNewAsset();

        if (GUI.Button(oGuide.GetNextRect(100, 36), "Copy Asset"))
        {
            I_CopyNewAsset();
        }

        oGuide.NewLine(2);


    }

    public virtual void V_DrawListOfAssets(Rect rect)
    {
        int nInternalHeigth = moa_assets.Count * 16;
        Rect rectInternal = new Rect(0, 0, rect.width, nInternalHeigth);

        GUIStyle oStyle = EditorStyles.helpBox;
        GUI.Box(rect, "", oStyle);

        RectGuide oGuide = new RectGuide(rectInternal);

        //Draw List of Assets in scroll view
        mvec_internalAssetListScroll = GUI.BeginScrollView(rect, mvec_internalAssetListScroll, rectInternal);


        EditorGUI.DrawRect(rectInternal, ARX.Colors.EditorGray1);


        for (int i = 0; i < moa_assets.Count; i++)
        {
            TAsset asset = moa_assets[i];
            //Remove the asset if it is invalid
            if (asset == null)
            {
                moa_assets.Remove(asset);
                i--;
                continue;
            }
            V_DrawAssetListEntry(asset, oGuide);
        }
        GUI.EndScrollView();

        //Draw buttons at the bottom of the asset list
        Rect rectAssetButtons = rect;
        rectAssetButtons.height = 150;
        rectAssetButtons.y += 550;
        V_DrawListOfAssetsButtons(rectAssetButtons);

        mo_window.Repaint();

    }

    public virtual void V_DrawFileOutputArea(Rect rect)
    {
        EditorGUI.DrawRect(rect, ARX.Colors.EditorGray1);
        RectGuide oGuide = new RectGuide(rect);
        Rect oTitleLabel = oGuide.GetNextRect(rect.width);
        oGuide.NewLine();

        //Draw Buttons
        if (GUI.Button(oGuide.GetNextRect(120, 36), "Generate Process"))
        {
            mstr_fileOutput = I_GetLeftFileOutput();
        }
        if (GUI.Button(oGuide.GetNextRect(120, 36), "Generate Load AI"))
        {
            mstr_fileOutput = I_GetRightFileOutput();
        }

        oGuide.NewLine(2);
        Rect oContentRect = oGuide.GetNextRect(rect.width, 160);

        GUI.Label(oTitleLabel, "File Output Area");
        mstr_fileOutput = GUI.TextArea(oContentRect, mstr_fileOutput);
    }
    
    public void Draw(Rect rect)
    {
        RectGuide oGuide = new RectGuide(rect, 16, false, true);

        Rect oLabelRect = oGuide.PeekNextRect(150, 16);
        oLabelRect.x += 25;
        GUI.Label(oLabelRect, typeof(TAsset).ToString());

        oGuide.MoveLastRect(25, 25);

        Rect oListOfAssetsRect = oGuide.GetNextRect(200, 700);
        oGuide.MoveLastRect(25);

        Rect oStatAreaRect = oGuide.GetNextRect(400, 450);
        oGuide.Return();
        oGuide.MoveLastRect(250, 475);

        Rect oFileOutputRect = oGuide.GetNextRect(400, 225);

        V_DrawListOfAssets(oListOfAssetsRect);

        V_DrawStatArea(oStatAreaRect);

        V_DrawFileOutputArea(oFileOutputRect);
    }



    #endregion


}
