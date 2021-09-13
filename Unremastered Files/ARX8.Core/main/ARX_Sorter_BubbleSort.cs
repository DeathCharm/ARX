using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ARX
{
    /// <summary>
    /// Basic bubble Sort algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ARX_Algorithm_BubbleSort<T>
    {
        /// <summary>
        /// Returns true if item one should be placed closer to the front of the list
        /// than item two.
        /// </summary>
        /// <param name="movingItem"></param>
        /// <param name="listedItem"></param>
        /// <returns></returns>
        public abstract bool I_IsHigherPriority(T movingItem, T listedItem);

        /// <summary>
        /// Places oItem into List[nIndex] and moves the item there to
        /// List[nIndex + 1]
        /// </summary>
        /// <param name="oItem"></param>
        /// <param name="oList"></param>
        /// <param name="nIndex"></param>
        public void SwitchPlaces(T oItem, List<T> oList, int nIndex)
        {
            T Buf = oList[nIndex];
            oList[nIndex] = oItem;
            oList[nIndex + 1] = Buf;
        }

        public virtual List<T> Sort(List<T> oList)
        {
            List<T> oBuf = new List<T>();
            foreach (T t in oList)
            {
                SortInto(t, oBuf);
            }

            return oBuf;
        }

        public virtual int SortInto(T oNewItem, List<T> oReturnList, int nIndex = int.MinValue)
        {
            //If the index is NullInt, this is the first recursion of the sorting.
            //Set the index to the very end of the list to be sorted into.
            if (nIndex == int.MinValue)
            {
                nIndex = oReturnList.Count - 1;
                oReturnList.Add(oNewItem);
            }

            //If the index is negative one, the sorting has reached the end of the
            //list. Return.
            if (nIndex == -1)
            {
                return 0;
            }

            if (I_IsHigherPriority(oNewItem, oReturnList[nIndex]))
            {
                SwitchPlaces(oNewItem, oReturnList, nIndex);
                return SortInto(oNewItem, oReturnList, nIndex - 1);
            }

            return 0;
        }

    }
    
    public class Algorithm_BubbleSort_EventEntry_StringVer : ARX_Algorithm_BubbleSort<ARX_EventHandlerEntry>
    {
        ARX_EventOrder mo_eventOrder;

        public Algorithm_BubbleSort_EventEntry_StringVer(ARX_EventOrder order)
        {
            mo_eventOrder = order;
        }

        public int GetPosition(ARX_EventHandlerEntry entry)
        {
            for (int i = 0; i < mo_eventOrder.moa_processNames.Count; i++)
            {
                if (entry.mstr_processName == mo_eventOrder.moa_processNames[i])
                    return i;
            }
            return -1;
        }
        
        public override bool I_IsHigherPriority(ARX_EventHandlerEntry one, ARX_EventHandlerEntry two)
        {
            if (mo_eventOrder.moa_processNames.Contains(one.mstr_processName) &&
                !mo_eventOrder.moa_processNames.Contains(two.mstr_processName))
                return true;

            int nEventOnePosition = GetPosition(one);
            int nEventTwoPosition = GetPosition(two);

            //If event one is not found within the event ordering list
            if (nEventOnePosition == -1)
                return false;

            return nEventOnePosition < nEventTwoPosition;
        }
    }

}
