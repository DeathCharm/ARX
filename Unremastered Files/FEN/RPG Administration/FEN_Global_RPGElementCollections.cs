using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;

namespace FEN
{
    /// <summary>
    /// Contains all RPG Elements that are initialized or created at runtime.
    /// </summary>
    public static class RPGElementCollections
    {
        /// <summary>
        /// Global unit used to hold instantiated global abilities.
        /// </summary>
        public static FEN_Unit _GlobalUnit = null;

        /// <summary>
        /// Global dictionary of all currently active abilities.
        /// </summary>
        public static Dictionary<int, FEN_Ability> AbilitiesInPlay = new Dictionary<int, FEN_Ability>();

        /// <summary>
        /// Dictionary of every active unit
        /// </summary>
        public static Dictionary<int, FEN_Unit> UnitsInPlay = new Dictionary<int, FEN_Unit>();

        /// <summary>
        /// Global repository of one of each Ability.
        /// </summary>
        public static Dictionary<CardIDs.CARDID, FEN_Ability> AbilityIndex = new Dictionary<CardIDs.CARDID, FEN_Ability>();

        /// <summary>
        /// Global repository of one of each Ability.
        /// </summary>
        public static Dictionary<CardIDs.UNITID, FEN_Unit> UnitIndex = new Dictionary<CardIDs.UNITID, FEN_Unit>();

        /// <summary>
        /// Collection of all game values.
        /// </summary>
        public static ARX_StatBox_Quad GameStats;

        /// <summary>
        /// Reinitialize the collections to a blank, empty state.
        /// Called by RPGAdministration.InitializeRPG() at the start of every game.
        /// </summary>
        public static void InitializeCollections()
        {
            if (_GlobalUnit != null)
                _GlobalUnit.Destroy();

            if (GameStats != null)
                GameStats.Clear();

            if (AbilitiesInPlay != null)
                foreach (FEN_Ability ab in AbilitiesInPlay.Values)
                    ab.Destroy();

            if (AbilityIndex != null)
                foreach (FEN_Ability ab in AbilityIndex.Values)
                    ab.Destroy();

            if (UnitsInPlay != null)
                foreach (FEN_Unit un in UnitsInPlay.Values)
                    un.Destroy();

            if (UnitIndex != null)
                foreach (FEN_Unit un in UnitIndex.Values)
                    un.Destroy();


            _GlobalUnit = ScriptableObject.CreateInstance<FEN_Unit>();
            AbilitiesInPlay = new Dictionary<int, FEN_Ability>();
            AbilityIndex = new Dictionary<CardIDs.CARDID, FEN_Ability>();
            UnitIndex = new Dictionary<CardIDs.UNITID, FEN_Unit>();
            UnitsInPlay = new Dictionary<int, FEN_Unit>();
            GameStats = ScriptableObject.CreateInstance<ARX_StatBox_Quad>();
        }

        /// <summary>
        /// Saves the 
        /// </summary>
        /// <param name="profile"></param>
        public static void LoadFromProfile(FEN_SaveProfile profile)
        {
            InitializeCollections();

            _GlobalUnit = profile._GlobalUnit;
            AbilitiesInPlay = profile.AbilitiesInPlay;
            AbilityIndex = profile.AbilityIndex;
            UnitIndex = profile.UnitIndex;
            UnitsInPlay = profile.UnitsInPlay;
            GameStats = profile.GameStats;
        }


    }
}
