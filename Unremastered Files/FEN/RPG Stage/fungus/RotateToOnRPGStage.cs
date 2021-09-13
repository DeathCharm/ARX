using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Rotates a game object to the specified angles over time.
    /// </summary>
    [CommandInfo("FEN",
                 "Rotate To On RPG Stage",
                 "Rotates a game object to the specified angles over time.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class RotateToOnRPGStage : iTweenCommand
    {
        [Tooltip("Target stage position that the GameObject will rotate to")]
        [SerializeField] protected StringData _stagePosition;

        [Tooltip("Target rotation that the GameObject will rotate to, if no To Transform is set")]
        [SerializeField] protected BooleanData _isOnCanvas = new BooleanData(false);

        #region Public members

        public override void DoTween()
        {
            Hashtable tweenParams = new Hashtable();
            tweenParams.Add("name", _tweenName.Value);

            ARX_Script_RPGStage.StageAnchor anchor =  ARX_Script_RPGStage.GetStageAnchor(_stagePosition.stringVal);

            Vector3 vecTargetPosition = new Vector3(), vecTargetRotation = new Vector3();


            if(_isOnCanvas.booleanVal == true)
            {
                vecTargetPosition = anchor.GetPositionOnCanvas();
            }
            else
            {
                vecTargetPosition = anchor.GetPositionOnStage();
            }

            vecTargetRotation = ARX.ToolBox.PointCanvasElementAt(_targetObject.gameObjectVal.transform.position, vecTargetPosition);


            tweenParams.Add("rotation", vecTargetRotation);
            tweenParams.Add("time", _duration.Value);
            tweenParams.Add("easetype", easeType);
            tweenParams.Add("looptype", loopType);
            tweenParams.Add("oncomplete", "OniTweenComplete");
            tweenParams.Add("oncompletetarget", gameObject);
            tweenParams.Add("oncompleteparams", this);
            iTween.RotateTo(_targetObject.Value, tweenParams);
        }

        public override bool HasReference(Variable variable)
        {
            return base.HasReference(variable);
        }

        public override string GetSummary()
        {
            if (GetButtonColor() == Color.red)
            {
                return "Error: Stage position " + _stagePosition.stringVal + " is not a valid position";
            }

            return "Moving from RPG Stage position " + _stagePosition.stringVal;
        }

        public override Color GetButtonColor()
        {
            if (_stagePosition.stringVal == "")
                return Color.green;
            if (FEN.RPGStage.IsValidStagePosition(_stagePosition.stringVal))
                return Color.green;
            return Color.red;
        }

        #endregion

    }
}