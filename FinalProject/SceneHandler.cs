using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace FinalProject
{
    /// <summary>
    /// The scene handler for the game.
    /// This is also the only class that requires Raylib, hopefully making it easy to swap to a different graphics library in the future.
    /// </summary>
    class SceneHandler
    {
        const float jump_force = 5f;
        const float move_speed = 100f;
        const float gravity = 3f;
        const float time_multiple = 2f;
        private int fps, text_size;
        private Vector2 size;
        private Vector2 buffer;
        private Color background;
        private Camera2D camera;
        private Random rnd;

        // Game Objects.
        public List<PlatformObject> platforms;
        public Player player;

        public SceneHandler(int height, int width, string scene_name, int fps, int h_buffer, int w_buffer, Vector4 background, int text_size)
        {
            this.rnd = new Random();
            this.size.Y = height;
            this.size.X = width;
            this.fps = fps;
            this.text_size = text_size;
            // The buffer is where the UI and stuff will be stuck.
            this.buffer.Y = h_buffer;
            this.buffer.X = w_buffer;
            this.background = convertColor(background);
            InitWindow(width + w_buffer, height + h_buffer, scene_name);
            SetTargetFPS(fps);

        }

        // Loop to be called externally.
        public void mainLoop()
        {
            bool nextRound = true;
            while (nextRound && !Raylib.WindowShouldClose())
            {
                nextRound = this.game_loop();
            }
        }

        private bool game_loop()
        {
            // Initialize Game.
            this.camera = new Camera2D();
            this.camera.target = new Vector2(0.0f, 0.0f);
            this.camera.zoom = 1;
            this.camera.rotation = 0;
            this.camera.offset = new Vector2(0, 0);

            // Create initial platforms.
            this.platforms = new List<PlatformObject>();
            this.platforms.Add(new BasePlatform(new Vector2(this.size.X / 2, this.size.Y + 10), 0.1f, 0));
            this.platforms.Add(new BasePlatform(new Vector2(this.size.X / 2, this.size.Y - 150), 0.1f, 0));
            this.platforms.Add(new BasePlatform(new Vector2(this.size.X / 2, this.size.Y - 300), 0.1f, 0));
            this.platforms.Add(new BasePlatform(new Vector2(this.size.X / 2, this.size.Y - 450), 0.1f, 0));
            this.platforms.Add(new BasePlatform(new Vector2(this.size.X / 2, this.size.Y - 600), 0.1f, 0));
            this.platforms.Add(new BasePlatform(new Vector2(this.size.X / 2, this.size.Y - 750), 0.1f, 0));

            Player player = new Player(new Vector2(this.size.X / 2, this.size.Y - 100), 0.1f, 0, move_speed, jump_force * time_multiple, gravity);
            this.player = player;

            bool game_over = false;
            while (!Raylib.WindowShouldClose() && !game_over)
            {
                // Check to see if the game is over or not.
                if (this.player.pos.Y > this.camera.target.Y + this.size.Y + 64)
                {
                    BeginDrawing();
                    BeginMode2D(camera);
                    ClearBackground(this.background);
                    DrawText("Score: " + (int)((this.player.pos.Y * -1) + this.size.Y), 5, (int)this.camera.target.Y, 25, Color.BLACK);
                    DrawText("Game Over", 25, (int)(this.camera.target.Y + (this.size.Y / 2)), 100, Color.BLACK);
                    DrawText("Press Space to\nPlay Again", 25, (int)(this.camera.target.Y + (this.size.Y / 2) + 100), 50, Color.BLACK);
                    EndDrawing();
                    if (IsKeyPressed(KeyboardKey.KEY_SPACE))
                    {
                        game_over = true;
                    }
                }
                else
                {
                    update();
                    draw();
                }
            }
            return game_over;
        }

        private void update()
        {
            // Delta Time for use in physics and stuff.
            // I feel like the game should be faster...
            float deltaTime = GetFrameTime() * time_multiple;

            // Player updates first.
            this.player.update(deltaTime, this.platforms);

            // Update the camera.
            if (this.camera.target.Y > this.player.pos.Y - 250)
            {
                this.camera.target.Y = this.player.pos.Y - 250;
                // If the camera has to be updated, then new platforms should be spawned.
                Vector2 new_loc = new Vector2((float)this.rnd.NextDouble() * this.size.X, ((float)this.rnd.NextDouble() * 50) + (this.camera.target.Y - 100));
                double chance = this.rnd.NextDouble();
                // Probabilities are multiplied by the time multiple, as platforms will have less chance to spawn the faster time moves.
                if (chance < 0.001 * time_multiple)
                {
                    this.platforms.Add(new ExtraBoostedPlatform(new_loc, 0.1f, 0));
                }
                else if (chance < 0.01 * time_multiple)
                {
                    this.platforms.Add(new BoostedPlatform(new_loc, 0.1f, 0));
                }
                else if (chance < 0.025 * time_multiple)
                {
                    this.platforms.Add(new BasePlatform(new_loc, 0.1f, 0));
                }
                else if (chance < 0.05 * time_multiple)
                {
                    this.platforms.Add(new WeakPlatform(new_loc, 0.1f, 0));
                }
            }

            // Update the platforms.
            List<PlatformObject> to_delete = new List<PlatformObject>();
            foreach (PlatformObject obj in this.platforms)
            {
                obj.update(deltaTime);
                // Delete platforms that are too low.
                if (obj.pos.Y > this.camera.target.Y + (this.size.Y * 2))
                {
                    to_delete.Add(obj);
                }
            }
            foreach (PlatformObject obj in to_delete)
            {
                this.platforms.Remove(obj);
            }
        }

        // I can do all the drawing in here, that way there is no need for Raylib stuff outside of this class.
        private void draw()
        {
            BeginDrawing();
            BeginMode2D(camera);
            ClearBackground(this.background);
            // Draw Score.
            DrawText("Score: " + (int)((this.player.pos.Y * -1) + this.size.Y), 5, (int)this.camera.target.Y, 25, Color.BLACK);
            foreach (PlatformObject obj in this.platforms)
            {
                obj.draw();
            }
            // Player draws last.
            this.player.draw();
            EndDrawing();
        }

        private Color convertColor(Vector4 color)
        {
            return new Color((int)color.X, (int)color.Y, (int)color.Z, (int)color.W);
        }
    }
}