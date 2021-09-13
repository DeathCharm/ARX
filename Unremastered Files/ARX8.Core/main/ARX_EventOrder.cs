using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARX
{
    /// <summary>
    /// Asset used by ARX_Events to determine the order in which event subscribers are called
    /// when ARX_Events are fired.
    /// </summary>
    [CreateAssetMenu(menuName = "ARX/Event Order")]
    public class ARX_EventOrder : ScriptableObject
    {

        public string mstr_event = "New Event";
        public List<string> moa_processNames = new List<string>();



    }
}
