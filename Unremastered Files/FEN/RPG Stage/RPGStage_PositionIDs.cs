using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FEN
{
    public static class RPGStage
    {
        /// <summary>
        /// Describes relative RPGSTage positions.
        /// </summary>
        public enum RelativePositions { AttackingUnit = 0, StruckUnit }

        public static bool IsValidStagePosition(string str)
        {
            //Check relative positions
            foreach (System.Enum e in System.Enum.GetValues(typeof(RelativePositions)))
            {
                if (e.ToString() == str)
                    return true;
            }

            //Check the RPG Stage Instance
            if(ARX_Script_RPGStage.Instance != null)
            {
                //If the stage has an instance of the 
                bool bHasPosition = ARX_Script_RPGStage.Instance.GetStageAnchorByName(str) != null;
                if(bHasPosition)
                {
                    return true;
                }
            }

            Debug.Log("There are no stage positions name " + str);
            return false;
        }
    }


}
