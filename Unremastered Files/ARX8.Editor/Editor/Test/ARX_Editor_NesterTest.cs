using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Panel : EditorWindow
{
    #region Constants

    #region Panel

    const float PANEL_WIDTH = 1024F;
    const float PANEL_HEIGHT = 768F;
    const float PANEL_X = 0F;
    const float PANEL_Y = 0F;
    readonly Color PANEL_COLOR = new Color(1F, 1F, 1F);
    private Rect GetPanelRect
    {
        get
        {
            return new Rect(PANEL_X, PANEL_Y, PANEL_WIDTH, PANEL_HEIGHT);
        }
    }


    #region Image

    const float IMAGE_WIDTH = 100F;
    const float IMAGE_HEIGHT = 100F;
    const float IMAGE_X = 462F;
    const float IMAGE_Y = 334F;
    readonly Color IMAGE_COLOR = new Color(1F, 1F, 1F);
    private Rect GetImageRect
    {
        get
        {
            return new Rect(IMAGE_X, IMAGE_Y, IMAGE_WIDTH, IMAGE_HEIGHT);
        }
    }


    #region LilImage

    const float LILIMAGE_WIDTH = 100F;
    const float LILIMAGE_HEIGHT = 100F;
    const float LILIMAGE_X = 462F;
    const float LILIMAGE_Y = 334F;
    readonly Color LILIMAGE_COLOR = new Color(1F, 1F, 1F);
    private Rect GetLilImageRect
    {
        get
        {
            return new Rect(LILIMAGE_X, LILIMAGE_Y, LILIMAGE_WIDTH, LILIMAGE_HEIGHT);
        }
    }


    #endregion // LilImage End

    #endregion // Image End

    #region MamaImage

    const float MAMAIMAGE_WIDTH = 100F;
    const float MAMAIMAGE_HEIGHT = 100F;
    const float MAMAIMAGE_X = 462F;
    const float MAMAIMAGE_Y = 334F;
    readonly Color MAMAIMAGE_COLOR = new Color(1F, 1F, 1F);
    private Rect GetMamaImageRect
    {
        get
        {
            return new Rect(MAMAIMAGE_X, MAMAIMAGE_Y, MAMAIMAGE_WIDTH, MAMAIMAGE_HEIGHT);
        }
    }


    #region AnotherImage

    const float ANOTHERIMAGE_WIDTH = 100F;
    const float ANOTHERIMAGE_HEIGHT = 100F;
    const float ANOTHERIMAGE_X = 462F;
    const float ANOTHERIMAGE_Y = 334F;
    readonly Color ANOTHERIMAGE_COLOR = new Color(1F, 1F, 1F);
    private Rect GetAnotherImageRect
    {
        get
        {
            return new Rect(ANOTHERIMAGE_X, ANOTHERIMAGE_Y, ANOTHERIMAGE_WIDTH, ANOTHERIMAGE_HEIGHT);
        }
    }


    #endregion // AnotherImage End

    #endregion // MamaImage End

    #region SuperImage

    const float SUPERIMAGE_WIDTH = 100F;
    const float SUPERIMAGE_HEIGHT = 100F;
    const float SUPERIMAGE_X = 462F;
    const float SUPERIMAGE_Y = 334F;
    readonly Color SUPERIMAGE_COLOR = new Color(1F, 1F, 1F);
    private Rect GetSuperImageRect
    {
        get
        {
            return new Rect(SUPERIMAGE_X, SUPERIMAGE_Y, SUPERIMAGE_WIDTH, SUPERIMAGE_HEIGHT);
        }
    }


    #region NotSoSuperImage

    const float NOTSOSUPERIMAGE_WIDTH = 100F;
    const float NOTSOSUPERIMAGE_HEIGHT = 100F;
    const float NOTSOSUPERIMAGE_X = 462F;
    const float NOTSOSUPERIMAGE_Y = 334F;
    readonly Color NOTSOSUPERIMAGE_COLOR = new Color(1F, 1F, 1F);
    private Rect GetNotSoSuperImageRect
    {
        get
        {
            return new Rect(NOTSOSUPERIMAGE_X, NOTSOSUPERIMAGE_Y, NOTSOSUPERIMAGE_WIDTH, NOTSOSUPERIMAGE_HEIGHT);
        }
    }


    #endregion // NotSoSuperImage End

    #endregion // SuperImage End

    #region TrumpImage

    const float TRUMPIMAGE_WIDTH = 100F;
    const float TRUMPIMAGE_HEIGHT = 100F;
    const float TRUMPIMAGE_X = 462F;
    const float TRUMPIMAGE_Y = 334F;
    readonly Color TRUMPIMAGE_COLOR = new Color(1F, 1F, 1F);
    private Rect GetTrumpImageRect
    {
        get
        {
            return new Rect(TRUMPIMAGE_X, TRUMPIMAGE_Y, TRUMPIMAGE_WIDTH, TRUMPIMAGE_HEIGHT);
        }
    }


    #region HomelanderImage

    const float HOMELANDERIMAGE_WIDTH = 100F;
    const float HOMELANDERIMAGE_HEIGHT = 100F;
    const float HOMELANDERIMAGE_X = 462F;
    const float HOMELANDERIMAGE_Y = 334F;
    readonly Color HOMELANDERIMAGE_COLOR = new Color(1F, 1F, 1F);
    private Rect GetHomelanderImageRect
    {
        get
        {
            return new Rect(HOMELANDERIMAGE_X, HOMELANDERIMAGE_Y, HOMELANDERIMAGE_WIDTH, HOMELANDERIMAGE_HEIGHT);
        }
    }


    #endregion // HomelanderImage End

    #endregion // TrumpImage End

    #endregion // Panel End

    #endregion //Constants
    
    #region Draw Functions
    void DrawPanel()
    {
        EditorGUI.DrawRect(GetPanelRect, PANEL_COLOR);
    }

    void DrawImage()
    {
        EditorGUI.DrawRect(GetImageRect, IMAGE_COLOR);
    }

    void DrawLilImage()
    {
        EditorGUI.DrawRect(GetLilImageRect, LILIMAGE_COLOR);
    }

    void DrawMamaImage()
    {
        EditorGUI.DrawRect(GetMamaImageRect, MAMAIMAGE_COLOR);
    }

    void DrawAnotherImage()
    {
        EditorGUI.DrawRect(GetAnotherImageRect, ANOTHERIMAGE_COLOR);
    }

    void DrawSuperImage()
    {
        EditorGUI.DrawRect(GetSuperImageRect, SUPERIMAGE_COLOR);
    }

    void DrawNotSoSuperImage()
    {
        EditorGUI.DrawRect(GetNotSoSuperImageRect, NOTSOSUPERIMAGE_COLOR);
    }

    void DrawTrumpImage()
    {
        EditorGUI.DrawRect(GetTrumpImageRect, TRUMPIMAGE_COLOR);
    }

    void DrawHomelanderImage()
    {
        EditorGUI.DrawRect(GetHomelanderImageRect, HOMELANDERIMAGE_COLOR);
    }

    private void OnGUI()
    {
        Draw();
    }

    void Draw()
    {
        DrawPanel();
        DrawImage();
        DrawLilImage();
        DrawMamaImage();
        DrawAnotherImage();
        DrawSuperImage();
        DrawNotSoSuperImage();
        DrawTrumpImage();
        DrawHomelanderImage();
    }


    #endregion
    
}
