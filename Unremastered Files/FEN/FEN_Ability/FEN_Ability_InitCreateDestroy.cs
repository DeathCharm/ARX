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
    public partial class FEN_Ability: ARX_Actor
    {

        private void Awake()
        {
            
        }

        /// <summary>
        /// Returns null.
        /// </summary>
        static FEN_Unit NULLUNIT { get { return null; } }


        /// <summary>
        /// Clones this ability.
        /// </summary>
        public FEN_Ability Clone
        {
            get
            {
                //Debug.Log(nameAndID + " beginning clone instantiation");
                FEN_Ability other = Instantiate<FEN_Ability>(this);

                //Debug.Log(nameAndID + " end clone instantiated into " + other.nameAndID);

                string nFirstID = other.nameAndID;
                other.SetAsUniqueInstance();
                string nSecondID = other.nameAndID;

                //Debug.Log("Clone of " + nameAndID + " renumbered to " + other.nameAndID);

                List<FEN_Ability> oBuf = new List<FEN_Ability>();

                other.Costs = other.Costs.Clone;
                other.Stats = other.Stats.Clone;
                if (other.mo_gameObject != null)
                    other.mo_gameObject = GameObject.Instantiate(other.mo_gameObject);

                for (int i = 0; i < ChildAbilities.Length; i++)
                {
                    oBuf.Add(ChildAbilities[i]);
                }

                other.mo_childAbilities = oBuf.ToArray();

                return other;
            }
        }
        

        /// <summary>
        /// Destroy's this ability and removes it from the
        /// global ability list.
        /// </summary>
        public void Destroy()
        {
            //Debug.Log("Destroying ability  " + nameAndID + " from allabilities.");
            RPGElementCollections.AbilitiesInPlay.Remove(UniqueID);
            EventRecord.UnsubscribeFromAllEvents();
            
            if (mo_abilityProcess != null)
            {
                mo_abilityProcess.EventRecord.UnsubscribeFromAllEvents();
                mo_abilityProcess.Terminate();
            }

            if (AbilityGameObject != null)
            {
                GameObject.Destroy(AbilityGameObject);
                AbilityGameObject = null;
                //Ez.Pooly.Pooly.Despawn(CardUnityObject.transform);
            }
        }

        /// <summary>
        /// Function callback ran when a Battle Encounter is unloaded.
        /// </summary>
        /// <param name="dat"></param>
        void ReactToUnloadBattleAssets(DataString dat)
        {

        }

        /// <summary>
        /// Creates this card's ability AI and sets the 
        /// parent of its child abilities to itself.
        /// </summary>
        /// <param name="eID"></param>
        public void Initialize(CardIDs.CARDID eID, bool bIsIndexCard)
        {
            //Debug.Log("Initializing card " + eID);
            if (bIsIndexCard == false)
            {
                LoadCardAI(this, Owner, eID);
            }
            RPGElementCollections.AbilitiesInPlay.Add(UniqueID, this);
        }

        /// <summary>
        /// Load the given card with the given card ID's AI.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="owner"></param>
        /// <param name="eID"></param>
        public static void LoadCardAI(FEN_Ability card, FEN_Unit owner, CardIDs.CARDID eID)
        {
            Debug.LogError("LoadCardAI is currently not implemented");
            throw new Exception();
           //FEN.Decks.Helper.LoadCardAI(card, owner, eID);
        }
        
    }
}
