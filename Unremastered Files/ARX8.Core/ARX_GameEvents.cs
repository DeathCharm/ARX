using ARX;
using UnityEngine;

namespace ARX
{

    public static partial class GameEvents
    {

        #region ARX Game Events
        public static ARX_Event onBattleCutsceneEnd;
        public static ARX_Event anotherTestEvent;
        public static ARX_Event soManyTestEvents;
        public static ARX_Event testEvent;
        public static ARX_Event onBlockBroken;
        public static ARX_Event onEndBattle;
        public static ARX_Event OnExecutedAbility;
        public static ARX_Event OnFullyBlockedDamageTaken;
        public static ARX_Event onGameEnd;
        public static ARX_Event onHealthDamageTaken;
        public static ARX_Event onPartiallyBlockedDamageTaken;
        public static ARX_Event onPlayerTurnEnd;
        public static ARX_Event onPlayerTurnStart;
        public static ARX_Event onSendAttackDamageToUnit;
        public static ARX_Event onSendHealToUnit;
        public static ARX_Event onStatAmountChange;
        public static ARX_Event OnTest;
        public static ARX_Event onTriggerAttackAnimation;
        public static ARX_Event onUnitDeath;
        public static ARX_Event onUnitInitialized;
        public static ARX_Event OnRPGStagePause;
        public static ARX_Event OnRPGStageUnpause;
        #endregion

        public static void InstantiateEvent(out ARX_Event oEvent, string strID)
        {
            oEvent = FEN.Loading.LoadEvent(strID);

            if (oEvent == null)
            {
                oEvent = ScriptableObject.CreateInstance<ARX_Event>();
            }
            oEvent.SetLoadStatus();
        }


        public static void Initialize()
        {
            #region Misc. Event
            InstantiateEvent(out anotherTestEvent, nameof(anotherTestEvent));
            InstantiateEvent(out soManyTestEvents, nameof(soManyTestEvents));
            InstantiateEvent(out testEvent, nameof(testEvent));
            InstantiateEvent(out onBlockBroken, nameof(onBlockBroken));
            InstantiateEvent(out onEndBattle, nameof(onEndBattle));
            InstantiateEvent(out OnExecutedAbility, nameof(OnExecutedAbility));
            InstantiateEvent(out OnFullyBlockedDamageTaken, nameof(OnFullyBlockedDamageTaken));
            InstantiateEvent(out onGameEnd, nameof(onGameEnd));
            InstantiateEvent(out onHealthDamageTaken, nameof(onHealthDamageTaken));
            InstantiateEvent(out onPartiallyBlockedDamageTaken, nameof(onPartiallyBlockedDamageTaken));
            InstantiateEvent(out onPlayerTurnEnd, nameof(onPlayerTurnEnd));
            InstantiateEvent(out onPlayerTurnStart, nameof(onPlayerTurnStart));
            InstantiateEvent(out onSendAttackDamageToUnit, nameof(onSendAttackDamageToUnit));
            InstantiateEvent(out onSendHealToUnit, nameof(onSendHealToUnit));
            InstantiateEvent(out onStatAmountChange, nameof(onStatAmountChange));
            InstantiateEvent(out OnTest, nameof(OnTest));
            InstantiateEvent(out onTriggerAttackAnimation, nameof(onTriggerAttackAnimation));
            InstantiateEvent(out onUnitDeath, nameof(onUnitDeath));
            InstantiateEvent(out onUnitInitialized, nameof(onUnitInitialized));
            InstantiateEvent(out OnRPGStagePause, nameof(OnRPGStagePause));
            InstantiateEvent(out OnRPGStageUnpause, nameof(OnRPGStageUnpause));
            #endregion Misc. Event

        }
    }
}