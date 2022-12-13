using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace FinalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            const int width = 600;
            const int height = 600;
            const int w_buffer = 50;
            const int h_buffer = 50;
            InitAudioDevice();
            SceneHandler main_scene = new SceneHandler(height, width, "JUMP", 60, h_buffer, w_buffer, new Vector4(255, 255, 255, 255), 15);
            main_scene.mainLoop();
            CloseAudioDevice();
        }
    }
}
