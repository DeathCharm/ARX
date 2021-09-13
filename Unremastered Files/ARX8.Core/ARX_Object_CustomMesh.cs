using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ARX
{
    public class Arx_Object_CustomMesh
    {
        List<Vector3> newVertices = new List<Vector3>();
        List<int> newTriangles = new List<int>();
        public List<Vector3> colVertices = new List<Vector3>();
        List<Vector3> visFaceVertices = new List<Vector3>();
        List<int> colTriangles = new List<int>();
        List<int> visTriangles = new List<int>();
        protected MeshFilter mo_meshFilter;
        protected MeshCollider mo_collider;
        protected MeshCollider mo_childCollider;
        List<Vector2> newUV = new List<Vector2>();
        int colCount = 0, visFaceCount = 0;

        public float mnf_width = 1, mnf_height = 1, mnf_depth = 1;

        public bool mb_northVisible = true, mb_southVisible = true, mb_setColliderToChild = false;

        public bool ThreeDimensional
        {
            set
            {
                if (value == true)
                {
                    mb_northVisible = true;
                    mb_southVisible = true;
                }
            }
            get
            {
                return (mb_northVisible && mb_southVisible);
            }
        }

        public bool Invisible
        {
            set
            {
                mb_northVisible = !value;
                mb_southVisible = !value;
            }
            get
            {
                return (!mb_northVisible && !mb_southVisible);
            }
        }

        public Arx_Object_CustomMesh() { }

        public Arx_Object_CustomMesh(GameObject oGameObject, Vector3 vecPosition, bool bSetToChild = true,
            float nfWidth = 1, float nfHeight = 1, float nfDepth = 1)
        {
            mb_setColliderToChild = bSetToChild;
            MeshFilter oFilter = oGameObject.GetComponent<MeshFilter>();
            MeshCollider oCollider = oGameObject.GetComponent<MeshCollider>();
            MeshRenderer oRenderer = oGameObject.GetComponent<MeshRenderer>();

            if (oFilter == null)
                oFilter = oGameObject.AddComponent<MeshFilter>();
            if (oCollider == null)
                oCollider = oGameObject.AddComponent<MeshCollider>();
            if (oRenderer == null)
                oRenderer = oGameObject.AddComponent<MeshRenderer>();

            Initialize(oFilter, oCollider, oRenderer, vecPosition, nfWidth, nfHeight, nfDepth);
            oCollider.enabled = false;
        }

        public Arx_Object_CustomMesh(MeshFilter oFilter, MeshCollider oCollider, MeshRenderer oRenderer, Vector3 vecPosition,
            bool bSetToChild = true,
            float nfWidth = 1, float nfHeight = 1, float nfDepth = 1)
        {
            mb_setColliderToChild = bSetToChild;
            Initialize(oFilter, oCollider, oRenderer, vecPosition, nfWidth, nfHeight, nfDepth);
        }

        public void Initialize(GameObject oGameObject, Vector3 vecPosition,
            float nfWidth = 1, float nfHeight = 1, float nfDepth = 1)
        {
            MeshFilter oFilter = oGameObject.GetComponent<MeshFilter>();
            MeshCollider oCollider = oGameObject.GetComponent<MeshCollider>();
            MeshRenderer oRenderer = oGameObject.GetComponent<MeshRenderer>();

            if (oFilter == null)
                oFilter = oGameObject.AddComponent<MeshFilter>();
            if (oCollider == null)
                oCollider = oGameObject.AddComponent<MeshCollider>();
            if (oRenderer == null)
                oRenderer = oGameObject.AddComponent<MeshRenderer>();

            Initialize(oFilter, oCollider, oRenderer, vecPosition, nfWidth, nfHeight, nfDepth);
        }

        public void Initialize(MeshFilter oFilter, MeshCollider oCollider, MeshRenderer oRenderer, Vector3 vecPosition,
            float nfWidth, float nfHeight, float nfDepth)
        {
            mo_meshFilter = oFilter;
            mo_collider = oCollider;
            mnf_width = nfWidth;
            mnf_height = nfHeight;
            mnf_depth = nfDepth;

            mo_meshFilter.sharedMesh = new Mesh();

            GenerateMesh(vecPosition);
        }

        public void GenerateMesh(Vector3 vecPosition)
        {
            GameObject obj;

            if (mo_childCollider == null)
            {
                //If the generated mesh's collider is to be a child object
                if (mb_setColliderToChild)
                {
                    obj = new GameObject();
                    obj.transform.SetParent(mo_collider.gameObject.transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.name = "Collider Mesh";
                    mo_childCollider = obj.AddComponent<MeshCollider>();
                }
                //Else if the generated mesh is to hold it's own collider
                else
                {
                    obj = mo_meshFilter.gameObject;
                    mo_childCollider = obj.GetComponent<MeshCollider>();
                    if (mo_childCollider == null)
                        mo_childCollider = obj.AddComponent<MeshCollider>();
                }
            }



            GenCollider(vecPosition.x, vecPosition.y, vecPosition.z);
            UpdateMesh();
        }

        Mesh CreateNewMesh(List<Vector3> oavecVertices, List<Vector2> oavecUV, List<int> oanTriangles)
        {
            Mesh newMesh = new Mesh();
            newMesh.vertices = oavecVertices.ToArray();
            if (oavecUV != null)
                newMesh.uv = oavecUV.ToArray();
            newMesh.triangles = oanTriangles.ToArray();
            newMesh.RecalculateNormals();
            newMesh.RecalculateTangents();
            return newMesh;
        }

        void InitializeComponents()
        {

        }

        public void UpdateMesh()
        {
            Mesh oVisibleMesh = CreateNewMesh(visFaceVertices, newUV, visTriangles);
            Mesh oColliderMesh = CreateNewMesh(colVertices, null, colTriangles);

            oColliderMesh.name = "Collider Mesh";
            oVisibleMesh.name = "FUM Box Mesh";

            InitializeComponents();

            mo_collider.sharedMesh = oVisibleMesh;
            mo_meshFilter.sharedMesh = oVisibleMesh;
            mo_childCollider.sharedMesh = oColliderMesh;


            colVertices.Clear();
            colTriangles.Clear();
            visFaceVertices.Clear();
            visTriangles.Clear();
            newUV.Clear();
            colCount = 0;
            visFaceCount = 0;
        }

        void GenCollider(float x, float y, float z)
        {
            //If either side is invisible, don't drop the tops or bottoms of the wall
            x -= mnf_width / 2;
            z -= mnf_depth / 2;

            //Top
            colVertices.Add(new Vector3(x, y, z + mnf_depth));
            colVertices.Add(new Vector3(x + mnf_width, y, z + mnf_depth));
            colVertices.Add(new Vector3(x + mnf_width, y, z));
            colVertices.Add(new Vector3(x, y, z));
            ColliderTriangles();
            colCount++;
            if (ThreeDimensional)
            {
                visFaceVertices.Add(new Vector3(x, y, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x + mnf_width, y, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x + mnf_width, y, z));
                visFaceVertices.Add(new Vector3(x, y, z));
                VisibleFaceTriangles();
                visFaceCount++;
                {
                    Vector2[] face = new Vector2[] {

                new Vector2(1,0),
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1)
                };
                    newUV.AddRange(face);
                }
            }


            //bot
            colVertices.Add(new Vector3(x, y - mnf_height, z));
            colVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z));
            colVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z + mnf_depth));
            colVertices.Add(new Vector3(x, y - mnf_height, z + mnf_depth));
            ColliderTriangles();
            colCount++;
            if (ThreeDimensional)
            {
                visFaceVertices.Add(new Vector3(x, y - mnf_height, z));
                visFaceVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z));
                visFaceVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x, y - mnf_height, z + mnf_depth));
                VisibleFaceTriangles();
                visFaceCount++;
                {
                    Vector2[] face = new Vector2[] {

                new Vector2(1,0),
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1)
                };
                    newUV.AddRange(face);
                }
            }


            //left
            colVertices.Add(new Vector3(x, y - mnf_height, z + mnf_depth));
            colVertices.Add(new Vector3(x, y, z + mnf_depth));
            colVertices.Add(new Vector3(x, y, z));
            colVertices.Add(new Vector3(x, y - mnf_height, z));
            ColliderTriangles();
            colCount++;
            if (ThreeDimensional)
            {
                visFaceVertices.Add(new Vector3(x, y - mnf_height, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x, y, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x, y, z));
                visFaceVertices.Add(new Vector3(x, y - mnf_height, z));
                VisibleFaceTriangles();
                visFaceCount++;
                {
                    Vector2[] face = new Vector2[] {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,0),
                };
                    newUV.AddRange(face);
                }
            }



            //right
            colVertices.Add(new Vector3(x + mnf_width, y, z + mnf_depth));
            colVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z + mnf_depth));
            colVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z));
            colVertices.Add(new Vector3(x + mnf_width, y, z));
            ColliderTriangles();
            colCount++;
            if (ThreeDimensional)
            {
                visFaceVertices.Add(new Vector3(x + mnf_width, y, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z));
                visFaceVertices.Add(new Vector3(x + mnf_width, y, z));
                VisibleFaceTriangles();
                visFaceCount++;
                {
                    Vector2[] face = new Vector2[] {
                new Vector2(1,1),
                new Vector2(1,0),
                new Vector2(0,0),
                new Vector2(0,1)
                };
                    newUV.AddRange(face);
                }
            }

            //In
            colVertices.Add(new Vector3(x, y - mnf_height, z + mnf_depth));
            colVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z + mnf_depth));
            colVertices.Add(new Vector3(x + mnf_width, y, z + mnf_depth));
            colVertices.Add(new Vector3(x, y, z + mnf_depth));
            ColliderTriangles();
            colCount++;
            if (mb_northVisible)
            {
                visFaceVertices.Add(new Vector3(x, y - mnf_height, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x + mnf_width, y, z + mnf_depth));
                visFaceVertices.Add(new Vector3(x, y, z + mnf_depth));
                VisibleFaceTriangles();
                visFaceCount++;

                {
                    Vector2[] face = new Vector2[] {
                new Vector2(1,0),
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1)
                };
                    newUV.AddRange(face);
                }
            }

            //Out
            colVertices.Add(new Vector3(x, y, z));
            colVertices.Add(new Vector3(x + mnf_width, y, z));
            colVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z));
            colVertices.Add(new Vector3(x, y - mnf_height, z));
            ColliderTriangles();
            colCount++;
            if (mb_southVisible)
            {
                visFaceVertices.Add(new Vector3(x, y, z));
                visFaceVertices.Add(new Vector3(x + mnf_width, y, z));
                visFaceVertices.Add(new Vector3(x + mnf_width, y - mnf_height, z));
                visFaceVertices.Add(new Vector3(x, y - mnf_height, z));
                VisibleFaceTriangles();
                visFaceCount++;
                {
                    Vector2[] face = new Vector2[] {
                new Vector2(1,0),
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1)
                };

                    newUV.AddRange(face);
                }
            }

        }

        void VisibleFaceTriangles()
        {
            visTriangles.Add(visFaceCount * 4);
            visTriangles.Add((visFaceCount * 4) + 1);
            visTriangles.Add((visFaceCount * 4) + 3);
            visTriangles.Add((visFaceCount * 4) + 1);
            visTriangles.Add((visFaceCount * 4) + 2);
            visTriangles.Add((visFaceCount * 4) + 3);
        }

        void ColliderTriangles()
        {
            colTriangles.Add(colCount * 4);
            colTriangles.Add((colCount * 4) + 1);
            colTriangles.Add((colCount * 4) + 3);
            colTriangles.Add((colCount * 4) + 1);
            colTriangles.Add((colCount * 4) + 2);
            colTriangles.Add((colCount * 4) + 3);
        }
    }
}