using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ARX
{
    public enum STATTYPE { FLOAT, STRING }

    /// <summary>
    /// A single stat that can contain either a Float with no upper-limit or an 
    /// Attribute-style String.
    /// </summary>
    [Serializable]
    public class ARX_StatSingle : ARX_BaseStat
    {
        #region Variables
        public STATTYPE me_statType = STATTYPE.FLOAT;

        public string mstr_attributeValue = "";

        [SerializeField]
        private float mnf_base;

        [SerializeField]
        private bool mb_readOnly = false;
        #endregion

        #region Accessors

        public string StringValue
        {
            get { return mstr_attributeValue; }

            set
            {
                mstr_attributeValue = value;
                string[] astr = mstr_attributeValue.Split();
                if (astr.Length < 2)
                    return;

                string strReturn = "";

                foreach (string s in astr)
                {
                    strReturn += s;
                }
                mstr_attributeValue = strReturn;
            }
        }

       

        public bool IsDeleteable
        {
            get
            {
                return mb_readOnly;
            }

            set { mb_readOnly = value; }
        }

        public void FillCurrent()
        {
            mnf_current = Max;
        }

        public override float Current
        {
            set
            {
                mnf_current = value;
                if (mnf_current < Minimum)
                    mnf_current = Minimum;
            }
            get { return mnf_current; }
        }


        public override float Max
        {
            get { return (Base + Bonus); }
        }

        public override float Base
        {
            set
            {
                //Any change to Base must be mirrored by Current
                float nLastBase = Base;
                mnf_base = value;
                Current += Base - nLastBase;
            }
            get { return mnf_base; }
        }
        #endregion

        #region Functions
        public void Reevaluate()
        {
            if (mnf_current < 0)
                mnf_current = 0;

            if (mnf_current > Max)
                mnf_current = Max;
        }


        public override void SetNew(string new_ID, float new_base, float new_bonus, float new_current)
        {
            mn_ID = new_ID;
            mnf_base = new_base;
            mnf_current = new_current;
            Reevaluate();
        }


        public override string ToString()
        {
            string str = ID;
            str += " " + mnf_current + "/" + Max;
            return str;
        }

        public void ClearValues() { Current = 0; Base = 0; Bonus = 0; mn_ID = ""; }
        public void AddToBase(float nValue) { Base = Base + nValue; Reevaluate(); }
        public void AddToBonus(float nValue) { Bonus = Bonus + nValue; Reevaluate(); }
        public void AddToCurrent(float nValue) { Current = Current + nValue; Reevaluate(); }
        #endregion
    }
}