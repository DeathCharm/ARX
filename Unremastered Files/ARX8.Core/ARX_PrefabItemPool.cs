using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ARX;

namespace ARX
{
    /// <summary>
    /// A Glyph-like structure containing a list of Prefabs or 
    /// </summary>
    [CreateAssetMenu(menuName = "ARX/Prefab Pool")]
    public class ARX_PrefabItemPool : ScriptableObject
    {
        #region Nested Item Class
        [Serializable]
        public class Item
        {
            /// <summary>
            /// The display name of this item.
            /// </summary>
            public string name = "New Item";

            /// <summary>
            /// The prefab contained with this item.
            /// </summary>
            public GameObject mo_itemGameObject;

            /// <summary>
            /// Instantiates a copy of the mo_item GameObject
            /// </summary>
            public GameObject InstantiateGameObject
            {
                get
                {
                    if (mo_itemGameObject == null)
                        return null;
                    return GameObject.Instantiate(mo_itemGameObject);
                }
            }

            /// <summary>
            /// An item pool.
            /// </summary>
            public ARX_PrefabItemPool mo_itemPool;

            /// <summary>
            /// The weighted chacne for this item to be drawn from the pool
            /// </summary>
            public int mn_weight = 1;
        }
        #endregion

        #region Variables
        /// <summary>
        /// List of GameObject prefab and recursive PrefabItemPool objects. 
        /// Encapsulated in an Item object
        /// </summary>
        [SerializeField]
        private List<Item> moa_items;

        /// <summary>
        /// Accessor for the moa_items list
        /// </summary>
        public List<Item> Items
        {
            get
            {
                if (moa_items == null)
                    moa_items = new List<Item>();
                return moa_items;
            }
        }

        #endregion

        #region Helper

        /// <summary>
        /// Returns the percentage chance of drawing the given item, or -999 if that item is not
        /// held in the list.
        /// </summary>
        /// <param name="oItem"></param>
        /// <returns></returns>
        public float GetDrawChancePercentage(Item oItem)
        {
            int nIndex = -1;


            RaffleList raffle = new RaffleList();
            for (int i = 0; i < Items.Count; i++)
            {
                if (oItem == Items[i])
                    nIndex = i;

                raffle.AddRaffle(Items[i].mn_weight);
            }

            if (nIndex == -1)
                return -1;

            return raffle.GetChance(nIndex);
        }

        #endregion

        #region Draw From Raffle 
        /// <summary>
        /// Draws a random Item containing either a GameObject or a Item Pool
        /// </summary>
        /// <returns></returns>
        public Item DrawItem()
        {
            if (moa_items.Count == 0)
                return null;

            RaffleList raffle = new RaffleList();
            foreach (Item item in moa_items)
                raffle.AddRaffle(item.mn_weight);

            Item i = moa_items[raffle.GetWinningIndex];

            return i;
        }


        /// <summary>
        /// Use Item Pool recursion to draw a random Item. If the Item has a gameObject, return it. Else if the item has an 
        /// Item Pool, recursively call its DrawGameObject function until a gameObject is found.
        /// </summary>
        /// <returns></returns>
        public GameObject DrawGameObject()
        {
            if (moa_items.Count == 0)
                return null;

            RaffleList raffle = new RaffleList();
            foreach (Item item in moa_items)
                raffle.AddRaffle(item.mn_weight);

            Item i = moa_items[raffle.GetWinningIndex];

            if (i.mo_itemGameObject != null)
                return i.InstantiateGameObject;

            if (i.mo_itemGameObject == null && i.mo_itemPool == null)
                return null;

            return i.mo_itemPool.DrawGameObject();
        }
        #endregion

    }
}
