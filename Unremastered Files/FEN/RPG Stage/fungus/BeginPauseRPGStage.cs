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
                 "Begin Pause ARX Process",
                 "Spawns a new object based on a reference to a scene or prefab game object.",
        Priority = 10)]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class BeginPauseARXProcess : Command
    {
        [Tooltip("Local position of newly spawned object.")]
        [SerializeField] protected StringData _processName = new StringData("");
        
        #region Public Members

        public override void OnEnter()
        {

            //If there is a battle process in existence, grab it and add a Pause Process to it.

            FEN_BattleProcess_Main battleProcess = ARX_Process.GetProcessInMain<FEN_BattleProcess_Main>();
            if (battleProcess != null)
            {
                battleProcess.PushToTopProcess(new FEN_PauseRPGStage(_processName.stringVal));
            }
            //Else, add the pause process to 
            else
            {
                Main.MainProcess.PushToTopProcess(new FEN_PauseRPGStage(_processName.stringVal));
            }
            
            //GameEvents.OnRPGStagePause.FireEvent();
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