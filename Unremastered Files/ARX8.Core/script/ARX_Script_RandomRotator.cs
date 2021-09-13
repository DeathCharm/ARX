using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets this object to a random rotation on start.
/// </summary>
public class ARX_Script_RandomRotator : MonoBehaviour
{
    /// <summary>
    /// Randomize the X axis?
    /// </summary>
    public bool mb_randomizeX = false;

    /// <summary>
    /// Randomize the Y axis?
    /// </summary>
    public bool mb_randomizeY = true;

    /// <summary>
    /// Randomize the Z axis?
    /// </summary>
    public bool mb_randomizeZ = false;

    /// <summary>
    /// Randomize the rotations of the children as well?
    /// </summary>
    public bool mb_randomizeChildren = false;

    //Has this script been initialized?
    bool mb_init = false;


    private void Update()
    {
        if (!mb_init)
        {
            mb_init = true;
            Randomize();
        }
    }

    /// <summary>
    /// Run the randomization of this object.
    /// If mb_randomizeChildren is true, randomizes this object's children as well.
    /// </summary>
    public void Randomize()
    {
        RandomizeRotation(gameObject);

        if (mb_randomizeChildren)
            RandomizeChildren(gameObject);
    }

    /// <summary>
    /// Randomize
    /// </summary>
    void RandomizeChildren(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform trans = obj.transform.GetChild(i);
            RandomizeRotation(trans.gameObject);
        }
    }

    /// <summary>
    /// Randomizes the given object.
    /// </summary>
    /// <param name="obj"></param>
    void RandomizeRotation(GameObject obj)
    {
        float X, Y, Z;

        X = obj.transform.localEulerAngles.x;
        Y = obj.transform.localEulerAngles.y;
        Z = obj.transform.localEulerAngles.z;

        if (mb_randomizeX)
            X = UnityEngine.Random.Range(0, 359);
        if (mb_randomizeY)
            Y = UnityEngine.Random.Range(0, 359);
        if (mb_randomizeZ)
            Z = UnityEngine.Random.Range(0, 359);

        Vector3 vec = new Vector3(X, Y, Z);
        obj.transform.localEulerAngles = vec;
    }
    
}
