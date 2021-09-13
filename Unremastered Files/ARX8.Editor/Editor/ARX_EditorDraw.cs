using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace ARX
{

    public static class EditorDraw
    {

        static Vector2 mvec_statboxScroll, mvec_scrollPosition;



        public static void DrawRect(Rect rect, Color color)
        {
            Textures.BlankYellowTexture.SetPixel(0, 0, color);
            Textures.BlankYellowTexture.wrapMode = TextureWrapMode.Repeat;
            Textures.BlankYellowTexture.Apply();

            GUIStyle style = new GUIStyle();
            style.normal.background = Textures.BlankYellowTexture;

            GUIStyle buf = GUI.skin.box;

            GUI.skin.box = style;
            GUI.Box(rect, "");
            GUI.skin.box = buf;

        }

        public static Rect DrawBackground(RectGuide oGuide, Rect oRect, int nBorder, Color oOuterColor, Color oContentColor)
        {
            return DrawBackground(oGuide, (int)oRect.width, (int)oRect.height, nBorder, oOuterColor, oContentColor);
        }

        public static Rect DrawBackground(RectGuide oGuide, int nWidth, int nHeight, int nBorder,
        Color oOuterColor, Color oContentColor)
        {
            Rect rectContent = oGuide.GetNextRect(nWidth, nHeight);

            Rect rectOuter = new Rect(rectContent);
            rectOuter.width += nBorder;
            rectOuter.height += nBorder;

            rectContent.x += nBorder / 2;
            rectContent.y += nBorder / 2;


            DrawRect(rectOuter, oOuterColor);
            DrawRect(rectContent, oContentColor);

            return rectContent;
        }


        public static void DrawAbilityList(RectGuide oGuide, ARX_Script_Debug_Ability oHighlightedAbility, ARX_Script_Debug_AbilityContainer oChosenList)
        {
            //Outer Box and Background
            float nfOuterBoxSize = 300;
            Rect rectOuter = oGuide.GetNextRect(300, nfOuterBoxSize);
            float nfGrayAlpha = 0.55F;
            EditorGUI.DrawRect(rectOuter, new Color(nfGrayAlpha, nfGrayAlpha, nfGrayAlpha));

            //Inner section
            RectGuide oInnerGuide = new RectGuide(new Rect(0, 0, 300, oChosenList.AbilityList.Count * 16 - 16), 16);
            mvec_scrollPosition = GUI.BeginScrollView(rectOuter, mvec_scrollPosition, oInnerGuide.BoundingRect);

            for (int i = 0; i < oChosenList.AbilityList.Count; i++)
            {
                ARX_Script_Debug_Ability oAbility = oChosenList.AbilityList[i];

                string strFocusedAbility = i.ToString();


                //Row Coloring

                //Deletion Button
                bool bDelete = false;
                bDelete = GUI.Button(oInnerGuide.GetNextRect(16, 16), "X");
                if (bDelete)
                {
                    oChosenList.AbilityList.Remove(oAbility);
                    continue;
                }

                //Outline Rect
                Rect rectOutline = oInnerGuide.GetNextRect(150, 16);

                GUI.SetNextControlName(strFocusedAbility);
                oAbility.mstr_name = GUI.TextField(rectOutline, oAbility.mstr_name);

                GUIStyle style = new GUIStyle();
                style.normal.background = null;

                //Select Ability Button
                if (GUI.Button(rectOutline, "", style))
                {
                    GUI.FocusControl(strFocusedAbility);
                }

                //Highlight chosen ability
                if (GUI.GetNameOfFocusedControl() == strFocusedAbility)
                {
                    EditorGUI.DrawRect(rectOutline, new Color(0, 1, 0, 0.25F));
                    oHighlightedAbility = oAbility;
                }

                //Change Focus


                oInnerGuide.NewLine();
            }

            GUI.EndScrollView();

            //New stat
            oGuide.MoveLastRect(0, nfOuterBoxSize);
            oGuide.NewLine();

            if (GUI.Button(oGuide.GetNextRect(100, 16), "New Ability"))
            {

                oChosenList.AddAbility();
            }

        }


        public static float GetContainerInnerHeight(List<ARX_Script_Debug_Ability> Abilities)
        {
            float nHeight = Abilities.Count * 16;
            return nHeight;

        }
        public static void DrawStatBox(RectGuide oGuide, ARX_StatSingleBox oStats, bool bReadOnly = true)
        {
            oGuide.MoveBoundingRect(16);

            float nfStatBoxWidth = 200;
            if (bReadOnly)
                GUI.Label(oGuide.GetNextRect(300, 16), "In Read Only mode: Stat values cannot be changed");

            oGuide.NewLine();

            //Upper labels

            Rect oLabelBar = oGuide.PeekNextRect(150, 16);
            EditorGUI.DrawRect(oLabelBar, new Color(0.75F, 0.75F, 0.75F, 0.5F));
            //Name
            oGuide.MoveLastRect(16);
            GUI.Label(oGuide.GetNextRect(75, 16), "Name");
            //Current
            GUI.Label(oGuide.GetNextRect(75, 16), "Value");
            oGuide.NewLine();

            //Outer Box and Background
            int nHeight = 16 * oStats.Count + 48;
            Rect rectOuter = DrawBackground(oGuide, (int)nfStatBoxWidth, nHeight, 16, Color.green, Color.black);

            //Inner section
            RectGuide oInnerGuide = new RectGuide(new Rect(0, 0, nfStatBoxWidth, oStats.Count * 16), 16);
            mvec_statboxScroll = GUI.BeginScrollView(rectOuter, mvec_statboxScroll, oInnerGuide.BoundingRect);

            //Draw Each Stat
            for (int i = 0; i < oStats.Count; i++)
            {
                ARX_StatSingle oStat = oStats.moa_statBoxes[i];

                //Row Coloring

                //Deletion Button
                bool bDelete = false;
                if (!bReadOnly && !oStat.IsDeleteable)
                    bDelete = GUI.Button(oInnerGuide.GetNextRect(16, 16), "X");
                if (bDelete)
                {
                    oStats.DeleteStat(oStat);
                    continue;
                }


                //Stat name
                if (!bReadOnly)
                    oStat.ID = GUI.TextField(oInnerGuide.GetNextRect(75, 16), oStat.ID);
                else
                    GUI.Label(oInnerGuide.GetNextRect(75, 16), oStat.ID);

                if (oStat.me_statType == STATTYPE.FLOAT)
                {
                    //Base
                    if (!bReadOnly)
                        oStat.Base = EditorGUI.FloatField(oInnerGuide.GetNextRect(100, 16), oStat.Base);
                    else
                        GUI.Label(oInnerGuide.GetNextRect(100, 16), oStat.Base.ToString());
                }
                else if (oStat.me_statType == STATTYPE.STRING)
                {
                    if (!bReadOnly)
                        oStat.StringValue = GUI.TextField(oInnerGuide.GetNextRect(100, 16), oStat.StringValue);
                    else
                        GUI.Label(oInnerGuide.GetNextRect(100, 16), oStat.StringValue.ToString());
                }


                oInnerGuide.NewLine();
                oGuide.NewLine();

            }

            GUI.EndScrollView();

            //New stat
            oGuide.NewLine();
            oGuide.MoveLastRect(12);

            if (!bReadOnly)
            {
                if (GUI.Button(oGuide.GetNextRect(100, 32), "New Stat"))
                {
                    oStats.CreateNewStat();
                }
            }

            oGuide.MoveBoundingRect(-16);
        }

        public static void DrawStatBox(RectGuide oGuide, ARX_StatBox_Quad oStats, float nWidth = 300, bool bReadOnly = true)
        {
            #region Variables
            float nFieldWidth = (nWidth - 16) / 5;
            //Outer Box and Background
            int nHeight = 16 * oStats.Count + 48;
            const float GREYSCALE = 0.4F;
            #endregion

            #region Print Upper labels

            if (bReadOnly)
            {
                GUIStyle redStyle = new GUIStyle();
                redStyle.normal.textColor = Color.red;
                //oGuide.NewLine();
                GUI.Label(oGuide.GetNextRect(300, 16), "In Read Only mode: Stat values cannot be changed", redStyle);
            }

            oGuide.NewLine();
            if (bReadOnly == false)
            {
                oGuide.MoveLastRect(16);
            }
            //Name
            GUI.Label(oGuide.GetNextRect(nFieldWidth, 16), "Name");
            //Base
            GUI.Label(oGuide.GetNextRect(nFieldWidth, 16), "Base");
            //Bonus
            GUI.Label(oGuide.GetNextRect(nFieldWidth, 16), "Bonus");
            //Current
            GUI.Label(oGuide.GetNextRect(nFieldWidth, 16), "Current");
            //Can Overflow
            GUI.Label(oGuide.GetNextRect(nFieldWidth, 16), "Overflow");
            oGuide.NewLine();
            #endregion

            #region Draw Background

            Rect rectOuter = oGuide.GetNextRect(nWidth, nHeight);
            EditorGUI.DrawRect(rectOuter, new Color(GREYSCALE, GREYSCALE, GREYSCALE));

            #endregion

            #region Draw Stats Area

            RectGuide oInnerGuide = new RectGuide(new Rect(0, 0, nWidth, oStats.Count * 16), 16);
            mvec_statboxScroll = GUI.BeginScrollView(rectOuter, mvec_statboxScroll, oInnerGuide.BoundingRect);

            //Draw Each Stat
            for (int i = 0; i < oStats.Count; i++)
            {
                ARX_StatQuad oStat = oStats.AsList[i];

                //Row Coloring

                //Deletion Button
                bool bDelete = false;

                if (!bReadOnly && !oStat.IsDeleteable)
                    bDelete = GUI.Button(oInnerGuide.GetNextRect(16, 16), "X");

                //Draw ReadOnly Ver.
                if(bReadOnly)
                {
                    GUI.Label(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.ID);
                    GUI.Label(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.Base.ToString());
                    GUI.Label(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.Bonus.ToString());
                    GUI.Label(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.Current.ToString() + "/" + oStat.Max.ToString());
                    GUI.Toggle(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.mb_allowOverflowCurrent, "");

                }
                //Draw Editable Ver
                else
                {

                    oStat.ID = GUI.TextField(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.ID).ToLower();
                    oStat.Base = EditorGUI.FloatField(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.Base);
                    oStat.Bonus = EditorGUI.FloatField(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.Bonus);
                    oStat.Current = EditorGUI.FloatField(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.Current);
                    oStat.mb_allowOverflowCurrent = GUI.Toggle(oInnerGuide.GetNextRect(nFieldWidth, 16), oStat.mb_allowOverflowCurrent, "");
                }
                

                if (bDelete)
                {
                    oStats.DeleteStat(oStat);
                }


                oInnerGuide.NewLine();
                oGuide.NewLine();

            }

            GUI.EndScrollView();

            //New stat
            oGuide.NewLine(2);


            #endregion

            #region Draw New Stat Button
            if (!bReadOnly)
            {
                if (GUI.Button(oGuide.GetNextRect(100, 16), "New Stat"))
                    oStats.CreateNewStat("", 0, 0, 0);
            }
            #endregion

        }


        public static void DrawBezierCurve(Rect start, Rect end, Color oColor)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, oColor, null, 1);
        }


        public static float DrawScalingField(RectGuide rectGuide, string strLabel, float nfCurrent)
        {
            rectGuide.MoveLastRect(15, 0);
            GUI.Label(rectGuide.GetNextRect(35, 16), strLabel);
            rectGuide.MoveLastRect(-15, 0);
            float nf = EditorGUI.FloatField(rectGuide.GetNextRect(50, 16), nfCurrent);
            rectGuide.MoveLastRect(15);
            return nf;
        }


        public static Vector3 DrawScalingSection(RectGuide rectGuide, float nWidth, string strLabel, Vector3 vec)
        {
            GUI.Label(rectGuide.GetNextRect(nWidth, 16), strLabel);
            //rectGuide.NewLine();
            //X Scaling Label and Field
            float x = DrawScalingField(rectGuide, "X", vec.x);
            //Y Scaling Label and Field
            float y = DrawScalingField(rectGuide, "Y", vec.y);
            //Z Scaling Label and Field        
            float z = DrawScalingField(rectGuide, "Z", vec.z);

            return new Vector3(x, y, z);
        }


        public static Rect GetEditorBound()
        {
            return new Rect(50, 75, 900, 900);
        }


        public static Rect GetEditorScrollView()
        {
            return new Rect(50, 75, 1000, 1000);
        }


        public static bool DrawButton(RectGuide rectGuide, string strLabel, string strImage, float nfWidth, float nfHeight, bool bDrawLabel = true)
        {
            Texture image = Resources.Load<Texture>(strImage);
            Rect rect = rectGuide.GetNextRect(nfWidth, nfHeight);

            GUIContent content = new GUIContent(strLabel, null, strLabel);
            content.image = image;
            content.text = "";
            bool bButton = GUI.Button(rect, content);
            //GUI.Label(rect, content);

            rect.y += nfHeight;
            rect.height = 20;

            if (bDrawLabel)
                GUI.Label(rect, strLabel);

            return bButton;
        }

        public static RectGuide DrawEditorBound()
        {
            //Bounds and Background
            Rect rectBounds = GetEditorBound();
            RectGuide rectGuide = new RectGuide(rectBounds, 25);
            //FUME_Global.EditorDraw.DrawQuad(rectGuide.BoundingRect, Color.gray);

            return rectGuide;
        }

        public static void DrawQuad(Rect position, Color color)
        {
            DrawQuad(Textures.BlankYellowTexture, position, color);
        }

        public static void DrawQuad(Texture2D texture, Rect position, Color color)
        {
            if (texture == null)
                texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(position, GUIContent.none);
        }

        public static void DrawDropdown(Rect rect, string[] astr, GenericMenu.MenuFunction2 func)
        {
            DrawQuad(rect, Color.white);

            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < astr.Length; i++)
                menu.AddItem(new GUIContent(astr[i]), false, func, i);

            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown)
                if (rect.Contains(currentEvent.mousePosition))
                    menu.DropDown(rect);


        }

        public static bool IsLeftClickedOn(Rect rect)
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown &&
                currentEvent.keyCode == KeyCode.Mouse0)
            {
                Vector2 mousePos = currentEvent.mousePosition;
                if (rect.Contains(mousePos))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsRightClickedOn(Rect rect)
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown &&
                currentEvent.keyCode == KeyCode.Mouse1)
            {
                Vector2 mousePos = currentEvent.mousePosition;
                if (rect.Contains(mousePos))
                {
                    return true;
                }
            }
            return false;
        }

        public static void ShowDropdown(RectGuide rectGuide, float nWidth, string strLabel, string[] astrChoices, GenericMenu.MenuFunction2 func)
        {
            Rect contextRect = rectGuide.GetNextRect(nWidth, 16);
            DrawQuad(contextRect, Color.white);
            GUI.Label(contextRect, strLabel);
            if (IsLeftClickedOn(contextRect))
                DrawDropdown(contextRect, astrChoices, func);
        }

        public static string[] DrawBoolLineFromType(RectGuide rectGuide, System.Type eType, string[] astrTrueBools)
        {
            System.Array aEnums = System.Enum.GetValues(eType);
            List<string> oList = new List<string>();
            foreach (Enum obj in aEnums)
                oList.Add(obj.ToString());
            return DrawBoolLineFromStrings(rectGuide, oList.ToArray(), astrTrueBools);
        }

        public static string[] DrawBoolLineFromObjects(RectGuide rectGuide, System.Object[] oaObjects, string[] astrTrueBools)
        {
            List<string> oList = new List<string>();
            foreach (System.Object obj in oaObjects)
                oList.Add(obj.ToString());
            return DrawBoolLineFromStrings(rectGuide, oList.ToArray(), astrTrueBools);
        }

        public static string[] DrawBoolLineFromStrings(RectGuide rectGuide, string[] strArray, string[] astrTrueBools)
        {
            float nfOffsetX = -25, nfOffsetY = 75;
            rectGuide.MoveLastRect(nfOffsetX, nfOffsetY);

            List<string> oList = new List<string>();
            Vector2 pivotPoint;

            //Save Bool Settings
            rectGuide.SaveBoolSettings();
            rectGuide.mb_exceedBounds = true;
            rectGuide.mb_autoNewLine = false;

            //Draw Lines
            int i = 0;
            float alpha = 1;
            foreach (string str in strArray)
            {
                i++;
                Rect rect = rectGuide.GetNextRect(100, 16);
                pivotPoint = new Vector2(rectGuide.LastRect.x, rectGuide.LastRect.y);
                EditorGUIUtility.RotateAroundPivot(45, pivotPoint);

                //Move Lines closer together Horizontally
                rectGuide.MoveLastRect(-75);

                //Alternate Line Colors
                if (i % 2 == 0)
                {
                    alpha = 0.5F;
                }
                else
                    alpha = 0.25F;
                DrawQuad(rect, new Color(200, 200, 200, alpha));

                //Create Button Rects
                Rect buttonRect = new Rect(rect);
                buttonRect.width = 25;
                buttonRect.x += 80;
                buttonRect.y -= 10;

                //Create Label
                GUI.Label(rect, str);

                //Rotate
                EditorGUIUtility.RotateAroundPivot(-45, pivotPoint);

                //Create Toggles
                bool bTrue = false;
                foreach (string strTrue in astrTrueBools)
                {
                    if (strTrue == str)
                    {
                        bTrue = true;
                        break;
                    }
                }

                //If the button was pressed
                if (EditorGUI.Toggle(buttonRect, bTrue))
                    oList.Add(str);
            }

            rectGuide.RestoreBoolSettings();
            rectGuide.MoveLastRect(-nfOffsetX, -nfOffsetY);
            return oList.ToArray();
        }

    }


}