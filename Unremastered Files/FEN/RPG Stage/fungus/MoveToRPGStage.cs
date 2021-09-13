using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Fungus
{
    /// <summary>
    /// Moves a game object to a specified position over time. The position can be defined by a transform in another object (using To Transform) or by setting an absolute position (using To Position, if To Transform is set to None).
    /// </summary>
    [CommandInfo("FEN",
                 "Move To RPG Stage",
                 "Moves a game object to a specified position over time. The position can be defined by a transform in another object (using To Transform) or by setting an absolute position (using To Position, if To Transform is set to None).")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class MoveToRPGStage : iTweenCommand
    {
        [Tooltip("If true, will place the object on the canvas. Else, object is placed on the stage.")]
        [SerializeField] protected BooleanData _placeOnCanvas = new BooleanData(false);

        [Tooltip("Local position of newly spawned object.")]
        [SerializeField] protected StringData _spawnPosition = new StringData("");


        [Tooltip("Random Offset of the spawned object")]
        [SerializeField] protected Vector3Data _randomOffsetMin = new Vector3Data(Vector3.zero);


        [Tooltip("Random Offset of the spawned object")]
        [SerializeField] protected Vector3Data _randomOffsetMax = new Vector3Data(Vector3.zero);

        [Tooltip("Whether to animate in world space or relative to the parent. False by default.")]
        [SerializeField] protected bool isLocal;


        #region Public members

        public override void DoTween()
        {
            ARX_Script_RPGStage.StageAnchor anchor = ARX_Script_RPGStage.GetStageAnchor(_spawnPosition.stringVal);
            if (anchor == null)
            {
                Debug.LogError("No anchor position named " + _spawnPosition.stringVal + " exists in the RPGStage");
                return;
            }
            
            Vector3 vecToPosition;

            //If the object is a UI element that is to be placed on the canvas
            if (_placeOnCanvas.booleanVal == true)
            {
                ARX_Script_RPGStage.AddToUICanvas(_targetObject.gameObjectVal.transform);
                vecToPosition = anchor.GetPositionOnCanvas();
            }
            //Else, if the object is to be placed on the RPG stage
            else
            {
                vecToPosition = anchor.GetPositionOnStage();
            }

            Vector3 vecRandomOffset = ARX.ToolBox.RandomRange(_randomOffsetMin, _randomOffsetMax);

            vecToPosition += vecRandomOffset;

            Debug.Log("vec To Position is " + vecToPosition);

            Hashtable tweenParams = new Hashtable();
            tweenParams.Add("name", _tweenName.Value);

            tweenParams.Add("position", vecToPosition);
            tweenParams.Add("time", _duration.Value);
            tweenParams.Add("easetype", easeType);
            tweenParams.Add("looptype", loopType);
            tweenParams.Add("isLocal", isLocal);
            tweenParams.Add("oncomplete", "OniTweenComplete");
            tweenParams.Add("oncompletetarget", gameObject);
            tweenParams.Add("oncompleteparams", this);
            iTween.MoveTo(_targetObject.Value, tweenParams);
        }

        public override bool HasReference(Variable variable)
        {
            return base.HasReference(variable);
        }

        public override string GetSummary()
        {
            if (GetButtonColor() == Color.red)
            {
                return "Error: Stage position " + _spawnPosition.stringVal + " is not a valid position";
            }

            return "Moving to RPG position " + _spawnPosition.stringVal;
        }

        public override Color GetButtonColor()
        {
            if (_spawnPosition.stringVal == "")
                return Color.green;
            if (FEN.RPGStage.IsValidStagePosition(_spawnPosition.stringVal))
                return Color.green;
            return Color.red;
        }

        #endregion

    }
}