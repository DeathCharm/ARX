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
        /// Returns a message containing this ability's uniqueID
        /// and cardID.
        /// </summary>
        public override DataString GetMessage
        {
            get
            {
                DataString dat = new DataString(this);
                dat.SetInt(GameIDs.ValueUniqueID, UniqueID);
                dat.SetInt("abilityID", (int)me_abilityID);
                return dat;
            }
        }
        

        public string GetCardLoreDescription()
        {
            string str = CardName + "\n\n";
            str += mstr_cardTooltip + "\n\n";
            str += mstr_flavor;

            return str;
        }

        /// <summary>
        /// Move this card to one of the field decks.
        /// </summary>
        /// <param name="eTo"></param>
        public void MoveCard(DECKTYPE eTo)
        {

            //Currently not implemented
            throw new Exception();
            //if (ParentAbility == null)
            //    FEN.PlayField.MoveCard(this, eTo);
            //else
            //    FEN.PlayField.MoveCard(ParentAbility, eTo);
        }


        public string GetAutofillDescription(string strOriginal = "", bool bShowExtra = false)
        {
            string str = "Auto filled";
            
            //Remove the final character if it is a newline
            str = ToolBox.RemoveFinalNewline(str);

            return str;
        }


        public string GetCosts()
        {
            string str = "";

            if (Costs.Count == 0)
                str += "No Cost";
            else
                str += "Costs: ";

            bool bFirst = true;
            foreach (ARX_StatQuad cost in Costs.AsList)
            {
                if (!bFirst)
                    str += " | ";
                bFirst = false;
                str += cost.ID.ToUpper() + " " + cost.Max;
            }

            return str;
        }

        
        /// <summary>
        /// To String override. Returns this ability's name and Unique ID.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return nameAndID;
        }

      
     

        /// <summary>
        /// Returns true if the message's ValueUniqueID is this process's UniqueID
        /// or the UniqueID of the mo_targetAbility
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public bool IsTargetingThisActor(DataString dat)
        {
            return dat.GetInt(GameIDs.ValueUniqueID) == UniqueID;
        }

        /// <summary>
        /// Sets the owning unit of this ability
        /// </summary>
        /// <param name="owner"></param>
        public void SetOwner(FEN_Unit owner)
        {
            mo_owner = owner;
        }

   

     

      

        /// <summary>
        /// Returns true if this ability or its parent is in the owner's list of status abilities
        /// </summary>
        public bool IsInStatuses
        {
            get
            {
                if (Owner != null || Owner.GetAbilityByAbilityID(me_abilityID) != null)
                    return true;
                return false;
            }
        }

      

        /// <summary>
        /// Returns a string detailing this ability's lift costs.
        /// </summary>
        public string GetCostsString
        {
            get
            {
                string str = "";
                for (int i = 0; i < Costs.Count; i++)
                {
                    str += Costs.AsList[i].ToString();
                    if (i < Costs.Count - 1)
                        str += ", ";

                }
                return str;
            }
        }

    
        
    }
}
