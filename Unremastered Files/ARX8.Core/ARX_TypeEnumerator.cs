using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;

namespace ARX
{
    /// <summary>
    /// Rreturns a list of Types derived from the given type.
    /// </summary>
    public static class TypeEnumerator
    {
        /// <summary>
        /// Returns a list of types derived from the given type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<System.Type> GetDerivedTypes(Type t)
        {
            List<System.Type> objects = new List<System.Type>();
            foreach (Type type in
                Assembly.GetAssembly(t).GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(t))
                    objects.Add(type);
            }
            return objects;
        }

        /// <summary>
        /// Returns a list of types derived from the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<System.Type> GetDerivedTypes<T>()
        {
            List<System.Type> objects = new List<System.Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(T)))
                    objects.Add(type);
            }
            return objects;
        }

    }
}

