using System;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace ARX
{
    /// <summary>
    /// A stat with the standard numeric mechanics used in RPG's.
    /// Has a minimum value, a current value, a base value that is returned to before all recalculations, and a bonus value
    /// that is the sum of all forces that affect this stat.
    /// </summary>
    [Serializable]
    public class ARX_StatQuad : ARX_BaseStat
    {
        #region Variables
        [FormerlySerializedAs("mnf_bonus")]
        [SerializeField]
        private float mnf_bonus;

        [FormerlySerializedAs("mnf_base")]
        [SerializeField]
        private float mnf_base;

        [FormerlySerializedAs("mnfa_mult")]
        [SerializeField]
        private float[] mnfa_mult;

        [FormerlySerializedAs("mnfa_div")]
        [SerializeField]
        private float[] mnfa_div;

        [FormerlySerializedAs("mnf_multMinimum")]
        [SerializeField]
        private float mnf_multMinimum = 0;

        [FormerlySerializedAs("mb_readOnly")]
        [SerializeField]
        private bool mb_readOnly = false;

        [FormerlySerializedAs("mb_overflowCurrent")]
        [SerializeField]
        public bool mb_allowOverflowCurrent = false;
        
        public const char delimiter = '\t';
        #endregion

        #region Accessors
        public ARX_StatQuad Clone
        {
            get
            {
                ARX_StatQuad other = (ARX_StatQuad)this.MemberwiseClone();
                other.mnf_bonus = mnf_bonus;
                other.mnf_base = mnf_base;
                other.mb_readOnly = mb_readOnly;
                other.mnfa_mult = GetCloneMult();
                other.mnfa_div = GetCloneDiv();
                return other;
            }
        }

        public float MinimumMult
        {
            set { mnf_multMinimum = value; }
            get { return mnf_multMinimum; }
        }

        public float[] GetDivArray
        {
            get
            {
                if (mnfa_div == null)
                {
                    ResetDiv();
                }
                return mnfa_div;
            }
        }

        public float[] GetMultArray
        {
            get
            {
                if (mnfa_mult == null)
                {
                    ResetMult();
                }
                return mnfa_mult;
            }
        }



        public int CurrentInt { get { return (int)Current; } }
        public override float Current
        {
            set
            {
                mnf_current = value;
                if (mnf_current < Minimum)
                    mnf_current = Minimum;

                if (mnf_current > Max && mb_allowOverflowCurrent == false)
                    mnf_current = Max;
            }
            get { return mnf_current; }
        }

        public int BonusInt { get { return (int)(Bonus); } }
        public override float Bonus
        {
            set
            {
                float nLastBonus = Bonus;
                mnf_bonus = value;
            }
            get { return mnf_bonus * Mult * DivMult; }
        }

        public int MaxInt { get { return (int)Max; } }
        public override float Max
        {
            get { return (Base + Bonus); }
        }

        public int BaseInt { get { return (int)(Base); } }
        public override float Base
        {
            set
            {
                //Any change to Base must be mirrored by Current
                float nLastBase = Base;
                mnf_base = value;
            }
            get { return mnf_base * Mult * DivMult; }
        }

        public bool IsDeleteable
        {
            get
            {
                return mb_readOnly;
            }

            set { mb_readOnly = value; }
        }


        /// <summary>
        /// Returns MaxInt - CurrentInt
        /// </summary>
        public int DifferenceInt { get { return (int)Max - (int)Current; } }

        /// <summary>
        /// Returns Max - Current
        /// </summary>
        public float Difference { get { return Max - Current; } }

        public float Mult
        {
            get
            {
                float nfTotalMult = 0;
                foreach (float f in GetMultArray)
                    nfTotalMult += f;

                if (nfTotalMult < mnf_multMinimum)
                    nfTotalMult = mnf_multMinimum;

                return nfTotalMult;
            }
        }

        /// <summary>
        /// Returns the value of 1 divided by all of the held divisors
        /// </summary>
        public float DivMult
        {
            get
            {
                float nfTotalMult = 1;
                foreach (float f in GetDivArray)
                    if(f != 0)
                    nfTotalMult /= f;
                
                return nfTotalMult;
            }
        }
        #endregion

        #region Helper

        public void AddMultiplier(float mult)
        {
            List<float> oaBuf = new List<float>(GetMultArray);
            oaBuf.Add(mult);
            mnfa_mult = oaBuf.ToArray();
        }

        public void AddDivisor(float div)
        {
            if(div == 0)
            {
                Debug.LogError("StatQuad " + ID + " can't had zero added as a divisor. Duh.");
                return;
            }

            List<float> oaBuf = new List<float>(GetDivArray);
            oaBuf.Add(div);
            mnfa_div = oaBuf.ToArray();
        }

        public void ResetMult()
        {
            mnfa_mult = new float[1] { 1 };
        }

        public void ResetDiv()
        {
            mnfa_mult = new float[0];
        }

        public float[] GetCloneMult()
        {
            List<float> oaBuf = new List<float>(GetMultArray);
            return oaBuf.ToArray();
        }

        public float[] GetCloneDiv()
        {
            List<float> oaBuf = new List<float>(GetDivArray);
            return oaBuf.ToArray();
        }

        public override string ToString()
        {
            string str = ID.ToUpper();
            str += " (" + mnf_current + "/" + Max + ")";
            return str;
        }

        public void FillCurrent()
        {
            mnf_current = Max;
        }

        public override void SetNew(string new_ID, float new_base, float new_bonus, float new_current)
        {
            mn_ID = new_ID;
            mnf_base = new_base;
            mnf_bonus = new_bonus;
            mnf_current = new_current;
            ResetDiv();
            ResetMult();
            Reevaluate();
        }

        public void SetNew(ARX_StatQuad other)
        {
            mnf_base = other.Base;
            mnf_bonus = other.Bonus;
            mnf_current = other.Current;
            mnfa_mult = other.GetCloneMult();
            mnfa_div = other.GetCloneDiv();
            Reevaluate();
        }
        

        public void Reevaluate()
        {
            if (mnf_current < 0)
                mnf_current = 0;

            if (mnf_current > Max)
                mnf_current = Max;
        }

        public void ClearValues() { Current = 0; Base = 0; Bonus = 0; mn_ID = ""; ResetMult(); ResetDiv(); }
        public void AddToBase(float nValue) { Base = Base + nValue; Reevaluate(); }
        public void AddToBonus(float nValue) { Bonus = Bonus + nValue; Reevaluate(); }
        public void AddToCurrent(float nValue) { Current = Current + nValue; Reevaluate(); }
        
        public string AsStatString
        {
            get
            {
                string str = ID + delimiter + Base + delimiter + Bonus + delimiter + Current + delimiter;
                return str;
            }
        }

        #endregion
    }

}