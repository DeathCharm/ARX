using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves this object's attached mesh and recreates it onEnable.
/// This script's purpose was to allow procedurally generated meshes
/// to persist after the UnityEditor is closed and reopened.
/// </summary>
[ExecuteInEditMode]
public class ARX_Script_SavedMesh : MonoBehaviour {

    public bool bSaveMesh = false, bLoadMesh = false;

    [HideInInspector]
    [SerializeField]
    public Vector2[] uv;

    [HideInInspector]
    [SerializeField]
    public Vector3[] vertices;

    [HideInInspector]
    [SerializeField]
    public int[] triangles;

    public void SaveMesh()
    {
        bSaveMesh = false;
        MeshFilter oFilter = GetComponent<MeshFilter>();
        if(oFilter == null)
            return;

        Mesh mesh = oFilter.sharedMesh;
        uv = mesh.uv;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
    }

    public void RecreateMesh()
    {
        if(vertices == null || triangles == null)
            return;

        bLoadMesh = false;
        MeshFilter oFilter = GetComponent<MeshFilter>();
        if(oFilter == null)
            return;

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.uv = uv;
        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();
        oFilter.sharedMesh = newMesh;
    }

    private void OnEnable()
    {
        RecreateMesh();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(bSaveMesh)
          SaveMesh();
        if(bLoadMesh)
            RecreateMesh();  
	}
}
