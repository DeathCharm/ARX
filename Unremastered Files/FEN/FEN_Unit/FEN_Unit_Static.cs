using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEN;
using ARX;
using UnityEngine;

namespace FEN
{
    public partial class FEN_Unit : ARX_Actor
    {


        /// <summary>
        /// Returns the active unit with the given uniqueID
        /// </summary>
        /// <param name="nUniqueID"></param>
        /// <returns></returns>
        public static FEN_Unit GetByUniqueID(int nUniqueID)
        {
            if (RPGElementCollections.UnitsInPlay.ContainsKey(nUniqueID))
                return RPGElementCollections.UnitsInPlay[nUniqueID];
            return null;
        }

        /// <summary>
        /// Returns the active unit with the given uniqueID.
        /// Pulls the uniqueID from the Datastring's 
        /// GameIDs.ValueUniqueID
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public static FEN_Unit GetByUniqueID(DataString dat)
        {
            if (dat == null)
                return null;

            int nTargetUniqueID = dat.GetInt(GameIDs.ValueUniqueID);
            FEN_Unit oTargetedUnit = FEN_Unit.GetByUniqueID(nTargetUniqueID);
            return oTargetedUnit;
        }

        /// <summary>
        /// Returns the active unit with the Source uniqueID.
        /// Pulls the uniqueID from the Datastring's 
        /// GameIDs.ValueSourceID
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public static FEN_Unit GetBySourceID(DataString dat)
        {
            if (dat == null)
                return null;

            int nTargetUniqueID = dat.GetInt(GameIDs.ValueSourceID);
            FEN_Unit oTargetedUnit = FEN_Unit.GetByUniqueID(nTargetUniqueID);
            return oTargetedUnit;
        }

        public static FEN_Unit GetByTargetID(DataString dat)
        {
            if (dat == null)
                return null;

            int nTargetUniqueID = dat.GetInt(IDs.Target);
            FEN_Unit oTargetedUnit = FEN_Unit.GetByUniqueID(nTargetUniqueID);
            return oTargetedUnit;
        }
    }
}