using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;
using UnityEngine;

namespace FEN
{
    public abstract class FEN_AbilityProcess : ARX_Process
    {

        protected FEN_Ability mo_targetAbility;
        public FEN_Ability OwningAbility { get { return mo_targetAbility; } }

        public FEN_Unit OwningUnit
        {
            get
            {
                if (mo_targetAbility == null)
                    return null;
                return mo_targetAbility.Owner;
            }

            set
            {
                if (mo_targetAbility != null)
                    mo_targetAbility.SetOwner(value);
            }
        }

        public string nameAndID { get { return OwningAbility.nameAndID; } }

        public CardIDs.CARDID CardID { get { return OwningAbility.me_abilityID; } }

        public virtual string[] GetCostVariableNames() { return new string[] { }; }

        public virtual FEN_AbilityTargetingInfo GetTargetingInfo()
        {
            return new FEN_AbilityTargetingInfo();
        }
        
        public virtual void ExecuteAnimation()
        {

        }

        public virtual void FailToUseAsSkill()
        {

        }

        public int GetUnitStatOfThisAbility()
        {
            return OwningUnit.Stats.GetStat(CardID.ToString().ToLower()).MaxInt;
        }

        public int GetAbilityStatOfThisAbility()
        {
            return OwningAbility.Stats.GetStat(CardID.ToString().ToLower()).MaxInt;
        }

      
        public virtual void OnAddedAsStatus()
        {
            Debug.Log("Adding " + name + " as a status effect to " + OwningUnit.name);
        }
        
        

        public virtual void OnTargetingProcessHoverPlayer() { }
        public virtual void OnTargetingProcessUnhoverPlayer() { }
        public virtual void OnTerminateTargeting() { 
            //ARX_Cursor.CursorBlank(); 
        }
        
        public virtual void OnLifted()
        {

        }

        public virtual string GetIconTooltip()
        {
            return "Icon Tooltip";
        }

        public virtual string GetDescription()
        {
            return OwningAbility.mstr_cardTooltip;
        }

        public void ResetCosts()
        {
            if (mo_targetAbility != null)
                mo_targetAbility.ResetCosts();
        }
        

        /// <summary>
        /// Returns true if the message's ValueUniqueID is this process's UniqueID
        /// or the UniqueID of the mo_targetAbility
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        protected override bool IsTargetingThisActor(DataString dat)
        {
            if (mo_targetAbility != null)
            {
                bool bUsedAbilityIDTag = dat.GetInt(GameIDs.ValueAbilityUniqueID) == UniqueID
                 || dat.GetInt(GameIDs.ValueAbilityUniqueID) == mo_targetAbility.UniqueID;

                if (bUsedAbilityIDTag)
                    return true;

                return dat.GetInt(GameIDs.ValueUniqueID) == UniqueID
                 || dat.GetInt(GameIDs.ValueUniqueID) == mo_targetAbility.UniqueID;
            }
            else
            {
                return dat.GetInt(GameIDs.ValueUniqueID) == UniqueID;
            }

        }
        
        public virtual void CreateTargetingProcess()
        {
            //Debug.Log("Creating targeting process for " + mo_targetAbility.nameAndID);
            //Main.PushToTopQueuedProcess(new ASMO_TargetingProcess(mo_targetAbility));

        }

        public virtual string GetChooseYourTargetsString(List<FEN_Unit> oaUnitTargets, List<FEN_Ability> oaCardTargets)
        {
            string str = "";

            if (oaUnitTargets != null && oaCardTargets != null)
            {
                if (oaUnitTargets.Count == 0 && oaCardTargets.Count == 0)
                {
                    str = "Choose your target(s).";
                }
                else
                {
                    str = "Use " + OwningAbility.CardName + " on ";

                    foreach (FEN_Unit unit in oaUnitTargets)
                        str += unit.nameAndID + " ";
                    foreach (FEN_Ability ab in oaCardTargets)
                        str += ab.nameAndID + " ";

                    str += "?";
                }
                return str;
            }

            str = "Use " + OwningAbility.CardName + "?";

            return str;
        }
        
        public virtual bool ExecuteAbility(List<FEN_Ability> moa_cardTargets, List<FEN_Unit> moa_unitTargets)
        {
            return true;
        }

        public void SendExecutionMessage()
        {
            //GameEvents.OnExecutedAbility.FireEvent(mo_targetAbility.GetMessage);
        }
        
        public virtual bool CanTargetCard(FEN_Ability oCard)
        {
            if (oCard != null)
                return true;
            return false;
        }

        public virtual bool CanTargetUnit(FEN_Unit oUnit)
        {
            if (oUnit != null)
                return true;
            return false;
        }


        public FEN_AbilityProcess(FEN_Ability oAbility, FEN_Unit owner, CardIDs.CARDID eCardID) : base(eCardID.ToString().ToLower())
        {
            mo_targetAbility = oAbility;

            Main.AddConstantProcess(this);

            
            //ARX.GameEvents.onGameEnd.Subscribe(ReactToGameEnd, name, EventRecord);
            
        }

        public override void OnTerminated()
        {
            EventRecord.UnsubscribeFromAllEvents();

            base.OnTerminated();
        }
        
        
        public bool CanBeUsedAsSkill()
        {
            return GetTargetingInfo().mb_canBeUsedAsSkill;
        }
        
        public bool IsCastOnFirstClick(List<FEN_Unit> oaUnitTargets = null, List<FEN_Ability> oaCardTargets = null)
        {
            return GetTargetingInfo().mb_castInstantlyOnClick;
        }

        public bool IsCastOnFirstTargetChosen()
        {
            return GetTargetingInfo().mb_activateImmediatelyWhenValid;
        }

        public virtual bool CheckAccuracy(FEN_Unit target)
        {
            int nRandom = UnityEngine.Random.Range(0, 100);
            int nCurrentAccuracy = OwningUnit.Stat_Accuracy.CurrentInt;
            if (nRandom <= nCurrentAccuracy)
            {
                return true;
            }
            
            return false;
        }

        public virtual bool CheckDodge(FEN_Unit target)
        {
            int nRandom = UnityEngine.Random.Range(0, 100);
            int nDodge = target.Stats.GetStat(IDs.Dodge).MaxInt;
            if (nRandom <= nDodge)
            {
                return false;
            }

            return true;
        }

        public virtual bool CheckCrit(FEN_Unit target)
        {
            int nRandom = UnityEngine.Random.Range(0, 100);
            int nCrit = target.Stats.GetStat(IDs.CriticalChance).MaxInt;
            if (nRandom <= nCrit)
            {
                return true;
            }

            return false;
        }
        
        
        void ReactToGameEnd(DataString dat) { Terminate(); }
        
        
        
    }
}
