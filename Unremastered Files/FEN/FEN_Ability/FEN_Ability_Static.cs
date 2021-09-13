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

        //#region Static Functions to be remade

        ///// <summary>
        ///// Spawns a random card with the given attribute and a Unity card object. Adds it to the given deck.
        ///// </summary>
        ///// <param name="strAttribute"></param>
        ///// <param name="eDeck"></param>
        ///// <returns></returns>
        //public static FEN_Ability SpawnRandomCardToDeck(string strAttribute, DECKTYPE eDeck)
        //{
        //    List<FEN_Ability> obuf = new List<FEN_Ability>();

        //    foreach (FEN_Ability ab in AbilityIndex.Values)
        //        if (ab.HasAttribute(strAttribute))
        //            obuf.Add(ab);

        //    Debug.Log("Found " + obuf.Count + " abilities with the " + strAttribute + " attribute.");

        //    if (obuf.Count == 0)
        //        return null;

        //    int nRandom = UnityEngine.Random.Range(0, obuf.Count);

        //    FEN_Ability oReturn = SpawnNewCardToDeck(obuf[nRandom], eDeck);

        //    return oReturn;

        //}


        //public static FEN_Ability SpawnRandomCard(string strAttribute)
        //{
        //    List<FEN_Ability> obuf = new List<FEN_Ability>();

        //    foreach (FEN_Ability ab in AbilityIndex.Values)
        //        if (ab.HasAttribute(strAttribute))
        //            obuf.Add(ab);

        //    Debug.Log("Found " + obuf.Count + " abilities with the " + strAttribute + " attribute.");

        //    if (obuf.Count == 0)
        //        return null;

        //    int nRandom = UnityEngine.Random.Range(0, obuf.Count);

        //    FEN_Ability oReturn = SpawnNewCard(obuf[nRandom].me_abilityID);

        //    return oReturn;

        //}

        //public static List<FEN_Ability> GetAbilitiesWithAnyOfTheseAttributes(string[] astrAttributes)
        //{
        //    List<FEN_Ability> buf = new List<FEN_Ability>();
        //    foreach (string str in astrAttributes)
        //    {
        //        buf.AddRange(GetAbilitiesWithAttribute(str));
        //    }
        //    return buf;

        //}

        //public static List<FEN_Ability> GetAbilitiesWithAttribute(string strAttribute)
        //{
        //    List<FEN_Ability> obuf = new List<FEN_Ability>();

        //    foreach (FEN_Ability ab in AbilityIndex.Values)
        //        if (ab.HasAttribute(strAttribute))
        //            obuf.Add(ab);

        //    //Debug.Log("Found " + obuf.Count + " abilities with the " + strAttribute + " attribute.");
        //    return obuf;
            
        //}

        //public static FEN_AbilityList GetAbilityListWithAttribute(string strAttribute)
        //{
        //    return FEN_AbilityList.GetAbilityListWithAttribute(strAttribute);
        //}

        ///// <summary>
        ///// Spawns one of the given card with a Unity card object and adds it to the given deck.
        ///// </summary>
        ///// <param name="eDestination"></param>
        ///// <param name="eCard"></param>
        ///// <returns></returns>
        //public static FEN_Ability SpawnNewCardToDeck(CardIDs.CARDID eCard, DECKTYPE eDestination, bool bShowPreview = true)
        //{
        //    FEN_Ability oNewCard = SpawnNewCard(eCard);

        //    if (bShowPreview)
        //        MessageFactory.SendSpawnedCardMessage(oNewCard, eDestination);

        //    if(oNewCard.AbilityGameObject != null)
        //        oNewCard.AbilityGameObject.transform.SetParent(null);
            
        //    FEN.PlayField.MoveCard(oNewCard, eDestination);

        //    //oNewCard.VisualCardScript.MoveToPoint(eDestination);
        //    //oNewCard.CardScript.ShowCanvasIfClickable();
        //    return oNewCard;
        //}

        ///// <summary>
        ///// Clones the given ability, spawns a Unity Card object and adds it to the given deck.
        ///// </summary>
        ///// <param name="ability"></param>
        ///// <param name="eDestination"></param>
        ///// <returns></returns>
        //public static FEN_Ability SpawnNewCardToDeck(FEN_Ability ability, DECKTYPE eDestination, bool bShowPreview = true)
        //{
        //    FEN_Ability oNewCard = SpawnNewCard(ability.me_abilityID);

        //    if (bShowPreview)
        //        MessageFactory.SendSpawnedCardMessage(oNewCard, eDestination);

        //    if(oNewCard.AbilityGameObject != null)
        //        oNewCard.AbilityGameObject.transform.SetParent(null);

        //    oNewCard.Stats = ability.Stats.Clone;
        //    oNewCard.Costs = ability.Costs.Clone;
        //    foreach (string str in ability.mastr_attributes)
        //        oNewCard.AddAttribute(str);

        //    oNewCard.SetOriginalCard(ability);

        //    FEN.PlayField.MoveCard(oNewCard, eDestination);

        //    //oNewCard.VisualCardScript.MoveToPoint(eDestination);
        //    return oNewCard;
        //}

        ///// <summary>
        ///// Transform one card into another. 
        ///// This has the distinct effect of merging the two card's stats instead
        ///// of cloning one. Be wary of unintended game balance effects! 
        ///// ;D
        ///// </summary>
        ///// <param name="eDestination"></param>
        ///// <param name="ability"></param>
        ///// <param name="eID"></param>
        ///// <returns></returns>
        //public static FEN_Ability TransmuteNewCardToDeck(FEN_Ability ability, CardIDs.CARDID eID, DECKTYPE eDestination)
        //{
        //    FEN_Ability oNewCard = SpawnNewCard(eID);
        //    oNewCard.AbilityGameObject.transform.SetParent(null);

        //    MessageFactory.SendSpawnedCardMessage(oNewCard, eDestination);

        //    //Merge stats instead of cloning stats.
        //    oNewCard.Stats.Merge(ability.Stats);

        //    oNewCard.Costs = ability.Costs.Clone;
        //    foreach (string str in ability.mastr_attributes)
        //        oNewCard.AddAttribute(str);

        //    FEN.PlayField.MoveCard(oNewCard, eDestination);
        //    //oNewCard.VisualCardScript.MoveToPoint(eDestination);
        //    return oNewCard;
        //}

        ///// <summary>
        ///// Creates a blank ability, gives it the given attributes and AI.
        ///// Used to create Status effect type abilities.
        ///// </summary>
        ///// <param name="oProcessAI"></param>
        ///// <param name="astrAttributes"></param>
        ///// <returns></returns>
        //public static FEN_Ability CreateBlankOrStatusAbility(FEN_AbilityProcess oProcessAI, string[] astrAttributes)
        //{
        //    FEN_Ability ability = ScriptableObject.CreateInstance<FEN_Ability>();
        //    ability.mo_abilityProcess = oProcessAI;
        //    ability.name = oProcessAI.name;
        //    ability.mastr_attributes.AddRange(astrAttributes);
        //    return ability;
        //}

        ///// <summary>
        ///// Creates a blank ability, gives it the given attributes and AI.
        ///// Used to create Status effect type abilities.        
        ///// <param name="oStatus"></param>
        ///// <param name="strAttribute"></param>
        ///// <returns></returns>
        //public static FEN_Ability CreateAbility(FEN_AbilityProcess oStatus, string strAttribute)
        //{
        //    return CreateBlankOrStatusAbility(oStatus, new string[] { strAttribute });
        //}
        

        ///// <summary>
        ///// Returns a debugging ability
        ///// </summary>
        //public static FEN_Ability GetTestAbility
        //{
        //    get
        //    {
        //        FEN_Ability card = FEN.Loading.LoadAbility(CardIDs.CARDID.MELEE_RABBITPUNCH, PlayField.mo_player, false);
        //        return card;
        //    }
        //}
        

        ///// <summary>
        ///// Creates an ability from the given ID and returns it.
        ///// Does not create a Unity Card object for it.
        ///// </summary>
        ///// <param name="eCard"></param>
        ///// <param name="owner"></param>
        ///// <returns></returns>
        //public static FEN_Ability CreateAbility(CardIDs.CARDID eCard, FEN_Unit owner)
        //{
        //    FEN_Ability card = FEN.Loading.LoadAbility(eCard, owner, false);
        //    return card;
        //}

        ///// <summary>
        ///// Creates an ability from the ID and returns it.
        ///// Owner is set as the Player.
        ///// Does not create a Unity Card object for it.
        ///// </summary>
        ///// <param name="eCard"></param>
        ///// <returns></returns>
        //public static FEN_Ability CreateAbility(CardIDs.CARDID eCard)
        //{
        //    FEN_Ability card = FEN.Loading.LoadAbility(eCard, PlayField.mo_player, false);
        //    //Debug.Log("Creating card-less ability " + card.ToString());
        //    return card;
        //}

        ///// <summary>
        ///// Creates an ability from the ID and returns it.
        ///// Owner is set as the Player.
        ///// Also creates a Unity Card object for it.
        ///// </summary>
        ///// <param name="eCard"></param>
        ///// <returns></returns>
        //public static FEN_Ability SpawnNewCard(CardIDs.CARDID eCard)
        //{

        //    FEN_Ability card = FEN.Loading.LoadAbility(eCard, PlayField.mo_player, false);
        //    //GameObject obj = card.InstantiateCardVisual();
        //    //obj.transform.position = new Vector3(40, 40, 0);

        //    return card;
        //}

        ///// <summary>
        ///// Returns the ability lying in the given space in the 
        ///// Global Dictionary of active abilities.
        ///// </summary>
        ///// <param name="nUniqueID"></param>
        ///// <returns></returns>
        //public static FEN_Ability GetByUniqueID(int nUniqueID)
        //{
        //    if (AbilitiesInPlay.ContainsKey(nUniqueID))
        //    {
        //        //Debug.Log("Global ability list has ability with uniqueid " + nUniqueID);
        //        return AbilitiesInPlay[nUniqueID];
        //    }
        //    else
        //    {

        //        Debug.LogError("Global ability list does not contain ability with uniqueid " + nUniqueID);
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Returns the ability lying in the given space in the 
        ///// Global Dictionary of active abilities.
        ///// </summary>
        ///// <param name="nUniqueID"></param>
        ///// <returns></returns>
        //public static FEN_Ability GetBySourceID(DataString dat)
        //{
        //    int nUniqueID = dat.GetInt(GameIDs.ValueSourceID);
        //    return GetByUniqueID(nUniqueID);
        //}

        ///// <summary>
        ///// Returns the ability lying in the given space in the 
        ///// Global Dictionary of active abilities.
        ///// Gets the UniqueID from the DataString's 
        ///// GameIDs.ValueUniqueID entry.
        ///// </summary>
        ///// <param name="dat"></param>
        ///// <returns></returns>
        //public static FEN_Ability GetByUniqueID(DataString dat)
        //{
        //    int nUniqueID = dat.GetInt(GameIDs.ValueUniqueID);
        //    FEN_Ability ab = GetByUniqueID(nUniqueID);

        //    if (ab == null)
        //    {
        //        nUniqueID = dat.GetInt(GameIDs.ValueAbilityUniqueID);
        //        ab = GetByUniqueID(nUniqueID);
        //    }
        //    return ab;
        //}
        //#endregion

    }
}

