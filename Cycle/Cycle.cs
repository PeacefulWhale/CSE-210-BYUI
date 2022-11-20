using System;
using System.Numerics;

namespace Cycle
{
    class Program
    {
        static void Main(string [] args)
        {
            const int width = 600;
            const int height = 600;
            const int w_buffer = 50;
            const int h_buffer = 50;
            SceneHandler main_scene = new SceneHandler(height, width, "Cycle", 60, h_buffer, w_buffer, new Vector4(0, 0, 0, 0), 15, 0.1f);
            main_scene.mainLoop();
        }
    }
}