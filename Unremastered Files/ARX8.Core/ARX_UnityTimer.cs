using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;


namespace ARX
{
    /// <summary>
    /// A helper object that simulates a timer
    /// </summary>
    public class UnityTimer
    {
        public float mnf_timeToTick;
        public float mnf_timeElapsed;
        protected bool mb_active = false;

        public UnityTimer() { }
        public UnityTimer(float nfTime) { Start(nfTime); }

        public void SkipForward(float mnf_skipForwardTime)
        {
            mnf_timeElapsed += mnf_skipForwardTime;
        }

        public virtual float CompletionRatio
        {
            get
            {
                if (IsFinished)
                    return 1;

                return mnf_timeElapsed / mnf_timeToTick;
            }
        }

        public virtual float Percentage
        {
            get
            {
                if (IsFinished)
                    return 100;

                return mnf_timeElapsed / mnf_timeToTick * 100;
            }
        }

        public virtual float Float
        {
            get
            {
                if (mnf_timeElapsed >= mnf_timeToTick)
                    return 1;

                return mnf_timeElapsed / mnf_timeToTick;
            }
        }

        public bool IsFinished
        {
            get
            {
                if (mnf_timeToTick <= mnf_timeElapsed)
                    return true;
                return false;
            }
        }

        public void Reset()
        {
            mnf_timeElapsed = 0;
        }

        public virtual void Tick()
        {
            if (mb_active)
                mnf_timeElapsed += Time.deltaTime;
        }

        public void Start(float nTimeToCount)
        {
            Reset();
            mnf_timeToTick = nTimeToCount;
            mb_active = true;
        }
    }
}