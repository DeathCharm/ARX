using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;

namespace ARX
{
    /// <summary>
    /// A customizable algorithm composed of AlgorithmActions
    /// </summary>
    public abstract class ARX_Asset_Algorithm : ScriptableObject
    {
        /// <summary>
        /// List of actions that compose the algorithm
        /// </summary>
        [SerializeField]
        List<ARX_AlgorithmAction> moa_algorithmActions = new List<ARX_AlgorithmAction>();

        /// <summary>
        /// Since Unity can't serialize derived classes, this function remakes all algorithm pieces
        /// held before running them.
        /// </summary>
        public void RemakeAlgorithm()
        {
            List<ARX_AlgorithmAction> obuf = new List<ARX_AlgorithmAction>();
            foreach (ARX_AlgorithmAction action in AlgorithmActions)
            {
                ARX_AlgorithmAction oRemake = action.Clone();
                obuf.Add(oRemake);
            }
            moa_algorithmActions.Clear();
            moa_algorithmActions = obuf;
        }

        /// <summary>
        /// Accessor for the list of actions that compose this algorithm
        /// </summary>
        public List<ARX_AlgorithmAction> AlgorithmActions
        {
            get
            {
                return moa_algorithmActions;
            }
        }

        /// <summary>
        /// This function is called after this algorithm is ran.
        /// Use it to prepare the algorithm for any further uses by 
        /// cleaning up any remaining variables.
        /// </summary>
        public abstract void ResetVariables();


        /// <summary>
        /// Add the given action to the bottom of this algorithm
        /// </summary>
        /// <param name="oAction"></param>
        public void Add(ARX_AlgorithmAction oAction)
        {
            oAction.mo_owningAlgorithm = this;
            moa_algorithmActions.Add(oAction);
            oAction.Initialize();
            oAction.SaveType();
        }

        /// <summary>
        /// Creates an action of the given class type and adds it to the bottom
        /// of this algorithm.
        /// Currently obselete and awaiting reimplementation of entire class
        /// </summary>
        /// <param name="strAlgorithmType"></param>
        public void Add(string strAlgorithmType)
        {
            //ObjectHandle varPiece = System.Activator.CreateInstance(null, strAlgorithmType);
            //ARX_AlgorithmAction oPiece = (ARX_AlgorithmAction)varPiece.Unwrap();
            //Add(oPiece);
            //oPiece.SaveType();
        }

        /// <summary>
        /// Runs the contained actions held in this algorithm.
        /// Remakes the actions, then runs each of their Preprocessing, Execute and
        /// Postprocessing functions.
        /// </summary>
        public void RunAlgorithm()
        {
            RemakeAlgorithm();
            Debug.Log("Now Running Algorithm Pieces: " + AlgorithmActions.Count);
            RunPreProcessing();
            foreach (ARX_AlgorithmAction p in AlgorithmActions)
                p.ExecuteAlgorithm();
            RunPostProcessing();
        }

        /// <summary>
        /// Runs each action's prepocessing function
        /// </summary>
        void RunPreProcessing()
        {
            foreach (ARX_AlgorithmAction p in AlgorithmActions)
                p.PreProcessing();
        }

        /// <summary>
        /// Runs each action's postprocessing function
        /// </summary>
        void RunPostProcessing()
        {
            foreach (ARX_AlgorithmAction p in AlgorithmActions)
                p.PostProcessing();
        }

    }
}