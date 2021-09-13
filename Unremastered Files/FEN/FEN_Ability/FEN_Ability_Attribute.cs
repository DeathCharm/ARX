using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ARX;
using FEN;

namespace FEN
{
    public partial class FEN_Ability : ARX_Actor
    {

        #region Attributes

        /// <summary>
        /// Returns a string with the attributes this ability
        /// has, arranged in a way to be displayed on the card.
        /// </summary>
        public string GetAttributeStringForDisplay
        {
            get
            {
                string strReturn = "";
                for (int i = 0; i < mastr_attributes.Count; i++)
                {
                    strReturn += mastr_attributes[i];
                    if (i < mastr_attributes.Count - 1)
                        strReturn += ",";
                }

                return strReturn;
            }
        }



        /// <summary>
        /// Adds the given attribute to this ability if it is not already present.
        /// </summary>
        /// <param name="strAtt"></param>
        public void AddAttribute(string strAtt)
        {
            if (mastr_attributes.Contains(strAtt) == false)
                mastr_attributes.Add(strAtt);
        }

        public void RemoveAttribute(string strAtt)
        {
            List<string> buf = new List<string>(mastr_attributes);
            buf.Remove(strAtt);
            mastr_attributes = new List<string>(buf);
        }

        /// <summary>
        /// Adds the given attributes to this ability if it is not already present.
        /// </summary>
        /// <param name="strAtt"></param>
        public void AddAttribute(string[] astrAtt)
        {
            foreach (string strAtt in astrAtt)
                if (mastr_attributes.Contains(strAtt) == false)
                    mastr_attributes.Add(strAtt);

        }

        /// <summary>
        /// Return true if this ability has the given attribute
        /// </summary>
        /// <param name="strAttribute"></param>
        /// <returns></returns>
        public bool HasAttribute(string strAttribute)
        {
            foreach (string str in mastr_attributes)
                if (str == strAttribute)
                    return true;
            return false;
        }


        /// <summary>
        /// Returns a list of the child abilities containing the given attributes.
        /// </summary>
        /// <param name="astrAttributes"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetChildAbilitiesByAttributes(string[] astrAttributes)
        {
            List<FEN_Ability> oReturn = new List<FEN_Ability>();
            foreach (FEN_Ability ab in ChildAbilities)
                if (ab.HasAttributes(astrAttributes))
                    oReturn.Add(ab);
            return oReturn;
        }

        /// <summary>
        /// Returns a list of the child abilities containing the given attributes.
        /// </summary>
        /// <param name="strAttribute"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetChildAbilitiesByAttribute(string strAttribute)
        {
            List<FEN_Ability> oReturn = new List<FEN_Ability>();
            foreach (FEN_Ability ab in ChildAbilities)
                if (ab.HasAttribute(strAttribute))
                    oReturn.Add(ab);
            return oReturn;
        }

        /// <summary>
        /// Return true if this ability has all of the given attributes.
        /// </summary>
        /// <param name="strAttribute"></param>
        /// <returns></returns>
        public bool HasAttributes(string[] astrAttribute)
        {
            foreach (string str in astrAttribute)
                if (mastr_attributes.Contains(str) == false)
                    return false;
            return true;
        }

        /// <summary>
        /// Return true if this ability has any of the given attributes.
        /// </summary>
        /// <param name="strAttribute"></param>
        /// <returns></returns>
        public bool HasAnyAttributes(string[] astrAttribute)
        {
            foreach (string str in astrAttribute)
                if (mastr_attributes.Contains(str) == true)
                    return true;
            return false;
        }


        /// <summary>
        /// Sets this ability with the attributes of a non-battle statuseffect
        /// </summary>
        public void SetAsStatusEffect() { AddAttribute(new string[] { IDs.StatusEffect }); }

        /// <summary>
        /// Sets this ability with the attributes of a battle status effect.
        /// </summary>
        public void SetAsBattleStatusEffect() { AddAttribute(new string[] { IDs.Battle, IDs.StatusEffect }); }
      

        #endregion

    }
}
