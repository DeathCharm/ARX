using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ARX
{
    /// <summary>
    /// A stat holding a current value with no upper-limit maximum value.
    /// </summary>
    [Serializable]
    public class StatDuo : ARX_BaseStat
    {
        public void DeleteStat() { mnf_current = 0f; mn_ID = ""; }

        public void AddToValue(float n)
        {
            Current += n;
        }


        public override void SetNew(string new_ID, float new_base, float new_bonus, float new_current)
        {
            mn_ID = new_ID;
            mnf_current = new_base + new_current + new_bonus;
        }

        public override void SetNew(string newID, float newValue)
        {
            mn_ID = newID;
            mnf_current = newValue;
        }
    }


}