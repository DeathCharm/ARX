using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEN
{

    /// <summary>
    /// Object created by an Ability Process. Contains variables which determine how its Targeting Process operates.
    /// </summary>
    public class FEN_AbilityTargetingInfo
    {
        /// <summary>
        /// Determines if the ability will activate automatically when its Max Targets is reached.
        /// </summary>
        public bool mb_activateImmediatelyWhenValid = true;
        
        /// <summary>
        /// Determines if this targeting process will activate once clicked
        /// </summary>
        public bool mb_castInstantlyOnClick = false;

        /// <summary>
        /// Determines if the ability can have a targeting process at all.
        /// </summary>
        public bool mb_canBeUsedAsSkill = true;

        /// <summary>
        /// The number of targets, card or unit, the ability can have maximum.
        /// </summary>
        public int mn_maxTargets = 1;

        /// <summary>
        /// The action taken by the targeting process on pressing the Escape key.
        /// </summary>
        public enum ONESCAPE { TERMINATE, CANCEL_SELECTION}
        public ONESCAPE me_escapeAction = ONESCAPE.TERMINATE;

        /// <summary>
        /// The action taken by the targeting process on right clicking a card or unit.
        /// </summary>
        public enum ONRIGHTCLICK { TERMINATE, CANCEL_SELECTION }
        public ONRIGHTCLICK me_rightClickAction = ONRIGHTCLICK.CANCEL_SELECTION;


        public FEN_AbilityTargetingInfo()
        {

        }

        public FEN_AbilityTargetingInfo
            (
            bool bActivateWhenValid = true,
            bool bConfirmBeforeActivation = false, 
            bool bCanBeUsedAsSkill = true,
            int nMaxTargets = 1, 
            ONESCAPE eEscapeAction = ONESCAPE.TERMINATE, 
            ONRIGHTCLICK eRightClickAction = ONRIGHTCLICK.CANCEL_SELECTION
            )
        {
            mb_activateImmediatelyWhenValid = bActivateWhenValid;
            mb_canBeUsedAsSkill = bCanBeUsedAsSkill;
            mn_maxTargets = nMaxTargets;
            me_escapeAction = eEscapeAction;
            me_rightClickAction = eRightClickAction;
        }

    }
}
