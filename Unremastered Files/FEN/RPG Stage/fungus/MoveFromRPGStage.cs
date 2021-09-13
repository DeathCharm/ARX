using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Fungus
{
    /// <summary>
    /// Moves a game object from a specified position back to its starting position over time. The position can be defined by a transform in another object (using To Transform) or by setting an absolute position (using To Position, if To Transform is set to None).
    /// </summary>
    [CommandInfo("FEN",
                 "Move From RPG Stage",
                 "Moves a game object from a specified position back to its starting position over time. The position can be defined by a transform in another object (using To Transform) or by setting an absolute position (using To Position, if To Transform is set to None).")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class MoveFromRPGStage : iTweenCommand
    {
        [Tooltip("If true, will place the object on the canvas. Else, object is placed on the stage.")]
        [SerializeField] protected BooleanData _placeOnCanvas = new BooleanData(false);

        [Tooltip("Target world position that the GameObject will move from, if no From Transform is set")]
        [SerializeField] protected StringData _fromStagePosition = new StringData("");

        [Tooltip("Whether to animate in world space or relative to the parent. False by default.")]
        [SerializeField] protected bool isLocal;

        #region Public members

        public override void DoTween()
        {

            ARX_Script_RPGStage.StageAnchor anchor = ARX_Script_RPGStage.GetStageAnchor(_fromStagePosition.stringVal);
            Vector3 vecToPosition = new Vector3();

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
            iTween.MoveFrom(_targetObject.Value, tweenParams);
        }

        public override bool HasReference(Variable variable)
        {
            return _fromStagePosition.stringRef == variable || 
                base.HasReference(variable);
        }

        public override string GetSummary()
        {
            if (GetButtonColor() == Color.red)
            {
                return "Error: Stage position " + _fromStagePosition.stringVal + " is not a valid position";
            }

            return "Moving from RPG Stage position " + _fromStagePosition.stringVal;
        }

        public override Color GetButtonColor()
        {
            if (_fromStagePosition.stringVal == "")
                return Color.green;
            if (FEN.RPGStage.IsValidStagePosition(_fromStagePosition.stringVal))
                return Color.green;
            return Color.red;
        }

        #endregion

    }
}