using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARX
{

    /// <summary>
    /// Delegate function that handles ARX game events.
    /// </summary>
    /// <param name="oEventData"></param>
    public delegate void ARX_EventHandlerDelegate(DataString dat);

    /// <summary>
    /// Static class containing a list of all ARX_Sctors and ARX_Script_Actor's created.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// A list of every Script Actor created.
        /// Script Actors automatically assign themselves to this list on creation
        /// </summary>
        static List<ARX_Script_Actor> moa_scripActors = new List<ARX_Script_Actor>();

        /// <summary>
        /// A list of every Actor created.
        /// Actors automatically assign themselves to this list on creation
        /// </summary>
        static List<ARX_Actor> moa_actors = new List<ARX_Actor>();

        #region Actors
        public static ARX_Actor GetActor(int nUniqueID)
        {
            foreach (ARX_Actor actor in moa_actors)
                if (actor.UniqueID == nUniqueID)
                    return actor;
            return null;
        }

        public static void RemoveActor(ARX_Actor oActor)
        {
            moa_actors.Remove(oActor);
        }

        public static void AddActor(ARX_Actor oActor)
        {
            //Initializes the actor's unique id if not already initialized
            int n = oActor.UniqueID;

            //Assert that the given actor is null
            Debug.Assert((oActor != null), "Actor given to ARX_Global.AddActor() was null.");

            //Search the database for the given actor
            bool bFound = moa_actors.Contains(oActor);

            if (bFound)
                return;
            
            //Add the actor
            moa_actors.Add(oActor);

        }
        #endregion

        #region Script Actors
        public static ARX_Script_Actor GetScriptActor(int nUniqueID)
        {
            foreach (ARX_Script_Actor actor in moa_scripActors)
                if (actor.UniqueID == nUniqueID)
                    return actor;
            return null;
        }
        
        public static void RemoveActor(ARX_Script_Actor oActor)
        {
            moa_scripActors.Remove(oActor);
        }

        public static void AddActor(ARX_Script_Actor oActor)
        {
            //Initializes the actor's unique id if not already initialized
            int n = oActor.UniqueID;

            //Assert that the given actor is null
            Debug.Assert((oActor != null), "Actor given to ARX_Global.AddActor() was null.");

            //Search the database for the given actor
            bool bFound = moa_scripActors.Contains(oActor);

            if (bFound)
                return;
            

            //Add the actor
            moa_scripActors.Add(oActor);

        }
        #endregion
    }
}