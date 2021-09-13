using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ARX;

public class DnDObject
{
    public DnDObject(Rect oRect) { rectNormal = oRect; }
    public Rect rectNormal, rectDragged;
    public Color color = Color.red;
    public string name = "Unnamed DnDObject";

    public bool bDropped = false;

    public override string ToString()
    {
        return name;
    }

    public virtual void Draw()
    {
        EditorGUI.DrawRect(rectNormal, color);
    }
}


public class ARX_DragAndDrop_UnityEditor<Item,DropZone> 
    : ARX_DragAndDrop<Item, DropZone> where Item : DnDObject where DropZone : DnDObject
{
    EditorWindow window;

    public ARX_DragAndDrop_UnityEditor(List<Item> oaDrags, List<DropZone> oaDropZones, EditorWindow oWindow) : base(oaDrags, oaDropZones)
    {
        window = oWindow;
    }

    #region Abstracts

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
        if (one == null || two == null)
            return false;
        return one.Equals(two);
    }

    public override bool I_IsEqualToZone(DropZone one, DropZone two)
    {
        if (one == null || two == null)
            return false;
        return one.Equals(two);
    }

    public override bool I_IsHoveringDragItem(Item item)
    {
        return item.rectNormal.Contains(I_GetMousePositionOnScreen());
    }

    public override bool I_IsHoveringDropZone(DropZone zone)
    {
        return zone.rectNormal.Contains(I_GetMousePositionOnScreen());
    }

    public override bool I_IsIntersecting(Item item, DropZone zone)
    {
        return item.rectDragged.Overlaps(zone.rectNormal, true);
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

    public override void V_ReactToHoverZone(DropZone item)
    {
        item.color = Color.yellow;
    }


    public override void V_ReactToUnhoverZone(DropZone item)
    {
        item.color = Color.blue;
    }

    public override void V_ReactToClickZone(DropZone zone)
    {

        zone.color = Color.black;
    }

    public override void V_ReactToDropInZone(Item item, DropZone zone)
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
        window.Repaint();
    }

    #endregion

}