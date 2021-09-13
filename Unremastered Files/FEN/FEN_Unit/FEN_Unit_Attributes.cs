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


        #region Attributes

        /// <summary>
        /// Returns a string of this unit's attributes
        /// for display.
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
        #endregion
    }
}