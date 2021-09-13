using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;
using System.Runtime.CompilerServices;

//public class LuaBlockViewer : ARX_IEnumerableViewer<LuaEntry>
//{
//    public LuaEntry mo_activeEntry = null;
//    public List<LuaEntry> mo_list = null;
//    public ARX_LuaBlock mo_event = null;

//    public LuaBlockViewer(ARX_LuaBlock eve)
//    {
//        mo_event = eve;
//    }

//    public override void IClickNewItem(List<LuaEntry> moa_list)
//    {
//        moa_list.Add(new LuaEntry(mo_event));
//    }

//    public override void IDrawName(RectGuide oGuide, LuaEntry oItem, int nIndex, List<LuaEntry> oList)
//    {
//        bool bClicked = GUI.Button(oGuide.GetNextRect(75, 16), oItem.mstr_processName);
//        if (bClicked)
//        {
//            mo_activeEntry = oItem;
//        }
//    }

//    public override void IDrawUnique(RectGuide oGuide, LuaEntry oItem, int nIndex, List<LuaEntry> oList)
//    {
//        if (GUI.Button(oGuide.GetNextRect(75, 16), "Execute"))
//        {
//            mo_event.InvokeLuaFunction(nIndex);
//        }
//    }

//    public override bool DrawDeleteButton(RectGuide oGuide)
//    {
//        if (GUI.Button(oGuide.GetNextRect(32, 16), "Del"))
//        {
//            return true;
//        }
//        return false;
//    }

//    public override bool IEqual(LuaEntry one, LuaEntry two)
//    {
//        return one == two;
//    }

//    public override void IReactToDeleteButtonClickedForItem(LuaEntry oItem, int nIndex)
//    {
//        if (mo_list != null)
//        {
//            mo_list.Remove(oItem);
//            if (mo_activeEntry == oItem)
//                mo_activeEntry = null;
//        }
//    }

//    public override void Draw(RectGuide oGuide, List<LuaEntry> oList)
//    {
//        mo_list = oList;
//        base.Draw(oGuide, oList);
//    }

//}

//[CustomEditor(typeof(ARX_LuaBlock))]
//public class ARX_CustomEditor_LuaEvent : Editor {

//    LuaBlockViewer mo_viewer;


//    Rect GetRect
//    {
//        get
//        {
//            return GUILayoutUtility.GetRect(0, 0, 300, 900);
//        }
//    }

//    ARX_LuaBlock GetTarget
//    {
//        get
//        {
//            return (ARX_LuaBlock)target;
//        }
//    }
    
//    public override void OnInspectorGUI()
//    {
//        if (mo_viewer == null)
//            mo_viewer = new LuaBlockViewer(GetTarget);

//        RectGuide oGuide = new RectGuide(GetRect, 16);

//        GUI.Label(oGuide.GetNextRect(150, 16), "Lua Scratch");
//        oGuide.NewLine(2);

        
//        ARX_LuaBlock oEvent = (ARX_LuaBlock)
//            EditorGUI.ObjectField(oGuide.GetNextRect(350, 16), "Lua Base File", GetTarget.mo_baseLuaFile, typeof(ARX_LuaBlock), false);

//        if (oEvent != GetTarget)
//            GetTarget.mo_baseLuaFile = oEvent;

//        oGuide.NewLine();

//        GetTarget.mb_injectBaseFileToLua = GUI.Toggle(oGuide.GetNextRect(150, 16), GetTarget.mb_injectBaseFileToLua, "Inject Base File?");
//        oGuide.NewLine(2);


//        GUI.Label(oGuide.GetNextRect(150, 16), "Lua Entries");
//        oGuide.NewLine();
//        mo_viewer.Draw(oGuide, GetTarget.moa_luaEntries);

//        oGuide.NewLine(4);

//        if (mo_viewer.mo_activeEntry != null)
//        {
//            mo_viewer.mo_activeEntry.mstr_processName = GUI.TextField(oGuide.GetNextRect(100, 16), mo_viewer.mo_activeEntry.mstr_processName);

//            oGuide.NewLine(2);

//            mo_viewer.mo_activeEntry.mstr_value = GUI.TextArea(oGuide.GetNextRect(300, 500), mo_viewer.mo_activeEntry.mstr_value);

//        }

//        oGuide.NewLine(2);
//        if (mo_viewer.mo_activeEntry != null)
//            oGuide.MoveLastRect(0, 500);

//        if (GUI.Button(oGuide.GetNextRect(100, 32), "Create Constant Process"))
//        {
//            ARX_Process process = new ARX_Process(GetTarget);
//            Main.PushToTopQueuedProcess(process);
//        }
//        oGuide.NewLine(2);
//        if (GUI.Button(oGuide.GetNextRect(100, 32), "Create Top Process"))
//        {
//            ARX_Process process = new ARX_Process(GetTarget);
//            Main.PushToTopQueuedProcess(process);
//        }
//    }



//    void CreateLuaBlockProcess()
//    {
        
//    }

//    void Close()
//    {
//        AssetDatabase.SaveAssets();
//    }
//}
