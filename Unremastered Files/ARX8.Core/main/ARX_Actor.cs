using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ARX;

/// <summary>
/// Base class for objects that interact with ARX_Events
/// </summary>
public class ARX_Actor : ScriptableObject
{

    #region Variables and Accessors

    /// <summary>
    /// The last unique ID selected by an ARX_Actor
    /// </summary>
    static int gnLastID = -1;

    /// <summary>
    /// Static function increments and then returns the next available global UniqueID
    /// </summary>
    public static int GetNextUniqueID
    {
        get
        {
            return ++gnLastID;
        }
    }

    /// <summary>
    /// The list of ARX_Events this actor has subscribed to. When this actor is destroyed,
    /// this actor is unsubscribed from all of these events.
    /// </summary>
    EventSubscriptionRecord mo_eventRecord;

    /// <summary>
    /// Accessor for the list of events this actor is subscribed to.
    /// </summary>
    protected EventSubscriptionRecord EventRecord
    {
        get
        {
            if (mo_eventRecord == null)
                mo_eventRecord = new EventSubscriptionRecord();
            return mo_eventRecord;
        }
    }


    /// <summary>
    /// Returns a datastring containing information
    /// pointing to this actor
    /// </summary>
    public virtual DataString GetMessage
    {
        get
        {
            DataString dat = new DataString(this);
            dat.SetInt(GameIDs.ValueUniqueID, UniqueID);
            return dat;
        }
    }

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
                mn_uniqueID = GetNextUniqueID;
            return mn_uniqueID;
        }
    }

    #endregion

    #region Functions

    /// <summary>
    /// Serializes this object and returns it in string form.
    /// </summary>
    /// <returns></returns>
    public string GetSerializedString()
    {
        return ARX_File.SerializeObject(this);
    }


    /// <summary>
    /// Changes this actor's unique id.
    /// Only to be used when dealing with Unity's CreateNewInstance function
    /// which copies the first instance it makes.
    /// </summary>
    public void SetAsUniqueInstance() { mn_uniqueID = GetNextUniqueID; }

    #endregion

    private void OnEnable()
    {
        ARX.Global.AddActor(this);
    }

    private void OnDestroy()
    {
        ARX.Global.RemoveActor(this);
    }
}

