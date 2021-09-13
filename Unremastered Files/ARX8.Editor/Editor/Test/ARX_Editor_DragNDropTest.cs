using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Item : DnDObject{
    public Item(Rect rect) : base(rect) { }
}
public class DropZ  : DnDObject
{
    public DropZ(Rect rect) : base(rect) { }
}

public class TestieThingie : ARX_DragAndDrop_UnityEditor<Item, DropZ>
{
    EditorWindow window;

    public TestieThingie(List<Item> oaDrags, List<DropZ> oaDropZones, EditorWindow oWindow) : base(oaDrags, oaDropZones, oWindow)
    {
        Debug.LogError("Creating Testie Thing");
        window = oWindow;
    }

   

}


public class ARX_Editor_DragNDropTest : EditorWindow
{
    #region constants

    const int WINDOW_HEIGHT = 800;
    const int WINDOW_WIDTH = 800;

    const int TITLEBAR_HEIGHT = 32;
    const int TITLEBAR_WIDTH = 400;

    const int TABBAR_HEIGHT = 60;
    const int TABBAR_WIDTH = 600;

    const int CONTENT_WIDTH = WINDOW_WIDTH - 50;
    const int CONTENT_HEIGHT = WINDOW_HEIGHT - 150;
    #endregion

    #region helper

    // The position of the window
    public static Rect GetWindowRect = new Rect(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

    // Scroll position
    public Vector2 scrollPos = Vector2.zero;


    [MenuItem("ARX/Rect Drag Test")]
    public static void ShowWindow()
    {
        EditorWindow oWindow = GetWindowWithRect<ARX_Editor_DragNDropTest>(GetWindowRect);
    }


    #endregion

    #region Draw

    DropZ drop;
    Item drag;
    TestieThingie test;

    private void OnGUI()
    {
        if (drop == null)
        {
            drop = new DropZ(new Rect(500, 500, 100, 100));
            drop.name = "Helipad";
        }
        if (drag == null)
        {
            drag = new Item(new Rect(0, 0, 40, 40));
            drag.name = "Payload";
        }


        List<Item> oaDrags = new List<Item>();
        oaDrags.Add(drag);
        List<DropZ> oaDropZones = new List<DropZ>();
        oaDropZones.Add(drop);

        if (test == null)
        {
            test = new TestieThingie(oaDrags, oaDropZones, this);
        }

        test.Run();

        drag.Draw();
        drop.Draw();

        //Debug.Log("Updating " + test.I_GetMousePositionOnScreen() + drop.rect.ToString());
    }



    #endregion

    #region OnStart and OnEnd
    private void OnLostFocus()
    {

    }

    private void OnFocus()
    {

    }
    #endregion

    #region Frame Updates
    int mn_inspectorUpdateFrame = 0;
    private void OnInspectorUpdate()
    {
        if (mn_inspectorUpdateFrame % 10 == 0) //once a second (every 10th frame)
        {
            Repaint(); //force the window to repaint
        }
        mn_inspectorUpdateFrame++;
    }
    #endregion
}
