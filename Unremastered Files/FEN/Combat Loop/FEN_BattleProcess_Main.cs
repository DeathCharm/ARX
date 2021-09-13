using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FEN;
using ARX;

namespace FEN
{
    public class FEN_BattleProcess_Main : FEN_BattleProcess
    {
        public FEN_BattleProcess_Main() : base("battleprocess_main")
        {

        }

        /// <summary>
        /// Creates the subprocesses used by the battle engine, such as Referee's, 
        /// Native Rulesets, etc
        /// </summary>
        void InitializeSubProcesses()
        {
            
        }

        public override void OnFirstUpdate()
        {
            Debug.Log("Beginning Battle Process");
            InitializeSubProcesses();
        }
    }
}
