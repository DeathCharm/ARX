using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace ARX
{
    /// <summary>
    /// Editor Only class for drawing lists of Algorithm actions
    /// </summary>
    public class AlgorithmViewer : ARX_IEnumerableViewer<ARX_AlgorithmAction>
    {
        public ARX_Asset_Algorithm mo_script = null;

        public AlgorithmViewer(ARX_Asset_Algorithm oScript)
        {
            mo_script = oScript;
        }

        public override bool V_DrawNewItemButton(RectGuide oGuide, bool bIsTopDrawn)
        {
            return false;
        }

        public override bool V_DrawDeleteButton(RectGuide oGuide)
        {
            oGuide.MoveLastRect(0, -16);
            Rect rectDelete = oGuide.GetNextRect(100, nLineHeight * 2);
            rectDelete.x += 114;
            bool bDelete = GUI.Button(rectDelete, "Delete Action");

            oGuide.NewLine(4);
            return bDelete;
        }

        public override void I_ClickNewItem(List<ARX_AlgorithmAction> oList)
        {

        }

        public override void I_DrawName(RectGuide oGuide, ARX_AlgorithmAction oItem, int nIndex, List<ARX_AlgorithmAction> moa_list)
        {
            int nStatBoxHeight = 16 * oItem.mo_stats.Count + 160;
            Rect oAlgorithmBox = oGuide.PeekNextRect(300, nStatBoxHeight);
            oAlgorithmBox.x -= 32;

            EditorGUI.DrawRect(oAlgorithmBox, new Color(0.85F, 0.85F, 0.85F, 0.7F));
            //Draw Action Name
            Rect oLabel = oGuide.GetNextRect(300, 16);
            GUI.Label(oLabel, oItem.ActionName);
            //oGuide.NewLine();
        }

        public override bool I_Equal(ARX_AlgorithmAction one, ARX_AlgorithmAction two)
        {
            return one == two;
        }

        public override void I_ReactToDeleteButtonClickedForItem(ARX_AlgorithmAction oItem, int nIndex)
        {
            return;
        }

        public override void I_DrawUnique(RectGuide oGuide, ARX_AlgorithmAction oItem, int nIndex, List<ARX_AlgorithmAction> moa_list)
        {
            //Debug information
            //Making sure the type to cast to is the original type the class was
            //when first instantiated. Unity Serialization has a habit of casting derived
            //classes as their base when reopening the editor.

            oGuide.NewLine();
            if (oItem.OriginalType != null)
                GUI.Label(oGuide.GetNextRect(250, 16), "Original Type :" + oItem.OriginalType.ToString());
            else
                GUI.Label(oGuide.GetNextRect(250, 16), "NULL TYPE");

            oGuide.NewLine();
            oItem.mstr_notes = GUI.TextArea(oGuide.GetNextRect(300, 32), oItem.mstr_notes);


            oGuide.NewLine(2);
            Rect oTypeLabel = oGuide.GetNextRect(300, 16);

            GUI.Label(oTypeLabel, oItem.OriginalClassType);

            ARX.EditorDraw.DrawStatBox(oGuide, oItem.mo_stats, false);
            oGuide.NewLine();
        }

        public int GetHeight(List<ARX_AlgorithmAction> moa_list)
        {
            int nReturn = 0;
            foreach (ARX_AlgorithmAction p in moa_list)
                nReturn += (16 * p.mo_stats.Count) + 192;
            return nReturn;

        }
    }
}