using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace ARX
{
    /// <summary>
    /// Class for encapsulating a single step of an algorithm as an object.
    /// To be added to an ARX_Algorithm object
    /// </summary>
    [Serializable]
    public class ARX_AlgorithmAction
    {
        /// <summary>
        /// Base constructor
        /// </summary>
        public ARX_AlgorithmAction()
        {

        }

        /// <summary>
        /// The name of the action
        /// </summary>
        [SerializeField]
        protected string mstr_name = "New Algorithm Piece";

        /// <summary>
        /// Accessor for the name of the action
        /// </summary>
        public string ActionName { get { return mstr_name; } }

        /// <summary>
        /// Notes on this action.
        /// </summary>
        public string mstr_notes = "";

        /// <summary>
        /// Description of the algorithm action shown in its tooltip
        /// </summary>
        [SerializeField]
        protected string mstr_description = "";

        /// <summary>
        /// A list of soft coded stats held by this algorithm action
        /// </summary>
        public ARX_StatSingleBox mo_stats = new ARX_StatSingleBox();

        /// <summary>
        /// The algorithm this action belongs to
        /// </summary>
        public ARX_Asset_Algorithm mo_owningAlgorithm;

        /// <summary>
        /// The original class type this action was when first instantiated.
        /// This is used to remake this class since Unitys serialization
        /// does not support some instances of polymorphism. Saved as a string
        /// since System.Type is not serializable
        /// </summary>
        [SerializeField]
        private string mstr_classType;
        public string OriginalClassType { get { return mstr_classType; } }

        public Type OriginalType
        {
            get
            {
                return Type.GetType(mstr_classType, false);
            }
        }

        /// <summary>
        /// Returns a clone of this action.
        /// </summary>
        /// <returns></returns>
        public ARX_AlgorithmAction Clone()
        {
            ARX_AlgorithmAction objectHandle = (ARX_AlgorithmAction)Activator.CreateInstance(OriginalType);

            Debug.Log("Creating a clone of " + mstr_name + " of type " + OriginalType.ToString());
            objectHandle.mo_stats = mo_stats;
            objectHandle.mo_owningAlgorithm = mo_owningAlgorithm;
            objectHandle.mstr_description = mstr_description;
            objectHandle.mstr_name = mstr_name;
            objectHandle.mstr_classType = mstr_classType;
            return objectHandle;

        }

        /// <summary>
        /// Saves the type of this action to a variable for later reinstantiation.
        /// </summary>
        public void SaveType()
        {
            mstr_classType = GetType().ToString();
        }

        /// <summary>
        /// Function ran when action is first added to an algorithm
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Function ran when action is ran by algorithm
        /// </summary>
        public virtual void ExecuteAlgorithm() { }

        /// <summary>
        /// Function ran prior to the algorithm funning the execute action function on all of its held actions
        /// </summary>
        public virtual void PreProcessing()
        {
            Debug.Log("Running PreProcessing " + mstr_name);
        }

        /// <summary>
        /// Function ran after the algorithm run the execute action function on all of its held actions
        /// </summary>
        public virtual void PostProcessing()
        {
            Debug.Log("Running PostProcessing " + mstr_name);
        }
    }
}