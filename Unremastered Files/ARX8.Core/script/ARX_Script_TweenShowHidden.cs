using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;


namespace ARX
{
    /// <summary>
    /// Tweens this object's saved position named "visible"
    /// This script is awaiting reworking.
    /// </summary>
    public class ARX_Script_TweenShowHidden : ARX_Script_Actor
    {
        public ARX_Script_SavedPositions mo_savedPositions;
        public float mnf_alpha = 0.8F;
        public float mnf_delay = 0.0F;
        public float mnf_time = 2.0F;
        public bool mb_fade = true;
        public bool mb_isHidden = true;

        [HideInInspector]
        public string mstr_visibleTag = "visible";

        void JumpIn()
        {
            if(mo_savedPositions)
                iTween.MoveTo(gameObject, mo_savedPositions.GetVectorPosition(mstr_visibleTag), mnf_time, mnf_delay);

            if(mb_fade)
            iTween.FadeTo(gameObject, mnf_alpha, mnf_time, mnf_delay, NameAndID);
            mb_isHidden = false;
        }

        void JumpOut()
        {
            if (mo_savedPositions)
                iTween.MoveTo(gameObject, mo_savedPositions.GetVectorPosition("hidden"), mnf_time, mnf_delay);
            if (mb_fade)
                iTween.FadeTo(gameObject, 0, mnf_time, 0, NameAndID);
            mb_isHidden = true;

        }

        public override void V_OnInitialize()
        {
            if (mo_savedPositions == null)
                mo_savedPositions = GetComponent<ARX_Script_SavedPositions>();
            base.V_OnInitialize();
            if (mb_isHidden)
                JumpIn();
            else
                JumpOut();
        }

        public override void V_OnEnable()
        {
            mb_isHidden = !mb_isHidden;
            if (mb_isHidden)
                JumpOut();
            else
                JumpIn();
        }

        public override void V_OnDisable()
        {
            JumpOut();
        }
    }
}
