using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;

namespace FEN
{
    /// <summary>
    /// Process that checks for a Win/Loss condition to be fulfilled.
    /// </summary>
    public abstract class FEN_BattleProcess_WinLoss : FEN_BattleProcess
    {
        public enum BATTLEWINLOSS {WIN, LOSS, CONTINUE }
        public FEN_BattleProcess_WinLoss(string strProcessName):base(strProcessName)
        {

        }

        #region Abstracts
        public abstract BATTLEWINLOSS I_WinLossCondition();
        public abstract void I_WinTerminate();
        public abstract void I_LossTerminate();
        #endregion

        #region React
        public virtual void ReactToEndBattle(DataString dat)
        {
            Terminate();
        }
        #endregion

        #region ARX Overrides
        public override void OnInitialized()
        {
            GameEvents.onEndBattle.Subscribe(ReactToEndBattle, this);
            base.OnInitialized();
        }

        public override void OnUpdate()
        {
            //Check the Win Loss condition once each frame. If a win or a loss occurs,
            //run the WinTerminate or LossTerminate functions respectively.
            switch(I_WinLossCondition())
            {
                case BATTLEWINLOSS.CONTINUE:
                    break;
                case BATTLEWINLOSS.LOSS:
                    I_LossTerminate();
                    Terminate();
                    break;
                case BATTLEWINLOSS.WIN:
                    I_WinTerminate();
                    Terminate();
                    break;
            }
            base.OnUpdate();
        }
        #endregion
    }
}
