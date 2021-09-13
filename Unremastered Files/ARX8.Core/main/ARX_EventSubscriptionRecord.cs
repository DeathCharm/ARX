using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARX
{
    /// <summary>
    /// Object used by ARX_Events to hold a list of all subscribers to that ARX_Event
    /// </summary>
    public class EventSubscriptionRecord
    {
        #region Nested Class: Subscribed Event
        /// <summary>
        /// An object referencing an event this actor has subsribed to.
        /// </summary>
        public class SubscribedEvent
        {
            public ARX_Event mo_event;
            public ARX_EventHandlerEntry mo_handler;

            public void Unsubscribe()
            {
                mo_event.Unsubscribe(mo_handler);
            }

            public SubscribedEvent(ARX_Event oEvent, ARX_EventHandlerEntry oHandler)
            {
                mo_event = oEvent;
                mo_handler = oHandler;
            }
        }
        #endregion

        /// <summary>
        /// A list of the events this actor has subscribed to.
        /// </summary>
        List<SubscribedEvent> moa_subscribedEvents = new List<SubscribedEvent>();

        public override string ToString()
        {
            string strRet =  moa_subscribedEvents.Count + " Events Subscribed to\n";
            foreach (SubscribedEvent ev in moa_subscribedEvents)
            {
                strRet += ev.mo_event.name + "\n";
            }
            return strRet;
        }

        public void AddSubscribedEvent(ARX_Event oEvent, ARX_EventHandlerEntry oHandler)
        {
            moa_subscribedEvents.Add(new SubscribedEvent(oEvent, oHandler));
        }

        public void RemoveSubscribedEvent(ARX_Event oEvent, ARX_EventHandlerEntry oHandler)
        {
            for (int i = 0; i < moa_subscribedEvents.Count; i++)
            {
                SubscribedEvent bufEvent = moa_subscribedEvents[i];

                if (bufEvent.mo_handler == oHandler && bufEvent.mo_event == oEvent)
                {
                    moa_subscribedEvents.Remove(bufEvent);
                    return;
                }
            }
        }

        public void UnsubscribeFromAllEvents()
        {
            
            for (int i = 0; i < moa_subscribedEvents.Count; i++)
            {
                    moa_subscribedEvents[i].Unsubscribe();
            }
            moa_subscribedEvents.Clear();
        }
    }
}