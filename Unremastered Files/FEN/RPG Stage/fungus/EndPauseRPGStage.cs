using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using FEN;
using ARX;

namespace Fungus
{
    /// <summary>
    /// Spawns a new object based on a reference to a scene or prefab game object.
    /// </summary>
    [CommandInfo("FEN",
                 "End Pause ARX Process",
                 "Ends the pause of ARX Processes",
        Priority = 10)]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class EndPauseARXProcess : Command
    {
        [Tooltip("Local position of newly spawned object.")]
        [SerializeField] protected StringData _processName = new StringData("");

        #region Public Members

        public override void OnEnter()
        {

            //If there is a battle process in existence, grab it and add a Pause Process to it.
            FEN_BattleProcess_Main battleProcess = ARX_Process.GetProcessInMain<FEN_BattleProcess_Main>();
            ARX_Process[] pauseProcesses = null;

            if (battleProcess != null)
            {
                pauseProcesses = battleProcess.GetProcesses(typeof(FEN_PauseRPGStage));
            }
            //Else, add the pause process to 
            else
            {
                pauseProcesses = Main.MainProcess.GetProcesses(typeof(FEN_PauseRPGStage));
            }

            if (pauseProcesses != null)
                foreach (FEN_PauseRPGStage p in pauseProcesses)
                {
                    //If process name has a value, only termnate processes with a process name identical to it
                    if (_processName.stringVal != "")
                    {
                        if (p.name == _processName.stringVal)
                        {
                            p.Terminate();
                        }
                    }
                    //Else if there is no particular process this is looking for, terminate all
                    else
                        p.Terminate();

                }

            //GameEvents.OnRPGStageUnpause.FireEvent();
            Continue();

        }

        public override string GetSummary()
        {

            return "Begin Pause RPG Stage";
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            return true;
        }

        #endregion

    }
}
