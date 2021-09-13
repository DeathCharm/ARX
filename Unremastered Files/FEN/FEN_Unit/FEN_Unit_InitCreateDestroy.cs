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

        /// <summary>
        /// Private constructor. Use ScriptableObject.CreateInstance<> to create an instance
        /// </summary>
        private FEN_Unit() { }

        public void Initialize()
        {

            Debug.LogError("Unit " + nameAndID + " is initializing.");
            ARX.GameEvents.onStatAmountChange.Subscribe(ReactToStatChange, name, EventRecord);
            ARX.GameEvents.onSendAttackDamageToUnit.Subscribe(ReactToBeingAttacked, name, EventRecord);
            ARX.GameEvents.onTriggerAttackAnimation.Subscribe(ReactToTriggerAttackAnimation, name, EventRecord);
            ARX.GameEvents.onSendHealToUnit.Subscribe(ReactToBeingHealed, name, EventRecord);
            ARX.GameEvents.onPlayerTurnStart.Subscribe(ReactToPlayerTurnStart, name, EventRecord);
            ARX.GameEvents.onPlayerTurnEnd.Subscribe(ReactToPlayerTurnEnd, name, EventRecord);
            ARX.GameEvents.onGameEnd.Subscribe(ReactToGameEnd, name, EventRecord);
            GameEvents.onEndBattle.Subscribe(ReactToBattleEnd, name, EventRecord);

            ARX.GameEvents.onUnitInitialized.FireEvent(GetMessage);

            foreach (FEN_Ability ab in ma_initialAbilities)
            {
                if (ab == null)
                    continue;

                int nValue = ab.Stats.GetStat(IDs.Amount).MaxInt;
                FEN_Ability oNewAbility = AddValueToStatus(ab.me_abilityID, nValue, STATPLACEMENT.BASEANDCURRENT);


                string str = "Initialized unit " + nameAndID + " with ability " + oNewAbility.nameAndID;
                str += " It now belongs to " + oNewAbility.Owner.nameAndID;
                Debug.Log(str);
            }

        }

        private void Awake()
        {

            RPGElementCollections.UnitsInPlay[UniqueID] = this;
            
        }

        void ReactToGameEnd(DataString dat)
        {
            Debug.Log(name + " unit is reacting to game end and destroying self.");
            Destroy();
        }

        void ReactToBattleEnd(DataString dat)
        {
            foreach (ARX_StatQuad quad in Stats.AsList)
                quad.Bonus = 0;
        }

        public void Unload()
        {

            EventRecord.UnsubscribeFromAllEvents();
            if (Application.isPlaying == false)
                return;

            RPGElementCollections.UnitsInPlay.Remove(UniqueID);
            

        }


 public void OnDestroy()
        {
            //Debug.Log("on Destroy unit " + NameAndID);
            Destroy();
        }
        public void Destroy()
        {

            //Debug.Log("Unload Destroy unit " + NameAndID);

            if (moa_abilityList != null)
            {
                foreach (FEN_Ability ab in moa_abilityList.AsList)
                    if (ab != null)
                        ab.Destroy();

                moa_abilityList.Clear();
            }

            Unload();
            GameObject.DestroyImmediate(mo_sprite);
            mo_sprite = null;

        }
        
        
        public FEN_Unit Clone
        {
            get
            {
                FEN_Unit other = Instantiate<FEN_Unit>(this);
                
                other.SetAsUniqueInstance();
                
                other.moa_abilityList = ScriptableObject.CreateInstance<FEN_AbilityList>();

                other.Stats = other.Stats.Clone;
                
                return other;
            }
        }
        

        ///// <summary>
        ///// Returns a debugging ready unit.
        ///// </summary>
        //public static FEN_Unit GetTestPlayer
        //{
        //    get
        //    {
        //        //FEN_Unit unit = ScriptableObject.CreateInstance<FEN_Unit>();

        //        ////Name
        //        //unit.name = "Player ";

        //        ////Set Test Stat
        //        //SetTestStat(unit.Stat_Health, 80);
        //        //SetTestStat(unit.Stat_Corruption, 0);
        //        //SetTestStat(unit.Stat_Agility, 4);
        //        //SetTestStat(unit.Stat_Dexterity, 5);
        //        //SetTestStat(unit.Stat_Strength, 4);
        //        //SetTestStat(unit.Stat_Intelligence, 7);
        //        //SetTestStat(unit.Stat_Endurance, 3);


        //        //unit.LoadModel("hero");
        //        //unit.IsPlayer = true;

        //        //return unit;
                
        //        FEN_Unit unit = FEN.Loading.LoadUnit("hero");
        //        unit.IsPlayer = true;
                
        //        return unit;
        //    }
        //}

        ///// <summary>
        ///// Returns a debugging ready unit.
        ///// </summary>
        //public static FEN_Unit GetTestAlly
        //{
        //    get
        //    {
        //        FEN_Unit unit = FEN.Loading.LoadUnit("ally");
        //        unit.IsPlayer = false;
        //        unit.AddAbility(FEN_Ability.SpawnNewCard(FEN.CardIDs.CARDID.AL_HIT));
        //        return unit;
        //    }
        //}

        static void SetTestStat(ARX_StatQuad quad)
        {
            quad.Base = 30;
            quad.Current = 20;
        }

        static void SetTestStat(ARX_StatQuad quad, int nBase, int nCurrent)
        {
            quad.Base = nBase;
            quad.Current = nCurrent;
        }

        static void SetTestStat(ARX_StatQuad quad, int nBase)
        {
            quad.Base = nBase;
            quad.Current = nBase;
        }

        public static FEN_Unit Deserialize(string str)
        {
            return ARX_File.DeserializeObject<FEN_Unit>(str);
        }

    }
}