using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Greed
{
    class Object
    {
        // I've got no clue if this is how I'm supposed to set this up, but it seems better than having a lot of public getXXX methods.
        public string icon;
        public Color color;
        public int font_size;
        public float speed;
        // Position should be able to be updated globally.
        public Vector2 pos;

        public Object(string icon, Color color, int size, Vector2 pos, float speed)
        {
            this.icon = icon;
            this.color = color;
            this.font_size = size;
            this.pos = pos;
            this.speed = speed;
        }
    }

    class FallingObject : Object
    {
        public int points;
        public FallingObject(string icon, Color color, int size, Vector2 pos, float speed, int points) : base(icon, color, size, pos, speed)
        {
            this.points = points;
        }
        // I was tempted to put a "moveDown" function or similar here, but I can do that in the Updater.
    }

    // I don't need a player class.
    // Updater (or the keyboard handler) can do all that.
}