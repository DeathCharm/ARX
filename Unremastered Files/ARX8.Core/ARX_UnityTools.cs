using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ARX
{
    public static partial class ToolBox
    {
        public static bool IsPrefab(this GameObject a_Object)
        {
            return a_Object.scene.rootCount == 0;
        }

        public static Vector3 RandomRange(Vector3 min, Vector3 max)
        {
            float x, y, z;
            x = UnityEngine.Random.Range(min.x, max.x);
            y = UnityEngine.Random.Range(min.y, max.y);
            z = UnityEngine.Random.Range(min.z, max.z);

            return new Vector3(x, y, z);

        }


        /// <summary>
        /// Returns a quaternion pointing at the target position.
        /// </summary>
        /// <param name="vecLookerPosition"></param>
        /// <param name="vecTargetPosition"></param>
        /// <returns></returns>
        public static Vector3 PointCanvasElementAt(Vector3 vecLookerPosition, Vector3 vecTargetPosition)
        {
            Vector2 vecDif = vecTargetPosition - vecLookerPosition;

            float nfSlope = vecDif.y / vecDif.x;

            float atanRad = Mathf.Atan(nfSlope);
            float nfDegrees = atanRad * Mathf.Rad2Deg;

            //If X is negative
            if (vecDif.x < 0)
            {
                nfDegrees -= 180;
            }

            nfDegrees += 90;

            return new Vector3 (0, 0, nfDegrees);
        }

        public static float RandomAngle { get { return UnityEngine.Random.Range(0, 359); } }

        public static Vector3[] GetBoundCornersAndCenter(Bounds bound)
        {
            float
                minx = bound.min.x,
                miny = bound.min.y,
                minz = bound.min.z;
            float
                maxx = bound.max.x,
                maxy = bound.max.y,
                maxz = bound.max.z;

            Vector3[] avecReturn = new Vector3[]{
            
            //Center
            bound.center,

            //Near face
            new Vector3(minx, miny, minz),
            new Vector3(maxx, miny, minz),
            new Vector3(minx, maxy, minz),
            new Vector3(maxx, maxy, minz),

            //Far face
            new Vector3(maxx, maxy, maxz),
            new Vector3(minx, maxy, maxz),
            new Vector3(maxx, miny, maxz),
            new Vector3(minx, miny, maxz)

        }
            ;

            return avecReturn;
        }

        public static Vector2 middleOfScreen
        {
            get
            {
                return new Vector2(Screen.width / 2, Screen.height / 2);
            }
        }

        static GameObject mo_blankGameObject = null;

        public static GameObject BlankGameObject
        {
            get
            {
                if (mo_blankGameObject == null)
                {
                    mo_blankGameObject = new GameObject();
                    mo_blankGameObject.name = "Blank Game Object";
                    mo_blankGameObject.hideFlags = HideFlags.HideAndDontSave;
                }
                return mo_blankGameObject;
            }
        }

        /// <summary>
        /// Saves the variables of a material
        /// </summary>
        [System.Serializable]
        public class MaterialBuffer
        {
            public Material[] moa_materials;
            Renderer mo_renderer;
            Material mo_bufferMaterial;
            public string mstr_bufferMaterialPath;

            public Material[] oMaterials { get { return moa_materials; } }
            public Renderer oRenderer { get { return mo_renderer; } }

            public MaterialBuffer(Renderer ren, string strBufferPath = "glass")
            {
                mstr_bufferMaterialPath = strBufferPath;
                mo_renderer = ren;
                moa_materials = ren.sharedMaterials;
            }

            Material GetBufferMaterial
            {
                get
                {
                    if (mo_bufferMaterial == null)
                        mo_bufferMaterial = ToolBox.LoadMaterial(mstr_bufferMaterialPath);
                    return mo_bufferMaterial;
                }
            }

            public void SetToBufferMaterial()
            {
                oRenderer.material = GetBufferMaterial;
            }

            public void SetToOriginalMaterial()
            {
                oRenderer.sharedMaterials = moa_materials;
            }

            public bool IsDeleted()
            {
                return (mo_renderer == null);
            }
        }

        [System.Serializable]
        public class MaterialBufferList
        {
            public List<MaterialBuffer> moa_list = new List<MaterialBuffer>();
            public MaterialBufferList(GameObject obj)
            {
                Renderer[] oaRenderers = obj.GetComponentsInChildren<Renderer>();
                foreach (Renderer ren in oaRenderers)
                    moa_list.Add(new MaterialBuffer(ren));
            }

            public void SetToOriginalMaterials()
            {
                foreach (MaterialBuffer buffer in moa_list)
                    buffer.SetToOriginalMaterial();
            }

            public void SetToBufferMaterials()
            {
                foreach (MaterialBuffer buffer in moa_list)
                    buffer.SetToBufferMaterial();
            }
        }


        public static Component AddOrGetComponent<T>(GameObject obj)
        {
            Component oComp = obj.GetComponent(typeof(T));
            if (oComp == null)
            {
                return obj.gameObject.AddComponent(typeof(T));
            }
            return oComp;
        }

        public static Vector3 ImpactForce(Collision collision)
        {
            return collision.impulse / Time.fixedDeltaTime;
        }

        public static Vector3 NearestVertexTo(Vector3 point, GameObject oTarget)
        {
            // convert point to local space
            point = oTarget.transform.InverseTransformPoint(point);

            Mesh mesh = oTarget.GetComponent<MeshFilter>().mesh;
            float minDistanceSqr = Mathf.Infinity;
            Vector3 nearestVertex = Vector3.zero;
            // scan all vertices to find nearest
            foreach (Vector3 vertex in mesh.vertices)
            {
                Vector3 diff = point - vertex;
                float distSqr = diff.sqrMagnitude;
                if (distSqr < minDistanceSqr)
                {
                    minDistanceSqr = distSqr;
                    nearestVertex = vertex;
                }
            }
            // convert nearest vertex back to world space
            return oTarget.transform.TransformPoint(nearestVertex);
        }
        

        public static void AddGUIStyle(GUISkin oSkin, GUIStyle oNewStyle)
        {
            List<GUIStyle> oaStyles = new List<GUIStyle>(oSkin.customStyles);
            oaStyles.Add(oNewStyle);
            oSkin.customStyles = oaStyles.ToArray();
        }

        public static void RemoveGUIStyle(GUISkin oSkin, GUIStyle oNewStyle)
        {
            List<GUIStyle> oaStyles = new List<GUIStyle>(oSkin.customStyles);
            oaStyles.Remove(oNewStyle);
            oSkin.customStyles = oaStyles.ToArray();
        }

        public static GameObject LoadPrefab(string strPath)
        {
            Debug.Log("Attemtping to create prefab " + strPath);
            GameObject obj = GameObject.Instantiate(Resources.Load(strPath)) as GameObject;
            if (obj == null)
            {
                Debug.LogError("Could not load prefab " + strPath);
            }


            return obj;
        }

        public static Texture LoadTexture(string strPath)
        {
            Texture obj = Resources.Load<Texture>(strPath);
            if (obj == null)
            {
                Debug.LogError("Could not load material " + strPath);
            }

            return obj;
        }

        public static Vector2 Range(Vector2 min, Vector2 max)
        {
            float x, y;
            x = UnityEngine.Random.Range(min.x, max.x);
            y = UnityEngine.Random.Range(min.y, max.y);
            return new Vector2(x, y);
        }

        public static Material LoadMaterial(string strPath)
        {
            Material obj = Resources.Load<Material>(strPath);
            if (obj == null)
            {
                Debug.LogError("Could not load material " + strPath);
            }

            return obj;
        }
    }
}