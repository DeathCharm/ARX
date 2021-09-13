using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

namespace FEN
{
    /// <summary>
    /// Serialization class holds clones of
    /// </summary>
    [System.Serializable]
    public class FEN_SaveProfile
    {
        /// <summary>
        /// Private constructor. Use GetSaveProfile to get an initialized copy of
        /// the current FEN_SaveProfile state.
        /// </summary>
        private FEN_SaveProfile() { }

        public FEN_Unit _GlobalUnit = null;

        public ARX_StatBox_Quad GameStats;

        public FEN_AbilityIndexList AbilitiesInPlay = new FEN_AbilityIndexList();
        public FEN_UnitIndexList UnitsInPlay = new FEN_UnitIndexList();

        public FEN_AbilityIndex AbilityIndex = new FEN_AbilityIndex();
        public FEN_UnitIndex UnitIndex = new FEN_UnitIndex();

        /// <summary>
        /// Returns a fully formatted and ready to use SaveProfile of the current game state.
        /// </summary>
        public static FEN_SaveProfile GetSaveProfile
        {
            get
            {
                FEN_SaveProfile profile = new FEN_SaveProfile();

                //Global Unit
                profile._GlobalUnit = RPGElementCollections._GlobalUnit.Clone;
                profile.GameStats = RPGElementCollections.GameStats.Clone;

                //Abilities in play
                int i = 0;
                foreach (FEN_Ability ability in RPGElementCollections.AbilitiesInPlay.Values)
                {
                    i++;
                    profile.AbilitiesInPlay.Add(i, ability.Clone);
                }

                //Units in Play
                i = 0;
                foreach (FEN_Unit unit in RPGElementCollections.UnitsInPlay.Values)
                {
                    i++;
                    profile.UnitsInPlay.Add(i, unit.Clone);
                }

                //Ability Index
                foreach (FEN_Ability ability in RPGElementCollections.AbilityIndex.Values)
                {
                    profile.AbilityIndex.Add(ability.me_abilityID, ability.Clone);
                }

                //Unit Index
                foreach (FEN_Unit unit in RPGElementCollections.UnitIndex.Values)
                {
                    profile.UnitIndex[unit.me_unitID] = unit.Clone;
                }


                return profile;
            }
        }

        /// <summary>
        /// Serializes the current game state to a string, then returns it.
        /// </summary>
        /// <returns></returns>
        public static string GetSaveProfileString
        {
            get
            {
                return JsonUtility.ToJson(GetSaveProfile);
            }
        }
    }
}
