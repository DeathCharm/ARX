using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace ARX
{

    public static partial class ToolBox
    {
        /// <summary>
        /// A class full of shameful math functions for the 
        /// mentally inferior programmer.
        /// </summary>
        public static class Derp
        {
            /// <summary>
            /// Returns the Multiplicative dif between the two values.
            /// For those whose memory is so poor they can't remember that
            /// "Difference equals New divided by Old
            /// </summary>
            /// <param name="newFloat"></param>
            /// <param name="oldFloat"></param>
            /// <returns></returns>
            public static float MultChange(float newFloat, float oldFloat)
            {
                return newFloat / oldFloat;
            }

        }

        public static string GetBottomType(System.Type eType)
        {
            string[] astr = eType.ToString().Split('+');

            return astr[astr.Length - 1];
        }

        public static string GetQualifiedType(System.Type eType)
        {
            string[] astr = eType.ToString().Split('+');
            string strReturn = "";

            for (int i = 0; i < astr.Length; i++)
            {
                string s = astr[i];
                strReturn += s;

                if (i + 1 != astr.Length)
                    strReturn += ".";
            }


            return strReturn;
        }

        public static Vector3 Vector(this Color c)
        {
            return new Vector3(c.r, c.g, c.b);
        }

        public static Color Color(this Vector3 vec, float a)
        {
            return new Color(vec.x, vec.y, vec.z, a);
        }

        public static float Sin(float waveSpeed)
        {
            //offset = waveHeight * Mathf.Sin((Time.time * waveSpeed) + (i * waveFrequency));
            return Mathf.Sin((Time.time * waveSpeed));
        }

        public static GameObject ToggleActive(this GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
            return obj;

        }

        public static Vector3 MultiplyVector3(Vector3 one, Vector3 two)
        {
            return new Vector3(one.x * two.x, one.y * two.y, one.z * two.z);
        }

        public static T GetRandom<T>(this List<T> buf)
        {
            if (buf.Count == 0)
                return default(T);

            int nRandom = UnityEngine.Random.Range(0, buf.Count);
            return buf[nRandom];
        }

        public static string ColorTag(Color color, string str)
        {
            string strColor = ColorUtility.ToHtmlStringRGBA(color);

            return "<color=#" + strColor + ">" + str + "</color>";
        }

        public static string Capitalize(string s)
        {
            if (s == "")
                return s;

            char c = s[0];

            s = s.Remove(0, 1);
            s = c.ToString().ToUpper() + s;
            return s;
        }

        /// <summary>
        /// Courtesy of https://www.exchangecore.com/blog/convert-number-words-c-sharp-console-application/
        /// Turns a given number value into prose.
        /// Ex. "3.4" becomes "three and four tenths"
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String NumWordsWrapper(double n, string strSingular = "", string strPlural = "")
        {
            string words = "";
            double intPart;
            double decPart = 0;
            if (n == 0)
            {
                if (strPlural != "")
                    return "zero " + strPlural;
                else
                    return "zero";
            }
            try
            {
                string[] splitter = n.ToString().Split('.');
                intPart = double.Parse(splitter[0]);
                decPart = double.Parse(splitter[1]);
            }
            catch
            {
                intPart = n;
            }

            words = NumWords(intPart);

            if (decPart > 0)
            {
                if (words != "")
                    words += " and ";
                int counter = decPart.ToString().Length;
                switch (counter)
                {
                    case 1: words += NumWords(decPart) + " tenths"; break;
                    case 2: words += NumWords(decPart) + " hundredths"; break;
                    case 3: words += NumWords(decPart) + " thousandths"; break;
                    case 4: words += NumWords(decPart) + " ten-thousandths"; break;
                    case 5: words += NumWords(decPart) + " hundred-thousandths"; break;
                    case 6: words += NumWords(decPart) + " millionths"; break;
                    case 7: words += NumWords(decPart) + " ten-millionths"; break;
                }
            }

            if (n == 1)
                words += " " + strSingular;
            else if (n > 1)
                words += " " + strPlural;

            return words;
        }

        /// <summary>
        /// Removes the final character from a given string
        /// if that character is a newline.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveFinalNewline(string str)
        {
            string retstr = str;
            if (str.EndsWith("\n"))
            {
                int nStartIndex = str.Length - 1;
                retstr = str.Remove(nStartIndex, 1);
            }
            return retstr;
        }

        /// <summary>
        /// Returns the direction enum of the given vector.
        /// Checks the given vector against Vector2.Up, Vector2.Down, etc.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static DIRECTION GetDirection(Vector2 vec)
        {
            if (vec.y > 0)
                vec.y = 1;
            else if (vec.y < 0)
                vec.y = -1;

            if (vec.x > 0)
                vec.x = 1;
            else if (vec.x < 0)
                vec.x = -1;

            if (vec == Vector2.up)
                return DIRECTION.NORTH;
            if (vec == Vector2.down)
                return DIRECTION.SOUTH;
            if (vec == Vector2.right)
                return DIRECTION.EAST;
            if (vec == Vector2.left)
                return DIRECTION.WEST;
            return DIRECTION.NONE;
        }

        /// <summary>
        /// Returns a random color whose values
        /// range from 0 to 1
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColorOverOne()
        {
            float r = UnityEngine.Random.Range(0F, 1F);
            float g = UnityEngine.Random.Range(0F, 1F);
            float b = UnityEngine.Random.Range(0F, 1F);
            
            return new Color(r, g, b, 1);
            
        }

        /// <summary>
        /// Returns the directio opposite to the given direction
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static DIRECTION GetOppositeDirection(DIRECTION dir)
        {
            switch (dir)
            {
                case DIRECTION.EAST:
                    return DIRECTION.WEST;
                case DIRECTION.NORTH:
                    return DIRECTION.SOUTH;
                case DIRECTION.SOUTH:
                    return DIRECTION.NORTH;
                case DIRECTION.WEST:
                    return DIRECTION.EAST;
                case DIRECTION.UP:
                    return DIRECTION.DOWN;
                case DIRECTION.DOWN:
                    return DIRECTION.UP;
                default:
                    return DIRECTION.NONE;

            }
        }

        /// <summary>
        /// Returns all directions perpendicular to the given direction.
        /// Will not return DIRECTION.UP or DIRECTION.DOWN unless includeVertical is true.
        /// When given a vertical direction (UP or DOWN), 
        /// will return all non-vertical directions (NORTH, SOUTH, EAST, WEST)
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="includeVertical"></param>
        /// <returns></returns>
        public static DIRECTION[] GetPerpendicularDirection(DIRECTION dir, bool includeVertical = false)
        {

            //Vertical NOT included
            if (includeVertical == false)
                switch (dir)
                {
                    case DIRECTION.EAST:
                        return new DIRECTION[] { DIRECTION.NORTH, DIRECTION.SOUTH };
                    case DIRECTION.NORTH:
                        return new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST };
                    case DIRECTION.SOUTH:
                        return new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST };
                    case DIRECTION.WEST:
                        return new DIRECTION[] { DIRECTION.NORTH, DIRECTION.SOUTH };
                    case DIRECTION.UP:
                        return new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST, DIRECTION.NORTH, DIRECTION.SOUTH };
                    case DIRECTION.DOWN:
                        return new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST, DIRECTION.NORTH, DIRECTION.SOUTH };
                    default:
                        return new DIRECTION[] { };
                }

            else
                //Vertical included
                switch (dir)
                {
                    case DIRECTION.EAST:
                        return new DIRECTION[] { DIRECTION.NORTH, DIRECTION.SOUTH, DIRECTION.UP, DIRECTION.DOWN };
                    case DIRECTION.NORTH:
                        return new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST, DIRECTION.UP, DIRECTION.DOWN };
                    case DIRECTION.SOUTH:
                        return new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST, DIRECTION.UP, DIRECTION.DOWN };
                    case DIRECTION.WEST:
                        return new DIRECTION[] { DIRECTION.NORTH, DIRECTION.SOUTH, DIRECTION.UP, DIRECTION.DOWN };
                    case DIRECTION.UP:
                        return new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST, DIRECTION.NORTH, DIRECTION.SOUTH };
                    case DIRECTION.DOWN:
                        return new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST, DIRECTION.NORTH, DIRECTION.SOUTH };
                    default:
                        return new DIRECTION[] { };
                }
        }

        /// <summary>
        /// Returns the Vector3Int of a given direction.
        /// The returned Vector's values will be between -1 and 1
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Vector3Int GetTranslation(DIRECTION dir)
        {
            switch (dir)
            {
                case DIRECTION.EAST:
                    return Vector3Int.right;
                case DIRECTION.NORTH:
                    return new Vector3Int(0, 0, 1);
                case DIRECTION.SOUTH:
                    return new Vector3Int(0, 0, -1);
                case DIRECTION.WEST:
                    return Vector3Int.left;
                case DIRECTION.UP:
                    return Vector3Int.up;
                case DIRECTION.DOWN:
                    return Vector3Int.down;
                default:
                    return Vector3Int.zero;

            }
        }


        /// <summary>
        /// Courtesy of https://www.exchangecore.com/blog/convert-number-words-c-sharp-console-application/
        /// Converts the given number value to words
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static String NumWords(double n) //converts double to words
        {
            string[] numbersArr = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tensArr = new string[] { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninty" };
            string[] suffixesArr = new string[] { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
            string words = "";

            bool tens = false;

            if (n < 0)
            {
                words += "negative ";
                n *= -1;
            }

            int power = (suffixesArr.Length + 1) * 3;

            while (power > 3)
            {
                double pow = Math.Pow(10, power);
                if (n >= pow)
                {
                    if (n % pow > 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1] + ", ";
                    }
                    else if (n % pow == 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1];
                    }
                    n %= pow;
                }
                power -= 3;
            }
            if (n >= 1000)
            {
                if (n % 1000 > 0) words += NumWords(Math.Floor(n / 1000)) + " thousand, ";
                else words += NumWords(Math.Floor(n / 1000)) + " thousand";
                n %= 1000;
            }
            if (0 <= n && n <= 999)
            {
                if ((int)n / 100 > 0)
                {
                    words += NumWords(Math.Floor(n / 100)) + " hundred";
                    n %= 100;
                }
                if ((int)n / 10 > 1)
                {
                    if (words != "")
                        words += " ";
                    words += tensArr[(int)n / 10 - 2];
                    tens = true;
                    n %= 10;
                }

                if (n < 20 && n > 0)
                {
                    if (words != "" && tens == false)
                        words += " ";
                    words += (tens ? "-" + numbersArr[(int)n - 1] : numbersArr[(int)n - 1]);
                    n -= Math.Floor(n);
                }
            }

            return words;

        }

        /// <summary>
        /// Courtesy of Amit Sanandiya https://www.c-sharpcorner.com/blogs/first-letter-in-uppercase-in-c-sharp1
        /// Changes the first character of each word in the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.  
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.  
            // ... Uppercase the lowercase letters following spaces.  
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        /// <summary>
        /// Returns the given color, but with the given nfAlpha value
        /// </summary>
        /// <param name="oBase"></param>
        /// <param name="nfAlpha"></param>
        /// <returns></returns>
        public static Color GetColorWithAlpha(this Color oBase, float nfAlpha)
        {
            return new Color(oBase.r, oBase.g, oBase.b, nfAlpha);
        }

        /// <summary>
        /// Creates a copy of this component and attaches it to the given gameobject using Reflection.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static Component CopyComponentTo(this Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }

        /// <summary>
        /// Copies the fields of one object to another
        /// </summary>
        /// <param name="original"></param>
        /// <param name="destination"></param>
        public static void HardCopyTo(this UnityEngine.Object original, UnityEngine.Object destination)
        {
            System.Type type = original.GetType();
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(destination, field.GetValue(original));
            }
        }

        /// <summary>
        /// Deletes this object's children.
        /// </summary>
        /// <param name="trans"></param>
        public static void DeleteChildren(this Transform trans)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                int nChildCountBeforeDestroy = trans.childCount;

                Transform child = trans.GetChild(i);

                if (Application.isPlaying == false)
                    GameObject.DestroyImmediate(child.gameObject);
                else
                    GameObject.Destroy(child.gameObject);

                //Destroy and DestroyImmediate act differently in regards to
                //changing a transform's childcount during a loop.
                if (nChildCountBeforeDestroy != trans.childCount)
                {
                    i--;
                }

            }
        }

        /// <summary>
        /// Returns a random direction perpendicular to the given direction.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="bIncludeVertical"></param>
        /// <returns></returns>
        public static DIRECTION GetRandomPerpendicularDirection(DIRECTION dir, bool bIncludeVertical = false)
        {
            DIRECTION[] eaDirections = GetPerpendicularDirection(dir, bIncludeVertical);

            int nRandom = UnityEngine.Random.Range(0, eaDirections.Length);
            return eaDirections[nRandom];
        }

        /// <summary>
        /// Copies the given component to the given gameobject using Reflection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static T CopyComponentTo<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }

        /// <summary>
        /// Converts the mouse's current Input.mousePosition to a position on a Canvas' Rect Transform.
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="bCenterImage"></param>
        /// <returns></returns>
        public static Vector3 GetCanvasPositionOfMouse(RectTransform rectTransform, bool bCenterImage = true)
        {
            Vector2 vec = Input.mousePosition;

            if (bCenterImage)
                vec -= new Vector2(rectTransform.sizeDelta.x, 0);


            Vector2 vecHalfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            vecHalfScreen.x -= rectTransform.rect.width / 2;
            vecHalfScreen.x -= rectTransform.rect.height / 2;
            return vec - vecHalfScreen;
        }
        

        /// <summary>
        /// Returns a random direction.
        /// </summary>
        /// <param name="includeVertical"></param>
        /// <returns></returns>
        public static DIRECTION GetRandomDirection(bool includeVertical = false)
        {
            if (includeVertical)
            {
                DIRECTION[] aDirectionArray = new DIRECTION[] { DIRECTION.UP, DIRECTION.DOWN, DIRECTION.EAST, DIRECTION.WEST, DIRECTION.NORTH, DIRECTION.SOUTH };
                int nRandom = UnityEngine.Random.Range(0, aDirectionArray.Length);
                return aDirectionArray[nRandom];
            }
            else
            {
                DIRECTION[] aDirectionArray = new DIRECTION[] { DIRECTION.EAST, DIRECTION.WEST, DIRECTION.NORTH, DIRECTION.SOUTH };
                int nRandom = UnityEngine.Random.Range(0, aDirectionArray.Length);
                return aDirectionArray[nRandom];
            }
        }
    }
}