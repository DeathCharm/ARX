using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX;
using UnityEngine;

namespace ARX
{
    public class Sort_Bubble_Alphebetize : ARX_Algorithm_BubbleSort<string>
    {
        public override bool I_IsHigherPriority(string movingItem, string listedItem)
        {
            if (movingItem == listedItem)
                return false;

            string a1 = movingItem;
            string a2 = listedItem;


            for(int i = 0; ; i++)
            {
                //If i is greater than a1's length
                //return true
                if (i >= a1.Length)
                    return true;

                //If i is greater than a2's length
                //return false
                if (i >= a2.Length)
                    return false;

                //If a1[i] and a2[i] are the same character, 
                //continue
                if (a1[i] == a2[i])
                    continue;

                //Else, return a1[i] < a2[i]
                return (int)a1[i] < (int)a2[i];
            }
        }

    }
}
