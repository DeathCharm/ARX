using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ARX
{
    /// <summary>
    /// A timer class that runs an function every time the timer ticks down.
    /// The function can be added 
    /// </summary>
    public class ARX_ActionTimer : ARX_Process
    {
        #region Constructor
        public ARX_ActionTimer(string strName, float delay) : base(strName)
        {
            mnf_delay = delay;
            mo_timer = new UnityTimer(delay);
            Main.AddConstantProcess(this);
        }
        #endregion

        #region Variables

        /// <summary>
        /// The Unity Timer
        /// </summary>
        UnityTimer mo_timer;

        /// <summary>
        /// The amount of time elepased between each activation of the list of timed actions.
        /// </summary>
        public float mnf_delay = 1F;


        /// <summary>
        /// The list of functions to be executed.
        /// </summary>
        public List<VoidDelegate> moa_delegates = new List<VoidDelegate>();
        #endregion

        #region Functions

        /// <summary>
        /// Removes all delegates from the lsit of timed actions
        /// </summary>
        public void Clear()
        {
            moa_delegates.Clear();
        }
        
        /// <summary>
        /// Adds the given delegate to the list of timed actions
        /// </summary>
        /// <param name="fDel"></param>
        public void AddAction(VoidDelegate fDel, float nfDelay = -1)
        {
            if (nfDelay > 0)
                mnf_delay = nfDelay;

            //If there are no queued actions
            //set the action to occur immediately
            if (moa_delegates.Count == 0)
            {
                mo_timer.Start(mnf_delay);
                //mo_timer.mnf_timeElapsed = mnf_delay;
            }
            moa_delegates.Add(fDel);
            
        }

        /// <summary>
        /// Removes the given delegate from the list of timed actions.
        /// </summary>
        /// <param name="fDel"></param>
        public void RemoveAction(VoidDelegate fDel)
        {
            moa_delegates.Remove(fDel);
        }

        /// <summary>
        /// Ticks down the timer. When the timer finishes, runs all held timed functions,
        /// the restarts the timer.
        /// </summary>
        public override void OnUpdate()
        {
            //If there are no actions to run, return
            if (moa_delegates.Count == 0)
                return;

            //Run the timer. 

            mo_timer.Tick();

            //On finish, run the next delegate and remove it from the queue
            if (mo_timer.IsFinished)
            {
                if(moa_delegates[0] != null)
                    moa_delegates[0]();

                moa_delegates.Remove(moa_delegates[0]);

                //If there is still delegates in the queue,
                //start the timer again.
                if (moa_delegates.Count > 0)
                    mo_timer.Start(mnf_delay);
                else
                {
                    mo_timer.Reset();
                }
            }
        }

        #endregion

    }
}
