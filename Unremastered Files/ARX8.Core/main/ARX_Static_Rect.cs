using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARX
{
    public static class ARX_Rect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="bIsBaseWindow"></param>
        /// <returns></returns>
        public static Rect ExtractEditorRectFromUIRect(Image img, bool bIsBaseWindow)
        {
            Rect extractRect = new Rect();
            if (img == null)
                return extractRect;

            //New Dimension Output
            float nfNewWidth = img.rectTransform.rect.width;
            float nfNewHeight = img.rectTransform.rect.height;

            //Positions
            Vector2 vecOldPosition = img.transform.position;
            Vector2 vecNewPosition = vecOldPosition;

            //Subtract Box Width
            vecNewPosition.x -= nfNewWidth / 2;

            //Add Box Height
            vecNewPosition.y += nfNewHeight / 2;

            //Subtract Screen Height

            vecNewPosition.y -= Screen.height;

            //Change Y negative/positive sign
            vecNewPosition.y *= -1;
            
            //vecNewPosition += new Vector2(nfNewWidth / -2, nfNewHeight / -2);

            if (bIsBaseWindow == false)
            {

            }

            //Debug.Log(img.name + " Old Position: " + vecOldPosition + " New Position: " + vecNewPosition + " Height: " + nfNewHeight + " Width: " + nfNewWidth);

            //Set Rect
            extractRect.width = nfNewWidth;
            extractRect.height = nfNewHeight;
            extractRect.position = vecNewPosition;

            //Debug.Log(img.name + extractRect);

            return extractRect;
        }
    }
}
