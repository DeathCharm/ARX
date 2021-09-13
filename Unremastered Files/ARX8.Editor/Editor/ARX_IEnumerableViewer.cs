using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using ARX;

namespace ARX
{
    public abstract class ARX_IEnumerableViewer<T>
    {

        public delegate void ButtonAction(T item);

        #region Nested
        public class ARXButton
        {

            ButtonAction mfunc_action;
            string mstr_title;
            float mnf_width, mnf_height;

            public ARXButton(ButtonAction action, string strTitle, float width, float height)
            {
                mnf_width = width;
                mnf_height = height;
                mfunc_action = action;
                mstr_title = strTitle;
            }

            public bool DrawButton(RectGuide oGuide, T item)
            {

                bool bPressed = GUI.Button(oGuide.GetNextRect(mnf_width, mnf_height), mstr_title);

                if (bPressed && mfunc_action != null)
                    mfunc_action(item);

                return bPressed;
            }

        }
        #endregion

        #region Variables
        public int nNewlinesAfterSlide = 1;
        public int nLineHeight = 16, mn_slideWidth = 280;
        public string mstr_newItemButtonLabel = "New Item";
        public string mstr_title = "";
        public bool mb_autoDelete = true;
        int mn_clickedIndex = 0;
        int mn_indexToDelete = 0;

        T mo_itemToDelete, mo_itemToMoveUp, mo_itemToMoveDown, mo_selectedGlyph;
        int nGlyphsDrawn = 0;
        public List<T> moa_data;
        public List<ARXButton> moa_buttons = new List<ARXButton>();
        #endregion

        #region Helper

        public void AddButton(ButtonAction action, string strTitle, float width, float height)
        {
            ARXButton button = new ARXButton(action, strTitle, width, height);
            moa_buttons.Add(button);
        }

        /// <summary>
        /// Moves the element at the given index upwards one index(by lowering its index).
        /// </summary>
        /// <param name="nIndex"></param>
        public void DecrementPlaceInList(T oItem, List<T> moa_list)
        {
            //int nIndex = GetIndex(oItem, moa_list);
            int nIndex = mn_clickedIndex ;
            if (nIndex <= 0)
                return;

            moa_list.Remove(oItem);
            moa_list.Insert(nIndex - 1, oItem);
        }

        /// <summary>
        /// Moves the element at the given index downwards by one(by increasing its index).
        /// </summary>
        /// <param name="nIndex"></param>
        public void IncrementPlaceInList(T oItem, List<T> moa_list)
        {
            //int nIndex = GetIndex(oItem, moa_list);
            int nIndex = mn_clickedIndex;

            if (nIndex >= moa_list.Count - 1)
                return;

            moa_list.Remove(oItem);
            moa_list.Insert(nIndex + 1, oItem);
        }

        /// <summary>
        /// Get the index of the given item in the list.
        /// </summary>
        /// <param name="oItem"></param>
        /// <returns></returns>
        public int GetIndex(T oItem, List<T> moa_list)
        {
            int nIndex = -1;
            for (nIndex = 0; nIndex < moa_list.Count; nIndex++)
            {
                if (I_Equal(oItem, moa_list[nIndex]))
                    return nIndex;
            }

            return -1;
        }

        #endregion

        #region Abstract
        /// <summary>
        /// Compare the two items, return true if they are the same.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public abstract bool I_Equal(T one, T two);

        /// <summary>
        /// Draw the name of the given item
        /// </summary>
        /// <param name="oGuide"></param>
        /// <param name="oItem"></param>
        /// <param name="nIndex"></param>
        /// <param name="oList"></param>
        public abstract void I_DrawName(RectGuide oGuide, T oItem, int nIndex, List<T> oList);

        /// <summary>
        /// Draw the stats and variables of the item
        /// </summary>
        /// <param name="oGuide"></param>
        /// <param name="oItem"></param>
        /// <param name="nIndex"></param>
        /// <param name="oList"></param>
        public abstract void I_DrawUnique(RectGuide oGuide, T oItem, int nIndex, List<T> oList);

        /// <summary>
        /// React to the user's request to delete the given item.
        /// </summary>
        /// <param name="oItem"></param>
        /// <param name="nIndex"></param>
        public abstract void I_ReactToDeleteButtonClickedForItem(T oItem, int nIndex);

        /// <summary>
        /// React to the user's request to add a new item to the viewer's list
        /// </summary>
        /// <param name="moa_list"></param>
        public abstract void I_ClickNewItem(List<T> moa_list);

        #endregion

        #region Virtual
        public virtual Rect V_DrawBackground(T oItem, RectGuide oGuide, int nIndex)
        {
            //Draw background
            Rect rectBackground = oGuide.PeekNextRect(mn_slideWidth, nLineHeight);
            //Choose background color
            Color oBgColor;
            if (I_Equal(oItem, mo_selectedGlyph))
                oBgColor = Color.green;
            else if (nGlyphsDrawn % 2 == 0)
                oBgColor = new Color(0, 0, 0, 0.1F);
            else
                oBgColor = new Color(0, 0, 0, 0.2F);

            EditorDraw.DrawQuad(rectBackground, oBgColor);
            return rectBackground;
        }

        public virtual bool V_DrawNewItemButton(RectGuide oGuide, bool bIsTopDrawnButton)
        {
            //DrawNewItem Button
            return GUI.Button(oGuide.GetNextRect(75, 32), mstr_newItemButtonLabel);
        }

        /// <summary>
        /// Main draw function for this viewer
        /// </summary>
        /// <param name="oGuide"></param>
        public virtual void V_Draw(RectGuide oGuide, List<T> oaList)
        {
            moa_data = oaList;
            nGlyphsDrawn = 0;

            if (mstr_title != "")
            {
                GUI.Label(oGuide.GetNextRect(mn_slideWidth), mstr_title);
                oGuide.NewLine();
            }

            //Draw New Item Button
            if (V_DrawNewItemButton(oGuide, true))
            {
                I_ClickNewItem(oaList);
            }
            oGuide.NewLine(3);

            for (int i = 0; i < oaList.Count; i++)
            {
                V_DrawSlide(oaList[i], oGuide, i, oaList);
            }


            //Draw New Item Button if there is more than one slide
            if(oaList.Count > 0)
            if (V_DrawNewItemButton(oGuide, false))
            {
                I_ClickNewItem(oaList);
            }

            //If there is an item to be deleted
            if (!I_Equal(mo_itemToDelete, default(T)))
            {
                I_ReactToDeleteButtonClickedForItem(mo_itemToDelete, mn_indexToDelete);

                if (mb_autoDelete)
                {
                    //if(mo_itemToDelete != null)
                    //    Debug.Log("EnumerableViewer removing " + mo_itemToDelete.ToString());
                    //else
                    //    Debug.Log("EnumerableViewer removing null item");

                    oaList.Remove(mo_itemToDelete);
                }
                mo_itemToDelete = default(T);
                mn_indexToDelete = 0;
            }

            //If there is an item to be moved up or down
            if (mo_itemToMoveDown != null)
                IncrementPlaceInList(mo_itemToMoveDown, oaList);

            if (mo_itemToMoveUp != null)
                DecrementPlaceInList(mo_itemToMoveUp, oaList);


            mo_itemToDelete = default(T);
            mo_itemToMoveUp = default(T);
            mo_itemToMoveDown = default(T);
            mn_clickedIndex = 0;
        }
        
        public virtual bool V_DrawDeleteButton(RectGuide oGuide)
        {
            Rect rectDelete = oGuide.GetNextRect(nLineHeight, nLineHeight);
            //rectDelete.position = new Vector2(nSlideWidth - nLineHeight, rectDelete.position.y);
            if (GUI.Button(rectDelete, "X"))
                return true;
            return false;
        }

        /// <summary>
        /// Function that draws the given glyph's generic buttons(delete, move up, move down),
        /// and this viewers custom glyph appearance.
        /// </summary>
        /// <param name="oItem"></param>
        /// <param name="oGuide"></param>
        /// <returns></returns>
        protected virtual bool V_DrawSlide(T oItem, RectGuide oGuide, int nIndex, List<T> moa_list)
        {
            Rect rectBackground = V_DrawBackground(oItem, oGuide, nIndex);

            nGlyphsDrawn++;
            //Draw Up and Down Buttons
            Rect rectUpButton = oGuide.GetNextRect(nLineHeight, nLineHeight);
            Rect rectDownButton = oGuide.GetNextRect(nLineHeight, nLineHeight);

            GUIStyle style = new GUIStyle();
            style.normal.background = null;


            if (GUI.Button(rectUpButton, "↑"))
            {
                mo_itemToMoveUp = oItem;
                mn_clickedIndex = nIndex;
            }

            if (GUI.Button(rectDownButton, "↓"))
            {
                mo_itemToMoveDown = oItem;
                mn_clickedIndex = nIndex;
            }


            //Draw name
            //GUI.SetNextControlName("name");
            I_DrawName(oGuide, oItem, nIndex, moa_list);
            I_DrawUnique(oGuide, oItem, nIndex, moa_list);
            oGuide.MoveLastRect(8);


            //Draw Delete
            if (V_DrawDeleteButton(oGuide))
            {
                if (mb_autoDelete)
                {
                    mo_itemToDelete = oItem;
                }
                mn_indexToDelete = nIndex;
                I_ReactToDeleteButtonClickedForItem(oItem, mn_indexToDelete);
                return false;
            }

            //Draw Alt Buttons
            foreach (ARXButton b in moa_buttons)
            {
                 b.DrawButton(oGuide, oItem);
            }


            //Select Ability Button
            if (GUI.Button(rectBackground, "", style))
            {
                mo_selectedGlyph = oItem;
                //GUI.FocusControl("name");
            }


            oGuide.NewLine(nNewlinesAfterSlide);

            return true;
        }
        #endregion

    }
}