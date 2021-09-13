using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;
using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

namespace ARX
{
    /// <summary>
    /// Determines the direction an ARX_EditorScrollWiew will arrange its buttons 
    /// </summary>
    public enum WindowDirection { VERTICAL, HORIZONTAL };

    /// <summary>
    /// Draws a scroll view of T items.
    /// </summary>
    /// <typeparam name="ScrollItem"></typeparam>
    public abstract class ARX_EditorScrollView<ScrollItem>
    {
        #region Variables
        /// <summary>
        /// The list of items to be shown by the scroll view.
        /// </summary>
        public List<ScrollItem> mo_list = new List<ScrollItem>();

        /// <summary>
        /// The position on the Editor in which the scroll view will be drawn.
        /// </summary>
        public Rect mo_displayRect;

        /// <summary>
        /// The dimensions of the button in which the items will be shown.
        /// </summary>
        public Rect mo_itemDimensionsRect;

        /// <summary>
        /// The current scroll position of the scroll view.
        /// </summary>
        public Vector2 mvec_scrollPosition;

        /// <summary>
        /// The padding space placed inbetween scroll view items
        /// </summary>
        public float mnf_padding = 2;

        /// <summary>
        /// Will the inactive buttons still be drawn using DrawInactive?
        /// </summary>
        public bool mb_drawInactive = false;

        /// <summary>
        /// The direction the scroll will 
        /// </summary>
        public WindowDirection me_windowDirection = WindowDirection.VERTICAL;

        #endregion

        #region Constructors
        public ARX_EditorScrollView(WindowDirection eShape, List<ScrollItem> oList, Rect rectMainImageRect, Rect rectItemRect, bool bDrawInactive = false)
        {
            mo_list = oList;
            mo_displayRect = rectMainImageRect;
            mo_itemDimensionsRect = rectItemRect;
            mb_drawInactive = bDrawInactive;
            me_windowDirection = eShape;
        }
        #endregion

        #region Abstracts
        /// <summary>
        /// Draw the background of the scroll view's external position.
        /// </summary>
        /// <param name="rectBackground"></param>
        public abstract void I_DrawBackground(Rect rectEditorPosition);

        /// <summary>
        /// Return false if the given item is not to be drawn with DrawItem.
        /// If the mb_drawInactive variable is true, the item will be drawn
        /// using the DrawInactive virtual function.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract bool I_IsToBeDrawn(ScrollItem item);

        /// <summary>
        /// Draw the given item using the given RectGuide
        /// </summary>
        /// <param name="item"></param>
        /// <param name="oGuide"></param>
        public abstract void I_DrawItem(ScrollItem item, Rect rect);


        /// <summary>
        /// Draw the given inactive item if the variable mb_drawInactive is set to draw
        /// inactive items.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rect"></param>
        public virtual void V_DrawInactive(ScrollItem item, Rect rect)
        {
            //DrawItem(item, rect);
        }
        #endregion

        #region Virtuals
        /// <summary>
        /// Return the height of the given item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual float GetItemHeight
        {
            get
            {
                return mo_itemDimensionsRect.height;
            }
        }

        /// <summary>
        /// Return the width of the given item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual float GetItemWidth
        {
            get
            {
                return mo_itemDimensionsRect.width;
            }
        }

        /// <summary>
        /// Moves the rectGuide's position to the next button to be drawn.
        /// Will move either vertically or horizontally depending on the
        /// me_windowDirection variable's value.
        /// </summary>
        /// <param name="oGuide"></param>
        /// <param name="item"></param>
        public void MoveToNextButton(RectGuide oGuide, ScrollItem item)
        {
            if (me_windowDirection == WindowDirection.VERTICAL)
                MoveToNextItemVertical(oGuide, item);
            else if (me_windowDirection == WindowDirection.HORIZONTAL)
                MoveToNextItemHorizontal(oGuide, item);

        }

        /// <summary>
        /// Moves the given rectGuide horizontally to the next button position.
        /// </summary>
        /// <param name="oGuide"></param>
        /// <param name="item"></param>
        public void MoveToNextItemHorizontal(RectGuide oGuide, ScrollItem item)
        {
            oGuide.MoveLastRect(GetItemWidth + mnf_padding);
        }

        /// <summary>
        /// Moves the given rectGuide vertically to the next button position.
        /// </summary>
        /// <param name="oGuide"></param>
        /// <param name="item"></param>
        public void MoveToNextItemVertical(RectGuide oGuide, ScrollItem item)
        {
            oGuide.NewLine();
            oGuide.MoveLastRect(0, mnf_padding);
        }

        #endregion

        #region Measurements
        /// <summary>
        /// Returns the total width of the internal scroll view rect.
        /// </summary>
        public float GetTotalDrawnInternalWidth
        {
            get
            {
                if (me_windowDirection == WindowDirection.VERTICAL)
                    return mo_itemDimensionsRect.width;

                float nf = 0;
                foreach (ScrollItem t in mo_list)
                {
                    if (I_IsToBeDrawn(t) || mb_drawInactive)
                    {
                        nf += GetItemWidth;
                        nf += mnf_padding;
                    }
                }
                return nf;
            }
        }

        /// <summary>
        /// Returns the total height of the internal scroll view rect.
        /// </summary>
        public float GetTotalDrawnInternalHeight
        {
            get
            {
                if (me_windowDirection == WindowDirection.HORIZONTAL)
                    return mo_displayRect.height;

                float nf = 0;
                foreach (ScrollItem t in mo_list)
                    if (I_IsToBeDrawn(t) || mb_drawInactive)
                    {
                        nf += GetItemHeight;
                        nf += mnf_padding;
                    }

                return nf;
            }
        }

        /// <summary>
        /// Returns the dimensions of the internal scroll view rect.
        /// </summary>
        public Rect GetInternalRect
        {
            get
            {
                Rect rect = new Rect(0, 0, GetTotalDrawnInternalWidth, GetTotalDrawnInternalHeight);
                //Debug.Log(rect);

                return rect;
            }
        }

        #endregion

        #region Draw
        /// <summary>
        /// Draws the Scroll View
        /// </summary>
        public void Draw()
        {
            Rect rectEditor = mo_displayRect;
            RectGuide rectGuide = new RectGuide(rectEditor, 16, false, true);

            I_DrawBackground(rectEditor);

            mvec_scrollPosition = GUI.BeginScrollView(rectEditor, mvec_scrollPosition, GetInternalRect);
            DrawInternal(rectGuide);
            GUI.EndScrollView();
        }

        /// <summary>
        /// Draw the internal scroll view rect and its items.
        /// </summary>
        /// <param name="oGuide"></param>
        private void DrawInternal(RectGuide oGuide)
        {
            int i = 0;
            foreach (ScrollItem t in mo_list)
            {
                //If the rect is not to be drawn and not to be drawn inactive,
                //skip
                if (mb_drawInactive == false && I_IsToBeDrawn(t) == false)
                    continue;

                oGuide.mnf_newLineHeight = GetItemHeight;
                Rect rect = oGuide.PeekNextRect(GetItemWidth, GetItemHeight);

                if (i % 2 == 0)
                    EditorGUI.DrawRect(rect, Color.gray);
                else
                    EditorGUI.DrawRect(rect, Color.blue);


                if (I_IsToBeDrawn(t))
                {

                    I_DrawItem(t, rect);
                    i++;
                }
                else
                {
                    V_DrawInactive(t, rect);
                    i++;
                }

                MoveToNextButton(oGuide, t);



            }
        }
        #endregion

    }

}

