// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using MarkerMetro.Unity.WinLegacy.Reflection;

namespace Fungus
{
    /// <summary>
    /// Invokes a method of a component via reflection. Supports passing multiple parameters and storing returned values in a Fungus variable.
    /// </summary>
    [CommandInfo("Scripting",
                 "Set Enabled",
                 "Invokes a method of a component via reflection. Supports passing multiple parameters and storing returned values in a Fungus variable.")]
    public class SetEnabled : Command
    {
        [Tooltip("A description of what this command does. Appears in the command summary.")]
        [SerializeField] protected string description = "";

        [Tooltip("GameObject containing the component method to be invoked")]
        [SerializeField] protected GameObject targetObject;

        [HideInInspector]
        [Tooltip("Name of assembly containing the target component")]
        [SerializeField] protected string targetComponentAssemblyName;

        [HideInInspector]
        [Tooltip("Full name of the target component")]
        [SerializeField] protected string targetComponentFullname;

        [HideInInspector]
        [Tooltip("Display name of the target component")]
        [SerializeField] protected string targetComponentText;
        
        
        [HideInInspector]
        [Tooltip("Janky Hack that sets the component to enabled or disabled")]
        [SerializeField] protected bool setEnabled = true;

        protected Type componentType;
        protected Component objComponent;



        protected virtual void Awake()
        {
            if (componentType == null)
            {
                componentType = ReflectionHelper.GetType(targetComponentAssemblyName);
            }

            if (objComponent == null)
            {
                if (targetObject == null)
                {
                    Debug.LogError("No target object for block " + ParentBlock.name + " in flowchart " + GetFlowchart().name);
                }
                objComponent = targetObject.GetComponent(componentType);
            }
            
        }
        

        #region Public members

        /// <summary>
        /// GameObject containing the component method to be invoked.
        /// </summary>
        public virtual GameObject TargetObject { get { return targetObject; } }

        public override void OnEnter()
        {
            try
            {
                Behaviour buf = (Behaviour)objComponent;
                buf.enabled = setEnabled;
            }
            catch
            {
                //Janky hack, m8
            }
            Continue();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 100, 217, 255);
        }

        public override string GetSummary()
        {
            if (targetObject == null)
            {
                return "Error: targetObject is not assigned";
            }

            if (!string.IsNullOrEmpty(description))
            {
                return description;
            }

            return targetObject.name + "." + targetComponentText + " to " + setEnabled.ToString();
        }

        #endregion
    }

}