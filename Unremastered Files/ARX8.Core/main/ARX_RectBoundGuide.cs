using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARX
{

    /// <summary>
    /// Helper object used to aid in the placement of Rects on a screen. 
    /// </summary>
    public class RectGuide
    {

        //VARIABLES
        private Rect mo_boundingRect, mo_lastRect;
        public float mnf_newLineHeight = 25;
        //float mnf_greatestHeight = 0;

        public float mnf_indentSpace = 25.0F;
        public bool mb_autoNewLine = true,
            mb_exceedBounds = false;
        

        //CONSTRUCTORS
        public RectGuide(Rect rectBounds, float nfNewLineHeight = 16, bool bAutoNewLine = false, bool bExceedBounds = false,
            bool bIncreaseNewlineHeight = false)
        {
            mo_boundingRect = rectBounds;
            mnf_newLineHeight = nfNewLineHeight;
            mb_autoNewLine = bAutoNewLine;
            mb_exceedBounds = bExceedBounds;
            mo_lastRect = new Rect(mo_boundingRect);
            mo_lastRect.width = 0;
            mo_lastRect.height = 0;
        }

        public bool Button(int nWidth, int nHeight, string text)
        {
            return GUI.Button(GetNextRect(nWidth, nHeight), text);
        }

        //PROPERTIES
        public Rect BoundingRect
        {
            get
            {
                return mo_boundingRect;
            }
            set
            {
                mo_boundingRect = value;
            }
        }

        public Rect LastRect
        {
            get { return mo_lastRect; }
            set {  mo_lastRect = value; }
        }

        public void SetLastRectPosition(float x, float y)
        {
            mo_lastRect.x = x;
            mo_lastRect.y = y;
            
        }

        //FUNCTIONS
        public void SaveBoolSettings()
        {
            bSaveAutoNewLine = mb_autoNewLine;
            bSaveExceedBounds = mb_exceedBounds;
        }

        bool bSaveAutoNewLine, bSaveExceedBounds;

        public void RestoreBoolSettings()
        {
            mb_autoNewLine = bSaveAutoNewLine;
            mb_exceedBounds = bSaveExceedBounds;
        }

        bool RectExceedsBounds()
        {
            if (mo_lastRect.max.x > mo_boundingRect.max.x)
                return true;
            return false;
        }

        public void MoveLastRect(float x)
        {
            MoveLastRect(x, 0);
        }

        public void MoveLastRect(float x, float y)
        {
            mo_lastRect.x += x;
            mo_lastRect.y += y;
        }

        public void MoveBoundingRect(float x)
        {
            MoveBoundingRect(x, 0);
        }

        public void MoveBoundingRect(float x, float y)
        {
            mo_boundingRect.x += x;
            mo_boundingRect.y += y;
            MoveLastRect(x, y);
        }

        public void SetBoundingRect(Rect rectBound)
        {
            mo_boundingRect = rectBound;
            ValidateRect();
        }

        public void NewLine(int n)
        {
            for (int i = 0; i < n; i++)
                NewLine();
        }

        public void Indent()
        {
            MoveLastRect(mnf_indentSpace);
        }

        public void Indent(int nTimes)
        {
            for (int i = 0; i < nTimes; i++)
                Indent();
        }

        public void NewLine()
        {
            mo_lastRect.x = mo_boundingRect.x;
            mo_lastRect.width = 0;

            mo_lastRect.y += mnf_newLineHeight;

        }

        /// <summary>
        /// A newline, but without moving the current rect downward.
        /// Just returning to the beginning of the current line.
        /// </summary>
        public void Return()
        {
            mo_lastRect.x = mo_boundingRect.x;
            mo_lastRect.width = 0;

        }


        void ValidateRect()
        {
            if (mb_exceedBounds)
                return;

            if (!RectExceedsBounds())
                return;

            if (mb_autoNewLine)
            {
                NewLine();
            }
        }

        public Rect PeekMiddleRect(float nfWidth, float nfHeight = 16)
        {
            return GetMiddleRect(nfWidth, nfHeight, false);
        }

        public Rect GetMiddleRect(float nfWidth, float nfHeight = 16, bool bMoveRect = true)
        {

            float nBoundEndPosition = BoundingRect.xMax;
            float nCurrentRectStartPosition = LastRect.xMin;

            //Get the remaining space between the current rect start and the end of the bound
            float nfSpaceRemaining = nBoundEndPosition - nCurrentRectStartPosition;

            //Get the location of the middle of that area
            float nMiddle = nCurrentRectStartPosition + nfSpaceRemaining / 2;

            //Get the start position of the new rect
            float nNewRectStartPosition = nMiddle - nfWidth / 2;

            //Find the difference bewteen the new rect's start and the current rect's start
            float nDifference = nNewRectStartPosition - nCurrentRectStartPosition;

            //Move the RectGuide's start positio to the new start position
            MoveLastRect(nDifference);

            //Return the next rect
            return GetNextRect(nfWidth, nfHeight, bMoveRect);
        }

        public Rect GetNextRect(float nfWidth, float nfHeight = 16, bool bMoveRect = true)
        {
            Rect rectNext = new Rect(LastRect);
            rectNext.width = nfWidth;
            rectNext.height = nfHeight;

            //If the given rectangle is not allowed to exceed bounds
            //Change the rect's size
            //if (RectExceedsBounds() && !mb_exceedBounds)
            //{
            //    rectNext.width = BoundingRect.max.x - rectNext.x;
            //}

            if (bMoveRect)
            {
                mo_lastRect = rectNext;
                mo_lastRect.x += nfWidth;
                ValidateRect();
            }

            return rectNext;
        }

        public void ScaleBoundingRect(float nfWidth, float nfHeight, bool bMoveToCenter = true)
        {
            Rect buf = BoundingRect;
            buf.height += nfHeight;
            buf.width += nfWidth;

            if (bMoveToCenter)
            {
                float xMove = Mathf.Abs(nfWidth / 2);
                float yMove = Mathf.Abs(nfHeight / 2);

                buf.x += xMove;
                buf.y += yMove;
                MoveLastRect(xMove, yMove);
            }

            BoundingRect = buf;
        }

        public static Rect ScaleRect(Rect oRect, float nfWidth, float nfHeight, bool bMoveToCenter = true)
        {
            Rect buf = oRect;
            buf.height += nfHeight;
            buf.width += nfWidth;

            if (bMoveToCenter)
            {
                buf.x += nfWidth / 2;
                buf.height += nfHeight / 2;
            }

            return buf;
        }

        public Rect PeekNextRect(float nfWidth, float nfHeight)
        {
            return GetNextRect(nfWidth, nfHeight, false);
        }
        
    }
}