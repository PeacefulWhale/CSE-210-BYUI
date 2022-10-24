using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace RFK
{
    class Program
    {
        static void Main(string[] args)
        {
            // In case it can't load the file, I have a few lines here.
            string[] messages = {
                "You found kitten!",
                "'I pity the fool who mistakes me for kitten!', sez Mr. T.",
                "That's just an old tin can.",
                "It's an altar to the horse god.",
                "A box of dancing mechanical pencils. They dance! They sing!",
                "It's an old Duke Ellington record.",
                "A box of fumigation pellets.",
                "A digital clock. It's stuck at 2:17 PM.",
                "That's just a charred human corpse.",
                "I don't know what that is, but it's not kitten."
            };
            if (System.IO.File.Exists("./Messages.txt"))
            {
                messages = System.IO.File.ReadAllLines("./Messages.txt");
            }

            // Scene initialization.
            Random rnd = new Random();
            const int width = 1200;
            const int height = 600;
            const int w_buffer = 15;
            const int h_buffer = 15;
            Camera2D main_camera = new Camera2D();
            main_camera.target = new Vector2(0.0f, 0.0f);
            // Player object.
            SceneHandler main_scene = new SceneHandler(height, width, "Robot Finds Kitten", 60, h_buffer, w_buffer, Color.BLACK, main_camera);
            main_scene.addPlayer(new Player(main_scene, rnd));
            foreach (string message in messages)
            {
                main_scene.addArtifact(new Artifact(main_scene, message, rnd, message == "You found kitten!"));
            }
            // Call the main game loop.
            main_scene.mainLoop();
        }
    }
}