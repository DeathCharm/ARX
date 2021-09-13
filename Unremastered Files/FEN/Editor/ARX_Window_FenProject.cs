using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;
using FEN;



public class ARX_Window_FenProject : EditorWindow
{

    #region Variables
    FEN_Project mo_fenProject;

    enum GENERATORVIEW {ABILITY, UNIT }
    GENERATORVIEW me_generatorView = GENERATORVIEW.ABILITY;
    GENERATORVIEW me_assetView = GENERATORVIEW.ABILITY;

    [SerializeField]
    StringViewer oAttributeListViewer;

    [SerializeField]
    ARX_EnumGenerator<CardIDs.CARDID> mo_abilityIDGenerator;

    [SerializeField]
    ARX_EnumGenerator<CardIDs.UNITID> mo_unitIDGenerator;

    [SerializeField]
    AbilityForge mo_abilityAssetsForge;

    [SerializeField]
    UnitForge mo_unitAssetsForge;
    
    #endregion

    #region Helper
    
    void AddAttribute(string str)
    {
        if (me_assetView == GENERATORVIEW.ABILITY)
        {
            if (mo_abilityAssetsForge.mo_highlightedAsset != null)
                mo_abilityAssetsForge.mo_highlightedAsset.AddAttribute(str);
        }
        else if (me_assetView == GENERATORVIEW.UNIT)
        {
            if (mo_unitAssetsForge.mo_highlightedAsset != null)
                mo_unitAssetsForge.mo_highlightedAsset.AddAttribute(str);
        }
    }

    /// <summary>
    /// The position of the enum creation window
    /// </summary>
    public static Rect GetEnumGenRect
    {
        get
        {
            return new Rect(0, 30, 300, 700);
        }
    }

    /// <summary>
    /// The editor window rect
    /// </summary>
    public static Rect GetWindowRect
    {
        get
        {
            return new Rect(0, 0, 1400, 800);
        }
    }

    /// <summary>
    /// The editor window rect
    /// </summary>
    public static Rect GetMainContentRect
    {
        get
        {
            return new Rect(0, 25, 1300, 775);
        }
    }

    [MenuItem("ARX/Fen Project Test")]
    public static void ShowWindow()
    {
        EditorWindow oWindow = GetWindowWithRect<ARX_Window_FenProject>(GetWindowRect);
    }
    #endregion

    #region Unity Overides
    private void OnDestroy()
    {
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Load a FEN Project asset. If none exists, create one in the Hierarchy.
    /// </summary>
    void LoadFenProject()
    {
        List<FEN_Project> oaProjectsInHierarchy = EditorHelper.FindAssetsByType<FEN_Project>();
        if (oaProjectsInHierarchy.Count == 0)
        {
            //Create a new Fen Project asset
            FEN_Project pr = EditorHelper.CreateAsset<FEN_Project>("New FEN Project");
            mo_fenProject = pr;
        }
        else
            mo_fenProject = oaProjectsInHierarchy[0];
    }

    private void OnEnable()
    {
        LoadFenProject();

        if (mo_abilityIDGenerator == null)
            mo_abilityIDGenerator = new AbilityEnumGenerator(GetEnumGenRect, mo_fenProject.mastr_abilityEnums);
        if (mo_unitIDGenerator == null)
            mo_unitIDGenerator = new UnitEnumGenerator(GetEnumGenRect, mo_fenProject.mastr_unitEnums);
        if (mo_unitAssetsForge == null)
            mo_unitAssetsForge = new UnitForge(this);
        if (mo_abilityAssetsForge == null)
            mo_abilityAssetsForge = new AbilityForge(this);
    }

    private void OnFocus()
    {
        mo_abilityAssetsForge.OnEnable();
        mo_unitAssetsForge.OnEnable();
    }

    private void OnGUI()
    {
        Draw();
    }
    #endregion

    #region Draw

    void Draw()
    {
        RectGuide oGuide = new RectGuide(GetMainContentRect, 16, false, true);

        //Draw buttons to switch between the Ability and Unit enum gen
        if (GUI.Button(oGuide.GetNextRect(100, 30), "Ability"))
        {
            me_generatorView = GENERATORVIEW.ABILITY;
        }
        if (GUI.Button(oGuide.GetNextRect(100, 30), "Unit"))
        {
            me_generatorView = GENERATORVIEW.UNIT;
        }

        oGuide.NewLine(4);

        //Draw either the ability enum gen or the unit enum gen
        if (me_generatorView == GENERATORVIEW.ABILITY)
        {
            mo_abilityIDGenerator.Draw(oGuide);
            mo_fenProject.mastr_abilityEnums = mo_abilityIDGenerator.mastr_definedEnums;
        }
        else
        {
            mo_unitIDGenerator.Draw(oGuide);
            mo_fenProject.mastr_unitEnums = mo_unitIDGenerator.mastr_definedEnums;
        }

        oGuide.NewLine();
        
        //Draw the main section of the project view
        oGuide.MoveLastRect(325, -50);
        Rect oContentRect = oGuide.GetNextRect(675, 725);

        oGuide.MoveLastRect(10, 0);

        Rect oAttributeListArea = oGuide.PeekNextRect(350, 725);

        DrawAttributeList(oAttributeListArea);

        RectGuide oTitleGuide = new RectGuide(oContentRect);
        oTitleGuide.MoveBoundingRect(250);
        //Draw buttons to switch between the Ability and Unit enum gen
        if (GUI.Button(oTitleGuide.GetNextRect(100, 30), "Ability"))
        {
            me_assetView = GENERATORVIEW.ABILITY;
            Debug.Log("Well, it's sensing this just fine...");
            mo_fenProject.mastr_attributes = mo_fenProject.Alphebetize(mo_fenProject.mastr_attributes);
        }
        if (GUI.Button(oTitleGuide.GetNextRect(100, 30), "Unit"))
        {
            me_assetView = GENERATORVIEW.UNIT;
            Debug.Log("Well, it's sensing this just fine...");
            mo_fenProject.mastr_attributes = mo_fenProject.Alphebetize(mo_fenProject.mastr_attributes);
        }

        //Draw Content Background
        GUIStyle oStyle = EditorStyles.helpBox;
        GUI.Box(oContentRect, "", oStyle);


        //Draw Asset Content
        if (me_assetView == GENERATORVIEW.ABILITY)
            mo_abilityAssetsForge.Draw(oContentRect);
        else if (me_assetView == GENERATORVIEW.UNIT)
            mo_unitAssetsForge.Draw(oContentRect);

    }


    Vector2 mvecAttributeScroll;
    void DrawAttributeList(Rect oRect)
    {
        int nShrinkHeight = 150;
        oRect.height -= nShrinkHeight;

        Rect oExternalScroll = oRect;
        EditorGUI.DrawRect(oExternalScroll, Colors.EditorGray1);


        //Draw attribute list
        if (oAttributeListViewer == null)
        {
            oAttributeListViewer = new StringViewer();
            oAttributeListViewer.mstr_title += "Attribute List";
            oAttributeListViewer.mn_slideWidth = 200;
            oAttributeListViewer.AddButton(AddAttribute, "Add", 36, 16);
        }

        //Draw Scroll View
        {
            int nAttributeListHeight = 16 * mo_fenProject.mastr_attributes.Count + 150;
            Rect oInternalRect = new Rect(0, 0, oExternalScroll.width, nAttributeListHeight);
            mvecAttributeScroll = GUI.BeginScrollView(oExternalScroll, mvecAttributeScroll, oInternalRect);
            RectGuide oGuide = new RectGuide(oInternalRect);
            oAttributeListViewer.V_Draw(oGuide, mo_fenProject.mastr_attributes);
            GUI.EndScrollView();
        }

        Rect oBottomRect = oRect;
        oBottomRect.height = 125;
        oBottomRect.y += oRect.height + 40;

        Rect oAlphabetizeButton = oBottomRect;
        oAlphabetizeButton.width = 200;
        oAlphabetizeButton.height = 30;
        oAlphabetizeButton.y -= 35;

        EditorGUI.DrawRect(oBottomRect, Colors.EditorGray1);
        GUI.TextArea(oBottomRect, FEN.AutoCodeCalculations.GetAttributeListFIleOutput(mo_fenProject.mastr_attributes));

        if (GUI.Button(oAlphabetizeButton, "Alphebetize Attribute List"))
        {

            Debug.Log("Sensing click on Alphebetize button...?");
            mo_fenProject.mastr_attributes = mo_fenProject.Alphebetize(mo_fenProject.mastr_attributes);
            EditorUtility.SetDirty(mo_fenProject);

        }


    }

    #endregion

    #region Frame Updates
    int nInspectorUpdateFrame = 0;
    private void OnInspectorUpdate()
    {
        if (nInspectorUpdateFrame % 4 == 0) 
        {
            Repaint(); //force the window to repaint
        }
        nInspectorUpdateFrame++;
    }
    #endregion
}
