using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ARX;
using UnityEditor;
using FEN;

/// <summary>
/// Something cool. God, I'm tired.
/// The given class template must be an Enum.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public abstract class ARX_EnumGenerator<TEnum>
{
    #region Constants
    const int WIDTH = 300;
    const int INTERNALVIEWHEIGHT = 500;
    #endregion

    #region Variables
    [SerializeField]
    public List<string> mastr_definedEnums = new List<string>();

    Rect mo_position;
    StringViewer mo_enumViewer;
    string mstr_overrideName = "";

    string mstr_fileOutput = "";

    Vector3 mvec_scrollPosition;
    #endregion

    #region Init and Constructors
    public ARX_EnumGenerator(Rect rectPosition, List<string> oaEnums)
    {
        mastr_definedEnums = oaEnums;
        mo_position = rectPosition;
        OnEnable();
    }

    public virtual void OnEnable()
    {
        //Set Enum output name
        string[] astrTypeNames = typeof(TEnum).ToString().Split('+');
        mstr_overrideName = astrTypeNames[astrTypeNames.Length - 1];


        //Initialize the list of definedEnums
        Array ar = System.Enum.GetValues(typeof(TEnum));
        foreach(TEnum t in ar)
        {
            if(mastr_definedEnums.Contains(t.ToString()) == false)
                mastr_definedEnums.Add(t.ToString());
        }
    }

    #endregion

    #region Rects


    public virtual Rect GetEnumGeneratorRect
    {
        get
        {
            return new Rect(mo_position.x, mo_position.y, WIDTH, mo_position.height);
        }
    }

    /// <summary>
    /// The dimensions for the list of Enums in the editor
    /// </summary>
    public virtual Rect GetEnumListExternalRectDimen
    {
        get
        {
            return new Rect(0,0, WIDTH, INTERNALVIEWHEIGHT);
        }
    }

    /// <summary>
    /// The dimensions for the internal scroll view of the list of enums
    /// </summary>
    public virtual Rect GetEnumListInternalRectDimen
    {
        get
        {
            const int nHeightOverhead = 150;
            int nInternalHeight = nHeightOverhead + (mastr_definedEnums.Count * 16);
            return new Rect(0, 0, WIDTH, nInternalHeight);
        }
    }
    #endregion

    #region Abstracts
    public abstract void GenerateMissingAssets();
    #endregion

    #region Functions

    public void Alphabetize()
    {
        Sort_Bubble_Alphebetize sorter = new Sort_Bubble_Alphebetize();
        mastr_definedEnums = sorter.Sort(mastr_definedEnums);
    }

    /// <summary>
    /// Generates an enum listing
    /// </summary>
    void GenerateText()
    {
        //If there are no defined enums, generate....nothing
        if (mastr_definedEnums.Count == 0)
        {
            mstr_fileOutput = "No Enums Defined";
            return;
        }

        ARX_VariableSpecs oEnmSpecs = new ARX_VariableSpecs();
        oEnmSpecs.me_output = ARX.VarGen.VariableGenOutputType.Enum;
        oEnmSpecs.me_security = ARX.VarGen.SecurityScope.Public;
        oEnmSpecs.mstr_name = mstr_overrideName;
        
        for(int i=0; i < mastr_definedEnums.Count; i++)
        {
            oEnmSpecs.GetArguments.Add(mastr_definedEnums[i].TrimWhitespace() );
        }

        mstr_fileOutput = oEnmSpecs.GetOutput();
    }

    public void Draw(RectGuide obufRect)
    {
        Rect genRect = obufRect.GetNextRect(GetEnumGeneratorRect.width, GetEnumGeneratorRect.height);

        RectGuide oGuide = new RectGuide(genRect);

        //Draw string viewer
        if (mo_enumViewer == null)
        {
            mo_enumViewer = new StringViewer();
            mo_enumViewer.mn_slideWidth = 225;
            GenerateText();
        }

        //Begin Scroll View for enum viewer
        Rect rectTitle = oGuide.PeekNextRect(WIDTH, 32);
        rectTitle.y -= 32;

        Rect rectScrollViewExternal = oGuide.GetNextRect(WIDTH, INTERNALVIEWHEIGHT);
        EditorGUI.DrawRect(rectScrollViewExternal, ARX.Colors.EditorGray1);

        mvec_scrollPosition = GUI.BeginScrollView(rectScrollViewExternal, mvec_scrollPosition, GetEnumListInternalRectDimen);


        RectGuide oScrollViewGuide = new RectGuide(new Rect(0,0,WIDTH, INTERNALVIEWHEIGHT));

        mo_enumViewer.V_Draw(oScrollViewGuide, mastr_definedEnums);
        GUI.EndScrollView();
        //End Scroll View for enum viewer

        oGuide.NewLine();
        oGuide.MoveLastRect(0, INTERNALVIEWHEIGHT - 10);

        //Draw the Enum Title
        GUI.Label(rectTitle, typeof(TEnum).ToString());
        //EditorGUI.DrawRect(rectTitle, Color.blue);

        //Generate the Enum Text every frame
        GenerateText();
        
        if (GUI.Button(oGuide.GetNextRect(100, 25), "Reinitiailze"))
        {
            OnEnable();
        }

        if (GUI.Button(oGuide.GetNextRect(100, 25), "Alphabetize"))
        {
            Alphabetize();
        }
        
        if (GUI.Button(oGuide.GetNextRect(100, 25), "Generate Assets"))
        {
            GenerateMissingAssets();
        }

        oGuide.NewLine(3);

        //File output area
        GUI.TextArea(oGuide.GetNextRect(mo_position.width, 80), mstr_fileOutput);

    }
    #endregion

}

public class UnitEnumGenerator:ARX_EnumGenerator<CardIDs.UNITID>
{
    public UnitEnumGenerator(Rect rectPosition, List<string> oaEnums):base(rectPosition, oaEnums)
    {

    }

    public override void GenerateMissingAssets()
    {
        //Get a list of all assets
        List<FEN_Unit> oaUnitsInEditor = EditorHelper.FindAssetsByType<FEN_Unit>();


        List<string> oaBuf = new List<string>(mastr_definedEnums);


        foreach(FEN_Unit uni in oaUnitsInEditor)
        {
            string strUpper = uni.name.ToUpper();
            if (oaBuf.Contains(strUpper))
                oaBuf.Remove(strUpper);
        }

        foreach(string strDefinedEnum in oaBuf)
        {
            string strFileName = strDefinedEnum.ToLower();
            FEN_Unit oNewUnit = EditorHelper.CreateAsset<FEN_Unit>(strFileName, false, FEN_Project.UnitFolderPath);
            oNewUnit.mstr_name = strDefinedEnum;
        }

        //For each asset
            //If the asset's name is one of the enum's, then its already created
                //Skip that asset
            //Else, Create a new asset with the enum's name
    }
}

public class AbilityEnumGenerator : ARX_EnumGenerator<CardIDs.CARDID>
{
    public AbilityEnumGenerator(Rect rectPosition, List<string> oaEnums) : base(rectPosition, oaEnums)
    {

    }

    public override void GenerateMissingAssets()
    {
        //Get a list of all assets
        List<FEN_Ability> oaAbilitiesInEditor = EditorHelper.FindAssetsByType<FEN_Ability>();

        List<string> oaBuf = new List<string>(mastr_definedEnums);

        foreach (FEN_Ability ab in oaAbilitiesInEditor)
        {
            string strUpper = ab.name.ToUpper();
            if (oaBuf.Contains(strUpper))
                oaBuf.Remove(strUpper);
        }

        foreach (string strDefinedEnum in oaBuf)
        {
            string strFileName = strDefinedEnum.ToLower();
            FEN_Ability oNewAbility = EditorHelper.CreateAsset<FEN_Ability>(strFileName, false, FEN_Project.AbilityFolderPath);
            oNewAbility.mstr_cardName = strDefinedEnum;
        }
    }
}
