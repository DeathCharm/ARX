using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;
using FEN;
using UnityEngine;

namespace FEN
{
    public partial class FEN_Ability : ARX_Actor
    {
        /// <summary>
        /// Accessor for the ability that his ability is a child of.
        /// If null, this ability has no parent.
        /// </summary>
        public FEN_Ability ParentAbility { get { return mo_parentAbility; } }

        /// <summary>
        /// Returns false if this is a sub ability(if it has a parent ability).
        /// Else, returns true.
        /// </summary>
        public bool IsChild { get { return ParentAbility != null; } }

       
        /// <summary>
        /// Accessor for the list of nested abilities this ability owns.
        /// </summary>
        public FEN_Ability[] ChildAbilities
        {
            get
            {
                if (mo_childAbilities == null)
                    mo_childAbilities = new FEN_Ability[0];
                return mo_childAbilities;
            }
        }

        /// <summary>
        /// Returns the number of child abilities this ability owns.
        /// </summary>
        public int ChildCount { get { return ChildAbilities.Length; } }

        /// <summary>
        /// Accessor for the list of nested abilities this ability owns.
        /// </summary>
        public List<FEN_Ability> ViewableAbilitiesAsList
        {
            get
            {
                
                    List<FEN_Ability> buf = new List<FEN_Ability>();
                    buf.Add(this);
                    return buf;
                
            }
        }

        ///// <summary>
        ///// Removes the given ability from this ability's child abilities.
        ///// </summary>
        ///// <param name="ab"></param>
        //public void RemoveAbility(FEN_Ability ab)
        //{
        //    ChildAbilities.RemoveAbility(ab);
        //}

        ///// <summary>
        ///// Removes the given ability from this ability's child abilities.
        ///// </summary>
        ///// <param name="ab"></param>
        //public void RemoveAbility(CardIDs.CARDID eID)
        //{
        //    ChildAbilities.RemoveAbility(GetChildAbility(eID));
        //}

    }
}
