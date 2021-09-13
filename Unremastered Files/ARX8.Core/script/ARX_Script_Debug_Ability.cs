using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARX;

namespace ARX
{
    /// <summary>
    /// Script to create an ARX_Ability in the Hierarchy and to
    /// alter its values in realtime.
    /// Used for debugging purposes.
    /// </summary>
    [ExecuteInEditMode]
    public class ARX_Script_Debug_Ability : MonoBehaviour
    {
        public ARX_StatBox_Quad mo_stats;
        public string mstr_name = "New Ability ";
        
        public string mstr_description = "Ability Description";
        
        private void Awake()
        {
            if (mo_stats == null)
                mo_stats = new ARX_StatBox_Quad();
        }

    }
}
