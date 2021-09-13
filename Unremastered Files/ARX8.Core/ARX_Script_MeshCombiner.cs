using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Combines all the meshes of this object and its children into a single mesh to
/// minimize draw calls.
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_MeshCombiner : MonoBehaviour
{
    public bool mb_combineMeshes = false;
    
    public Mesh output;

    private void Update()
    {
        if (mb_combineMeshes)
        {
            MeshFilter[] buf = GetComponentsInChildren<MeshFilter>();

            List<Mesh> oaMeshes = new List<Mesh>();
            foreach (MeshFilter filter in buf)
                oaMeshes.Add(filter.sharedMesh);

            output = CombineMeshes(oaMeshes.ToArray());
            mb_combineMeshes = false;
        }

        if (output != null)
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            if (filter != null)
                filter.mesh = output;
        }
    }

    private Mesh CombineMeshes(Mesh[] meshes)
    {
        var combine = new CombineInstance[meshes.Length];
        for (int i = 0; i < meshes.Length; i++)
        {
            combine[i].mesh = meshes[i];
            combine[i].transform = transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        mesh.name = "New Mesh";
        return mesh;
    }
}