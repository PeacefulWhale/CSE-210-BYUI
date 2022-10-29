using System;
using System.Numerics;
using Raylib_cs;

namespace Greed
{
    class Program
    {
        static void Main(string [] args)
        {
            const int width = 600;
            const int height = 600;
            const int w_buffer = 15;
            const int h_buffer = 15;
            const float player_speed = 200.0f;
            Camera2D main_camera = new Camera2D();
            main_camera.target = new Vector2(0.0f, 0.0f);
            SceneHandler main_scene = new SceneHandler(height, width, "Greed", 60, h_buffer, w_buffer, Color.BLACK, main_camera);
            main_scene.player = new Object("#", Color.WHITE, 25, new Vector2(0, height), player_speed);
            main_scene.mainLoop();
        }
    }
}