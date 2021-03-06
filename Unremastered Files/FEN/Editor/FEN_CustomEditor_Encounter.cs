using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using FEN;
using ARX;

namespace FEN
{
    class SortUnitsAlphabetically : ARX_Algorithm_BubbleSort<FEN_Unit>
    {
        public override bool I_IsHigherPriority(FEN_Unit movingItem, FEN_Unit listedItem)
        {
            return movingItem.name[0] < listedItem.name[0];
        }
    }

    public class EnemyEncounterViewer : ARX_IEnumerableViewer<FEN_Unit>
    {
        public override void I_ReactToDeleteButtonClickedForItem(FEN_Unit oItem, int nIndex)
        {
            moa_data.Remove(oItem);
        }

        public override void I_DrawName(RectGuide oGuide, FEN_Unit oItem, int nIndex, List<FEN_Unit> oList)
        {

            oItem = (FEN_Unit)EditorGUI.ObjectField(oGuide.GetNextRect(150, 16), oItem, typeof(FEN_Unit), false);
            oList[nIndex] = oItem;
        }

        public override void I_ClickNewItem(List<FEN_Unit> moa_list)
        {
            moa_list.Add(null);
        }

        public override void I_DrawUnique(RectGuide oGuide, FEN_Unit oItem, int nIndex, List<FEN_Unit> oList)
        {
            if (oItem == null)
                return;
        }

        public override bool I_Equal(FEN_Unit one, FEN_Unit two)
        {
            return one == two;
        }
    }

    [CustomEditor(typeof(FEN_Encounter))]
    class FEN_CustomEditor_Encounter : Editor
    {
        public FEN_Encounter GetTarget { get { return (FEN_Encounter)target; } }
        public Rect GetRect
        {
            get
            {
                return GUILayoutUtility.GetRect(300, 4000);
            }
        }

        static FEN_Unit[] allUnitAssets;

        private void OnDisable()
        {
            EditorUtility.SetDirty(GetTarget);
            AssetDatabase.SaveAssets();
        }
        

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            EnemyEncounterViewer viewer = new EnemyEncounterViewer();
            RectGuide oGuide = new RectGuide(GetRect, 32, true);

            oGuide.NewLine(4);

            if(GUI.Button(oGuide.GetNextRect(200,32), "Initiate this Combat"))
            {
                InitiateCombat();
            }

            oGuide.NewLine(4);


            CreateFileOutput();
        }

        public void CreateFileOutput()
        {
            List<FEN_Encounter> oaEncounters = EditorHelper.FindAssetsByType<FEN_Encounter>();
            string strReturn = "public static partial class CombatEncounters{\npublic static class EncounterList{\n";

            foreach(FEN_Encounter en in oaEncounters)
            {
                string strU = "public static FEN_Encounter " + en.name.FirstCharToUpper() + "{\nget\n{\n";
                strU += "return FEN.Loading.LoadEncounter(\"" + en.name + "\");\n}\n}\n";
                strReturn += strU;
            }

            strReturn += "\n}\n}";
            strReturn = ARX.VarGen.Defines.strUsing + "//Auto-Generated by FEN_CustomEditor_Encounter.cs\n\n" + strReturn;

            strReturn = ARX.VarGen.Defines.EncloseInNamespace(strReturn, "FEN");

           

            GetTarget.mstr_fileOuput = strReturn;
            
        }

        public void InitiateCombat()
        {
            if(Application.isPlaying == false)
            {
                Debug.LogError("This is a Playmode Only function. Combat can only be initiated while the game is playing.");
                return;
            }
            FEN.CombatEncounters.InitiateCombat(GetTarget);
        }
    }
}
