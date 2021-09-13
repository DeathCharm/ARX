using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ARX
{
    /// <summary>
    /// Holds a container of item T. For each item T, determines if the item should be 
    /// drawn to a UI element and carries out the drawing function if needed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ARX_IconDrawer<T>
    {
        public List<T> mo_container;
        GameObject mo_root;

        public ARX_IconDrawer(GameObject oRoot, List<T> oContainer)
        {
            mo_root = oRoot;
            mo_container = oContainer;

            if (mo_container == null)
                mo_container = new List<T>();
        }

        public abstract bool ShouldItemBeDrawn(T item);
        public abstract void PrepareItemToBeShown(T item, GameObject oGameObject);
        public virtual void ShowItem(T item, GameObject oGameObject)
        {
            oGameObject.SetActive(true);
        }

        public virtual void HideItem(T item, GameObject oGameObject)
        {
            oGameObject.SetActive(false);
        }
        
        public void DrawItems()
        {
            for (int nChildCount = 0,
                nItemCount = 0; nChildCount < mo_root.transform.childCount ; nChildCount++)
            {
                GameObject oGameObject = mo_root.transform.GetChild(nChildCount).gameObject;
                if (nItemCount >= mo_container.Count)
                {
                    HideItem(default(T), oGameObject);
                    continue;
                }

                T oItem = mo_container[nItemCount];

                if (ShouldItemBeDrawn(oItem))
                {
                    PrepareItemToBeShown(oItem, oGameObject);
                    nItemCount++;
                    ShowItem(oItem, oGameObject);
                    continue;
                }
                else
                {
                    HideItem(oItem, oGameObject);
                }

            }
        }

    }
}
