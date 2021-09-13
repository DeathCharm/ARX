using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;
using UnityEngine.SceneManagement;

namespace FEN
{
    public static partial class CombatEncounters
    {
        /// <summary>
        /// The current encounter set to be loaded when the game enters the battle scene
        /// </summary>
        public static FEN_Encounter mo_activeEncounter = null;

        /// <summary>
        /// Called by the Battle Scene script when a combat encounter is finished or canceled.
        /// It sets the active encounter to null.
        /// </summary>
        public static void ClearCombatEncounterInitiation()
        {
            mo_activeEncounter = null;
        }
        
        /// <summary>
        /// Initiates combat using the given encounter asset. Sets the current encounter and changes the scene
        /// to battleScene if needed, then begins the battle process.
        /// </summary>
        /// <param name="encounter"></param>
        public static void InitiateCombat(FEN_Encounter encounter)
        {
            //If a battle is already underway, 

            if (encounter == null)
            {
                Debug.LogError("Invalid FEN_Encounter given so the application is terminating.");
                Application.Quit();
            }

            Debug.Log("Initiating combat encounter " + encounter);

            //Set the active encounter
            mo_activeEncounter = encounter;

            //If not in Battle Scene, load a scene transition
            if (Scenes.IsBattleScene == false)
            {
                //Spawn a scene transition flow chart.
                //The scene transition flowchart will be responsible for changing the 
                //scene to battle.
                GameObject oSceneTransitionFlowchart = GetSceneTransitionFlowchart();

                //If no scene transition flowchart could be spawned, 
                //manually change the scene to "battle"
                if (oSceneTransitionFlowchart == null)
                {
                    Debug.LogError("No scene transition flowchart was found. Manually changing scene to \"battleScene\"");
                    Scenes.LoadBattleScene();
                }
            }
            else
                BeginBattleProcess();
        }

        /// <summary>
        /// Loads the encounter with the given filename, then calls InitiateCombat(FEN_Encounter) to 
        /// begin the combat encounter.
        /// </summary>
        /// <param name="strEncounter"></param>
        public static void InitiateCombat(string strEncounter)
        {
            FEN_Encounter encounter = FEN.Loading.LoadEncounter(strEncounter);
            if (encounter == null)
            {
                Debug.LogError("Could not load FEN_Encounter named " + strEncounter + " so the application is terminating.");
                Application.Quit();
            }

            InitiateCombat(encounter);
        }

        /// <summary>
        /// Loads and returns a scene transition flowchart based on the current floor.
        /// </summary>
        /// <returns></returns>
        public static GameObject GetSceneTransitionFlowchart()
        {
            return FEN.Loading.LoadPrefab("sceneTransition");
        }

        /// <summary>
        /// Called to load the Battle Process
        /// </summary>
        public static void BeginBattleProcess()
        {
            //If a battle process is already in progress, return
            if (ARX.Main.MainProcess.GetProcess<FEN_BattleProcess_Main>() != null)
            {
                Debug.LogError("A Battle is already in progress");
                return;
            }

            //If the active encounter is null, load a test encounter
            if (mo_activeEncounter == null)
                mo_activeEncounter = CombatEncounters.EncounterList.DebugBattle;

            //Load a Battle Process
            ARX.Main.MainProcess.PushToTopProcess(new FEN_BattleProcess_Main());
        }

    }

}

