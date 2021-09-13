using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace ARX
{
    public class LicenseViewer : ARX_IEnumerableViewer<ARX_LicenseListing.License>
    {
        public override void I_ClickNewItem(List<ARX_LicenseListing.License> moa_list)
        {
            moa_data.Add(new ARX_LicenseListing.License());
        }

        public override bool I_Equal(ARX_LicenseListing.License one, ARX_LicenseListing.License two)
        {
            return one == two;
        }

        public override void I_DrawName(RectGuide oGuide, ARX_LicenseListing.License oItem, int nIndex, List<ARX_LicenseListing.License> oList)
        {
            oGuide.NewLine();
            oItem.mstr_name = GUI.TextField(oGuide.GetNextRect(400), oItem.mstr_name);
            oGuide.NewLine();
        }

        public override void I_DrawUnique(RectGuide oGuide, ARX_LicenseListing.License oItem, int nIndex, List<ARX_LicenseListing.License> oList)
        {
            oItem.mstr_type = GUI.TextField(oGuide.GetNextRect(400), oItem.mstr_type);
            oGuide.NewLine();
            oItem.mstr_notes = GUI.TextArea(oGuide.GetNextRect(400, 50), oItem.mstr_notes);
            oGuide.NewLine(3);
        }

        public override void I_ReactToDeleteButtonClickedForItem(ARX_LicenseListing.License oItem, int nIndex)
        {
            moa_data.Remove(oItem);
        }
    }


    [CustomEditor(typeof(ARX_LicenseListing))]
    public class ARX_CustomEditor_LicenseListing : Editor
    {

        public ARX_LicenseListing GetTarget
        {
            get
            {
                return (ARX_LicenseListing)target;
            }

        }
        public int GetHeight
        {
            get
            {
                int nHeightOfEachLicense = 112;
                int nStandardHeight = 100;
                return (GetTarget.Licenses.Count * nHeightOfEachLicense) + nStandardHeight;
            }
        }

        public Rect GetRect
        {
            get
            {
                return GUILayoutUtility.GetRect(300, GetHeight);
            }
        }

        private void OnDestroy()
        {
            Save();
        }

        void Save()
        {
            EditorUtility.SetDirty(GetTarget);
            AssetDatabase.SaveAssets();
        }

        public override void OnInspectorGUI()
        {
            RectGuide oGuide = new RectGuide(GetRect);
            LicenseViewer oViewer = new LicenseViewer();
            oViewer.V_Draw(oGuide, GetTarget.Licenses);

        }

    }
}
