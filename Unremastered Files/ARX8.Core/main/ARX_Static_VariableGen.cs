using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ARX
{
    namespace VarGen
    {
        #region Enums
        /// <summary>
        /// Determines the type of data output.
        /// </summary>
        public enum VariableGenOutputType { Method = 0, MethodCall, Accessor, AccessorCall, Variable, Class, Enum }

        /// <summary>
        /// Determines if the outputted data will be marked as public, private or protected.
        /// </summary>
        public enum SecurityScope { Public = 0, Private, Protected }

        /// <summary>
        /// Defines some common, primitive data types.
        /// </summary>
        public enum PrimitiveVariable { Int = 0, String, Float, Bool, Void }
        #endregion

        public static class Defines
        {
            public const string strUsing = "using ARX;\nusing FEN;\nusing UnityEngine;\n";
            /// <summary>
            /// Encloses the given string in a namespace
            /// </summary>
            /// <param name="str"></param>
            /// <param name="strNamespace"></param>
            /// <returns></returns>
            public static string EncloseInNamespace(string str, string strNamespace)
            {

                return "namespace " + strNamespace + "{\n\n" + str + "\n}";
            }

            public static string FullyQualify(string str, string strNamespace, string strAutoGenerationNote = "")
            {
                return strUsing + strAutoGenerationNote + "\n" + EncloseInNamespace(str, strNamespace);
            }
        }

        /// <summary>
        /// Contains functions to generate strings of functions, classes and variables.
        /// </summary>
        public static class VariableGen
        {
            #region Helpers

            /// <summary>
            /// Returns a string containing the given image's color. 
            /// The returned values for r, g, and b are between 0 and 255.
            /// The returned value for a is between 0 and 100.
            /// The returned string is in the standard format for Image Editors
            /// </summary>
            /// <param name="img"></param>
            /// <returns></returns>
            public static string[] ExtractColor255Alpha100(Image img)
            {
                Color col = img.color;
                const float max = 255;

                float r = max * col.r;
                float g = max * col.g;
                float b = max * col.b;
                float a;
                a = 100 * col.a;

                return new string[] { r.ToString(), g.ToString(), b.ToString(), a.ToString() };
            }

            /// <summary>
            /// Returns a string containing the given image's color. 
            /// The color values range from 0 to 1.
            /// The returned string is in the standard format for UnityEngine.UI elements.
            /// </summary>
            /// <param name="img"></param>
            /// <returns></returns>
            public static string ExtractColorOverOne(Image img)
            {
                return ExtractColorOverOne(img.color);
            }

            public static string ExtractColorOverOne(Color col)
            {
                float r = col.r;
                float g = col.g;
                float b = col.b;
                float a = col.a;

                return "(" + r + "F, " + g + "F, " + b + "F, " + a + "F);";
            }

            /// <summary>
            /// Returns string versions of the default values of common data types.
            /// For any other given data type, returns "default(givenDataType)"
            /// </summary>
            /// <param name="strClass"></param>
            /// <returns></returns>
            public static string GetDefault(string strClass)
            {
                switch (strClass)
                {
                    case "int":
                    case "double":
                    case "float":
                    case "long":
                        return "0";
                    case "string":
                        return "\"VarGenString\"";
                    case "bool":
                        return "false";
                    default:
                        return "default(" + strClass + ")";
                }
            }

            /// <summary>
            /// Encloses the given content in a #region preprocessor directive.
            /// </summary>
            /// <param name="strRegionName"></param>
            /// <param name="strContent"></param>
            /// <returns></returns>
            public static string EncloseInRegion(string strRegionName, string strContent)
            {
                return "#region " + strRegionName + "\n" + strContent + "\n#endregion";
            }

            /// <summary>
            /// Returns a string of the given security permission. 
            /// Returns either "private", "public" or "protected"
            /// </summary>
            /// <param name="scope"></param>
            /// <returns></returns>
            public static string GetSecurityScope(SecurityScope scope)
            {
                switch (scope)
                {
                    case SecurityScope.Private:
                        return "private";
                    case SecurityScope.Protected:
                        return "protected";
                    case SecurityScope.Public:
                        return "public";
                    default:
                        return scope.ToString().ToLower();
                }
            }

            /// <summary>
            /// Returns a string definition of the given primitive variable type.
            /// Returns either "float", "int", or "string"
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static string GetPrimitiveVariable(PrimitiveVariable type)
            {
                switch (type)
                {
                    case PrimitiveVariable.Float:
                        return "float";
                    case PrimitiveVariable.Int:
                        return "int";
                    case PrimitiveVariable.String:
                        return "string";
                    case PrimitiveVariable.Bool:
                        return "bool";
                    case PrimitiveVariable.Void:
                        return "void";
                    default:
                        return type.ToString().ToLower();
                }
            }

            /// <summary>
            /// Returns a string definition describing if a variable is static or not.
            /// Returns either "static" or nothing.
            /// </summary>
            /// <param name="bIsStatic"></param>
            /// <returns></returns>
            public static string GetStatic(bool bIsStatic)
            {
                if (bIsStatic)
                    return "static";
                else
                    return "";
            }

            /// <summary>
            /// Combines the given array of strings into a single comma-seperated string,
            /// such as one would see used as a function's arguments.
            /// </summary>
            /// <param name="astr"></param>
            /// <returns></returns>
            public static string CombineArguments(string[] astr)
            {
                string strArgument = "";
                for (int i = 0; i < astr.Length; i++)
                {
                    strArgument += astr[i];
                    if (i + 1 >= astr.Length)
                        continue;
                    strArgument += ", ";

                }
                return strArgument;
            }

            /// <summary>
            /// Combines this spec's parent classes into a single string for defining this class
            /// as a derived class, complete with a colon. Example output would be:
            /// " : ParentClassOne, ParentClassTwo"
            /// </summary>
            /// <param name="specs"></param>
            /// <returns></returns>
            public static string GetParentClassesString(ARX_VariableSpecs specs)
            {
                string strArgument = "";
                string[] astr = specs.GetParentClasses.ToArray();
                for (int i = 0; i < astr.Length; i++)
                {
                    if (i == 0)
                        strArgument += ": ";

                    strArgument += astr[i];
                    if (i + 1 >= astr.Length)
                        continue;
                    strArgument += ", ";

                }
                return strArgument;
            }

            #endregion

            #region Generate Method Definitions

            /// <summary>
            /// Generate a Function/Method definition
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="isStatic"></param>
            /// <param name="strPrimitiveVariableType"></param>
            /// <param name="strName"></param>
            /// <param name="astrArguments"></param>
            /// <param name="strContent"></param>
            /// <param name="bEncloseInBrackets"></param>
            /// <returns></returns>
            public static string GenerateMethod(SecurityScope scope, bool isStatic, PrimitiveVariable strPrimitiveVariableType,
                string strName, string[] astrArguments, string strContent, bool bEncloseInBrackets = true)
            {
                string strVariable = GetPrimitiveVariable(strPrimitiveVariableType);
                return GenerateMethod(scope, isStatic, strVariable, strName, astrArguments, strContent, bEncloseInBrackets);
            }

            /// <summary>
            /// Generate a Function/Method definition
            /// </summary>
            /// <param name="oBase"></param>
            /// <returns></returns>
            public static string GenerateMethod(ARX_VariableSpecs oBase)
            {
                return GenerateMethod(oBase.me_security, oBase.mb_isStatic, oBase.GetVariableTypeString, oBase.mstr_name, oBase.GetArgumentsArray, oBase.mstr_content, oBase.mb_encloseInBrackets);
            }

            /// <summary>
            /// Generate a Function/Method definition
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="isStatic"></param>
            /// <param name="strVariableType"></param>
            /// <param name="strName"></param>
            /// <param name="astrArguments"></param>
            /// <param name="strContent"></param>
            /// <param name="bEncloseInBrackets"></param>
            /// <returns></returns>
            public static string GenerateMethod(SecurityScope scope, bool isStatic, string strVariableType,
               string strName, string[] astrArguments, string strContent, bool bEncloseInBrackets = true)
            {
                string strReturn = "";

                string strSecurity = GetSecurityScope(scope);
                string strStatic = GetStatic(isStatic);
                string strArgument = CombineArguments(astrArguments);

                strReturn += strSecurity + " ";
                if (strStatic != "")
                    strReturn += strStatic + " ";

                strReturn += strVariableType + " " + strName;

                if (bEncloseInBrackets)
                {
                    strReturn += "(" + strArgument + ") \n{\n";
                    if (strContent == "")
                        strReturn += "\tthrow new Exception();";
                    else
                        strReturn += strContent;
                    strReturn += "\n}\n";
                }
                else
                    strReturn += "(" + strArgument + ");";


                return strReturn;
            }

            #endregion//Generate Method Definitions

            #region Generate Method Call
            public static string GenerateMethodCall(string strMethodName, string strArgument)
            {
                return strMethodName + "(" + strArgument + ")";
            }

            public static string GenerateMethodCall(string strMethodName, string[] strArguments)
            {
                string strCombinedArguments = CombineArguments(strArguments);
                return GenerateMethodCall(strMethodName, strCombinedArguments);
            }

            public static string GenerateMethodCall(ARX_VariableSpecs specs, bool isAccessor = false)
            {
                if (isAccessor)
                    return GenerateAccessorCall(specs);
                return GenerateMethodCall(specs.mstr_name, specs.GetArgumentsArray);
            }

            #endregion

            #region Generate Accessor

            /// <summary>
            /// Generate an Accessor
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="isStatic"></param>
            /// <param name="strPrimitiveVariableType"></param>
            /// <param name="strName"></param>
            /// <param name="strContent"></param>
            /// <param name="bEncloseInBrackets"></param>
            /// <returns></returns>
            public static string GenerateAccessor(SecurityScope scope, bool isStatic, PrimitiveVariable strPrimitiveVariableType,
                string strName, string strContent, bool bEncloseInBrackets = true)
            {

                return GenerateAccessor(scope, isStatic, GetPrimitiveVariable(strPrimitiveVariableType), strName,
                    strContent, bEncloseInBrackets);
            }



            /// <summary>
            /// Generate an Accessor
            /// </summary>
            /// <param name="specs"></param>
            /// <returns></returns>
            public static string GenerateAccessor(ARX_VariableSpecs specs)
            {
                return GenerateAccessor(specs.me_security, specs.mb_isStatic, specs.GetVariableTypeString, specs.mstr_name, specs.mstr_content, specs.mb_encloseInBrackets);
            }

            /// <summary>
            /// Generate an Accessor
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="isStatic"></param>
            /// <param name="strVariableType"></param>
            /// <param name="strName"></param>
            /// <param name="strContent"></param>
            /// <param name="bEncloseInBrackets"></param>
            /// <returns></returns>
            public static string GenerateAccessor(SecurityScope scope, bool isStatic, string strVariableType,
               string strName, string strContent, bool bEncloseInBrackets = true)
            {
                string strReturn = "";

                string strSecurity = GetSecurityScope(scope);
                string strStatic = GetStatic(isStatic);

                strReturn += strSecurity + " ";
                if (strStatic != "")
                    strReturn += strStatic + " ";

                strReturn += strVariableType + " " + strName;

                if (bEncloseInBrackets)
                {
                    strReturn += "\n{\n\tget\n\t{\n\t\t";
                    if (strContent == "")
                        strReturn += "return " + GetDefault(strVariableType) + ";";
                    else
                        strReturn += strContent;
                    strReturn += "\n\t}\n\t\n}";
                }
                else
                    strReturn += "();";


                return strReturn;
            }
            #endregion

            #region Generate Accessor Call
            /// <summary>
            /// Generates an Accessor Call.
            /// Even though those are just literally the method's name...
            /// </summary>
            /// <param name="strMethodName"></param>
            /// <returns></returns>
            public static string GenerateAccessorCall(string strMethodName)
            {
                return strMethodName;
            }

            /// <summary>
            /// Generates an Accessor Call.
            /// Even though those are just the method's name...
            /// </summary>
            /// <param name="specs"></param>
            /// <returns></returns>
            public static string GenerateAccessorCall(ARX_VariableSpecs specs)
            {
                return GenerateAccessorCall(specs.mstr_name);
            }
            #endregion

            #region Generate Variable Definitions

            /// <summary>
            /// Generate a variable definition
            /// </summary>
            /// <param name="oBase"></param>
            /// <returns></returns>
            public static string GenerateVariable(ARX_VariableSpecs oBase)
            {
                string strName = oBase.mstr_name;

                if (oBase.GetArguments.Count > 0)
                    strName += ", " + oBase.GetCombinedArgumentString;

                return GenerateVariable(oBase.me_security, oBase.mb_isStatic, oBase.mb_isReadOnly, oBase.mb_isLiteral,
                    oBase.GetVariableTypeString, strName, oBase.mstr_content);
            }

            /// <summary>
            /// Generate a variable definition
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="isStatic"></param>
            /// <param name="strPrimitiveVariable"></param>
            /// <param name="strName"></param>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static string GenerateVariable(SecurityScope scope, bool isStatic, bool isReadOnly, bool isLiteral, PrimitiveVariable strPrimitiveVariable,
               string strName, string strValue = "")
            {
                string strVariable = GetPrimitiveVariable(strPrimitiveVariable);
                return GenerateVariable(scope, isStatic, isReadOnly, isLiteral, strVariable, strName, strValue);
            }

            /// <summary>
            /// Generate a variable definition
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="isStatic"></param>
            /// <param name="strVariableType"></param>
            /// <param name="strName"></param>
            /// <param name="strValue"></param>
            /// <returns></returns>
            public static string GenerateVariable(SecurityScope scope, bool isStatic, bool isReadOnly, bool bLiteral, string strVariableType,
           string strName, string strValue = "")
            {
                if (strValue == null)
                    strValue = "";

                    strValue = strValue.TrimWhitespace();

                string strReturn = "";

                string strSecurity = GetSecurityScope(scope);
                string strStatic = GetStatic(isStatic);

                if (strSecurity != "")
                    strReturn += strSecurity + " ";

                if (strStatic != "")
                    strReturn += strStatic + " ";

                if (isReadOnly)
                    strReturn += "readonly ";

                strReturn += strVariableType + " " + strName;

                //If the value string is not blank, add it to the return
                if (strValue != "")
                {
                    Debug.Log("Variable " + strName + " value is |" + strValue + "|");
                    if (bLiteral == false)
                    {
                        strValue = "new " + strVariableType + "(" + strValue + ")";
                    }
                    else
                    {
                        switch (strVariableType)
                        {
                            case "float":
                                strValue += "F";
                                break;
                            case "string":
                                strValue = "\"" + strValue + "\"";
                                break;
                        }
                    }

                    strReturn += " = " + strValue;
                }
                strReturn += ";";
                return strReturn;
            }
            #endregion//Generate Variable Definitions

            #region Generate Class Definitions

            /// <summary>
            /// Generate Class Definitions
            /// </summary>
            /// <param name="oBase"></param>
            /// <returns></returns>
            public static string GenerateClass(ARX_VariableSpecs oBase)
            {
                return GenerateClass(oBase.me_security, oBase.mb_isStatic, oBase.mb_isPartial, oBase.mstr_name, oBase.GetParentClassesString, oBase.mstr_content);
            }

            /// <summary>
            /// Generate Class Definitions
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="isStatic"></param>
            /// <param name="strName"></param>
            /// <param name="strContent"></param>
            /// <returns></returns>
            public static string GenerateClass(SecurityScope scope, bool isStatic, bool isPartial, string strName, string strParentClasses,
                string strContent = "")
            {
                string strReturn = "";
                string strScope = GetSecurityScope(scope);
                string strStatic = GetStatic(isStatic);

                if (strScope != "")
                    strReturn += strScope + " ";

                if (strStatic != "")
                    strReturn += strStatic + " ";

                if (isPartial)
                    strReturn += "partial ";
                strReturn += "class " + strName + strParentClasses + "\n{\n";
                strReturn += strContent;
                strReturn += "\n}";

                return strReturn;
            }

            /// <summary>
            /// Generate Class Definitions
            /// </summary>
            /// <param name="scope"></param>
            /// <param name="isStatic"></param>
            /// <param name="strName"></param>
            /// <param name="oContent"></param>
            /// <returns></returns>
            public static string GenerateClass(SecurityScope scope, bool isStatic, string strName, string strParentClasses,
                ARX_VariableSpecs oContent)
            {
                string strReturn = "";
                string strScope = GetSecurityScope(scope);
                string strStatic = GetStatic(isStatic);

                strReturn += strScope + " " + strStatic + " class " + strName;

                //strReturn += oContent.GenerateDerivedClassCall();

                strReturn += "\n{\n";
                strReturn += "//" + strName;
                //strReturn += oContent.GenerateClassContent();
                strReturn += "\n}";

                return strReturn;
            }

            #endregion//Generate Class

            #region Generate Enum
            public static string GenerateEnum(ARX_VariableSpecs specs)
            {
                string strReturn = "";
                strReturn += specs.Security + " enum " + specs.mstr_name + "{";

                for (int i = 0; i < specs.GetArguments.Count; i++)
                {
                    strReturn += specs.GetArguments[i] + "=" + i;
                    if (i + 1 == specs.GetArguments.Count)
                    {

                    }
                    else
                    {
                        strReturn += ", ";
                    }
                }
                strReturn += "};";

                return strReturn;
            }
            #endregion

        }
    }
}