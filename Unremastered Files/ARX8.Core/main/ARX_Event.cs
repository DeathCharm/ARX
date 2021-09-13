using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using ARX.VarGen;

namespace ARX
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "ARX/ARX Event")]
    public class ARX_Event : ScriptableObject
    {
        #region Variables
        /// <summary>
        /// A list of all the ARX_EventHandler's subscribed to this event
        /// Gated for use in Play Mode
        /// Use the Accessor EventHandlerSeries to access the correct version.
        /// </summary>
        private List<ARX_EventHandlerEntry> _eventHandlerEntries = null;

        /// <summary>
        /// A list of all the ARX_EventHandler's subscribed to this event
        /// Gated for use in Edit Mode
        /// Use the Accessor EventHandlerSeries to access the correct version.
        /// </summary>
        private List<ARX_EventHandlerEntry> mo_eventHandlerEntries = null;

        /// <summary>
        /// Gated accessor for the _eventHandlerEntries and mo_eventHandlerEntries
        /// variables. Returns all the ARX_EventHandler's subscribed to this variable
        /// </summary>
        public List<ARX_EventHandlerEntry> EventHandlerSeries
        {
            get
            {
                //Create a Gate for the eventHandlerEntries variable

                if (mo_eventHandlerEntries == null)
                    mo_eventHandlerEntries = new List<ARX_EventHandlerEntry>();

                //If the application is playing
                if (Application.isPlaying)
                {
                    if (_eventHandlerEntries == null)
                        _eventHandlerEntries = new List<ARX_EventHandlerEntry>(mo_eventHandlerEntries.ToArray());
                    return _eventHandlerEntries;
                }
                //If the application is in Edit Mode
                else
                {
                    return mo_eventHandlerEntries;
                }

            }
        }

        /// <summary>
        /// An object detailing te order in qhich this events subscribers will be called.
        /// </summary>
        public ARX_EventOrder mo_eventOrderList;

        /// <summary>
        /// Has this event had its scriptable object loaded.
        /// Events will appear colored in the Unity Editor according to if they were loaded.
        /// Green = Loaded, Red = Not Loaded, Yellow = Loaded Twice
        /// </summary>
        public enum LOADSTATUS { UNLOADED, LOADED, LOADEDTWICE };

        /// <summary>
        /// The current load status of this event.
        /// Is not serialized so a new value is made on each compilation.
        /// </summary>
        [NonSerialized]
        private LOADSTATUS me_loadStatus = LOADSTATUS.UNLOADED;
        
        /// <summary>
        /// The last recorded load status of this event.
        /// Is serialized so the last recorded load state of this
        /// event is remembered and whown in the Unity Editor
        /// </summary>
        [SerializeField]
        private LOADSTATUS me_lastLoadStatusInPlayer = LOADSTATUS.UNLOADED;

        /// <summary>
        /// Used by the GameEvent file creator to place this event into a region
        /// </summary>
        public string mstr_eventRegion = "Misc. Event";

        /// <summary>
        /// Creates a file with all ARX_Events currently existing in the Editor.
        /// </summary>
        [TextArea(2,15)]
        [NonSerialized]
        public string mstr_gameEventFile = "";

        #endregion

        #region Accessors

        /// <summary>
        /// Accessor the the load status of this event.
        /// Returns either loadStatus in Play Mode or lastLoadStatusInPlayer in Edit Mode
        /// </summary>
        public LOADSTATUS LoadStatus {
            get {
                //If in Edit mode, return the lastLoadStatus
                //recorded before the last time Play Mode ended
                //Else if in Play mode, return the current load status
                if (Application.isPlaying == false)
                    return me_lastLoadStatusInPlayer;
                return me_loadStatus;
            }

            set
            {
                //If in Edit mode, 
                //Set the lastLoadStatus to the given value
                //Else if in Play mode, set the current load status to the given value
                if (Application.isPlaying == false)
                    me_lastLoadStatusInPlayer = value;
                else
                    me_loadStatus = value;
            }
        }
        #endregion
        
        #region Functions
        /// <summary>
        /// Increments the current load status of this event.
        /// If Unloaded, sets to Loaded.
        /// If Loaded, sets to LoadedTwice
        /// </summary>
        public void SetLoadStatus()
        {
            if (LoadStatus == LOADSTATUS.UNLOADED)
                LoadStatus = LOADSTATUS.LOADED;
            else if (LoadStatus == LOADSTATUS.LOADED)
                LoadStatus = LOADSTATUS.LOADEDTWICE;
        }
        
        /// <summary>
        /// Loads this event's Order List
        /// </summary>
        /// <param name="strFilename"></param>
        public void LoadEventOrderAsset(string strFilename)
        {
            mo_eventOrderList = FEN.Loading.LoadEventOrder(strFilename);
        }
        
        /// <summary>
        /// Fires the given event, sends the given dataString to all subscribers
        /// in order according to the eventOrderList 
        /// </summary>
        /// <param name="oDat"></param>
        public void FireEvent(DataString oDat = null)
        {
            if (oDat == null)
                oDat = new DataString(this);

            //oEventData.AddString(GameIDs.ValueEventName, this.name);

            //Using a buffer list in case the collection is altered during execution
            List<ARX_EventHandlerEntry> obuf = new List<ARX_EventHandlerEntry>(EventHandlerSeries);


            for (int i = 0; i < obuf.Count; i++)
            {
                if (oDat.IsConsumed == true)
                    break;
                
                obuf[i].RunEntry(oDat);
            }
        }

        /// <summary>
        /// Fires an event to all subscibers to this event.
        /// The fired message contains the given field "GameIDs.ValueUniqueID, nUniqueID"
        /// </summary>
        /// <param name="nUniqueID"></param>
        public void FireEvent(int nUniqueID)
        {
            DataString dat = new DataString(this);
            dat.SetInt(GameIDs.ValueUniqueID, nUniqueID);
            FireEvent(dat);
        }

        /// <summary>
        /// Fires an event to all subscibers to this event.
        /// The fired message contains the given field "GameIDs.ValueUniqueID, oActor.nUniqueID"
        /// </summary>
        /// <param name="oActor"></param>
        public void FireEvent(ARX_Script_Actor oActor)
        {
            FireEvent(oActor.UniqueID);
        }
        
        /// <summary>
        /// Loads and returns the EventOrder asset for the given event name
        /// </summary>
        /// <param name="strEvent"></param>
        /// <returns></returns>
        ARX_EventOrder GetEventOrderList(string strEvent)
        {
            ARX_EventOrder oOrder = Resources.Load<ARX_EventOrder>("eventorder/" + strEvent);
            if (oOrder == null)
            {
                Debug.LogError("Could not load event order list that should be located at Resources/event/" + strEvent);
                CreateEmptyEventOrderList();
            }

            return oOrder;
        }

        /// <summary>
        /// The given event handler will run whenever the given event occurs.
        /// If a process is given, the event handler will only be called when the given process
        /// is the top process of its parent.
        /// </summary>
        public void Subscribe(ARX_EventHandlerDelegate oEventAction, ARX_Process oProcess, bool bSetAsTop = false)
        {
            if (bSetAsTop)
            {
                Subscribe(oEventAction, oProcess.name, oProcess.EventRecord, oProcess);
            }
            else
            {
                Subscribe(oEventAction, oProcess.name, oProcess.EventRecord);
            }
        }

        /// <summary>
        /// Subscribes the given actor to this event.
        /// </summary>
        /// <param name="oEventAction"></param>
        /// <param name="actor"></param>
        public void Subscribe(ARX_EventHandlerDelegate oEventAction, ARX_Script_Actor actor)
        {
            Subscribe(oEventAction, actor.name, actor.EventRecord);
        }

            /// <summary>
            /// The given event handler will run whenever the given event occurs.
            /// If a process is given, the event handler will only be called when the given process
            /// is the top process of its parent.
            /// </summary>
            /// <param name="oEventAction"></param>
            /// <param name="oActor"></param>
            /// <param name="strEventID"></param>
            /// <param name="strProcessID"></param>
            public void Subscribe(ARX_EventHandlerDelegate oEventAction, string strProcessID, EventSubscriptionRecord oRecord, ARX_Process oProcess = null)
        {
            ARX_EventHandlerEntry oEntry = new ARX_EventHandlerEntry(oEventAction, strProcessID, oProcess);
            //Debug.Log("Adding item to Event subscriptions " + strProcessID);

            oRecord.AddSubscribedEvent(this, oEntry);

            if (mo_eventOrderList == null)
            {
                //Debug.LogError("Event " + name + " should be given an Event Order asset prior to having any subscribers.");
                CreateEmptyEventOrderList();
                Algorithm_BubbleSort_EventEntry_StringVer oSort = new Algorithm_BubbleSort_EventEntry_StringVer(mo_eventOrderList);
                oSort.SortInto(oEntry, EventHandlerSeries);
                return;
            }

            Algorithm_BubbleSort_EventEntry_StringVer oSorter = new Algorithm_BubbleSort_EventEntry_StringVer(mo_eventOrderList);
            oSorter.SortInto(oEntry, EventHandlerSeries);

            //string strList = "Sorting event into list for event " + name + ". List is now ";
            //foreach (ARX_EventHandlerEntry en in EventHandlerSeries)
            //    strList += en.mstr_processName + " ";
            //Debug.Log(strList);

        }
        

        /// <summary>
        /// Creates an empty EventOrder asset and saves it the mo_eventOrderList variable
        /// </summary>
        void CreateEmptyEventOrderList()
        {
            mo_eventOrderList = ScriptableObject.CreateInstance<ARX_EventOrder>();
        }
        
        /// <summary>
        /// Unsubscribes the goven Handler from this event.
        /// The empty string is for compatability reasons.
        /// </summary>
        /// <param name="oEventHandler"></param>
        /// <param name="strEmptyString"></param>
        /// <param name="oRecord"></param>
        public void Unsubscribe(ARX_EventHandlerDelegate oEventHandler, string strEmptyString, EventSubscriptionRecord oRecord = null)
        {
            Unsubscribe(oEventHandler);
        }

        /// <summary>
        /// Unsubscribes the given handler from this event.
        /// </summary>
        /// <param name="oEventHandler"></param>
        public void Unsubscribe(ARX_EventHandlerDelegate oEventHandler)
        {
            for (int i = 0; i < EventHandlerSeries.Count; i++)
            {
                if (EventHandlerSeries[i].mfunc_eventAction == oEventHandler)
                {
                    EventHandlerSeries.Remove(EventHandlerSeries[i]);
                    continue;
                }
            }
        }

        /// <summary>
        /// Destroys the given EventHanlder entry, then 
        /// unsubscribes it from this event.
        /// </summary>
        /// <param name="oEntry"></param>
        public void Unsubscribe(ARX_EventHandlerEntry oEntry)
        {
            oEntry.Destroy();
            EventHandlerSeries.Remove(oEntry);
        }

        #endregion


    }

}