using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ARX;
using ARX.VarGen;

/// <summary>
/// Editor script for Editor Highlighter.
/// Highlights a Unity Object's name in the Hierarchy View.
/// </summary>
[InitializeOnLoad]
public class ARX_CustomEditor_EditorHighlighter
{
    static ARX_CustomEditor_EditorHighlighter()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItem_CB;
    }

    private static void HierarchyWindowItem_CB(int selectionID, Rect selectionRect)
    {
        UnityEngine.Object o = EditorUtility.InstanceIDToObject(selectionID);
        if (o == null)
            return;

        GameObject obj = (o as GameObject);
        
        if ((o as GameObject).GetComponent<ARX_Script_EditorHighlighter>() != null)
        {
            ARX_Script_EditorHighlighter h = (o as GameObject).GetComponent<ARX_Script_EditorHighlighter>();
            if (h.mb_highlightOn)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    EditorGUI.DrawRect(selectionRect, h.mo_color);
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    }
}

