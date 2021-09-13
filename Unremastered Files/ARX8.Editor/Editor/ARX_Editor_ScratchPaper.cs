using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyWindow : EditorWindow
{


    static Rect GetWindowRect
    {
        get
        {
            Rect rect = new Rect(100, 100, 800, 800);
            return rect;
        }
    }

    [MenuItem("MyItem/My SubMenu")]
    public static void ShowWindow() { EditorWindow oWindow = GetWindowWithRect<MyWindow>(GetWindowRect); }

}