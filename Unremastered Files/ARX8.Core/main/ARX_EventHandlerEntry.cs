using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARX;
using UnityEngine.Events;
using UnityEngine;

namespace ARX
{
    /// <summary>
    /// Helper class holding variables for EventHandlers.
    /// </summary>
    public class ARX_EventHandlerEntry
    {
        public string mstr_processName = "unnamedProcess";
        public ARX_EventHandlerDelegate mfunc_eventAction;
        public ARX_Process mo_process = null;

        /// <summary>
        /// Set's the held process and eventDelegate to null.
        /// To be used before this entry is removed from a list
        /// </summary>
        public void Destroy()
        {
            //Debug.Log("Destroying EventHandlerEntry " + mstr_processName);
            mo_process = null;
            mfunc_eventAction = null;
        }

        public void RunEntry(DataString dat = null)
        {
            if (dat == null)
                dat = new DataString(this);
            
            //UnityEngine.Debug.Log("Running entry " + mstr_processName + " on datastring " + dat.ToString());
            
            if (mfunc_eventAction != null)
            {
                //If the entry has an associated process that is accepting messages
                //run the entry
                if (mo_process != null && mo_process.IsAcceptingMessages())
                {
                    Debug.Log("The process " + mo_process.name + " is accepting messages");
                    mfunc_eventAction(dat);
                    return;
                }
                //Else if the entry has no associated process
                //run the entry
                else if (mo_process == null)
                {

                    Debug.Log("No process is in this event handler entry. Running the function without it.");
                    mfunc_eventAction(dat);
                }
            }
            else if (mfunc_eventAction == null)
            {
                UnityEngine.Debug.LogError("No Lua Event or event action for " + mstr_processName + ". This means messages cannot be processed at all for this process!.");
            }
        }

        public ARX_EventHandlerEntry(string strProcessName)
        {
            mstr_processName = strProcessName;
        }

        

        public ARX_EventHandlerEntry(ARX_EventHandlerDelegate oEventAction,  string strProcessName, ARX_Process oProcess = null)
        {
            mo_process = oProcess;
            mstr_processName = strProcessName;
            mfunc_eventAction = oEventAction;
            
        }
        
    }
}