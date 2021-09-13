// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace Fungus.EditorUtils
{
    [CustomEditor(typeof(SetEnabled))]
    public class SetEnabledEditor : CommandEditor
    {
        SetEnabled targetSetEnabled;

        public override void DrawCommandGUI()
        {
            base.DrawCommandGUI();

            targetSetEnabled = target as SetEnabled;

            if (targetSetEnabled == null || targetSetEnabled.TargetObject == null)
                return;

            SerializedObject objSerializedTarget = new SerializedObject(targetSetEnabled);

            string component = ShowComponents(objSerializedTarget, targetSetEnabled.TargetObject);
            
        }

        private string ShowComponents(SerializedObject objTarget, GameObject gameObject)
        {
            var targetComponentAssemblyName = objTarget.FindProperty("targetComponentAssemblyName");
            var targetComponentFullname = objTarget.FindProperty("targetComponentFullname");
            var targetComponentText = objTarget.FindProperty("targetComponentText");
            var objComponents = gameObject.GetComponents<Component>();
            var objTypesAssemblynames = (from objComp in objComponents select objComp.GetType().AssemblyQualifiedName).ToList();
            var objTypesName = (from objComp in objComponents select objComp.GetType().Name).ToList();

            int index = objTypesAssemblynames.IndexOf(targetComponentAssemblyName.stringValue);

            index = EditorGUILayout.Popup("Target Component", index, objTypesName.ToArray());

            if (index != -1)
            {
                targetComponentAssemblyName.stringValue = objTypesAssemblynames[index];
                targetComponentFullname.stringValue = objComponents.GetType().FullName;
                targetComponentText.stringValue = objTypesName[index];
            }
            else
            {
                targetComponentAssemblyName.stringValue = null;
            }


            var jankySetEnabledProp = objTarget.FindProperty("setEnabled");

            EditorGUILayout.PropertyField(jankySetEnabledProp);

            objTarget.ApplyModifiedProperties();

            return targetComponentAssemblyName.stringValue;
        }
        
        
        private object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
    }
}