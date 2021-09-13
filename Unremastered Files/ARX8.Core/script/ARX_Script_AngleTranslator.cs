using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Moves the children of the childRoot object independently of it,
/// as if the childRoot's children were children of the mo_mover object.
/// Can make some pretty awesome visual effects, but this wasn't an
/// intended use for it.
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_AngleTranslator : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The rotation of the mo_mover object.
    /// </summary>
    public Vector3 mvec_rotation;

    /// <summary>
    /// The localPosition of the mo_mover object from the mo_pivot 
    /// </summary>
    public Vector3 mvec_localOffset;

    /// <summary>
    /// The localPosition of the moved childObjects from the mo_mover
    /// </summary>
    public Vector3 mvec_added;

    /// <summary>
    /// The pivot point around which the childRoot's children are moved.
    /// It's rotation is frozen and can only be changed using this script's
    /// Vector3 variables.
    /// By default, this point is the owner of this script.
    /// </summary>
    public Transform mo_pivot;

    /// <summary>
    /// The parent of the objects to be moved.
    /// </summary>
    public Transform mo_childRoot;

    /// <summary>
    /// The gameobject used as an anchor for the moved objects.
    /// This object's position is calculated each frame and
    /// all of the moved objects will be placed at this object's location
    /// </summary>
    [HideInInspector]
    public GameObject mo_mover;
    #endregion

    private void Start()
    {
        if(mo_pivot == null)
            mo_pivot = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Create a child object name "mover"
        if (mo_mover == null)
        {
            mo_mover = new GameObject();
            mo_mover.transform.SetParent(transform);
            mo_mover.name = "mover";
        }

        if (mo_pivot == null || mo_childRoot == null || mo_childRoot.childCount == 0)
            return;
        
        mo_pivot.transform.localEulerAngles = Vector3.zero;
        mo_mover.transform.SetParent(mo_pivot);
        mo_mover.transform.localPosition = mvec_localOffset;

        for (int i = 0; i < mo_childRoot.childCount; i++)
        {
            Transform oChildTrans = mo_childRoot.GetChild(i);
            oChildTrans.position = mo_mover.transform.position + (mvec_added * i);

            mo_mover.transform.localPosition = mvec_localOffset;
            mo_childRoot.GetChild(i).position = mo_mover.transform.position;

            mo_pivot.Rotate(mvec_rotation);
        }
    }
}
