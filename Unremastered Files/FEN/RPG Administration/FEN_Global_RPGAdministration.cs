using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace FEN
{
    /// <summary>
    /// Responsible for the Initialization, Serialization and Deserialization of FEN Project games.
    /// </summary>
    public static class RPGAdministration
    {
        #region Initializations

        /// <summary>
        /// Initializes the RPG
        /// </summary>
        public static void InitializeRPG()
        {
            RPGElementCollections.InitializeCollections();
            InitializeAbilityIndex();
            InitializeUnitIndex();
        }

        /// <summary>
        /// Initialization function creates one of each type of card
        /// and saves them to a static library.
        /// </summary>
        public static void InitializeAbilityIndex()
        {
            Debug.Log("Initializing Ability Index.");
            if (RPGElementCollections.AbilityIndex.Count > 0)
            {
                Debug.Log("Ability Index has already been initialized.");
                return;
            }

            int i = 0;
            //Initialize main cards
            while (true)
            {
                CardIDs.CARDID eID = (CardIDs.CARDID)i;

                if (System.Enum.IsDefined(typeof(CardIDs.CARDID), eID) == false)
                    break;

                FEN_Ability ability = FEN.Loading.LoadAbility(eID, null, true);
                RPGElementCollections.AbilityIndex[eID] = ability;
                i++;
            }

            Debug.Log("Ability Index Initialization complete.");

        }


        /// <summary>
        /// Initialization function creates one of each type of card
        /// and saves them to a static library.
        /// </summary>
        public static void InitializeUnitIndex()
        {
            Debug.Log("Initializing Unit Index.");
            if (RPGElementCollections.UnitIndex.Count > 0)
            {
                Debug.Log("Ability Unit has already been initialized.");
                return;
            }

            int i = 0;
            //Initialize main cards
            while (true)
            {
                CardIDs.UNITID eID = (CardIDs.UNITID)i;

                if (System.Enum.IsDefined(typeof(CardIDs.UNITID), eID) == false)
                    break;

                FEN_Unit unit = FEN.Loading.LoadUnit(eID.ToString().ToLower());
                RPGElementCollections.UnitIndex[eID] = unit;
                i++;
            }

            Debug.Log("Unit Index Initialization complete.");

        }
        #endregion
        
        #region Serialization

        /// <summary>
        /// Serializes the current game state to a string, then returns it.
        /// </summary>
        /// <returns></returns>
        public static string GetSaveProfileSerializationString()
        {
            return FEN_SaveProfile.GetSaveProfileString;
        }

        /// <summary>
        /// Deserializes a profile from the given string, then copies its values
        /// to the game state.
        /// </summary>
        /// <param name="jsonSaveProfileString"></param>
        public static void LoadFromString(string jsonSaveProfileString)
        {
            FEN_SaveProfile profile = null;
            try
            {
                profile = JsonUtility.FromJson<FEN_SaveProfile>(jsonSaveProfileString);
            }
            catch
            {
                Debug.LogError("Failed to load profile from string");
            }
            RPGElementCollections.LoadFromProfile(profile);
        }
        #endregion
    }
}
