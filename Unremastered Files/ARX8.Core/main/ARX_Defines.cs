using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace ARX
{

    public enum DIRECTION { NORTH, SOUTH, EAST, WEST, UP, DOWN, NONE }

    public delegate void VoidDelegate();

    public static class Defines
    {
        public static DIRECTION[] CardinalDirections = new DIRECTION[] {DIRECTION.NORTH, DIRECTION.SOUTH, DIRECTION.EAST, DIRECTION.WEST };
    }
}
