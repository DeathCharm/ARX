using UnityEngine;
using UnityEngine.Serialization;
using FEN;
using ARX;

namespace Fungus
{
    /// <summary>
    /// Spawns a new object based on a reference to a scene or prefab game object.
    /// </summary>
    [CommandInfo("FEN",
                 "Spawn Object On RPG Stage",
                 "Spawns a new object based on a reference to a scene or prefab game object.",
        Priority = 10)]
    [CommandInfo("GameObject",
                 "Instantiate",
                 "Instantiate a game object")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class SpawnObjectOnRPGStage : Command
    {
        [Tooltip("Game object to copy when spawning. Can be a scene object or a prefab.")]
        [SerializeField] protected GameObjectData _sourceObject;

        [Tooltip("Transform to use as parent during instantiate.")]
        [SerializeField] protected TransformData _parentTransform;

        [Tooltip("If true, will place the object on the canvas. Else, object is placed on the stage.")]
        [SerializeField] protected BooleanData _placeOnCanvas = new BooleanData(false);

        [Tooltip("If true, will use the spawnPosition and spawnRotation of this block to set the spawned object's transform.")]
        [SerializeField] protected BooleanData _changeRotation = new BooleanData(false);
        
        [Tooltip("Local position of newly spawned object.")]
        [SerializeField] protected StringData _spawnPosition = new StringData("");

        [Tooltip("Local rotation of newly spawned object.")]
        [SerializeField] protected Vector3Data _spawnRotation;
        
        [Tooltip("Optional variable to store the GameObject that was just created.")]
        [SerializeField]
        protected GameObjectData _newlySpawnedObject;

        #region Public Members

        public override void OnEnter()
        {
            if (_sourceObject.Value == null)
            {
                Continue();
                return;
            }

            #region Get Stage Anchor

            //Get the stage position
            //If null is returned, do not create a new object
            ARX_Script_RPGStage.StageAnchor anchor = ARX_Script_RPGStage.GetStageAnchor(_spawnPosition.stringVal);
            if(anchor == null)
            {
                Debug.LogError("No anchor position named " + _spawnPosition.stringVal + " exists in the RPGStage");
                Continue();
                return;
            }
            #endregion


            #region Create Object
            GameObject newObject = null;

            if (_parentTransform.Value != null)
            {
                newObject = GameObject.Instantiate(_sourceObject.Value, _parentTransform.Value);
            }
            else
            {
                newObject = GameObject.Instantiate(_sourceObject.Value);
            }
            #endregion


            #region Set Object Position To Stage

            //If the object is a UI element that is to be placed on the canvas
            if(_placeOnCanvas.booleanVal == true)
            {
                anchor.AddToUICanvas(newObject.transform);
                newObject.transform.position = anchor.GetPositionOnCanvas();
            }
            //Else, if the object is to be placed on the RPG stage
            else
            {
                newObject.transform.position = anchor.GetPositionOnStage();
            }
                
                //If change spawn variables is true
                if (_changeRotation.Value)
                {
                    newObject.transform.localRotation = Quaternion.Euler(_spawnRotation.Value);
                }

            #endregion

            _newlySpawnedObject.Value = newObject;

            Continue();
        }

        public override string GetSummary()
        {
            if (_sourceObject.Value == null)
            {
                return "Error: No source GameObject specified";
            }

            if(GetButtonColor() == Color.red)
            {
                return "Error: Stage position " + _spawnPosition.stringVal + " is not a valid position";
            }

            return "Creating " + _sourceObject.Value.name + " at RPG Stage position " + _spawnPosition.stringVal;
        }

        public override Color GetButtonColor()
        {
            if (_spawnPosition.stringVal == "")
                return Color.green;
            if (FEN.RPGStage.IsValidStagePosition(_spawnPosition.stringVal))
                return Color.green;
            return Color.red;
        }

        public override bool HasReference(Variable variable)
        {
            if (_sourceObject.gameObjectRef == variable || _parentTransform.transformRef == variable ||
                _spawnRotation.vector3Ref == variable)
                return true;

            return false;
        }
        

        #endregion

        #region Backwards compatibility

        [HideInInspector] [FormerlySerializedAs("sourceObject")] public GameObject sourceObjectOLD;
        [HideInInspector] [FormerlySerializedAs("parentTransform")] public Transform parentTransformOLD;
        [HideInInspector] [FormerlySerializedAs("spawnPosition")] public string spawnPositionOLD;
        [HideInInspector] [FormerlySerializedAs("spawnRotation")] public Vector3 spawnRotationOLD;

        protected virtual void OnEnable()
        {
            if (sourceObjectOLD != null)
            {
                _sourceObject.Value = sourceObjectOLD;
                sourceObjectOLD = null;
            }
            if (parentTransformOLD != null)
            {
                _parentTransform.Value = parentTransformOLD;
                parentTransformOLD = null;
            }
            if (spawnPositionOLD != default(string))
            {
                _spawnPosition.Value = spawnPositionOLD;
                spawnPositionOLD = default(string);
            }
            if (spawnRotationOLD != default(Vector3))
            {
                _spawnRotation.Value = spawnRotationOLD;
                spawnRotationOLD = default(Vector3);
            }
        }

        #endregion
    }
}