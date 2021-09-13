using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace ARX
{
    /// <summary>
    /// Base object for the ZRPG system's stats.
    /// It calculates its values in an RPG-like manner.
    /// </summary>
    [Serializable]
    public class ARX_BaseStat
    {
        #region Variables

        /// <summary>
        /// The unique string ID this stat is searched by
        /// </summary>
        [FormerlySerializedAs("mn_id")]
        [SerializeField]
        protected string mn_ID = "";

        /// <summary>
        /// This stat's current value. It's value will not drop below the value of the
        /// mnf_minimum variable.
        /// </summary>
        [FormerlySerializedAs("mnf_current")]
        [SerializeField]
        protected float mnf_current = 0f;

        /// <summary>
        /// The lowest value the mnf_current variable can be.
        /// </summary>
        [FormerlySerializedAs("mnf_minimum")]
        [SerializeField]
        [HideInInspector]
        protected float mnf_minimum = 0;
        #endregion
        
        #region Accessors
        public string ID
        {
            set { mn_ID = value; }
            get { return mn_ID; }
        }

        public float Minimum { get { return mnf_minimum; } set { mnf_minimum = value; } }

        /// <summary>
        /// Returns the Current value of this stat.
        /// Validates the current value to ensure it does not drop below the 
        /// minimum value.
        /// </summary>
        public virtual float Current
        {
            set
            {
                //If the current value is below the minimum value
                //set the current value to the minimum value.
                mnf_current = value;
                if (mnf_current < Minimum)
                    mnf_current = Minimum;
            }
            get
            {
                //If the current value is below the minimum value
                //set the current value to the minimum value.
                if (mnf_current < Minimum)
                    mnf_current = Minimum;
                
                return mnf_current;
            }
        }

        /// <summary>
        /// Returns the Max value.
        /// For base stats, the Max and the Current value are the same
        /// </summary>
        public virtual float Max
        {
            get { return Current; }
        }

        /// <summary>
        /// Returns the Base value.
        /// For base stats, the Base and the Current Value are the same.
        /// </summary>
        public virtual float Base
        {
            set
            {
                Current = value;
            }
            get { return Current; }
        }

        /// <summary>
        /// Returns zero.
        /// For base stats, there is no Bonus value, but this Accessor was added for
        /// compatibility reasons.
        /// </summary>
        public virtual float Bonus
        {
            set
            {

            }
            get { return 0; }
        }
        #endregion

        #region Set Functions

        /// <summary>
        /// Set's this stat's variables
        /// For base stats, there is no Bonus value, but this argument was added for
        /// compatibility reasons.
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="nfBase"></param>
        /// <param name="nfBonus"></param>
        /// <param name="nfCurrent"></param>
        public virtual void SetNew(string strID, float nfBase, float nfBonus, float nfCurrent)
        {
            mn_ID = strID;
            mnf_current = nfCurrent;
        }

        /// <summary>
        /// Sets this stat's variables.
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="nfValue"></param>
        public virtual void SetNew(string strID, float nfValue)
        {
            mn_ID = strID;
            mnf_current = nfValue;
        }
        #endregion

    }
}
