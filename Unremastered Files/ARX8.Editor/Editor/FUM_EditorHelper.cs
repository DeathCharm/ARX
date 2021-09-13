using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;


namespace ARX
{
    public static class EditorHelper
    {

        public static bool IsClickedInEditor(Rect rect)
        {
            return (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition));

        }

        public static bool IsHoveredInEditor(Rect rect)
        {
            return (rect.Contains(Event.current.mousePosition));

        }

        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        /// <summary>
        ///	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// Courtesy of http://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
        /// </summary>
        public static T CreateAsset<T>(string strName , bool bAddAssetsDirectoryToFront = true, string strPath = "") where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            //if(strPath == "")
            //    strPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            //if (strPath == "")
            //{
            //    strPath = "Assets";
            //}
            //else if (Path.GetExtension(strPath) != "")
            //{
            //    strPath = strPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            //}

            string assetPathAndName;
            if(bAddAssetsDirectoryToFront)
                assetPathAndName = "Assets/" + strPath + "/" + strName + ".asset";
            else
                assetPathAndName = strPath + "/" + strName + ".asset";


            Debug.Log("Attempting to create asset at " + assetPathAndName);

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            //Selection.activeObject = asset;
            return asset;
        }

        public static void CreateAsset(Type type)
        {
            string[] astr_splits = type.ToString().Split('.');
            var fileName = astr_splits[astr_splits.Length - 1];
            var asset = ScriptableObject.CreateInstance(type);
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            path = string.IsNullOrEmpty(path)
              ? "Assets"
              : Path.GetDirectoryName(path);

            path = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", path, fileName));

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}

