//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using MoonSharp.Interpreter;

//namespace ARX
//{
//    [System.Serializable]
//    public class LuaEntry
//    {
//        public string mstr_processName = "New Lua Entry";
//        public string mstr_value = "";
//        public DynValue dynValue = null;
//        ARX_LuaBlock mo_parent = null;
//        protected DataString mo_currentEvent = null;

//        public void ProcessMessage(DataString dat)
//        {
//            mo_currentEvent = dat;
//            InvokeEntry();
//        }

//        public void InvokeEntry()
//        {
//            Script script = new Script();
//            if (mo_parent != null)
//                if (mo_parent.mb_injectBaseFileToLua == true && mo_parent.mo_baseLuaFile != null)
//                    InvokeEntry(mo_parent.mo_baseLuaFile.ToString(), script);
//        }

//        /// <summary>
//        /// Runs the inner value of this entry as a lua block.
//        /// </summary>
//        public void InvokeEntry(Script script)
//        {
//            Debug.Log("Attempting to invoke string " + mstr_value);
//            script.Globals["Debug"] = typeof(UnityEngine.Debug);
//            dynValue = script.DoString(mstr_value);

//        }

//        /// <summary>
//        /// Appends the prevalue to the inner value, then runs them both.
//        /// </summary>
//        /// <param name="strPrevalue"></param>
//        public void InvokeEntry(string strPrevalue, Script script)
//        {
//            dynValue = script.DoString(strPrevalue + mstr_value);
//            Debug.Log(dynValue.Number);
//        }

//        public LuaEntry(ARX_LuaBlock parent)
//        {
//            mo_parent = parent;
//        }
//    }
//}
