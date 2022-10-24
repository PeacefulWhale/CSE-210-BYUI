using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace RFK
{
    class Player : GameObject
    {
        private Vector2 position;
        // Pixels per second I think...
        private float speed = 2.0f;
        private SceneHandler scene;
        private string symbol = "#";
        private int font_size = 25;
        private Color color = new Color(255, 255, 255, 255);
        private float pulse_length = 0.5f;
        private float pulse_size = 5f;
        private float pulse_time;
        private bool pulse_dir = true;
        public Player(SceneHandler scene, Random rnd)
        {
            // Initialize player stuff I guess. There isn't much to do here.
            this.scene = scene;
            this.pulse_time = (((float)rnd.NextDouble() * 2) - 1) * this.pulse_length;
            this.position = new Vector2(0, 0);
        }

        public void update()
        {
            // Player movement. Let the SceneHandler deal with collisions between the player and artifacts.
            Vector2 movement = new Vector2(0, 0);
            if (IsKeyDown(KeyboardKey.KEY_A) || IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                movement.X = -1;
            }
            else if (IsKeyDown(KeyboardKey.KEY_D) || IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                movement.X = 1;
            }

            if (IsKeyDown(KeyboardKey.KEY_W) || IsKeyDown(KeyboardKey.KEY_UP))
            {
                movement.Y = -1;
            }
            else if (IsKeyDown(KeyboardKey.KEY_S) || IsKeyDown(KeyboardKey.KEY_DOWN))
            {
                movement.Y = 1;
            }
            
            // Normalize the movement vector and multiply it by the speed.
            float magnitude = (float) Math.Sqrt(Math.Pow(movement.X, 2) + Math.Pow(movement.Y, 2));
            if (magnitude != 0)
            {
                magnitude = 1.0f / magnitude;
            }
            movement = new Vector2(movement.X * magnitude, movement.Y * magnitude) * speed;
            this.position += movement;
            Vector2 bounds = this.scene.getSize();
            this.position.X = Math.Clamp(this.position.X, 0, bounds.X);
            this.position.Y = Math.Clamp(this.position.Y, 0, bounds.Y);
            this.pulse_time += (pulse_dir) ? Raylib.GetFrameTime() : -Raylib.GetFrameTime();
            if (this.pulse_time >= this.pulse_length)
            {
                this.pulse_dir = false;
            }
            else if (this.pulse_time <= -this.pulse_length)
            {
                this.pulse_dir = true;
            }
        }

        public void draw()
        {
            // I might make this pretty... Maybe.
            // I'll add a pulsing size so it's easier to see the robot.
            DrawText(this.symbol, (int) this.position.X, (int) this.position.Y, (int) (this.font_size + (this.pulse_time * this.pulse_size)), this.color);
        }

        public Vector2 getPosition()
        {
            return this.position;
        }
    }
}