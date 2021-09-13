using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using UnityEngine;


namespace ARX
{
    /// <summary>
    /// Helper class simulates a weighted-chance raffle.
    /// </summary>
    public class RaffleList
    {
        public class Raffle
        {
            bool mb_isWinningTicket = false;
            public void SetTicketWin(bool bWon)
            {
                mb_isWinningTicket = bWon;
            }

            public bool IsWinningTicket
            {
                get
                {
                    return mb_isWinningTicket;
                }
            }
            public int mn_weight = 1;
            public static Raffle CreateRaffle(int nWeight)
            {
                Raffle raffle = new Raffle(nWeight);
                return raffle;
            }
            private Raffle(int nWeight)
            {
                mn_weight = nWeight;
            }
        }

        List<Raffle> moa_raffles = new List<Raffle>();
        


        public int GetTotalWeight
        {
            get
            {
                int i = 0;
                foreach (Raffle raffle in moa_raffles)
                    i += raffle.mn_weight;

                return i;
            }
        }

        public float GetChance(int nIndex)
        {
            if (nIndex < 0 || nIndex >= moa_raffles.Count)
                return -1;

            return ((float)moa_raffles[nIndex].mn_weight / (float)GetTotalWeight) * 100;
        }

        public int GetWinningIndex
        {
            get
            {
                if (moa_raffles.Count == 0)
                {
                    Debug.LogError("Raffle List had no entries. Returning null value of -1");
                    return -1;
                }

                int nTotalWeight = GetTotalWeight;

                int nWinningWeight = UnityEngine.Random.Range(0, nTotalWeight);
                int nBufMax = 0, nBufMin = 0;
                int nWinner = -1;

                for (int i = 0; i < moa_raffles.Count; i++)
                {
                    moa_raffles[i].SetTicketWin(false);

                    nBufMax += moa_raffles[i].mn_weight;

                    if (nWinningWeight >= nBufMin && nWinningWeight <= nBufMax - 1)
                    {
                        moa_raffles[i].SetTicketWin(true);
                        nWinner = i;
                    }
                    nBufMin = nBufMax;
                }

                return nWinner;
            }
        }

        public void AddRaffle(int nWeight)
        {
            Raffle oRaffle = Raffle.CreateRaffle(nWeight);
            moa_raffles.Add(oRaffle);
        }

        public int GetWeightedIndex(int nWeight)
        {
            int nTotal = 0;

            int i = 0;
            foreach (Raffle raffle in moa_raffles)
            {
                nTotal += raffle.mn_weight;
                if(nTotal >= nWeight)
                    return i;

                i++;
            }

            return i - 1;
        }

    }

}