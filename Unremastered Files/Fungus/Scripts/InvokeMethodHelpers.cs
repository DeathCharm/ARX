using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using MarkerMetro.Unity.WinLegacy.Reflection;

namespace Fungus
{
    [System.Serializable]
    public class InvokeMethodParameter
    {
        [SerializeField]
        public ObjectValue objValue;

        [SerializeField]
        public string variableKey;
    }

    [System.Serializable]
    public class ObjectValue
    {
        public string typeAssemblyname;
        public string typeFullname;

        public int intValue;
        public bool boolValue;
        public float floatValue;
        public string stringValue;

        public Color colorValue;
        public GameObject gameObjectValue;
        public Material materialValue;
        public UnityEngine.Object objectValue;
        public Sprite spriteValue;
        public Texture textureValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;

        public object GetValue()
        {
            switch (typeFullname)
            {
                case "System.Int32":
                    return intValue;
                case "System.Boolean":
                    return boolValue;
                case "System.Single":
                    return floatValue;
                case "System.String":
                    return stringValue;
                case "UnityEngine.Color":
                    return colorValue;
                case "UnityEngine.GameObject":
                    return gameObjectValue;
                case "UnityEngine.Material":
                    return materialValue;
                case "UnityEngine.Sprite":
                    return spriteValue;
                case "UnityEngine.Texture":
                    return textureValue;
                case "UnityEngine.Vector2":
                    return vector2Value;
                case "UnityEngine.Vector3":
                    return vector3Value;
                default:
                    var objType = ReflectionHelper.GetType(typeAssemblyname);

                    if (objType.IsSubclassOf(typeof(UnityEngine.Object)))
                    {
                        return objectValue;
                    }
                    else if (objType.IsEnum())
                        return System.Enum.ToObject(objType, intValue);

                    break;
            }

            return null;
        }
    }

    public static class ReflectionHelper
    {
        static Dictionary<string, System.Type> types = new Dictionary<string, System.Type>();

        public static System.Type GetType(string typeName)
        {
            if (types.ContainsKey(typeName))
                return types[typeName];

            types[typeName] = System.Type.GetType(typeName);

            return types[typeName];
        }
    }
}
