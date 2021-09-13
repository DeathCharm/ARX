using System.Collections.Generic;
using UnityEngine;
using ARX;
using FEN;


namespace FEN
{
    [CreateAssetMenu(menuName = "FEN/Ability List")]
    [System.Serializable]
    public class FEN_AbilityList : ScriptableObject
    {

        public string nameAndID
        {
            get
            {
                return name + "_" + UniqueID;
            }
        }
        int mn_uniqueID = -1;
        public int UniqueID
        {
            get
            {
                if (mn_uniqueID == -1)
                    mn_uniqueID = ARX_Actor.GetNextUniqueID;
                return mn_uniqueID;
            }
        }

        public override string ToString()
        {
            string str = "";
            str += name + " Ability List.\n";
            str += "Size: " + Count;
            for (int i = 0; i < Count; i++)
            {
                str += i + ". " + mo_abilities[i].name;

                str += "\t";
            }
            return str;
        }

        public string GetDrawChances()
        {
            string str = "";
            
            List<CardIDs.CARDID> oaCardsInDeck = new List<CardIDs.CARDID>();

            foreach (FEN_Ability ab in mo_abilities)
            {
                if (oaCardsInDeck.Contains(ab.me_abilityID) == false)
                    oaCardsInDeck.Add(ab.me_abilityID);
            }

            foreach (CardIDs.CARDID eID in oaCardsInDeck)
            {
                float nCopies = GetAbilitiesByCardID(eID).Count;
                float nfChance = nCopies / (float)Count * 100;

                str += RPGElementCollections.AbilityIndex[eID].CardName + ": ";
                str += nCopies + "/" + Count + " = " + (int)nfChance + "% Draw Chance\n";
            }
            
            return str;
        }

        public FEN_AbilityList Clone
        {
            get
            {
                FEN_AbilityList other = (FEN_AbilityList)this.MemberwiseClone();

                other.Abilities = new List<FEN_Ability>(other.Abilities.ToArray());
                return other;
            }
        }
        

        public static FEN_AbilityList GetAbilityListWithAttribute(string strAttribute)
        {
            FEN_AbilityList oList = ScriptableObject.CreateInstance<FEN_AbilityList>();

            foreach (FEN_Ability ab in RPGElementCollections.AbilityIndex.Values)
                if (ab.HasAttribute(strAttribute))
                    oList.AddAbility(ab);

            return oList;

        }

        public bool IsFullyPaid
        {
            get
            {
                foreach (FEN_Ability ability in mo_abilities)
                {
                    if (ability.IsFullyPaid == false)
                        return false;
                }
                return true;
            }
        }

        public int GetTotalStatCost
        {

            get
            {
                int n = 0;
                foreach (FEN_Ability ability in Abilities)
                    n += ability.GetTotalStatCost;
                return n;
            }

        }

        /// <summary>
        /// Returns a debugging list of abilities.
        /// </summary>
        public static FEN_AbilityList GetStarterInventory
        {
            get
            {
                FEN_AbilityList buf = ScriptableObject.CreateInstance<FEN_AbilityList>();
                
                
                buf.name = "Test Deck";
                return buf;
            }
        }

        /// <summary>
        /// Returns a datastring containing information
        /// pointing to this actor
        /// </summary>
        public virtual DataString GetThisActorMessage
        {
            get
            {
                DataString dat = new DataString(this);
                dat.SetInt("source", UniqueID);
                return dat;
            }
        }

        public int Count
        {
            get
            {
                return Abilities.Count;
            }
        }

        /// <summary>
        /// Adds an ability to this list.
        /// </summary>
        /// <param name="ability"></param>
        public bool AddAbility(FEN_Ability ability)
        {
            if (Abilities.Contains(ability) == false)
            {
                Abilities.Add(ability);
                return true;

                //GameEvents.onCardMovedToDeck.FireEvent(ability.GetMessage);
            }
            else
                return false;
        }

        public void AddAbility(List<FEN_Ability> oList)
        {
            foreach (FEN_Ability ability in oList)
                AddAbility(ability);
        }

        public void AddAbility(CardIDs.CARDID eID)
        {
            AddAbility(RPGElementCollections.AbilityIndex[eID]);
        }

        public void AddAbility(CardIDs.CARDID[] aeID)
        {
            if (aeID == null)
            {
                Debug.LogError("Null list of abilities given to ability list " + name);
                return;
            }
            foreach(CardIDs.CARDID eID in aeID)
                AddAbility(eID);
        }

        /// <summary>
        /// Removes an ability from this list.
        /// </summary>
        /// <param name="ability"></param>
        public bool RemoveAbility(FEN_Ability ability)
        {
            return Abilities.Remove(ability);
        }

        public bool RemoveAbility(FEN.CardIDs.CARDID eCardID)
        {
            return RemoveAbility(GetAbilityByCardID(eCardID));
        }

        /// <summary>
        /// Removes all abilities with the given attribute from this ability list.
        /// </summary>
        /// <param name="strAttribute"></param>
        public void RemoveAbility(string strAttribute)
        {
            for (int i = 0; i < Abilities.Count; i++)
            {
                FEN_Ability a = Abilities[i];
                if (a.HasAttribute(strAttribute))
                {
                    mo_abilities.Remove(a);
                    i--;
                    continue;
                }
            }
        }

        public void Clear()
        {
            Abilities.Clear();
        }

        public void Destroy()
        {
            foreach (FEN_Ability ab in Abilities)
                if(ab != null)
                    ab.Destroy();

            Clear();
        }

        /// <summary>
        /// A list of processes attached to this unit that act as their abilities.
        /// Includes buffs, debuffs, statuses, cards and attacks.
        /// </summary>
        [SerializeField]
        private List<FEN_Ability> mo_abilities;

        [SerializeField]
        public List<CardIDs.CARDID> me_debugIDs;

        public List<FEN_Ability> Abilities
        {
            get
            {
                if (mo_abilities == null)
                    mo_abilities = new List<FEN_Ability>();
                return mo_abilities;
            }
            set
            {
                mo_abilities = value;
            }
        }

        public CardIDs.CARDID[] AbilityIDs
        {
            get
            {
                List<CardIDs.CARDID> buf = new List<CardIDs.CARDID>();
                foreach (FEN_Ability ab in mo_abilities)
                    buf.Add(ab.me_abilityID);
                return buf.ToArray();

            }
        }

        /// <summary>
        /// Returns a clone of this object's list of abilities.
        /// </summary>
        public List<FEN_Ability> AsList
        {
            get
            {
                List<FEN_Ability> buf = new List<FEN_Ability>(Abilities);
                return buf;
            }
        }

        /// <summary>
        /// Move the given card up by one position
        /// </summary>
        /// <param name="ab"></param>
        public void MoveUpByOneIndex(FEN_Ability ab)
        {
            int nOldIndex = mo_abilities.IndexOf(ab);


            if (nOldIndex > 0)
            {
                mo_abilities.Remove(ab);
                mo_abilities.Insert(nOldIndex - 1, ab);

                int nNewIndex = mo_abilities.IndexOf(ab);

                Debug.Log("Moved card " + ab.nameAndID + " up in deck " + name + " from index " + nOldIndex + " to " + nNewIndex);
            }
        }

        /// <summary>
        /// Move the given card down by one position
        /// </summary>
        /// <param name="ab"></param>
        public void MoveDownByOneIndex(FEN_Ability ab)
        {
            int nIndex = mo_abilities.IndexOf(ab);

            if (nIndex < mo_abilities.Count - 1)
            {
                mo_abilities.Remove(ab);
                mo_abilities.Insert(nIndex + 1, ab);
            }
        }
        
        public void MoveCardsTo(DECKTYPE eDeck)
        {
            for (int i = 0; i < mo_abilities.Count; i++)
            {
                FEN_Ability ab = mo_abilities[i];
                ab.MoveCard(eDeck);
                i--;
            }
        }


        public void MoveCardsTo(DECKTYPE eDeck, string strAttribute)
        {
            for (int i = 0; i < mo_abilities.Count; i++)
            {
                FEN_Ability ab = mo_abilities[i];

                if (ab.HasAttribute(strAttribute))
                {
                    ab.MoveCard(eDeck);
                    i--;
                }
            }

        }

        public void MoveCards(DECKTYPE eDeck, string[] strAttribute)
        {
            for (int i = 0; i < mo_abilities.Count; i++)
            {
                FEN_Ability ab = mo_abilities[i];

                if (ab.HasAttributes(strAttribute))
                {
                    ab.MoveCard(eDeck);
                    i--;
                }
            }

        }

        //Ability Accessors 
        #region Ability Accessors

        /// <summary>
        /// Returns this unit's ability with the given Unique ID.
        /// If thie unit does not have ability with the given ID, 
        /// returns null.
        /// </summary>
        /// <param name="nUniqueID"></param>
        /// <returns></returns>
        public FEN_Ability GetAbilityByUniqueID(int nUniqueID)
        {
            foreach (FEN_Ability ab in Abilities)
                if (ab.UniqueID == nUniqueID)
                    return ab;
            return null;
        }

        /// <summary>
        /// Returns true if the given ability is found within this deck.
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool HasAbility(FEN_Ability ability)
        {
            foreach (FEN_Ability ab in Abilities)
                if (ab == ability)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns a list of this units abilities that bear the given attribute
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilitiesAttribute(string str)
        {
            List<FEN_Ability> oaReturn = new List<FEN_Ability>();
            foreach (FEN_Ability ab in Abilities)
                if (ab.HasAttribute(str))
                    oaReturn.Add(ab);
            return oaReturn;
        }

        /// <summary>
        /// Returns a list of this units abilities that bear the given attribute
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public FEN_Ability GetRandomAbilityByAttribute(string str)
        {
            List<FEN_Ability> oaReturn = new List<FEN_Ability>();
            foreach (FEN_Ability ab in Abilities)
                if (ab.HasAttribute(str))
                    oaReturn.Add(ab);

            if (oaReturn.Count == 0)
                return null;

            return ToolBox.GetRandom<FEN_Ability>(oaReturn);
        }

        /// <summary>
        /// Returns a list of this units abilities that bear the given attributes
        /// </summary>
        /// <param name="astr"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilitiesByAttributeArray(string[] astr)
        {
            List<FEN_Ability> oaReturn = new List<FEN_Ability>();
            foreach (FEN_Ability ab in Abilities)
                if (ab.HasAttributes(astr))
                    oaReturn.Add(ab);
            return oaReturn;
        }

        /// <summary>
        /// Returns a list of this units abilities that bear ANY of the given attributes
        /// </summary>
        /// <param name="astr"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilitiesByAnyAttributeArray(string[] astr)
        {
            Dictionary<int, FEN_Ability> oaReturn = new Dictionary<int, FEN_Ability>();

            foreach(string str in astr)
            {
                List<FEN_Ability> oa = GetAbilitiesByAttribute(str);
                foreach (FEN_Ability ab in oa)
                {
                    oaReturn[ab.UniqueID] = ab;
                }
            }

            return new List<FEN_Ability> (oaReturn.Values);
        }


        /// <summary>
        /// Returns a list of this units abilities that bear the given attributes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilitiesByCardID(FEN.CardIDs.CARDID str)
        {
            List<FEN_Ability> oaReturn = new List<FEN_Ability>();
            foreach (FEN_Ability ab in Abilities)
                if (ab.me_abilityID == str)
                    oaReturn.Add(ab);
            return oaReturn;
        }

        /// <summary>
        /// Returns a list of this units abilities that bear the given attributes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public FEN_Ability GetAbilityByCardID(FEN.CardIDs.CARDID str)
        {
            List<FEN_Ability> oaReturn = new List<FEN_Ability>();
            foreach (FEN_Ability ab in Abilities)
                if (ab.me_abilityID == str)
                    return ab;
            return null;
        }

        /// <summary>
        /// Returns a list of this units abilities that bear the given attributes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<FEN_Ability> GetAbilitiesByAttribute(string str)
        {
            return GetAbilitiesByAttributeArray(new string[] { str });
        }

        public FEN_Ability GetRandomCard()
        {
            if (Abilities.Count == 0)
                return null;
            int nRandom = UnityEngine.Random.Range(0, Abilities.Count);

            return Abilities[nRandom];
        }
        #endregion

        /// <summary>
        /// Initializes each card in the inventory list, then terminates the
        /// created ability process.
        /// </summary>
        public void InitializeInventoryAfterLoad()
        {
            foreach (FEN_Ability ab in mo_abilities)
            {
                ab.Initialize(ab.me_abilityID, false);
                ab.mo_abilityProcess.Terminate();
            }
        }

        public string GetSerializedString()
        {
            return ARX_File.SerializeObject(this);
        }

        public string GetSerializedCardIDString()
        {
            return ARX_File.SerializeObject(AbilityIDs);
        }

        public static FEN_AbilityList Deserialize(string str)
        {
            return ARX_File.DeserializeObject<FEN_AbilityList>(str);
        }
    }
}
