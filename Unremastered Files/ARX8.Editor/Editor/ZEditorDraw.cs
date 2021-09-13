using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using ARX;

using QuickEngine.Core;
using UnityEditor.AnimatedValues;
using QuickEditor;

public static class ZEditor
{
    static bool bInitialized = false;
    public static void Initialize()
    {
        if (bInitialized)
            return;

        bInitialized = true;
        IMG.Initialize();
    }
    public static class IMG
    {
        public static Texture2D RoundedRect_50 = null;

        public static void Initialize()
        {
            RoundedRect_50 = Resources.Load<Texture2D>("button_roundedrect_50");
        }

    }

    public static class Styles
    {
        static GUIStyle _dropZoneBoxStyle;
        public static GUIStyle DropZoneBoxStyle
        {
            get
            {
                if (_dropZoneBoxStyle == null)
                {
                    _dropZoneBoxStyle = new GUIStyle
                    {
                        normal = { background = QResources.backgroundLowGray.normal2D, textColor = QUI.IsProSkin ? QColors.GreenLight.Color : QColors.GreenDark.Color },
                        hover = { background = QResources.backgroundLowGreen.normal2D, textColor = QUI.IsProSkin ? QColors.GreenLight.Color : QColors.GreenDark.Color },
                        font = QResources.GetFont(FontName.Ubuntu.Light),
                        fontSize = 12,
                        alignment = TextAnchor.MiddleCenter,
                        border = new RectOffset(8, 8, 8, 8)
                    };
                }
                return _dropZoneBoxStyle;
            }
        }

        static GUIStyle _itemQuickViewTextStyle;
        public static GUIStyle ItemQuickViewTextStyle
        {
            get
            {
                if (_itemQuickViewTextStyle == null)
                {
                    _itemQuickViewTextStyle = new GUIStyle
                    {
                        normal = { textColor = QUI.IsProSkin ? QColors.GreenLight.Color : QColors.GreenDark.Color },
                        font = QUI.IsProSkin ? QResources.GetFont(FontName.Ubuntu.Regular) : QResources.GetFont(FontName.Ubuntu.Bold),
                        fontStyle = FontStyle.Normal,
                        fontSize = 13,
                        alignment = TextAnchor.MiddleLeft,
                        padding = new RectOffset(0, 0, 0, 2)
                    };
                }
                return _itemQuickViewTextStyle;
            }
        }
    }

    static Color savedBackgroudnColor;
    static void SaveColor() { savedBackgroudnColor = GUI.backgroundColor; }
    public static void SetColor(Color oColor) { GUI.backgroundColor = oColor; }
    public static void ResetColor() { GUI.backgroundColor = savedBackgroudnColor; }

    public static GUIStyle MiniButtonStyle
    {
        get
        {
            GUIStyle oStyle = EditorStyles.miniButton;
            //oStyle.normal.background = ZEditor.IMG.RoundedRect_50;
            //oStyle.stretchHeight = true;
            oStyle.fixedHeight = 100;
            oStyle.active.textColor = Color.cyan;
            oStyle.hover.background = ZEditor.IMG.RoundedRect_50;
            return oStyle;
        }
    }

    public static GUIStyle OptionButtonStyle
    {
        get
        {
            GUIStyle oStyle = EditorStyles.miniButton;
            //oStyle.normal.background = ZEditor.IMG.RoundedRect_50;
            //oStyle.stretchHeight = true;
            oStyle.fixedHeight = 40;
            oStyle.fixedWidth = 80;
            oStyle.active.textColor = Color.cyan;
            oStyle.hover.background = ZEditor.IMG.RoundedRect_50;
            return oStyle;
        }
    }

    public static bool Button(Rect oPosition, GUIContent content, Color oColor)
    {
        SaveColor();
        SetColor(oColor);

        GUIStyle oStyle = EditorStyles.miniButton;
        //oStyle.normal.background = ZEditor.IMG.RoundedRect_50;
        //oStyle.stretchHeight = true;
        oStyle.fixedHeight = 100;
        oStyle.active.textColor = Color.cyan;
        oStyle.hover.background = ZEditor.IMG.RoundedRect_50;

        bool bClicked = GUI.Button(oPosition, content, oStyle);
        ResetColor();
        return bClicked;
    }

    public static bool Button(Rect oPosition, string str, Color oColor)
    {
        GUIContent oContent = new GUIContent();
        oContent.text = str;
        return Button(oPosition, oContent, oColor);
    }

    public static void DrawBackgroundBox(RectGuide oGuide, Color color, float nMargins)
    {
        Rect backgroundRect = oGuide.BoundingRect;
        DrawBackgroundBox(backgroundRect, color, nMargins);
    }

    public static void DrawBackgroundBox(RectGuide oGuide, float nMargins)
    {
        DrawBackgroundBox(oGuide, Color.white, nMargins);
    }

    public static void DrawBackgroundBox(Rect oRect, Color color, float nMargins = 20)
    {
        float nHalfMargins = nMargins / 2;

        //Set rect position
        Rect backgroundRect = oRect;
        backgroundRect.width -= nMargins;
        backgroundRect.height -= nMargins;
        backgroundRect.x += nHalfMargins;
        backgroundRect.y += nHalfMargins;

        //Draw
        SetColor(color);
        QUI.Box(backgroundRect, QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray));
        ResetColor();
    }

    public static void DrawBackgroundBox(Rect oRect, float nMargins = 20)
    {
        DrawBackgroundBox(oRect, Color.white, nMargins);
    }

    public static void DropOperation(Rect dropZone)
    {
        //Rect dropZone = GUILayoutUtility.GetLastRect();

        if (Event.current.type == EventType.DragPerform)
        {
            if (dropZone.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                DragAndDrop.AcceptDrag();
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            }
        }
    }

    public static void DrawLabelWithBackground(Rect titleRect, string str,  QColors.Color qColor)
    {
        AnimBool aBool = new AnimBool(true);
        //QUI.GhostBar(titleRect, "Hi", QColors.Color.Orange, aBool);
        GUIStyle style = QStyles.GetBackgroundStyle(Style.BackgroundType.Low, qColor);
        QUI.Box(titleRect, style);

        GUIStyle MiddleLabel = new GUIStyle(QStyles.GetStyle(QStyles.GetStyleName(Style.Text.Title)));
        MiddleLabel.alignment = TextAnchor.MiddleCenter;
        EditorGUI.LabelField(titleRect, str, MiddleLabel);
    }

}
