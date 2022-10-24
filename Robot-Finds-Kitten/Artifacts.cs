using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace RFK
{
    class Artifact : GameObject
    {
        // At first I thought that it mattered what character was associated with which object... But then I realized that it was all random... So I won't worry about that I guess.
        private Vector2 position, size, buffer;
        private string symbol, message;
        private Color color;
        private int font_size;
        public bool touched;
        private bool was_touched;
        private AnimatedText animated_text;
        private SceneHandler scene;
        private bool pulsing;
        private float pulse_length = 0.5f;
        private float pulse_size = 5f;
        private float pulse_time;
        private bool pulse_dir = true;
        public Artifact(SceneHandler scene, string message, Random rnd, bool pulsing)
        {
            this.pulsing = pulsing;
            this.pulse_time = (((float)rnd.NextDouble() * 2) - 1) * this.pulse_length;
            this.scene = scene;
            this.touched = false;
            this.was_touched = false;
            this.size = scene.getSize();
            this.buffer = scene.getBuffer();
            this.position = new Vector2(rnd.NextInt64((int) (0 + this.buffer.X), (int) (this.size.X - this.buffer.X)), rnd.NextInt64((int) (0 + this.buffer.Y), (int) (this.size.Y - this.buffer.Y)));
            this.color = new Color((int) rnd.NextInt64(100, 255), (int) rnd.NextInt64(100, 255), (int) rnd.NextInt64(100, 255), 255);
            // I just took the allowed character range from the example.
            this.symbol = char.ToString((char) rnd.NextInt64(35, 100));
            this.font_size = (int) rnd.NextInt64(10, 15);
            this.message = message;
        }
        public void update()
        {
            if (!this.was_touched && this.touched)
            {
                // This object is now touched, create a new animated text object!
                this.animated_text = new AnimatedText(this.message, 0, 0.5f, new Vector2(0, 0), 15, this.color, this.scene);
            }
            else if (this.was_touched && !this.touched)
            {
                // The object is no longer being touched, replace the animated text object with an empty one.
                this.animated_text = new AnimatedText("", 0, 0, new Vector2(0, 0), 15, this.color, this.scene);
            }

            if (this.touched)
            {
                this.animated_text.update();
            }
            this.was_touched = this.touched;

            if (this.pulsing)
            {
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
        }

        public void draw()
        {
            if (this.pulsing)
            {
                DrawText(this.symbol, (int) this.position.X, (int) this.position.Y, (int) (this.font_size + (this.pulse_time * this.pulse_size)), this.color);
            }
            else
            {
                DrawText(this.symbol, (int) this.position.X, (int) this.position.Y, this.font_size, this.color);
            }
            if (this.touched)
            {
                this.animated_text.draw();
            }
        }

        public Vector2 getPosition()
        {
            return this.position;
        }
    }
}