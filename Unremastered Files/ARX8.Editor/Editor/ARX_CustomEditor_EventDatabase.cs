//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEditor;
//using ARX;


//public class EventDataBaseViewer : ARX_IEnumerableViewer<string>
//{
//    ARX_EventDatabase mo_database = null;
//    Dictionary<string, ARX_Event> mo_dictionary
//    {
//        get
//        {
//            return mo_database.mo_eventDictionary;
//        }
//    }

//    public EventDataBaseViewer(ARX_EventDatabase oDatabase)
//    {
//        mo_database = oDatabase;
//    }

//    public override void IClickNewItem(List<string> moa_list)
//    {

//    }

//    public override void IDrawName(RectGuide oGuide, string oItem, int nIndex, List<string> oList)
//    {
//        ARX_Event EV = mo_dictionary[oItem];
//        if (EV != null)
//            EditorGUI.ObjectField(oGuide.GetNextRect(150, 16), EV, typeof(ARX_Event), false);
//        else
//            GUI.Label(oGuide.GetNextRect(150, 16), "Error: Event not present in dictionary.");

//    }

//    public override void IDrawUnique(RectGuide oGuide, string oItem, int nIndex, List<string> oList)
//    {

//    }

//    public override bool IEqual(string one, string two)
//    {
//        return one == two;
//    }

//    public override void IReactToDeleteButtonClickedForItem(string oItem, int nIndex)
//    {
//        mo_dictionary.Remove(oItem);
//    }

//    public override bool DrawNewItemButton(RectGuide oGuide)
//    {
//        ARX_Event oEvent = (ARX_Event)EditorGUI.ObjectField(oGuide.GetNextRect(150, 16), null, typeof(ARX_Event), false);
//        if (oEvent != null)
//            mo_dictionary[oEvent.name] = oEvent;

//        return false;
//    }

//    public void Draw(RectGuide oGuide)
//    {
//        Draw(oGuide, ARX_EventDatabase.GetNamesOfEvents());
//    }
//}


//[CustomEditor(typeof(ARX_EventDatabase))]
//    public class ARX_CustomEditor_EventDatabase : Editor
//    {


//    EventDataBaseViewer mo_viewer = null;
//    Rect GetRect
//    {
//        get
//        {
//            int nHeight = 16;

//            nHeight += 16 * GetTarget.mo_eventDictionary.Count;

//            return GUILayoutUtility.GetRect(300, nHeight);
//        }
//    }

//    ARX_EventDatabase GetTarget { get { return (ARX_EventDatabase)target; } }

//    public override void OnInspectorGUI()
//    {
//        if (mo_viewer == null)
//            mo_viewer = new EventDataBaseViewer(GetTarget);

//        RectGuide oGuide = new RectGuide(GetRect, 16);

//        mo_viewer.Draw(oGuide);

//        if (GUI.changed)
//            AssetDatabase.SaveAssets();
//    }

//}

