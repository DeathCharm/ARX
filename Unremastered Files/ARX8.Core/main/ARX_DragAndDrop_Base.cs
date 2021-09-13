using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for implementing DragAndDrop functionality
/// </summary>
/// <typeparam name="DragItem"></typeparam>
/// <typeparam name="DropZone"></typeparam>
public abstract class ARX_DragAndDrop <DragItem, DropZone>
{
    #region Constructors
    public ARX_DragAndDrop(List<DragItem> oaDrags, List<DropZone> oaZones)
    {
        moa_dragItems = oaDrags;
        moa_dropZones = oaZones;
    }
    public ARX_DragAndDrop()
    {
        moa_dragItems = new List<DragItem>();
        moa_dropZones = new List<DropZone>();
    }

    #endregion

    #region Variables
    //Private
    private DragItem mo_currentlyHoveringItem = default(DragItem);
    private DropZone mo_currentlyHoveringZone = default(DropZone);
    private DragItem mo_currentlyDragging = default(DragItem);

/// <summary>
    /// Is the drag button being held.
    /// </summary>
    private bool bIsClickHeld = false;

    //Public
    public List<DragItem> moa_dragItems = new List<DragItem>();
    public List<DropZone> moa_dropZones = new List<DropZone>();

    
    #endregion
    
    #region Abstract Functions

    /// <summary>
    /// Return true if a drag should be canceled.
    /// </summary>
    /// <returns></returns>
    public abstract bool I_IsDragCancelConditionMet();

    /// <summary>
    /// Return true when the click or drag button is released
    /// </summary>
    /// <returns></returns>
    public abstract bool I_IsClickReleased();

    /// <summary>
    /// Return true with the click or drag button is pressed
    /// </summary>
    /// <returns></returns>
    public abstract bool I_IsClicking();

    /// <summary>
    /// Return true if the item is intersecting the dropzone.
    /// If click is released while thie returns true, ReactToDropInZone will
    /// execute.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="zone"></param>
    /// <returns></returns>
    public abstract bool I_IsIntersecting(DragItem item, DropZone zone);

    /// <summary>
    /// Return true when the mouse is hovering over this zone.
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    public abstract bool I_IsHoveringDropZone(DropZone zone);

    /// <summary>
    /// Return true when the mouse if hovering over this item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public abstract bool I_IsHoveringDragItem(DragItem item);

    /// <summary>
    /// Return the mouse's position on screen.
    /// </summary>
    /// <returns></returns>
    public abstract Vector2 I_GetMousePositionOnScreen();

    /// <summary>
    /// Return true if the two items are identical.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    public abstract bool I_IsEqualToItem(DragItem one, DragItem two);

    /// <summary>
    /// Return true if the two items are identical.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    public abstract bool I_IsEqualToZone(DropZone one, DropZone two);

    #endregion

    #region Virtual Functions
    public virtual void V_ReactToDrag(DragItem item, Vector2 vecMousePosition) { }

    public virtual void V_ReactToHoverItem(DragItem item) { }
    public virtual void V_ReactToUnhoverItem(DragItem item) { }
    public virtual void V_ReactToHoverZone(DropZone item) { }
    public virtual void V_ReactToUnhoverZone(DropZone item) { }

    public virtual void V_ReactToDragStart(DragItem item) { }
    public virtual void V_ReactToDragEnd(DragItem item) { }

    public virtual void V_ReactToClickZone(DropZone zone) { }

    public virtual void V_ReactToDropInZone(DragItem item, DropZone zone) { }
    public virtual void V_ReactToDragOverZone(DragItem item, DropZone zone) { }

    /// <summary>
    /// Executed every frame an item is being dragged.
    /// </summary>
    public virtual void V_RepaintScene() { }

    public virtual void V_DrawDragGhosts(DragItem itemDragged, Vector2 vecCurrentMousePositionOnScreen) { }
    #endregion 

    #region Helper

    bool IsCurrentlyDragging()
    {
        return I_IsEqualToItem(mo_currentlyDragging, default(DragItem)) == false && bIsClickHeld == true
            && bIsClickHeld;
    }

    void ResetDrag() {

        Debug.LogError("Ending Drag");
        //If something was being dragged
        if (IsCurrentlyDragging())
        {
            //React to end drag
            V_ReactToDragEnd(mo_currentlyDragging);
        }
        mo_currentlyDragging = default(DragItem);
        bIsClickHeld = false;
    }

    #endregion

    #region Execute Run
    void RunCheckClick() {
        //If a drag item is currently hovered
        if (I_IsEqualToItem(mo_currentlyHoveringItem, default(DragItem)) == false)
        {
            //Set item as current drag target
            mo_currentlyDragging = mo_currentlyHoveringItem;
            
            Debug.LogError("Beginning Drag on item " + mo_currentlyDragging);

            //Click drag item
            V_ReactToDragStart(mo_currentlyHoveringItem);

        }
        else
        {
            //Debug.Log("No drag item is hovered.");
        }

        //If a zone is currently hovered
        if (I_IsEqualToZone(mo_currentlyHoveringZone, default(DropZone)) == false)
        {
            //Click drag item
            V_ReactToClickZone(mo_currentlyHoveringZone);
        }
        else
        {
            //Debug.LogError("No drop zone is hovered.");
        }

    }

    void FindCurrentHovers()
    {
        bool isAnyZoneHovered = false;
        foreach (DropZone zone in moa_dropZones)
        {
            if (I_IsHoveringDropZone(zone))
            {
                Debug.Log("Hovering Zone " + zone);
                V_ReactToHoverZone(zone);
                isAnyZoneHovered = true;

                if (I_IsEqualToZone(mo_currentlyHoveringZone, zone) == false &&
                    I_IsEqualToZone(mo_currentlyHoveringZone, default(DropZone)) == false)
                {
                    //React to exit hover
                    Debug.Log("Unhovered zone " + mo_currentlyHoveringZone);
                    V_ReactToUnhoverZone(mo_currentlyHoveringZone);
                }
                //Set new current hover
                mo_currentlyHoveringZone = zone;
            }
            else
            {

            }

        }

        //If nothing was hovered
        //Set hover to none
        if (isAnyZoneHovered == false)
        { 
            //If the hovered zone is different from the last hovered zone
          //and the last hovered zone is not null
            if (I_IsEqualToZone(mo_currentlyHoveringZone, default(DropZone)) == false)
            {
                //React to exit hover
                Debug.Log("Unhovered zone " + mo_currentlyHoveringZone);
                V_ReactToUnhoverZone(mo_currentlyHoveringZone);
            }
            mo_currentlyHoveringZone = default(DropZone);
        }
        
        bool isAnyItemHovered = false;
        foreach (DragItem item in moa_dragItems)
        {
            if (I_IsHoveringDragItem(item))
            {
                //Run hover item
                V_ReactToHoverItem(item);
                isAnyItemHovered = true;
                //If the hovered item is different from the last hovered item
                //and the last hovered item is not null
                if (I_IsEqualToItem(item, mo_currentlyHoveringItem) == false 
                    && I_IsEqualToItem( mo_currentlyHoveringItem , default(DragItem)) == false)
                {
                    //React to exit hover
                    V_ReactToUnhoverItem(mo_currentlyHoveringItem);
                }
                //Set new current hover
                mo_currentlyHoveringItem = item;
                //Debug.Log("Currently hovering drag item " + item.ToString());
            }
        }

        //If nothing was hovered
        //Set hover to none
        if (isAnyItemHovered == false)
        {
            //If the hovered item is different from the last hovered item
         //and the last hovered item is not null
            if (I_IsEqualToItem(mo_currentlyHoveringItem, default(DragItem)) == false)
            {
                //React to exit hover
                V_ReactToUnhoverItem(mo_currentlyHoveringItem);
            }
            mo_currentlyHoveringItem = default(DragItem);
        }
    }

    public void Run()
    {
        
        
        FindCurrentHovers();
        
        //If dragging an item
        if (IsCurrentlyDragging())
        {
           RunDrag();
           V_RepaintScene();
            return;
            //Run Drag
            //Return
        }

        if (I_IsClicking())
        //If Click
        {
            Debug.Log("Click detected");

            bIsClickHeld = true;
            RunCheckClick();
        }

        if (I_IsClickReleased())
        {
            Debug.Log("Release detected");

            bIsClickHeld = false;
        }

        if (bIsClickHeld)
            Debug.Log("Held Click");

    }

    void RunDrag()
    {
        Debug.Log("Run Drag");

       

        //Run Drag Item
        //Draw Drag Ghosts
        V_ReactToDrag(mo_currentlyDragging, I_GetMousePositionOnScreen());

        Debug.Log("Drawing Drag Ghosts");
        V_DrawDragGhosts(mo_currentlyDragging, I_GetMousePositionOnScreen());

        //If over a drop zone
        foreach (DropZone zone in moa_dropZones)
        {
            if(I_IsEqualToItem(mo_currentlyDragging, default(DragItem)) == false)
                Debug.Log("Checking zone " + zone + " against " + mo_currentlyDragging);
            if (I_IsIntersecting(mo_currentlyDragging, zone))
            {
                Debug.Log("Item " + mo_currentlyDragging + " intersecting with " + zone);
                //Run Drag Over
                V_ReactToDragOverZone(mo_currentlyDragging, zone);

                //If click released
                if (I_IsClickReleased())
                {
                    //Run Drop Into
                    V_ReactToDropInZone(mo_currentlyDragging, zone);
                }
            }
        }

        //If cancel drag 
        if (I_IsDragCancelConditionMet())
        {
            //Reset drag items
            //Return
            ResetDrag();
            return;
        }
        
        //If click released
        if (I_IsClickReleased())
        {
            //Reset drag items
            ResetDrag();
        }

    }
    #endregion

}
