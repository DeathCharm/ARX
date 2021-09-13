using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ARX.VarGen;

namespace ARX
{

    [Serializable]
    public class ARX_VariableSpecs
    {
        #region Helper
        
        #endregion

        #region Variables

        /// <summary>
        /// The data type whose definition will be output.
        /// Can be in the form of a variable, class or function.
        /// </summary>
        public VariableGenOutputType me_output;

        /// <summary>
        /// The security scope of the variable. 
        /// Can be public, private or protected.
        /// </summary>
        public SecurityScope me_security;

        /// <summary>
        /// Is the variable static?
        /// </summary>
        public bool mb_isStatic;

        /// <summary>
        /// Is the variable partial?
        /// </summary>
        public bool mb_isPartial;

        /// <summary>
        /// Is the variable readonly?
        /// </summary>
        public bool mb_isReadOnly;

        /// <summary>
        /// If true, when initializing this variable, 
        /// the value will be a literal. Else, it will be a "new"
        /// </summary>
        public bool mb_isLiteral;

        /// <summary>
        /// The variable type or the function return type.
        /// Can be an int, float, bool or string.
        /// </summary>
        public PrimitiveVariable me_primitiveType;

        /// <summary>
        /// The variable type or the function return type.
        /// </summary>
        public string mstr_variableType;

        /// <summary>
        /// The name of the variable, class or function
        /// </summary>
        public string mstr_name;

        /// <summary>
        /// A list of function arguments
        /// </summary>
        [SerializeField]
        List<string> mastr_arguments;

        /// <summary>
        /// A list of classes this class is derived from
        /// </summary>
        [SerializeField]
        List<string> mastr_parentClasses;
      

        /// <summary>
        /// If true, the generated function will be given brackets.
        /// Else, it will be treated as a function preview.
        /// </summary>
        public bool mb_encloseInBrackets;

        /// <summary>
        /// The content of a function, accessor or class.
        /// This will be placed inbetween the variable's curly brackets.
        /// </summary>
        [TextArea(3, 7)]
        public string mstr_content;
        #endregion

        #region String

        public string Security { get
            {
                switch(me_security)
                {
                    case SecurityScope.Private:
                        return "private";
                    case SecurityScope.Protected:
                        return "proected";
                    case SecurityScope.Public:
                        return "public";
                    default:
                        return "";
                }

            } }
        #endregion

        #region Get Functions


        /// <summary>
        /// Combines this spec's parent classes into a single string for defining this class
        /// as a derived class, complete with a colon. Example output would be:
        /// " : ParentClassOne, ParentClassTwo"
        /// </summary>
        public string GetParentClassesString
        {
            get
            {
                return VariableGen.GetParentClassesString(this);
            }
        }

        /// <summary>
        /// Returns the list of argument strings as an array of strings
        /// </summary>
        public string[] GetArgumentsArray { get { return GetArguments.ToArray(); } }

        /// <summary>
        /// Initializes the array of argument strings if needed,
        /// then returns it.
        /// </summary>
        public List<string> GetArguments
        {
            get
            {

                if (mastr_arguments == null)
                    mastr_arguments = new List<string>();
                return mastr_arguments;

            }
        }

        /// <summary>
        /// Initialzes the array of parent class names if needed,
        /// then returns it.
        /// </summary>
        public List<string> GetParentClasses
        {
            get
            {
                if (mastr_parentClasses == null)
                    mastr_parentClasses = new List<string>();
                return mastr_parentClasses;
            }
        }

        /// <summary>
        /// Combines the list of argument strings into a single, comma-separated string,
        /// as one would see in a function header, then returns it.
        /// </summary>
        public string GetCombinedArgumentString
        {
            get
            {
                if (GetArguments.Count > 0)
                    return VariableGen.CombineArguments(GetArguments.ToArray());

                return "";
            }
        }

        /// <summary>
        /// If the mstr_variableType string is not empty, returns it.
        /// Else, returns a string definition of the me_primitiveType variable.
        /// </summary>
        public string GetVariableTypeString
        {
            get
            {
                if (string.IsNullOrEmpty(mstr_variableType) || mstr_variableType == "")
                    return VariableGen.GetPrimitiveVariable(me_primitiveType);
                return mstr_variableType;
            }
        }

        /// <summary>
        /// Returns a string of the generated class, variable or method
        /// using the given variableSpecs object.
        /// </summary>
        /// <param name="eOutputType"></param>
        /// <returns></returns>
        public static string GetOutput(ARX_VariableSpecs specs, VariableGenOutputType eOutputType)
        {
            VariableGenOutputType eBuf = specs.me_output;
            specs.me_output = eOutputType;
            string strReturn = specs.GetOutput();
            specs.me_output = eBuf;

            return strReturn;
        }

        /// <summary>
        /// Returns a string definition of either a class, method or variable
        /// depending on the value of the me_output variable.
        /// </summary>
        /// <returns></returns>
        public string GetOutput()
        {
            switch (me_output)
            {
                case VariableGenOutputType.Class:
                    return VariableGen.GenerateClass(this);
                case VariableGenOutputType.Method:
                    return VariableGen.GenerateMethod(this);
                case VariableGenOutputType.Variable:
                    return VariableGen.GenerateVariable(this);
                case VariableGenOutputType.MethodCall:
                    return VariableGen.GenerateMethodCall(this);
                case VariableGenOutputType.Accessor:
                    return VariableGen.GenerateAccessor(this);
                case VariableGenOutputType.AccessorCall:
                    return VariableGen.GenerateAccessorCall(this);
                case VariableGenOutputType.Enum:
                    return VariableGen.GenerateEnum(this);
                default:
                    return "No Output type defined.";
            }
        }

        #endregion

        #region Spawn 

        #endregion
    }
}