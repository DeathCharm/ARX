using ARX;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A unique entity in the game world.
/// </summary>
public class ARX_Script_Actor : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler,
    IPointerExitHandler, IPointerUpHandler
{

    #region Variables

    #region Init
    /// <summary>
    /// Helper variable determines if this actor been initialized
    /// </summary>
    protected bool mb_initialized = false;

    /// <summary>
    /// Helper variable determines if this actor had its first update yet
    /// </summary>
    protected bool mb_firstUpdate = true;
    #endregion

    #region Name

    /// <summary>
    /// Returns this object's Unity name + its UniqueID
    /// </summary>
    public string NameAndID
    {
        get
        {
            return name + UniqueID;
        }
    }
    #endregion

    #region Event Record

    /// <summary>
    /// The list of ARX_Events this actor has subscribed to. When this actor is destroyed,
    /// this actor is unsubscribed from all of these events.
    /// </summary>
    EventSubscriptionRecord mo_eventRecord;

    /// <summary>
    /// Accessor for the list of events this actor is subscribed to.
    /// </summary>
    public EventSubscriptionRecord EventRecord
    {
        get
        {
            if (mo_eventRecord == null)
                mo_eventRecord = new EventSubscriptionRecord();
            return mo_eventRecord;
        }
    }

    #endregion

    #region Mouse 

    /// <summary>
    /// When the mouse is over this object, 
    /// returns true.
    /// </summary>
    private bool mb_pointerOver = false;

    /// <summary>
    /// Returns true when the mouse button is held down.
    /// </summary>
    private bool mb_mouseOneDown = false;

    /// <summary>
    /// The amount of time the mouse has been hovering over this actor.
    /// </summary>
    float mnf_hoverTime = 0;

    /// <summary>
    /// The amount of time in seconds the mouse one button has been held on this actor.
    /// </summary>
    float mnf_mouseDownTime = 0;

    /// <summary>
    /// The time at which the mouse one button was pressed on this actor.
    /// </summary>
    private float mnf_mouseDownStart = 0;

    /// <summary>
    /// The threshhold in seconds by which two clicks will count as a double click.
    /// </summary>
    const float DOUBLECLICK_DELAY = 0.5F;

    /// <summary>
    /// Returns the time in seconds that the mouse cursor has been hovering over the actor.
    /// </summary>
    public float HoverTime { get { return mnf_hoverTime; } }

    /// <summary>
    /// Returns the time in seconds that the mouse one button has been down after this actor was clicked.
    /// was clicked.
    /// </summary>
    public float MouseOneDownTime { get { return mnf_mouseDownTime; } }
    #endregion//End Mouse region

    #region UniqueID

    /// <summary>
    /// This actor's unique ID. Use UniqueID to access it.
    /// </summary>
    int mn_uniqueID = 0;

    /// <summary>
    /// This actor's unique ID. Use this variable to get this game actor from ARX.Global.GetActor(int nID)
    /// </summary>
    public int UniqueID
    {
        get
        {
            if (mn_uniqueID == 0)
                mn_uniqueID = ARX_Actor.GetNextUniqueID;
            return mn_uniqueID;
        }
    }

    #endregion

    #endregion//End Variables region

    #region Functions
    /// <summary>
    /// Returns this object as a serialized string
    /// </summary>
    /// <returns></returns>
    public string GetSerializedString()
    {
        return ARX_File.SerializeObject(this);
    }
    #endregion

    #region Virtuals
    /// <summary>
    /// Returns a datastring containing information
    /// pointing to this actor
    /// </summary>
    public virtual DataString V_GetThisActorMessage
    {
        get
        {
            DataString dat = new DataString(this);
            dat.SetInt("source", UniqueID);
            return dat;
        }
    }

    /// <summary>
    /// Method called by Factory objects after creating an instance of this script, passing on
    /// to this script the data to be used in constructing this.
    /// </summary>
    /// <param name="oData"></param>
    public virtual void V_Construct(DataString oData) { }

    /// <summary>
    /// Method called on this object's first active frame during Play.
    /// </summary>
    public virtual void V_OnInitialize() { }

    /// <summary>
    /// Doesn't do much of anything right now. I should change that.
    /// </summary>
    /// <returns></returns>
    public virtual string V_GetActionReport()
    {
        return "";
    }

    /// <summary>
    /// Method called on the first update of the script.
    /// </summary>
    public virtual void V_OnFirstUpdate()
    {

    }

    /// <summary>
    /// Processes a generic ARX message in the form of a Data String. A precursor to the Event subscription
    /// architecture currently used.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public bool V_ProcessMessage(DataString message)
    {
        return true;
    }

    /// <summary>
    /// Called once every processor rotation.
    /// </summary>
    public virtual bool V_OnUpdate() { return true; }

    /// <summary>
    /// Called once every frame. Use for physical world calculations like movement and physics interactions.
    /// </summary>
    public virtual bool V_OnFixedUpdate() { return true; }

    /// <summary>
    /// Called when this actor is destroyed. The actor is also automatically removed from the global actor list.
    /// </summary>
    public virtual void V_OnDestroy() { }

    /// <summary>
    /// Called whenever this actor is enabled in the Unity Hierarchy.
    /// </summary>
    public virtual void V_OnEnable() { }

    /// <summary>
    /// Called when this actor is double clicked.
    /// </summary>
    public virtual void V_OnDoubleClick() { }

    /// <summary>
    /// Called every frame this actor is hovered over by the mouse.
    /// </summary>
    /// <param name="nfTimeFloated"></param>
    public virtual void V_OnHover(float nfTimeFloated) { }

    /// <summary>
    /// Called every frame this actor is clicked on.
    /// </summary>
    /// <param name="nfTimeMouseDown"></param>
    public virtual void V_OnMouseHeld(float nfTimeMouseDown) { }

    /// <summary>
    /// Called when this actor is right clicked.
    /// </summary>
    public virtual void V_OnRightMouseDown() { }

    /// <summary>
    /// Called when this actor is first hovered over
    /// </summary>
    public virtual void V_OnHoverStart()
    {

    }

    /// <summary>
    /// Called when the right click button for this actor is released.
    /// </summary>
    /// <param name="nfTimeMouseDown"></param>
    public virtual void V_OnRightMouseUp() { }

    /// <summary>
    /// Called when this actor is clicked.
    /// </summary>
    public virtual void V_MouseDown() { }

    /// <summary>
    /// Called when the click button for this actor is released.
    /// </summary>
    /// <param name="nfTimeMouseDown"></param>
    public virtual void V_OnMouseUp(float nfTimeMouseDown) { }

    /// <summary>
    /// Called when the mouse first hits this actor.
    /// </summary>
    public virtual void V_OnMouseEnter() { }

    /// <summary>
    /// Called when this actor is disabled
    /// </summary>
    public virtual void V_OnDisable() { }

    /// <summary>
    /// Called when the mouse cursor leaves this actor.
    /// </summary>
    public virtual void V_OnMouseExit() { }
    #endregion

    #region Unity Overrides

    /// <summary>
    /// Call virtual V_OnDisable
    /// </summary>
    private void OnDisable()
    {
        V_OnDisable();
    }

    /// <summary>
    /// Calls V_OnLeftMouseDown or V_OnRightMouseDown upon a 
    /// left or right click on this object. Works with UnityUI Elements
    /// </summary>
    /// <param name="oData"></param>
    public void OnPointerDown(PointerEventData oData)
    {
        if (oData.button == PointerEventData.InputButton.Right)
        {
            V_OnRightMouseDown();
            return;
        }

        OnMouseDown();
    }

    /// <summary>
    /// Calls V_OnLeftMouseUp or V_OnRightMouseUp upon a 
    /// left or right click release on this object. Works with UnityUI Elements
    /// </summary>
    /// <param name="oData"></param>
    public void OnPointerUp(PointerEventData oData)
    {
        if (oData.button == PointerEventData.InputButton.Right)
        {
            V_OnRightMouseUp();
            return;
        }

        OnMouseUp();
    }

    /// <summary>
    /// Called when the mouse first passes over it.
    /// Works on UnityUI Elements
    /// </summary>
    /// <param name="oData"></param>
    public void OnPointerEnter(PointerEventData oData)
    {
        mb_pointerOver = true;
        OnMouseEnter();
    }

    /// <summary>
    /// Called when the mouse first exits it.
    /// Works on UnityUI Elements
    /// </summary>
    /// <param name="oData"></param>
    public void OnPointerExit(PointerEventData oData)
    {
        mb_pointerOver = false;
        OnMouseExit();
    }

    /// <summary>
    /// Called when the left mouse is released.
    /// Works on non-Unity UI elements.
    /// </summary>
    private void OnMouseUp()
    {
        V_OnMouseUp(mnf_mouseDownTime);
        mb_mouseOneDown = false;
        mnf_mouseDownTime = 0;

    }

    /// <summary>
    /// Called when the left mouse is clicked down.
    /// Works on non-Unity UI elements.
    /// </summary>
    private void OnMouseDown()
    {
        //If a double click was just performed
        if (Time.time - mnf_mouseDownStart <= DOUBLECLICK_DELAY)
        {
            mb_mouseOneDown = true;
            mnf_mouseDownStart = Time.time;
            V_OnDoubleClick();
        }
        //Else, normal click
        else
        {
            mb_mouseOneDown = true;
            mnf_mouseDownStart = Time.time;
            V_MouseDown();
        }

    }

    /// <summary>
    /// Called when the mouse enters over this object.
    /// Works on non-Unity UI elements.
    /// </summary>
    private void OnMouseEnter()
    {
        //Begin Hover timer
        V_OnMouseEnter();
    }

    /// <summary>
    /// Called when the mouse exits this object.
    /// Works on non-Unity UI elements.
    /// </summary>
    private void OnMouseExit()
    {
        mnf_hoverTime = 0;

        //End Hover Timer
        V_OnMouseExit();
    }

    /// <summary>
    /// Called eery frame the mouse is over this object.
    /// Works on non-Unity UI elements.
    /// </summary>
    private void OnMouseOver()
    {
        if (mnf_hoverTime == 0)
            V_OnHoverStart();

        //Add to the hover timer        
        mnf_hoverTime += Time.deltaTime;

        //Run IHover with the saved hover timer
        V_OnHover(mnf_hoverTime);
    }

    private void OnEnable()
    {
        V_OnEnable();
    }

    private void OnDestroy()
    {
        ARX.Global.RemoveActor(this);
        EventRecord.UnsubscribeFromAllEvents();
        V_OnDestroy();
    }

    private void FixedUpdate()
    {
        V_OnFixedUpdate();
    }

    void Awake()
    {

        ARX.Global.AddActor(this);

        Debug.Assert((mb_initialized == false), name + " has run before being updated.");

        if (!mb_initialized)
        {
            V_OnInitialize();
            mb_initialized = true;
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (mb_firstUpdate)
        {
            mb_firstUpdate = false;
            V_OnFirstUpdate();
        }
        if (mb_mouseOneDown)
        {
            mnf_mouseDownTime += Time.deltaTime;
            V_OnMouseHeld(Time.time - mnf_mouseDownStart);
        }
        if (mb_pointerOver)
            OnMouseOver();

        V_OnUpdate();
    }

    #endregion
    
}


