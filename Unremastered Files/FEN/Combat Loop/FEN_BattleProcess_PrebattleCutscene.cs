using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;
using FEN;
using Fungus;
using UnityEngine;

namespace FEN
{
    public class FEN_BattleProcess_Cutscene : FEN_BattleProcess
    {
        
        public enum BATTLESTATUS {PREBATTLE, DURINGBATTLE, POSTBATTLE }

        /// <summary>
        /// Determines which flowchart this process will use
        /// </summary>
        BATTLESTATUS me_battleStatus = BATTLESTATUS.PREBATTLE;
        Flowchart mo_midBattleFlowchart = null;

        public FEN_BattleProcess_Cutscene(BATTLESTATUS eStatus):base("battleprocess_prebattlecutscene")
        {
            me_battleStatus = eStatus;
            if(eStatus == BATTLESTATUS.DURINGBATTLE)
            {
                Debug.LogError("Midbattle cutscenes are only valid if initialized with a flowchart. Terminating midbattle cutscene process.");
                Terminate();
            }
        }

        public FEN_BattleProcess_Cutscene(Flowchart oMidBattleFlowchart) : base("battleprocess_prebattlecutscene")
        {
            me_battleStatus = BATTLESTATUS.DURINGBATTLE;
            mo_midBattleFlowchart = oMidBattleFlowchart;
        }

        public override void OnInitialized()
        {
            GameEvents.onBattleCutsceneEnd.Subscribe(ReactToBattleCutsceneEnd, this);
            InitiateCutscene();
            base.OnInitialized();
        }

        /// <summary>
        /// Returns the flowchart this process will use.
        /// Prebattle and post battle will return the current encounter's Pre or Post flowchart.
        /// Midbattle will return the flowchart given by this processes constructor
        /// </summary>
        /// <returns></returns>
        Flowchart GetTargetFlowchart()
        {
            switch(me_battleStatus)
            {
                case BATTLESTATUS.DURINGBATTLE:
                    return mo_midBattleFlowchart;
                case BATTLESTATUS.POSTBATTLE:
                    return CombatEncounters.mo_activeEncounter.PostBattleFlowchart;
                case BATTLESTATUS.PREBATTLE:
                    return CombatEncounters.mo_activeEncounter.PreBattleFlowchart;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Runs the current encounter's battle cutscene if it has one.
        /// Else, terminates this process.
        /// </summary>
        void InitiateCutscene()
        {
            Flowchart oTargetFlowchart = GetTargetFlowchart();

            if(oTargetFlowchart != null)
            {
                Block oStartBlock = oTargetFlowchart.FindBlock("Start");

                //If there is no block named Start in the active encounter's prebattle flowchart
                if(oStartBlock == null)
                {
                    Debug.LogError("There is no Start block in the prebattle flowchart of " + oTargetFlowchart.name);
                    Terminate();
                    return;
                }

                oTargetFlowchart.ExecuteBlock("Start");
            }
            else
            {
                Terminate();
            }
        }

        void ReactToBattleCutsceneEnd(DataString dat)
        {
            Terminate();
        }
    }
}
