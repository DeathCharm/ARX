using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ARX;

namespace ARX
{
    [CustomEditor(typeof(ARX_QuestVariableBox))]
    public class ARX_CustomEditor_QuestVariableBox:Editor
    {
        ARX_QuestVariableBox GetTarget { get { return (ARX_QuestVariableBox)target; } }
        Rect GetRect
        {
            get 
            {
                int nHeight = 16 * GetTarget.Variables.Count + 80;
                return GUILayoutUtility.GetRect(300, nHeight);
            }
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(GetTarget);
            AssetDatabase.SaveAssets();
        }

        void Print(RectGuide oGuide)
        {
            List<KeyValuePair<string, int>> olist = new List<KeyValuePair<string, int>>();

            List<KeyValuePair<string, int>> oDelete = new List<KeyValuePair<string, int>>();
            foreach (KeyValuePair<string, int> keyvalue in GetTarget.Variables)
            {
                bool bDelete = GUI.Button(oGuide.GetNextRect(16, 16), "X");
                GUI.changed = false;

                //Label the key
                GUI.Label(oGuide.GetNextRect(150, 16), keyvalue.Key);
                //Set the value
                int n = EditorGUI.IntField(oGuide.GetNextRect(50, 16), keyvalue.Value);

                if (GUI.changed)
                {
                    GetTarget.Variables[keyvalue.Key] = n;
                    GUI.changed = false;
                }
                oGuide.NewLine();
                if (bDelete)
                {
                    oDelete.Add(keyvalue);
                }

            }

            for (int i = 0; i < oDelete.Count; i++)
            {
                GetTarget.Variables.Remove(oDelete[i].Key);
            }
            
        }
        

        int nInt = 0;
        string strString = "";


        public override void OnInspectorGUI()
        {
            RectGuide oGuide = new RectGuide(GetRect, 16);

            //Add New thingie

            //Label the key
            strString = GUI.TextField(oGuide.GetNextRect(150, 16), strString);
            //Set the value
            nInt = EditorGUI.IntField(oGuide.GetNextRect(50, 16), nInt);

            oGuide.NewLine(2);
            bool bAdd = GUI.Button(oGuide.GetNextRect(100, 16), "Add new?");
            if (bAdd)
            {
                GetTarget.Variables[strString] = nInt;
            }
            oGuide.NewLine(2);

            //Print old thingies
            Print(oGuide);

        }
    }
}
