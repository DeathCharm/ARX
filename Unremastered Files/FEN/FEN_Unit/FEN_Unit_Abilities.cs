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
    public partial class FEN_Unit:ARX_Actor
    {

        /// <summary>
        /// Adds an ability to this unit.
        /// </summary>
        /// <param name="ability"></param>
        public void AddAbility(FEN_Ability ability)
        {
            ability.SetOwner(this);
            AbilityList.AddAbility(ability);
        }

        /// <summary>
        /// Adds an ability to this unit.
        /// </summary>
        /// <param name="ability"></param>
        public FEN_Ability AddAbility(CardIDs.CARDID eID)
        {
            FEN_Ability ability = FEN.Loading.LoadAbility(eID, this, false);
            AbilityList.AddAbility(ability);
            return ability;
        }

        /// <summary>
        /// Adds value to the given status's given stat. If the status is not within this unit's 
        /// owned abilities, add the status to the unit.
        /// </summary>
        /// <param name="eCardID"></param>
        /// <param name="strStatID"></param>
        /// <param name="nValue"></param>
        public FEN_Ability AddValueToStatus(CardIDs.CARDID eCardID, int nValue = 1, STATPLACEMENT eStatPlacement = STATPLACEMENT.BASE)
        {
                string strStatID = eCardID.ToString().ToLower();

            FEN_Ability ability = GetAbilityByAbilityID(eCardID);
            if (ability == null)
            {
                ability = AddAbility(eCardID);
                ability.mo_abilityProcess.OnAddedAsStatus();
            }


            switch (eStatPlacement)
            {
                case STATPLACEMENT.BASE:
                    ability.AddToBase(strStatID, nValue);
                    Debug.Log("Added " + nValue + " to base status " + strStatID + " of unit " + nameAndID + "'s ability " + ability.nameAndID + " owned by " + ability.Owner.nameAndID);
                    break;
                case STATPLACEMENT.BONUS:
                    ability.AddToBonus(strStatID, nValue);
                    Debug.Log("Added " + nValue + " to bonus status " + strStatID + " of unit " + nameAndID + "'s ability " + ability.nameAndID + " owned by " + ability.Owner.nameAndID);
                    break;
                case STATPLACEMENT.CURRENT:
                    ability.AddToCurrent(strStatID, nValue);
                    Debug.Log("Added " + nValue + " to current status " + strStatID + " of unit " + nameAndID + "'s ability " + ability.nameAndID + " owned by " + ability.Owner.nameAndID);
                    break;
                case STATPLACEMENT.BASEANDCURRENT:
                    ability.AddToBase(strStatID, nValue);
                    ability.AddToCurrent(strStatID, nValue);
                    Debug.Log("Added " + nValue + " to base and current value of status " + strStatID + " of unit " + nameAndID + "'s ability " + ability.nameAndID + " owned by " + ability.Owner.nameAndID);
                    break;
            }

            return ability;

        }


        /// <summary>
        /// Removes value from the given status's 
        /// </summary>
        /// <param name="eCardID"></param>
        /// <param name="strStatID"></param>
        /// <param name="nValue"></param>
        public FEN_Ability RemoveValueFromStatus(CardIDs.CARDID eCardID, int nValue)
        {
            FEN_Ability ability = GetAbilityByAbilityID(eCardID);
            if (ability == null)
            {
                ability = AddAbility(eCardID);
            }

            ability.AddToBase(eCardID.ToString().ToLower(), -nValue);

            if (ability.Stats.GetStat(eCardID.ToString().ToLower()).Base <= 0)
            {
                RemoveAbility(ability);
            }

            return ability;

        }
        

        /// <summary>
        /// Removes an ability with the given Card ID.
        /// Note that this only removes one such ability.
        /// </summary>
        /// <param name="eCardID"></param>
        public void RemoveAbility(CardIDs.CARDID eCardID)
        {
            AbilityList.RemoveAbility(eCardID);
        }

        /// <summary>
        /// Removes an ability with the given Card ID.
        /// Note that this only removes one such ability.
        /// </summary>
        /// <param name="eCardID"></param>
        public void RemoveAbility(FEN_Ability ab)
        {
            AbilityList.RemoveAbility(ab);
        }


        //Ability Accessors 
        #region Ability 

        public FEN_Ability GetAbilityByAbilityID(CardIDs.CARDID eID)
        {
            foreach (FEN_Ability ability in AbilityList.Abilities)
            {
                if (ability.me_abilityID == eID)
                {

                    return ability;
                }
            }

            return null;
        }

        public FEN_AbilityProcess GetAbilityProcessByAbilityID(CardIDs.CARDID eID)
        {
            foreach (FEN_Ability ability in AbilityList.Abilities)
            {
                if (ability.me_abilityID == eID)
                {

                    return ability.mo_abilityProcess;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns this unit's ability with the given Unique ID.
        /// If thie unit does not have ability with the given ID, 
        /// returns null.
        /// </summary>
        /// <param name="nUniqueID"></param>
        /// <returns></returns>
        public FEN_Ability GetAbilityByUniqueID(int nUniqueID)
        {
            return AbilityList.GetAbilityByUniqueID(nUniqueID);
        }

        public bool HasAbility(CardIDs.CARDID ab)
        {
            return GetAbilityByAbilityID(ab) != null;
        }

        public bool HasAbility(FEN_Ability ab)
        {
            return GetAbilityByUniqueID(ab.UniqueID) != null;
        }

        public bool HasAbility(FEN_AbilityProcess ab)
        {
            return GetAbilityByUniqueID(ab.OwningAbility.UniqueID) != null;
        }

        /// <summary>
        /// Returns a list of this units abilities that bear the given attribute
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilityList(string str)
        {
            return AbilityList.GetAbilitiesAttribute(str);
        }

        /// <summary>
        /// Returns a list of this units abilities that bear the given attributes
        /// </summary>
        /// <param name="astr"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilitiesByAttribute(string[] astr)
        {
            return AbilityList.GetAbilitiesByAttributeArray(astr);
        }

        /// <summary>
        /// Returns a list of this units abilities that bear ANY of the given attributes
        /// </summary>
        /// <param name="astr"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilitiesByAnyAttribute(string[] astr)
        {
            return AbilityList.GetAbilitiesByAnyAttributeArray(astr);
        }

        /// <summary>
        /// Returns a list of this units abilities that bear the given attributes
        /// </summary>
        /// <param name="astr"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilitiesByAttribute(string astr)
        {
            return AbilityList.GetAbilitiesAttribute(astr);
        }
        #endregion
        
    }
}
