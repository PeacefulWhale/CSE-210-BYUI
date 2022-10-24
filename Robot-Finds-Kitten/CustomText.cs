using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace RFK
{
    class AnimatedText : GameObject
    {
        private string text;
        private int characters_printed;
        private float time_step;
        private float start_time;
        private float end_time;
        private float time;
        private float current_time;
        private int font_size;
        private Color color;
        private SceneHandler scene_handler;
        private Vector2 position;
        public AnimatedText(string text, float start_time, float end_time, Vector2 position, int font_size, Color color, SceneHandler scene_handler)
        {
            this.text = text;
            this.start_time = start_time;
            this.end_time = end_time;
            this.time = Math.Abs(end_time - start_time);
            this.position = position;
            this.font_size = font_size;
            this.color = color;

            this.characters_printed = 0;

            // Calculate time_step.
            // time_step is the time between drawing letters.
            this.time_step = this.time / text.Length;
            this.current_time = 0;

            this.scene_handler = scene_handler;
        }

        // Update loop.
        // Returns true if there is nothing left to update.
        public void update()
        {
            this.current_time += Raylib.GetFrameTime();
            if (this.start_time != 0)
            {
                if (this.current_time >= this.start_time)
                {
                    this.current_time = 0;
                    this.start_time = 0;
                }
            }
            else
            {
                if (this.start_time == 0 && (this.current_time >= time_step && this.characters_printed < this.text.Length))
                {
                    this.current_time = 0;
                    this.characters_printed++;
                }
            }
        }
        public void draw()
        {
            int line_characters = (int) Math.Ceiling((float)(this.scene_handler.getSize().X - this.position.X) / (float) this.font_size);
            // I should maybe get this to work with MeasureText instead.
            line_characters *= 2;
            int sub_start = 0;
            Vector2 camera_target = this.scene_handler.getCameraTarget();
            Vector2 offset = camera_target;
            // Wrapped text logic.
            while (sub_start < this.text.Length && sub_start < this.characters_printed)
            {
                int text_length = this.text.Length - sub_start;
                if (text_length > line_characters)
                {
                    text_length = line_characters;
                }
                if (text_length > this.characters_printed - sub_start)
                {
                    text_length = this.characters_printed - sub_start;
                }
                DrawText(this.text.Substring(sub_start, text_length), (int) (this.position.X + offset.X), (int) (this.position.Y + offset.Y), this.font_size, this.color);
                sub_start += line_characters;
                offset.Y += this.font_size;
            }
        }
    }
}