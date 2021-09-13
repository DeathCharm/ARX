using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace ARX
{

    public class PrefabItemPoolViewer : ARX_IEnumerableViewer<ARX_PrefabItemPool.Item>
    {

        public PrefabItemPoolViewer(ARX_PrefabItemPool target)
        {
            mo_target = target;
            nNewlinesAfterSlide = 2;
        }

        ARX_PrefabItemPool mo_target;
        public override bool V_DrawNewItemButton(RectGuide oGuide, bool bIsTopDrawn)
        {
            oGuide.NewLine();
            return base.V_DrawNewItemButton(oGuide, bIsTopDrawn);
        }

        public override bool I_Equal(ARX_PrefabItemPool.Item one, ARX_PrefabItemPool.Item two)
        {
            return one == two;
        }

        public override void I_DrawUnique(RectGuide oGuide, ARX_PrefabItemPool.Item oItem, int nIndex, List<ARX_PrefabItemPool.Item> oList)
        {
            oGuide.MoveLastRect(30);
            GUI.Label(oGuide.GetNextRect(100), "Item Prefab");
            oGuide.MoveLastRect(25);
            oItem.mo_itemGameObject = EditorGUI.ObjectField(oGuide.GetNextRect(150), oItem.mo_itemGameObject, typeof(GameObject), false) as GameObject;
            oGuide.MoveLastRect(50);


            oGuide.MoveLastRect(25);
            GUI.Label(oGuide.GetNextRect(100), "Item Pool");
            oGuide.MoveLastRect(25);
            oItem.mo_itemPool = EditorGUI.ObjectField(oGuide.GetNextRect(150), oItem.mo_itemPool, typeof(ARX_PrefabItemPool), false) as ARX_PrefabItemPool;
            

        }

        public override void I_ClickNewItem(List<ARX_PrefabItemPool.Item> moa_list)
        {
            moa_list.Add(new ARX_PrefabItemPool.Item());
        }

        public override void I_DrawName(RectGuide oGuide, ARX_PrefabItemPool.Item oItem, int nIndex, List<ARX_PrefabItemPool.Item> oList)
        {
            GUI.Label(oGuide.GetNextRect(100), "Name");
            oGuide.MoveLastRect(25);
            oItem.name = GUI.TextField(oGuide.GetNextRect(100), oItem.name);
            oGuide.MoveLastRect(50);


            GUI.Label(oGuide.GetNextRect(100), "Weight");
            oGuide.MoveLastRect(25);
            oItem.mn_weight = EditorGUI.IntField(oGuide.GetNextRect(100), oItem.mn_weight);
            oGuide.MoveLastRect(25);


            GUI.Label(oGuide.GetNextRect(100), mo_target.GetDrawChancePercentage(oItem) + "%");
            oGuide.NewLine();
        }

        public override void I_ReactToDeleteButtonClickedForItem(ARX_PrefabItemPool.Item oItem, int nIndex)
        {
            moa_data.Remove(oItem);
        }
    }

    [CustomEditor(typeof(ARX_PrefabItemPool))]
    public class ARX_CustomEditor_PrefabItemPool:Editor
    {

        ARX_PrefabItemPool GetTarget { get { return (ARX_PrefabItemPool)target; } }
        Rect GetRect
        {
            get
            {
                float width = 1000;
                float height = 5000;
                return GUILayoutUtility.GetRect(width, height);
            }
        }

        void Save()
        {
            EditorUtility.SetDirty(GetTarget);
            AssetDatabase.SaveAssets();
        }
        
        private void OnDestroy()
        {
            Save();
        }

        public override void OnInspectorGUI()
        {
            RectGuide oGuide = new RectGuide(GetRect);
            PrefabItemPoolViewer viewer = new PrefabItemPoolViewer(GetTarget);



            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.black;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = 36;


            style.fontStyle = FontStyle.Normal;
            GUI.Label(oGuide.GetNextRect(200, 60), "Prefab Pool", style);

            oGuide.NewLine(2);
            GUI.Label(oGuide.GetNextRect(200, 60), "\"" + GetTarget.name + "\"", style);

            oGuide.NewLine(4);

            viewer.V_Draw(oGuide, GetTarget.Items);

            base.OnInspectorGUI();
        }

    }
}
