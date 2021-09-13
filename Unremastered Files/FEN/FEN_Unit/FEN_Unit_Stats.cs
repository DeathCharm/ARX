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
    public partial class FEN_Unit : ARX_Actor
    {
        #region Stat Accessors

        /// <summary>
        /// This unit's stats.
        /// </summary>
        [SerializeField]
        ARX_StatBox_Quad mo_stats;

        void SendAddStatMessage(string strIntType, string strStatID, float nfValue, object sender)
        {
            ARX_StatQuad stat = Stats.GetStat(strStatID);

            float nfPrevious = 0;
            float nfPost = 0;

            switch (strIntType)
            {
                case GameIDs.ValueCurrent:
                    nfPrevious = stat.Current;
                    stat.Current += nfValue;
                    nfPost = stat.Current;
                    break;
                case GameIDs.ValueBonus:
                    nfPrevious = stat.Bonus;
                    stat.Bonus += nfValue;
                    nfPost = stat.Bonus;
                    break;
                case GameIDs.ValueBase:
                    nfPrevious = stat.Base;
                    stat.Base += nfValue;
                    nfPost = stat.Base;
                    break;
            }

            DataString dat = new DataString(this);
            dat.SetSender(sender);
            dat.SetString(GameIDs.ValueStatName, strStatID);
            dat.SetInt(GameIDs.ValueUniqueID, UniqueID);
            dat.SetDouble(GameIDs.ValueAmount, nfValue);
            dat.SetDouble(GameIDs.ValuePrevious, nfPrevious);
            dat.SetDouble(GameIDs.ValuePost, nfPost);
            dat.SetDouble(GameIDs.ValueDelta, nfPost - nfPrevious);
            dat.SetString(GameIDs.ValueIntType, strIntType);

            ARX.GameEvents.onStatAmountChange.FireEvent(dat);
        }

        public void AddToCurrent(string strStatID, float nfValue, object sender)
        {
            SendAddStatMessage(GameIDs.ValueCurrent, strStatID, nfValue, sender);
        }

        public void AddToBase(string strStatID, float nfValue, object sender)
        {
            SendAddStatMessage(GameIDs.ValueBase, strStatID, nfValue, sender);
        }

        public void AddToBonus(string strStatID, float nfValue, object sender)
        {
            SendAddStatMessage(GameIDs.ValueBonus, strStatID, nfValue, sender);
        }
        
        /// <summary>
        /// Accessor for this unit's stats.
        /// </summary>
        public ARX_StatBox_Quad Stats
        {
            get
            {
                if (mo_stats == null)
                    mo_stats = ScriptableObject.CreateInstance<ARX_StatBox_Quad>();
                return mo_stats;
            }
            set
            {
                if (value != null)
                    mo_stats = value;
                else
                {
                    Debug.LogError("Player stats cannot be set to null.");
                }
            }
        }

        /// <summary>
        /// Accessor for Health Stat
        /// </summary>
        public ARX_StatQuad Stat_Health { get { return Stats.GetStat(IDs.HP); } }


        /// <summary>
        /// Accessor for Money Stat.
        /// </summary>
        public ARX_StatQuad Stat_Money { get { return Stats.GetStat(IDs.Money); } }

        /// <summary>
        /// Accessor for Block Stat
        /// </summary>
        public ARX_StatQuad Stat_Block { get { return Stats.GetStat(IDs.Block); } }

        /// <summary>
        /// Accessor for Accuracy Stat
        /// </summary>
        public ARX_StatQuad Stat_Accuracy { get { return Stats.GetStat(IDs.Accuracy); } }

        /// <summary>
        /// Accessor for Threat Stat
        /// </summary>
        public ARX_StatQuad Stat_Threat { get { return Stats.GetStat(IDs.Threat); } }

        /// <summary>
        /// Accessor for Stamina Stat
        /// </summary>
        public ARX_StatQuad Stat_Stamina { get { return Stats.GetStat(IDs.Stamina); } }

        /// <summary>
        /// Accessor for Strength Stat
        /// </summary>
        public ARX_StatQuad Stat_Strength { get { return Stats.GetStat(IDs.Strength); } }
        
        /// <summary>
        /// Accessor for the extra damage stat
        /// </summary>
        public ARX_StatQuad Stat_Damage { get { return Stats.GetStat(IDs.Damage); } }

        #endregion
    }
}