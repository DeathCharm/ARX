using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;



public class TestPaletteEntry {
    public bool isActive = true;
    public string description = "";
    public Sprite sprite;

    public TestPaletteEntry() { }
    public TestPaletteEntry(string strDescription, bool bActive = true)
    {
        description = strDescription;
        isActive = bActive;
    }
}

public class ARX_NodePalette : ARX_EditorScrollView<TestPaletteEntry>
{
    public ARX_NodePalette(WindowDirection eShape, List<TestPaletteEntry> list, Rect rectMainImageRect, Rect rectImage, bool bDrawInactive = false) : 
        base(eShape, list, rectMainImageRect, rectImage,  bDrawInactive)
    {

    }


    public override void I_DrawBackground(Rect rectEditorPosition)
    {
        ZEditor.DrawBackgroundBox(rectEditorPosition, Color.gray, 3);
    }

    public override void I_DrawItem(TestPaletteEntry item, Rect rect)
    {
        ZEditor.DrawLabelWithBackground(rect, item.description, QuickEditor.QColors.Color.Blue);
        if (GUI.Button(rect, ""))
            item.isActive = !item.isActive;
    }
    
    public override bool I_IsToBeDrawn(TestPaletteEntry item)
    {
        if (item.isActive == false)
            return false;
        return true;
    }

    public override void V_DrawInactive(TestPaletteEntry item, Rect rect)
    {
        ZEditor.DrawLabelWithBackground(rect, "INACTIVE", QuickEditor.QColors.Color.Gray);
        if (GUI.Button(rect, ""))
            item.isActive = !item.isActive;
    }
}

public class AutoPaletteGen : EditorWindow
{

    static Rect GetWindowRect
    {
        get
        {
            Rect rect = new Rect(100, 100, 800, 800);
            return rect;
        }
    }
    static Rect GetPaletteRect
    {
        get
        {
            Rect rect = new Rect(0, 0, 800, 250);
            return rect;
        }
    }

    static Rect GetItemRect
    {
        get
        {
            Rect rect = new Rect(0, 0, 120, 40);
            return rect;
        }
    }


    [MenuItem("ArxTest/Palette Gen")]
    public static void ShowWindow() { EditorWindow oWindow = GetWindowWithRect<AutoPaletteGen>(GetWindowRect); }

    List<TestPaletteEntry> mo_list;
    ARX_NodePalette palette;

    void Initialize()
    {
        mo_list = new List<TestPaletteEntry>();

        mo_list.Add(new TestPaletteEntry("First item"));
        mo_list.Add(new TestPaletteEntry("Second item"));
        mo_list.Add(new TestPaletteEntry("Third item", false));
        mo_list.Add(new TestPaletteEntry("Fourth item"));
        mo_list.Add(new TestPaletteEntry("Fifth item", false));
        mo_list.Add(new TestPaletteEntry("Sixth item"));
        mo_list.Add(new TestPaletteEntry("Seventh item"));
        mo_list.Add(new TestPaletteEntry("Eigth item", false));
        mo_list.Add(new TestPaletteEntry("Ninth item "));
        palette = new ARX_NodePalette(WindowDirection.HORIZONTAL, mo_list, GetPaletteRect, GetItemRect, false);
        palette.mnf_padding = 3;
    }

    private void Awake()
    {
        Initialize();
    }

    private void OnGUI()
    {

        if (palette == null)
            Initialize();

            palette.Draw();
    }

}