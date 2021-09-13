using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;

namespace FEN
{
    /// <summary>
    /// Battle process creates a copy of the current encounter's units and saves them in the 
    /// Party static class's variables. Also assigns units to slides on the RPG Canvas
    /// </summary>
    public class FEN_BattleProcess_LoadUnits : FEN_BattleProcess
    {
        public FEN_BattleProcess_LoadUnits():base("battleprocess_loadUnits")
        {

        }

        public override void OnInitialized()
        {
            Party.InitializeParty(CombatEncounters.mo_activeEncounter, Party.ALIGNMENT.ENEMY);

            base.OnInitialized();

            //Placeholder termination
            Terminate();
        }
    }
}
