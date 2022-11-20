using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cycle
{

    // The GameObject class from which other classes will extend.
    interface GameObject
    {
        public void update();
    }
}