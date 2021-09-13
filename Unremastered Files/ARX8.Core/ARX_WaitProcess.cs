using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;

namespace ARX
{
    /// <summary>
    /// ARX_Process that waits for a set time, then Terminates.
    /// </summary>
    public class ARX_WaitProcess : ARX_Process
    {
        #region Variables
        /// <summary>
        /// This object's timer
        /// </summary>
        UnityTimer mo_timer = new UnityTimer();

        /// <summary>
        /// The array of function delegates to run
        /// </summary>
        VoidDelegate[] moafunc_delegates;

        /// <summary>
        /// If this process is terminated early, does it run its given function delegates?
        /// </summary>
        public bool mb_executeDelegateWhenTerminatedEarly = false;

        /// <summary>
        /// Has this object's timer finished/functions been ran?
        /// </summary>
        bool mb_functionRanOrTimerFinished = false;
        #endregion

        public ARX_WaitProcess(float nSeconds, bool bExecuteDelegateWhenTerminatedEarly, VoidDelegate[] afuncCallbacks = null) : base("wait")
        {
            mo_timer = new UnityTimer(nSeconds);
            moafunc_delegates = afuncCallbacks;
            mb_executeDelegateWhenTerminatedEarly = bExecuteDelegateWhenTerminatedEarly;
        }

        void RunFunctions()
        {
            foreach (VoidDelegate del in moafunc_delegates)
                if (del != null)
                    del();
            mb_functionRanOrTimerFinished = true;
        }

        public override void OnTerminated()
        {
            //If the function has not been ran and the function is to be ran when terminated early,
            //Run the function
            if (mb_functionRanOrTimerFinished == false && mb_executeDelegateWhenTerminatedEarly)
            {
                if (moafunc_delegates != null)
                {
                    RunFunctions();
                }
            }
        }

        /// <summary>
        /// Ticks the timer down. When the timer is finished,
        /// run function delegates and terminate.
        /// </summary>
        public override void OnUpdate()
        {
            mo_timer.Tick();
            if (mo_timer.IsFinished)
            {
                if (moafunc_delegates != null)
                {
                    RunFunctions();
                }
                mb_functionRanOrTimerFinished = true;
                Terminate();
            }
        }

    }
}
