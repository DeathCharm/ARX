using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using ARX;




public class DataStringViewer : ARX_IEnumerableViewer<DataPair>
{

    public override void I_ClickNewItem(List<DataPair> moa_list)
    {
        moa_list.Add(new DataPair());
    }

    public override void I_DrawName(RectGuide oGuide, DataPair oItem, int nIndex, List<DataPair> oList)
    {
        //Name
        oItem.mstr_name = GUI.TextField(oGuide.GetNextRect(150, 16), oItem.mstr_name);

        //Value
        oItem.mstr_value = GUI.TextField(oGuide.GetNextRect(150, 16), oItem.mstr_value);
    }

    public override void I_DrawUnique(RectGuide oGuide, DataPair oItem, int nIndex, List<DataPair> oList)
    {

    }


    public override bool I_Equal(DataPair one, DataPair two)
    {
        return one == two;
    }

    public override void I_ReactToDeleteButtonClickedForItem(DataPair oItem, int nIndex)
    {

    }
}

[CustomEditor(typeof(ARX_DataStringAsset))]
    public class ARX_CustomEditor_DataStringAsset : Editor
    {

    DataStringViewer mo_viewer = null;

    ARX_DataStringAsset GetTarget
    {
        get
        {
            return (ARX_DataStringAsset)target;
        }
    }

    Rect GetRect
    {
        get
        {
            return GUILayoutUtility.GetRect(0, 0, 300, 600);
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
        if (mo_viewer == null)
            mo_viewer = new DataStringViewer();

        RectGuide oGuide = new RectGuide(GetRect, 16);

        GUI.Label(oGuide.GetNextRect(150, 16), "Message: " + GetTarget.name);
        oGuide.NewLine(2);
        
        List<DataPair> olist = GetTarget.DataPairs;
        mo_viewer.V_Draw(oGuide, olist);
        GetTarget.mo_dataString.mstr_data = DataPair.CombineDataPairsIntoString(olist);
    }

}

