
using UnityEngine;

public class ARX_Scratch3 : MonoBehaviour
{
    public float size = 5;
    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(transform.position, size);
    }
}