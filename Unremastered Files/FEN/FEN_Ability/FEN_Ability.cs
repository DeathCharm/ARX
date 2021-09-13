using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;
using UnityEngine;
using FEN;
using UnityEngine.UI;

namespace FEN
{
    [CreateAssetMenu(menuName = "FEN/Ability")]
    public partial class FEN_Ability : ARX_Actor
    {

        #region Variables
        /// <summary>
        /// If this ability has an instantiated GameObject, such as a card ability
        /// having a card gameobject, it will be kept here.
        /// </summary>
        [NonSerialized]
        GameObject mo_gameObject = null;
        
        [SerializeField]
        public FEN.CardIDs.CARDID me_abilityID = CardIDs.CARDID.NULL;


        /// <summary>
        /// The shown name of the card.
        /// </summary>
        [SerializeField]
        public string mstr_cardName = "Unnamed Card";

        [SerializeField]
        public Sprite mo_iconSprite = null;

        [NonSerialized]
        public FEN_AbilityProcess mo_abilityProcess = null;

        /// <summary>
        /// Amusingly snarky anecdotes about this ability. Not helpful, but entertaining.
        /// </summary>
        [TextArea(2, 14)]
        public string mstr_flavor = "This has an as of yet unknown history.";


        /// <summary>
        /// Icon Tooltip information
        /// </summary>
        [TextArea(2, 4)]
        public string mstr_iconTooltip = "Update Icon Tooltip Text";


        /// <summary>
        /// Card Tooltip information
        /// </summary>
        [TextArea(2, 4)]
        public string mstr_cardTooltip = "Update Card Tooltip Text!";



        /// <summary>
        /// The ability that this ability is a child of.
        /// If null, this ability has no parent.
        /// </summary>
        protected FEN_Ability mo_parentAbility = null;

        /// <summary>
        /// The unit owning this ability
        /// </summary>
        FEN_Unit mo_owner = null;

        /// <summary>
        /// A list of nested abilities this ability owns.
        /// </summary>
        [HideInInspector]
        public FEN_Ability[] mo_childAbilities;

        /// <summary>
        /// List of attributes this ability has
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public List<string> mastr_attributes = new List<string>();

        /// <summary>
        /// A list of the costs needed to activate or purchase this ability.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public ARX_StatBox_Quad mo_costs;

        [NonSerialized]
        [HideInInspector]
        public bool isDictionaryAbility = false;

        /// <summary>
        /// A list of the costs needed to activate or purchase this ability.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public ARX_StatBox_Quad mo_stats;

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
        #endregion

        #region Accessors

        public GameObject AbilityGameObject
        {
            get
            {
                if (IsChild)
                    return mo_parentAbility.AbilityGameObject;
                else
                    return mo_gameObject;
            }
            set
            {
                if (IsChild)
                    mo_parentAbility.AbilityGameObject = value;
                else
                    mo_gameObject = value;
            }
        }


        public string nameAndID { get { return name + " " + UniqueID; } }


        public FEN_Unit Owner
        {
            get
            {

                if (ParentAbility != null)
                    return ParentAbility.Owner;
                return mo_owner;
            }
        }



        public string CardName
        {
            get
            {
                return mstr_cardName;
            }
        }


        /// <summary>
        /// A description of this ability.
        /// </summary>
        public string Description
        {
            get
            {
                if (mo_abilityProcess != null)
                    return mo_abilityProcess.GetDescription();
                else
                    return "Update Description!";
            }
        }
        #endregion

    }
}

