using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Greed
{
    class SceneHandler
    {
        // Constants.
        private const float fall_speed = 100.0f;
        // Variables.
        private int fps;
        private Vector2 size;
        private Vector2 buffer;
        private Color background;
        private Camera2D camera;
        public Object player;
        private List<FallingObject> falling;

        // Game Variables.
        private int score = 0;
        // Starting chance for the rocks and gem.
        private float rock_chance = 0.0f;
        private float gem_chance = .1f;
        // Change in the rock and gem chances per second.
        private float chance_change = 0.001f;
        private Random rnd;
        public SceneHandler(int height, int width, string scene_name, int fps, int h_buffer, int w_buffer, Color background, Camera2D camera)
        {
            this.rnd = new Random();
            this.size.Y = height;
            this.size.X = width;
            this.fps = fps;
            // The buffer is where the UI and stuff will be stuck.
            this.buffer.Y = h_buffer;
            this.buffer.X = w_buffer;
            this.background = background;
            this.camera = camera;
            this.falling = new List<FallingObject>();
            InitWindow(width + w_buffer, height + h_buffer, scene_name);
            SetTargetFPS(fps);
        }

        // I could make separate classes for drawing, updating, and controlling the player...
        // But those would just be single functions in each...
        // So is there any real reason to do that?
        // It feels weird to have a class that will only have a single function.

        public void mainLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                // Update Objects.
                this.update();
                // Draw Everything.
                this.draw();
            }
        }
        private void update()
        {
            // Update rock / gem chance.
            if (this.rock_chance < 1 - (this.chance_change * 2))
            {
                this.rock_chance += this.chance_change * GetFrameTime();
            }
            if (this.gem_chance > this.chance_change * 2)
            {
                this.gem_chance -= this.chance_change * GetFrameTime();
            }
            // Spawn rocks.
            while (rnd.NextDouble() < this.rock_chance)
            {
                this.falling.Add(
                    new FallingObject("[]",
                    new Color((int)(255 * rnd.NextDouble()), (int)(255 * rnd.NextDouble()), (int)(255 * rnd.NextDouble()), 255),
                    25,
                    new Vector2(this.size.X * (float)rnd.NextDouble(), -30.0f),
                    (float)((rnd.NextDouble() + 0.25f) * fall_speed),
                    -1)
                );
            }
            // Spawn Gems
            while (rnd.NextDouble() < this.gem_chance)
            {
                this.falling.Add(
                    new FallingObject("*",
                    new Color((int)(255 * rnd.NextDouble()), (int)(255 * rnd.NextDouble()), (int)(255 * rnd.NextDouble()), 255),
                    25,
                    new Vector2(this.size.X * (float)rnd.NextDouble(), -30.0f),
                    (float)((rnd.NextDouble() + 0.25f) * fall_speed),
                    1)
                );
            }
            // Move Player.
            this.player_move();

            // Update all falling objects.
            for (int i = 0; i < this.falling.Count; i++)
            {
                if (this.falling[i].pos.Y < this.size.Y + this.buffer.Y)
                {
                    this.falling[i].pos.Y += this.falling[i].speed * GetFrameTime();
                    // Check for collisions between the player and objects.
                    if ((this.falling[i].pos.Y > this.size.Y - 25 && this.falling[i].pos.Y < this.size.Y) &&
                        (this.falling[i].pos.X <= this.player.pos.X + 10 && this.falling[i].pos.X >= this.player.pos.X - 10))
                    {
                        this.score += this.falling[i].points;
                        this.falling.RemoveAt(i);
                    }
                }
                else
                {
                    this.falling.RemoveAt(i);
                }
            }
        }
        private void draw()
        {
            BeginDrawing();
            ClearBackground(this.background);
            // Draw all falling objects.
            foreach (FallingObject obj in this.falling)
            {
                DrawText(obj.icon, (int)obj.pos.X, (int)obj.pos.Y, obj.font_size, obj.color);
            }
            // Draw Score.
            DrawText(String.Format("Score: {0}", this.score), 5, 5, 15, Color.WHITE);
            // Draw player last so they're always on top.
            DrawText(player.icon, (int)player.pos.X, (int)player.pos.Y, player.font_size, player.color);
            EndDrawing();
        }
        private void player_move()
        {
            Vector2 movement = new Vector2(0, 0);
            if (IsKeyDown(KeyboardKey.KEY_A) || IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                movement.X = -1;
            }
            else if (IsKeyDown(KeyboardKey.KEY_D) || IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                movement.X = 1;
            }
            // Forgot that the player isn't allowed to move up and stuff.
            // if (IsKeyDown(KeyboardKey.KEY_W) || IsKeyDown(KeyboardKey.KEY_UP))
            // {
            //     movement.Y = -1;
            // }
            // else if (IsKeyDown(KeyboardKey.KEY_S) || IsKeyDown(KeyboardKey.KEY_DOWN))
            // {
            //     movement.Y = 1;
            // }

            // Normalize the movement vector and multiply it by the speed.
            float magnitude = (float)Math.Sqrt(Math.Pow(movement.X, 2) + Math.Pow(movement.Y, 2));
            if (magnitude != 0)
            {
                magnitude = 1.0f / magnitude;
            }
            movement = new Vector2(movement.X * magnitude, movement.Y * magnitude) * this.player.speed;
            this.player.pos += movement * GetFrameTime();
            this.player.pos.X = Math.Clamp(this.player.pos.X, this.buffer.X, this.size.X - this.buffer.X);
            this.player.pos.Y = Math.Clamp(this.player.pos.Y, this.buffer.Y, this.size.Y - this.buffer.Y);
        }
    }
}