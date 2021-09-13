using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARX_ObjectDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject);
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
    
}
