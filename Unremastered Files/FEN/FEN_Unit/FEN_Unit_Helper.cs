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

        public Animator mo_animator;
        public Animator UnitAnimator
        {
            get
            {

                return mo_animator;
            }
        }

        /// <summary>
        /// Function callback for being struck by an attack
        /// </summary>
        /// <param name="dat"></param>
        public void ReactToBeingAttacked(DataString dat)
        {
            if (IsDead)
                return;

            int nTargetID = dat.GetInt(IDs.Target);

            if (nTargetID != UniqueID)
            {
                return;
            }
            BLURBTYPE eType;

            int nDamage = dat.GetInt(GameIDs.ValueAmount);

            //If the damage was healing
            if (nDamage < 0)
            {
                Debug.Log(nameAndID + " is healing for " + nDamage);
                eType = BLURBTYPE.HEAL;
                ProcessHealing(dat, nDamage);
            }
            else
            {

                //If the health damage is negative, the attack was fully blocked
                if (nDamage - (int)Stat_Block.Current <= 0)
                {
                    eType = BLURBTYPE.BLOCKED;
                    ProcessBlockedDamage(dat, nDamage);
                }
                //Else the attack will hit and deal damage
                else
                {
                    eType = BLURBTYPE.UNBLOCKED;
                    ProcessUnblockedDamage(dat, nDamage);
                }

            }

            ShowEffect(eType, nDamage, this);
            ShowBlurb(eType, nDamage, this);
            //PlayView.SpawnDamageBlurb(nDamage, eType, this);

        }

        void ProcessHealing(DataString dat, int nAmount)
        {

        }

        void ProcessBlockedDamage(DataString dat, int nDamage)
        {
            Debug.Log(nameAndID + " is taking fully blocking " + nDamage);

            AddToCurrent(IDs.Block, -nDamage, dat.Sender);

            DataString datBlocked = new DataString(this);
            datBlocked.SetInt(IDs.Amount, nDamage);
            datBlocked.SetInt(IDs.UniqueID, UniqueID);
            ARX.GameEvents.OnFullyBlockedDamageTaken.FireEvent(datBlocked);
        }

        void ProcessUnblockedDamage(DataString dat, int nDamage)
        {
            int nBlock = Stat_Block.CurrentInt;
            bool bHadBlock = nBlock > 0;

            nDamage -= nBlock;

            AddToCurrent(IDs.Block, -(int)Stat_Block.Current, dat.Sender);

            DataString datBlocked = new DataString(this);
            datBlocked.SetInt(IDs.Amount, nBlock);
            datBlocked.SetInt(IDs.UniqueID, UniqueID);
            //Partial Block message
            if (nDamage > 0)
            {
                ARX.GameEvents.onPartiallyBlockedDamageTaken.FireEvent(datBlocked);
            }

            //Block Broken
            if (bHadBlock)
            {
                ARX.GameEvents.onBlockBroken.FireEvent(datBlocked);
            }

            AddToCurrent(IDs.HP, -nDamage, dat.Sender);
            Debug.Log(nameAndID + " is taking damage " + nDamage);

            if (UnitAnimator != null)
                UnitAnimator.SetTrigger("hurt");

            //DataString healthLostDat = GetMessage;
            DataString healthLostDat = dat.Clone;
            healthLostDat.SetInt(GameIDs.ValueUniqueID, UniqueID);
            healthLostDat.SetInt(GameIDs.ValueAmount, nDamage);
            healthLostDat.SetInt(GameIDs.ValueAbilityUniqueID, dat.GetInt(GameIDs.ValueAbilityUniqueID));
            ARX.GameEvents.onHealthDamageTaken.FireEvent(healthLostDat);

            //Set the amount of health damage dealt by the attack back to the original message
            dat.SetInt(IDs.Damage, nDamage);

            if (IsDead)
            {
                if (UnitAnimator != null)
                    UnitAnimator.SetTrigger("death");

                ARX.GameEvents.onUnitDeath.FireEvent(healthLostDat);
                if (FlowChart != null)
                {
                    //Set current event to Unit Death

                    //Run Flowchart

                    Fungus.Flowchart oChart = FlowChart.GetComponent<Fungus.Flowchart>();
                    if (oChart != null)
                        oChart.ExecuteBlock("Start");
                }
            }
        }

        public virtual void ShowBlurb(BLURBTYPE eType, int nAmount, FEN_Unit target)
        {
            Debug.LogError("Sho Blurb is currently no implemented");
            //string strText = nAmount.ToString();

            //switch (eType)
            //{
            //    case BLURBTYPE.BLOCKED:
            //        ASMO_Script_Blurb.SpawnEffectBlurbOnUnit(Blurbs.Damage, target, Color.blue, "Blocked");
            //        break;
            //    case BLURBTYPE.HEAL:
            //        ASMO_Script_Blurb.SpawnEffectBlurbOnUnit(Blurbs.Damage, target, Color.green, strText);
            //        break;
            //    case BLURBTYPE.UNBLOCKED:
            //        ASMO_Script_Blurb.SpawnEffectBlurbOnUnit(Blurbs.Damage, target, Color.red, strText);
            //        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            //        {
            //            ASMO.Helper.SpawnJokeAttackBlurb();
            //        }

            //        break;
            //}
        }

        public virtual void ShowEffect(BLURBTYPE eType, int nAmount, FEN_Unit target)
        {

            Debug.LogError("Sho Blurb is currently no implemented");
            //Color violet = new Color(100, 0, 255);
            //string strText = nAmount.ToString();

            //switch (eType)
            //{
            //    case BLURBTYPE.BLOCKED:
            //        ASMO_Script_Blurb.SpawnEffectBlurbAtMousePosition(Blurbs.BigBulletImpact, Color.cyan);
            //        break;
            //    case BLURBTYPE.HEAL:
            //        ASMO_Script_Blurb.SpawnEffectBlurbOnUnit(Blurbs.Bright, target, Color.green);
            //        break;
            //    case BLURBTYPE.UNBLOCKED:
            //        ASMO_Script_Blurb.SpawnEffectBlurbAtMousePosition(Blurbs.BigBulletImpact, Color.red);
            //        break;
            //}
        }

        /// <summary>
        /// Function callback for receiving healing
        /// </summary>
        /// <param name="dat"></param>
        public void ReactToBeingHealed(DataString dat)
        {
            int nTargetID = dat.GetInt(IDs.Target);
            if (nTargetID != UniqueID)
                return;

            Debug.Log(nameAndID + " Unit reacting to sent heal message ");

            int nHealAmount = dat.GetInt(GameIDs.ValueAmount);
            if (nHealAmount == 0)
                return;

            AddToCurrent(IDs.HP, nHealAmount, dat.Sender);

            //PlayView.SpawnDamageBlurb(nHealAmount, eType, this);
        }

        /// <summary>
        /// Function callback for a change in stat values
        /// </summary>
        /// <param name="dat"></param>
        public void ReactToStatChange(DataString dat)
        {
            int nTargetID = dat.GetInt(IDs.Target);
            if (nTargetID != UniqueID)
                return;

            ARX_StatQuad stat = Stats.GetStat(dat.GetString(GameIDs.ValueStatName));

            if (stat.ID == IDs.HP)
            {
                if (IsDead)
                {
                    if (UnitAnimator != null)
                        UnitAnimator.SetTrigger("death");
                }
                return;
            }

        }

        /// <summary>
        /// Function callback for Player Turn start
        /// </summary>
        /// <param name="dat"></param>
        public void ReactToPlayerTurnStart(DataString dat)
        {

        }

        /// <summary>
        /// Function callback for Player turn end
        /// </summary>
        /// <param name="dat"></param>
        public void ReactToPlayerTurnEnd(DataString dat)
        {

        }

        /// <summary>
        /// Function callback for Unloading a battle scene
        /// </summary>
        /// <param name="dat"></param>
        void ReactToBattleSceneUnload(DataString dat)
        {
            Destroy();
        }

        /// <summary>
        /// To string override.
        /// Returns this unit's name and ID and its stats.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = nameAndID;
            foreach (ARX_StatQuad quad in mo_stats.AsList)
            {
                str += "\t" + quad.ToString();
            }

            return str;
        }

        /// <summary>
        /// Function callback for attack animations.
        /// </summary>
        /// <param name="dat"></param>
        void ReactToTriggerAttackAnimation(DataString dat)
        {
            if (IsTargetingThisUnit(dat))
                if (UnitAnimator != null)
                    UnitAnimator.SetTrigger("attack");
        }

        /// <summary>
        /// Returns true if the given datastring is targeting 
        /// this unit's uniqueID. Pulls the uniqueID from
        /// the datastring's ValueUniqueID or ValueTargetUniqueID fields.
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public bool IsTargetingThisUnit(DataString dat)
        {
            if (dat.GetInt(GameIDs.ValueUniqueID) == UniqueID ||
                dat.GetInt(IDs.Target) == UniqueID)
                return true;
            return false;
        }

    }
}