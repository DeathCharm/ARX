using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Assets.ZRPG.Editor
{


    //public class MyWindow : EditorWindow
    //{
    //    static Rect GetWindowRect
    //    {
    //        get
    //        {
    //            Rect rect = new Rect(0, 0, ZNODEEDITOR_WIDTH, ZNODEEDITOR_HEIGHT);
    //            return rect;
    //        }
    //    }

    //    [MenuItem("ZRPG/Node Editor")]
    //    public static void ShowWindow() { EditorWindow oWindow = GetWindowWithRect<MyWindow>(GetWindowRect); }


    //    #region Constants

    //    #region ZNodeEditor

    //    const float ZNODEEDITOR_WIDTH = 1024F;
    //    const float ZNODEEDITOR_HEIGHT = 768F;
    //    const float ZNODEEDITOR_X = 0F;
    //    const float ZNODEEDITOR_Y = 0F;
    //    readonly Color ZNODEEDITOR_COLOR = new Color(23, 255, 0);
    //    private Rect GetZNodeEditorRect
    //    {
    //        get
    //        {
    //            return new Rect(0F, 0F, 1024F, 768F);
    //        }
    //    }


    //    #region GridWindow

    //    const float GRIDWINDOW_WIDTH = 463.7484F;
    //    const float GRIDWINDOW_HEIGHT = 467.5523F;
    //    const float GRIDWINDOW_X = 15.70001F;
    //    const float GRIDWINDOW_Y = -20.164F;
    //    readonly Color GRIDWINDOW_COLOR = new Color(0, 0, 0);
    //    private Rect GetGridWindowRect
    //    {
    //        get
    //        {
    //            return new Rect(15.70001F, -20.164F, 463.7484F, 467.5523F);
    //        }
    //    }


    //    #region GridWindow_Info

    //    const float GRIDWINDOW_INFO_WIDTH = 463.7484F;
    //    const float GRIDWINDOW_INFO_HEIGHT = 25.82501F;
    //    const float GRIDWINDOW_INFO_X = 15.70001F;
    //    const float GRIDWINDOW_INFO_Y = 226.5246F;
    //    readonly Color GRIDWINDOW_INFO_COLOR = new Color(0, 0, 0);
    //    private Rect GetGridWindow_InfoRect
    //    {
    //        get
    //        {
    //            return new Rect(15.70001F, 226.5246F, 463.7484F, 25.82501F);
    //        }
    //    }


    //    #endregion // GridWindow_Info End

    //    #endregion // GridWindow End

    //    #region NodePalette

    //    const float NODEPALETTE_WIDTH = 281.0306F;
    //    const float NODEPALETTE_HEIGHT = 633.8084F;
    //    const float NODEPALETTE_X = -356.69F;
    //    const float NODEPALETTE_Y = -46.56F;
    //    readonly Color NODEPALETTE_COLOR = new Color(0, 0, 0);
    //    private Rect GetNodePaletteRect
    //    {
    //        get
    //        {
    //            return new Rect(-356.69F, -46.56F, 281.0306F, 633.8084F);
    //        }
    //    }


    //    #region NodePalette_Info

    //    const float NODEPALETTE_INFO_WIDTH = 281.0306F;
    //    const float NODEPALETTE_INFO_HEIGHT = 76.72046F;
    //    const float NODEPALETTE_INFO_X = -356.69F;
    //    const float NODEPALETTE_INFO_Y = 308.8F;
    //    readonly Color NODEPALETTE_INFO_COLOR = new Color(0, 0, 0);
    //    private Rect GetNodePalette_InfoRect
    //    {
    //        get
    //        {
    //            return new Rect(-356.69F, 308.8F, 281.0306F, 76.72046F);
    //        }
    //    }


    //    #endregion // NodePalette_Info End

    //    #endregion // NodePalette End

    //    #region MenuBar

    //    const float MENUBAR_WIDTH = 714.8285F;
    //    const float MENUBAR_HEIGHT = 108.8212F;
    //    const float MENUBAR_X = 141.2F;
    //    const float MENUBAR_Y = 293.8F;
    //    readonly Color MENUBAR_COLOR = new Color(0, 0, 0);
    //    private Rect GetMenuBarRect
    //    {
    //        get
    //        {
    //            return new Rect(141.2F, 293.8F, 714.8285F, 108.8212F);
    //        }
    //    }


    //    #endregion // MenuBar End

    //    #region Inspector

    //    const float INSPECTOR_WIDTH = 251.04F;
    //    const float INSPECTOR_HEIGHT = 493.3298F;
    //    const float INSPECTOR_X = 373.09F;
    //    const float INSPECTOR_Y = -7.275208F;
    //    readonly Color INSPECTOR_COLOR = new Color(0, 0, 0);
    //    private Rect GetInspectorRect
    //    {
    //        get
    //        {
    //            return new Rect(373.09F, -7.275208F, 251.04F, 493.3298F);
    //        }
    //    }


    //    #endregion // Inspector End

    //    #region InfoBar

    //    const float INFOBAR_WIDTH = 714.8285F;
    //    const float INFOBAR_HEIGHT = 108.8212F;
    //    const float INFOBAR_X = 141.2F;
    //    const float INFOBAR_Y = -309F;
    //    readonly Color INFOBAR_COLOR = new Color(0, 0, 0);
    //    private Rect GetInfoBarRect
    //    {
    //        get
    //        {
    //            return new Rect(141.2F, -309F, 714.8285F, 108.8212F);
    //        }
    //    }


    //    #endregion // InfoBar End

    //    #region TitleBar

    //    const float TITLEBAR_WIDTH = 995.8196F;
    //    const float TITLEBAR_HEIGHT = 35.78943F;
    //    const float TITLEBAR_X = 0.7044678F;
    //    const float TITLEBAR_Y = 366.11F;
    //    readonly Color TITLEBAR_COLOR = new Color(0, 0, 0);
    //    private Rect GetTitleBarRect
    //    {
    //        get
    //        {
    //            return new Rect(0.7044678F, 366.11F, 995.8196F, 35.78943F);
    //        }
    //    }


    //    #endregion // TitleBar End

    //    #endregion // ZNodeEditor End

    //    #endregion //Constants


    //    #region Draw Functions
    //    void DrawZNodeEditor()
    //    {
    //        EditorGUI.DrawRect(GetZNodeEditorRect, ZNODEEDITOR_COLOR);
    //    }

    //    void DrawGridWindow()
    //    {
    //        EditorGUI.DrawRect(GetGridWindowRect, GRIDWINDOW_COLOR);
    //    }

    //    void DrawGridWindow_Info()
    //    {
    //        EditorGUI.DrawRect(GetGridWindow_InfoRect, GRIDWINDOW_INFO_COLOR);
    //    }

    //    void DrawNodePalette()
    //    {
    //        EditorGUI.DrawRect(GetNodePaletteRect, NODEPALETTE_COLOR);
    //    }

    //    void DrawNodePalette_Info()
    //    {
    //        EditorGUI.DrawRect(GetNodePalette_InfoRect, NODEPALETTE_INFO_COLOR);
    //    }

    //    void DrawMenuBar()
    //    {
    //        EditorGUI.DrawRect(GetMenuBarRect, MENUBAR_COLOR);
    //    }

    //    void DrawInspector()
    //    {
    //        EditorGUI.DrawRect(GetInspectorRect, INSPECTOR_COLOR);
    //    }

    //    void DrawInfoBar()
    //    {
    //        EditorGUI.DrawRect(GetInfoBarRect, INFOBAR_COLOR);
    //    }

    //    void DrawTitleBar()
    //    {
    //        EditorGUI.DrawRect(GetTitleBarRect, TITLEBAR_COLOR);
    //    }

    //    void Draw()
    //    {
    //        DrawZNodeEditor();
    //        DrawGridWindow();
    //        DrawGridWindow_Info();
    //        DrawNodePalette();
    //        DrawNodePalette_Info();
    //        DrawMenuBar();
    //        DrawInspector();
    //        DrawInfoBar();
    //        DrawTitleBar();
    //    }


    //    private void OnGUI()
    //    {
    //        Draw();
    //    }
    //    #endregion

    //}


    public class MyWindow : EditorWindow
    {
        static Rect GetWindowRect
        {
            get
            {
                Rect rect = new Rect(0, 0, ZNODEEDITOR_WIDTH, ZNODEEDITOR_HEIGHT);
                return rect;
            }
        }

        [MenuItem("ZRPG/Node Editor")]
        public static void ShowWindow() { EditorWindow oWindow = GetWindowWithRect<MyWindow>(GetWindowRect); }

        #region Constants

        #region ZNodeEditor

        const float ZNODEEDITOR_WIDTH = 1024F;
        const float ZNODEEDITOR_HEIGHT = 768F;
        const float ZNODEEDITOR_X = 0F;
        const float ZNODEEDITOR_Y = 0F;
        readonly Color ZNODEEDITOR_COLOR = new Color(0.09040952F, 1F, 0F);
        private Rect GetZNodeEditorRect
        {
            get
            {
                return new Rect(ZNODEEDITOR_X, ZNODEEDITOR_Y, ZNODEEDITOR_WIDTH, ZNODEEDITOR_HEIGHT);
            }
        }


        #region GridWindow

        const float GRIDWINDOW_WIDTH = 463.7484F;
        const float GRIDWINDOW_HEIGHT = 467.5523F;
        const float GRIDWINDOW_X = 295.8258F;
        const float GRIDWINDOW_Y = 170.3878F;
        readonly Color GRIDWINDOW_COLOR = new Color(0.2754233F, 0.06127627F, 0.764151F);
        private Rect GetGridWindowRect
        {
            get
            {
                return new Rect(GRIDWINDOW_X, GRIDWINDOW_Y, GRIDWINDOW_WIDTH, GRIDWINDOW_HEIGHT);
            }
        }


        #region GridWindow_Info

        const float GRIDWINDOW_INFO_WIDTH = 463.7484F;
        const float GRIDWINDOW_INFO_HEIGHT = 25.82501F;
        const float GRIDWINDOW_INFO_X = 295.8258F;
        const float GRIDWINDOW_INFO_Y = 144.5629F;
        readonly Color GRIDWINDOW_INFO_COLOR = new Color(0F, 0F, 0F);
        private Rect GetGridWindow_InfoRect
        {
            get
            {
                return new Rect(GRIDWINDOW_INFO_X, GRIDWINDOW_INFO_Y, GRIDWINDOW_INFO_WIDTH, GRIDWINDOW_INFO_HEIGHT);
            }
        }


        #endregion // GridWindow_Info End

        #endregion // GridWindow End

        #region NodePalette

        const float NODEPALETTE_WIDTH = 281.0306F;
        const float NODEPALETTE_HEIGHT = 633.8084F;
        const float NODEPALETTE_X = 14.79468F;
        const float NODEPALETTE_Y = 113.6558F;
        readonly Color NODEPALETTE_COLOR = new Color(0F, 0.3255157F, 1F);
        private Rect GetNodePaletteRect
        {
            get
            {
                return new Rect(NODEPALETTE_X, NODEPALETTE_Y, NODEPALETTE_WIDTH, NODEPALETTE_HEIGHT);
            }
        }


        #region NodePalette_Info

        const float NODEPALETTE_INFO_WIDTH = 281.0306F;
        const float NODEPALETTE_INFO_HEIGHT = 76.72046F;
        const float NODEPALETTE_INFO_X = 14.79468F;
        const float NODEPALETTE_INFO_Y = 36.83978F;
        readonly Color NODEPALETTE_INFO_COLOR = new Color(0F, 0.07410216F, 1F);
        private Rect GetNodePalette_InfoRect
        {
            get
            {
                return new Rect(NODEPALETTE_INFO_X, NODEPALETTE_INFO_Y, NODEPALETTE_INFO_WIDTH, NODEPALETTE_INFO_HEIGHT);
            }
        }


        #endregion // NodePalette_Info End

        #endregion // NodePalette End

        #region MenuBar

        const float MENUBAR_WIDTH = 714.8285F;
        const float MENUBAR_HEIGHT = 108.8212F;
        const float MENUBAR_X = 295.7858F;
        const float MENUBAR_Y = 35.78943F;
        readonly Color MENUBAR_COLOR = new Color(0.8822016F, 1F, 0F);
        private Rect GetMenuBarRect
        {
            get
            {
                return new Rect(MENUBAR_X, MENUBAR_Y, MENUBAR_WIDTH, MENUBAR_HEIGHT);
            }
        }


        #endregion // MenuBar End

        #region Inspector

        const float INSPECTOR_WIDTH = 251.04F;
        const float INSPECTOR_HEIGHT = 493.3298F;
        const float INSPECTOR_X = 759.5699F;
        const float INSPECTOR_Y = 144.6103F;
        readonly Color INSPECTOR_COLOR = new Color(1F, 0.06132078F, 0.06132078F);
        private Rect GetInspectorRect
        {
            get
            {
                return new Rect(INSPECTOR_X, INSPECTOR_Y, INSPECTOR_WIDTH, INSPECTOR_HEIGHT);
            }
        }


        #endregion // Inspector End

        #region InfoBar

        const float INFOBAR_WIDTH = 714.8285F;
        const float INFOBAR_HEIGHT = 108.8212F;
        const float INFOBAR_X = 295.7858F;
        const float INFOBAR_Y = 638.5894F;
        readonly Color INFOBAR_COLOR = new Color(0F, 0.9150943F, 0.7346963F);
        private Rect GetInfoBarRect
        {
            get
            {
                return new Rect(INFOBAR_X, INFOBAR_Y, INFOBAR_WIDTH, INFOBAR_HEIGHT);
            }
        }


        #endregion // InfoBar End

        #region TitleBar

        const float TITLEBAR_WIDTH = 995.8196F;
        const float TITLEBAR_HEIGHT = 35.78943F;
        const float TITLEBAR_X = 14.79468F;
        const float TITLEBAR_Y = -0.004699707F;
        readonly Color TITLEBAR_COLOR = new Color(0.8931503F, 0.1290495F, 0.9433962F);
        private Rect GetTitleBarRect
        {
            get
            {
                return new Rect(TITLEBAR_X, TITLEBAR_Y, TITLEBAR_WIDTH, TITLEBAR_HEIGHT);
            }
        }


        #endregion // TitleBar End

        #endregion // ZNodeEditor End

        #endregion //Constants

        #region Draw Functions
        void DrawZNodeEditor()
        {
            EditorGUI.DrawRect(GetZNodeEditorRect, ZNODEEDITOR_COLOR);
        }

        void DrawGridWindow()
        {
            EditorGUI.DrawRect(GetGridWindowRect, GRIDWINDOW_COLOR);
        }

        void DrawGridWindow_Info()
        {
            EditorGUI.DrawRect(GetGridWindow_InfoRect, GRIDWINDOW_INFO_COLOR);
        }

        void DrawNodePalette()
        {
            EditorGUI.DrawRect(GetNodePaletteRect, NODEPALETTE_COLOR);
        }

        void DrawNodePalette_Info()
        {
            EditorGUI.DrawRect(GetNodePalette_InfoRect, NODEPALETTE_INFO_COLOR);
        }

        void DrawMenuBar()
        {
            EditorGUI.DrawRect(GetMenuBarRect, MENUBAR_COLOR);
        }

        void DrawInspector()
        {
            EditorGUI.DrawRect(GetInspectorRect, INSPECTOR_COLOR);
        }

        void DrawInfoBar()
        {
            EditorGUI.DrawRect(GetInfoBarRect, INFOBAR_COLOR);
        }

        void DrawTitleBar()
        {
            EditorGUI.DrawRect(GetTitleBarRect, TITLEBAR_COLOR);
        }

        private void OnGUI()
        {
            Draw();
        }

        void Draw()
        {
            DrawZNodeEditor();
            DrawGridWindow();
            DrawGridWindow_Info();
            DrawNodePalette();
            DrawNodePalette_Info();
            DrawMenuBar();
            DrawInspector();
            DrawInfoBar();
            DrawTitleBar();
        }


        #endregion
    }

}
