using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARX;
using Fungus;

namespace FEN
{
    /// <summary>
    /// Asset representing a combat participant and their stats, statuses and abilities.
    /// </summary>
    [CreateAssetMenu(menuName = "FEN/Unit")]
    public partial class FEN_Unit : ARX_Actor
    {
        /// <summary>
        /// The display name of the unit. Distinct from
        /// name, which is the filename of the unit's asset.
        /// </summary>
        public string mstr_name = "Player";

        /// <summary>
        /// The unit's ID
        /// </summary>
        public CardIDs.UNITID me_unitID;

        /// <summary>
        /// This Unit's name and Unique ID.
        /// </summary>
        public string nameAndID { get { return name + UniqueID; } }

        /// <summary>
        /// This unit's list of abilities
        /// </summary>
        FEN_AbilityList moa_abilityList;

        /// <summary>
        /// Accessor for this unit's list of abilities.
        /// </summary>
        public FEN_AbilityList AbilityList
        {
            get
            {
                if (moa_abilityList == null)
                    moa_abilityList = ScriptableObject.CreateInstance<FEN_AbilityList>();
                return moa_abilityList;

            }
        }

        /// <summary>
        /// The Fungus flowchart responsible for banter.
        /// </summary>
        public GameObject mo_flowChartPrefab;

        private GameObject _flowchart;
        public GameObject FlowChart
        {
            get
            {
                if (mo_flowChartPrefab == null)
                    return null;
                if (_flowchart == null)
                    _flowchart = GameObject.Instantiate<GameObject>(mo_flowChartPrefab);
                return _flowchart;
            }
        }
        
        /// <summary>
        /// This unit's Unity object.
        /// </summary>
        public Sprite mo_sprite = null;

        /// <summary>
        /// Abilities this unit starts each battle with.
        /// </summary>
        public FEN_Ability[] ma_initialAbilities;

        /// <summary>
        /// Is thie unit the Player Character?
        /// </summary>
        public bool IsPlayer = false;


        /// <summary>
        /// Determines if this unit has attacked this turn or not.
        /// For use with ally units, not the Player.
        /// </summary>
        [HideInInspector]
        public bool mb_attackedThisTurn = false;

        /// <summary>
        /// Returns true if this unit's hp is at or less than zero.
        /// </summary>
        public bool IsDead { get { return Stat_Health.Current <= 0; } }

        /// <summary>
        /// List of attributes this ability has
        /// </summary>
        public List<string> mastr_attributes = new List<string>();

        /// <summary>
        /// A List of events used when auto-generating Ability definitions.
        /// </summary>
        [HideInInspector]
        public List<ARX_Event> moa_callBacks;
        public List<ARX_Event> GetCallbacks
        {
            get
            {
                if (moa_callBacks == null)
                    moa_callBacks = new List<ARX_Event>();
                return moa_callBacks;
            }
        }


        /// <summary>
        /// Developer's notes on the unit.
        /// </summary>
        [HideInInspector]
        public string mstr_devNotes;
        
    }




}