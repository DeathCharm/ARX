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
    /// <summary>
    /// A basic WinLoss condition that returns a loss if all allies are dead or a win if all enemies are dead (in that order)
    /// </summary>
    public class FEN_BattleProcess_WinLoss_Basic : FEN_BattleProcess_WinLoss
    {
        public FEN_BattleProcess_WinLoss_Basic():base("battleprocess_winloss_basic")
        {

        }

        #region Abstract Overrides
        public override void I_LossTerminate()
        {
            Debug.Log("You Lost...");
        }

        public override BATTLEWINLOSS I_WinLossCondition()
        {
            if (Party.GetLivingUnits(Party.ALIGNMENT.ALLIED).Count == 0)
                return BATTLEWINLOSS.LOSS;
            if (Party.GetLivingUnits(Party.ALIGNMENT.ENEMY).Count == 0)
                return BATTLEWINLOSS.WIN;

            return BATTLEWINLOSS.CONTINUE;
        }

        public override void I_WinTerminate()
        {
            Debug.Log("You won!");
        }
        #endregion
    }
}
