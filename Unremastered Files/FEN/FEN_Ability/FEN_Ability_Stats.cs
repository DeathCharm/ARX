using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEN;
using ARX;
using UnityEngine;

namespace FEN
{
    public partial class FEN_Ability : ARX_Actor
    {
        /// <summary>
        /// Returns true if a given Stat Cost is fully paid for
        /// </summary>
        /// <param name="strStatID"></param>
        /// <returns></returns>
        public bool IsStatFullyPaid(string strStatID)
        {
            ARX_StatQuad stat = Costs.GetStat(strStatID);
            if (stat.Current >= stat.Max)
                return true;
            return false;
        }

        public int GetTotalStatCost
        {
            get
            {
                int n = 0;
                foreach (ARX_StatQuad quad in Costs.AsList)
                    n += quad.MaxInt;
                return n;
            }
        }

        /// <summary>
        /// Accessor for this ability's lift costs.
        /// </summary>
        public ARX_StatBox_Quad Costs
        {
            get
            {
                if (mo_costs == null)
                    mo_costs = ScriptableObject.CreateInstance<ARX_StatBox_Quad>();
                return mo_costs;
            }
            set
            {
                mo_costs = value;
            }
        }

        /// <summary>
        /// Accessor for this ability's stats.
        /// </summary>
        public ARX_StatBox_Quad Stats
        {
            get
            {
                if (mo_stats == null)
                {
                    mo_stats = ScriptableObject.CreateInstance<ARX_StatBox_Quad>();
                }
                return mo_stats;
            }
            set
            {
                mo_stats = value;
            }
        }

        /// <summary>
        /// In the event of a cloning, saves the UniqueID of its parent.
        /// </summary>
        /// <param name="oOriginal"></param>
        public void SetOriginalCard(FEN_Ability oOriginal)
        {
            //Debug.Log("Setting original card of " + nameAndID + " to " + oOriginal.nameAndID);
            OriginalAbility = oOriginal;

        }

        /// <summary>
        /// The original card this card hails from.
        /// </summary>
        [HideInInspector]
        public FEN_Ability OriginalAbility = null;

        /// <summary>
        /// A very, very debugging ability. Just pays all the card's costs, just like it says on the tin.
        /// </summary>
        public void DebugPayAllCosts()
        {
            foreach (ARX_StatQuad cost in Costs.AsList)
                cost.Current = cost.Max;
        }

        /// <summary>
        /// Returns the next unpaid stat cost.
        /// If no stat costs are unpaid or exists, returns null.
        /// </summary>
        /// <returns></returns>
        public ARX_StatQuad GetNextUnpaidCost()
        {
            foreach (ARX_StatQuad quad in Costs.AsList)
            {
                ARX_StatQuad playerStat = Owner.Stats.GetStat(quad.ID);
                if (quad.Current >= quad.Max)
                    continue;

                if (playerStat.Current > 0)
                {
                    return quad;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns true if any cost has been paid for any ability.
        /// </summary>
        public bool HasPaidForAnyAbility
        {
            get
            {
                foreach (ARX_StatQuad cost in Costs.AsList)
                    if (cost.Current != 0)
                        return true;

                return false;
            }
        }

        /// <summary>
        /// Resets all held costs' Current value to zero.
        /// </summary>
        public void ResetCosts()
        {
            foreach (ARX_StatQuad quad in Costs.AsList)
                quad.Current = 0;
        }

        /// <summary>
        /// Returns true if no unpaid costs remain.
        /// </summary>
        public bool IsFullyPaid { get {
                foreach (ARX_StatQuad quad in Costs.AsList)
                    if (quad.Current < quad.Max)
                        return false;
                return true;
            } }

        /// <summary>
        /// Adds a cost to this ability if it is not already present. Else, sets the cost to the given
        /// value.
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="nBase"></param>
        public void AddCost(string strID, int nBase)
        {
            if (Costs.Count == 3)
            {
                string strCosts = "";
                foreach (ARX_StatQuad d in Costs.AsList)
                {
                    strCosts += d.ID + " " + d.Max + "\t";
                }
                Debug.LogError("Ability " + name + " can only have a maximum of 3 costs. Current costs: " + strCosts);
                return;
            }

            if (Costs.GetStat(strID) == null)
            {
                Costs.CreateNewStat(strID, 0, nBase, 0);
            }
            else
            {
                Costs.GetStat(strID).Bonus = nBase;
            }

        }
        
        
        /// <summary>
        /// Sends message regarding a change in stat values.
        /// </summary>
        /// <param name="ePlacement"></param>
        /// <param name="strStatID"></param>
        /// <param name="nfValue"></param>
        void SendAddStatMessage(FEN.STATPLACEMENT ePlacement ,string strStatID, float nfValue)
        {
            ARX_StatQuad stat = Stats.GetStat(strStatID);

            float nfPrevious = 0;
            float nfPost = 0;

            switch (ePlacement)
            {
                case STATPLACEMENT.CURRENT:
                    nfPrevious = stat.Current;
                    stat.Current += nfValue;
                    nfPost = stat.Current;
                    break;
                case STATPLACEMENT.BONUS:
                    nfPrevious = stat.Bonus;
                    stat.Bonus += nfValue;
                    nfPost = stat.Bonus;
                    break;
                case STATPLACEMENT.BASE:
                    nfPrevious = stat.Base;
                    stat.Base += nfValue;
                    nfPost = stat.Base;
                    break;
            }

            DataString dat = new DataString(this);
            dat.SetString(GameIDs.ValueStatName, strStatID);
            dat.SetInt(GameIDs.ValueUniqueID, Owner.UniqueID);
            dat.SetInt(GameIDs.ValueAbilityUniqueID, UniqueID);
            dat.SetDouble(GameIDs.ValueAmount, nfValue);
            dat.SetDouble(GameIDs.ValuePrevious, nfPrevious);
            dat.SetDouble(GameIDs.ValuePost, nfPost);
            dat.SetDouble(GameIDs.ValueDelta, nfPost - nfPrevious);
            dat.SetString(GameIDs.ValueIntType, ePlacement.ToString().ToLower());

            ARX.GameEvents.onStatAmountChange.FireEvent(dat);
        }


        /// <summary>
        /// Adds an amount to a stat in an event based manner.
        /// Adds to the stat's current value.
        /// </summary>
        /// <param name="strStatID"></param>
        /// <param name="nfValue"></param>
        public void AddToCurrent(string strStatID, float nfValue)
        {
            SendAddStatMessage(STATPLACEMENT.CURRENT, strStatID, nfValue);
        }

        /// <summary>
        /// Adds an amount to a stat in an event based manner.
        /// Adds to the stat's base value.
        /// </summary>
        /// <param name="strStatID"></param>
        /// <param name="nfValue"></param>
        public void AddToBase(string strStatID, float nfValue)
        {
            SendAddStatMessage(STATPLACEMENT.BASE, strStatID, nfValue);
        }

        /// <summary>
        /// Adds an amount to a stat in an event based manner.
        /// Adds to the stat's bonus value.
        /// </summary>
        /// <param name="strStatID"></param>
        /// <param name="nfValue"></param>
        public void AddToBonus(string strStatID, float nfValue)
        {
            SendAddStatMessage(STATPLACEMENT.BONUS, strStatID, nfValue);
        }
    }
}
