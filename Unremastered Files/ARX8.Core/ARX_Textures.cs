using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARX
{
    public static class Textures
    {
        private static Texture2D _transparent;
        public static Texture2D Transparent
        {
            get
            {
                if (_transparent == null)
                {
                    _transparent = new Texture2D(1, 1);
                    _transparent.SetPixel(0, 0, Color.clear);
                }
                return _transparent;
            }
        }

        static Texture2D _yellow;

        public static Texture2D BlankYellowTexture
        {
            get
            {
                if (_yellow == null)
                    _yellow = new Texture2D(1, 1);
                _yellow.SetPixel(1, 1, Color.yellow);
                return _yellow;
            }
        }
    }
}
