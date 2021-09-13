using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ARX;


public class GridTester : ARX_EditorGrid<Item, DropZ>
{

    public GridTester(int row, int column, float nodeWidth, float nodeHeight, Rect rect) : base(row, column, nodeWidth, nodeHeight, rect)
    {

    }

    #region Abstracts
    public override Item I_GetNewDragItem(Rect rect)
    {
        return new Item(rect);
    }


    public override DropZ I_GetNewDropZone(Rect rect)
    {
        return new DropZ(rect);
    }

    public override void V_ReactToDrag(Item item, Vector2 vecMousePosition)
    {
        Rect rect = new Rect(vecMousePosition.x, vecMousePosition.y, item.rectNormal.width, item.rectNormal.height);
        item.rectDragged = rect;
    }

    public override Vector2 I_GetMousePositionOnScreen()
    {
        Vector2 newMousePosition = Event.current.mousePosition;
        //newMousePosition.y = Screen.height - (newMousePosition.y + 25);
        return newMousePosition;
    }

    public override bool I_IsClicking()
    {
        return Event.current.type == EventType.MouseDown;
    }

    public override bool I_IsDragCancelConditionMet()
    {
        return Event.current.type == EventType.MouseLeaveWindow ||
            Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape ||
            Event.current.type == EventType.MouseDown && Event.current.button == 1;
    }

    public override bool I_IsClickReleased()
    {
        return Event.current.type == EventType.MouseUp;
    }

    public override bool I_IsEqualToItem(Item one, Item two)
    {
        return one.Equals(two);
    }

    public override bool I_IsEqualToZone(DropZ one, DropZ two)
    {
        return one.Equals(two);
    }

    public override bool I_IsHoveringDragItem(Item item)
    {
        return item.rectNormal.Contains(I_GetMousePositionOnScreen());
    }

    public override bool I_IsHoveringDropZone(DropZ zone)
    {
        return zone.rectNormal.Contains(I_GetMousePositionOnScreen());
    }

    public override bool I_IsIntersecting(Item item, DropZ zone)
    {
        return item.rectDragged.Overlaps(zone.rectNormal, true);
    }

    public override void I_DrawItem(Item item, Rect rect)
    {
        item.Draw();
    }

    public override void I_DrawBackground(Rect rect)
    {

    }

    #endregion

    #region Reacts
    public override void V_ReactToHoverItem(Item item)
    {
        item.color = Color.green;
    }

    public override void V_ReactToUnhoverItem(Item item)
    {
        item.color = Color.red;
    }

    public override void V_ReactToHoverZone(DropZ item)
    {
        item.color = Color.yellow;
    }


    public override void V_ReactToUnhoverZone(DropZ item)
    {
        item.color = Color.blue;
    }

    public override void V_ReactToClickZone(DropZ zone)
    {

        zone.color = Color.black;
    }

    public override void V_ReactToDropInZone(Item item, DropZ zone)
    {
        item.bDropped = true;
        zone.bDropped = true;
        base.V_ReactToDropInZone(item, zone);
    }

    public override void V_DrawDragGhosts(Item itemDragged, Vector2 vecCurrentMousePositionOnScreen)
    {
        EditorGUI.DrawRect(itemDragged.rectDragged, Color.white);
    }

    public override void V_RepaintScene()
    {

    }

    #endregion

}

public class ARX_Editor_DragGridTest : EditorWindow
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


    [MenuItem("ARX/Grid Test")]
    public static void ShowWindow()
    {
        EditorWindow oWindow = GetWindowWithRect<ARX_Editor_DragGridTest>(GetWindowRect);
    }


    #endregion

    #region Draw

    GridTester tester;
    private void OnGUI()
    {
        if (tester == null)
            tester = new GridTester(5, 5, 36, 36, GetWindowRect);
        tester.Draw();
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
    int nInspectorUpdateFrame = 0;
    private void OnInspectorUpdate()
    {
        if (nInspectorUpdateFrame % 10 == 0) //once a second (every 10th frame)
        {
            Repaint(); //force the window to repaint
        }
        nInspectorUpdateFrame++;
    }
    #endregion
}
