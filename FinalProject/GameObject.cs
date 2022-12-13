using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace FinalProject
{

    // The GameObject class from which other classes will extend.
    interface GameObject
    {
        public void update(float deltaTime);
        public void draw();
    }

    class TexturedObject : GameObject
    {
        public Image img;
        public Texture2D tex;
        public Vector2 pos;
        public float scale;
        public float rot;
        public TexturedObject(string img_path, Vector2 pos, float scale, float rot)
        {
            this.pos = pos;
            this.scale = scale;
            this.rot = rot;
            this.img = LoadImage(img_path);
            this.tex = LoadTextureFromImage(this.img);
        }
        public virtual void update(float deltaTime) { }
        public void draw()
        {
            DrawTextureEx(this.tex, this.pos, this.rot, this.scale, Color.WHITE);
        }
    }

    // All the different platforms will extend this class.
    class PlatformObject : TexturedObject
    {
        public Vector2 size;
        public bool collide;
        public float boost_power;
        public PlatformObject(string img_path, Vector2 pos, float scale, float rot) : base(img_path, pos, scale, rot)
        {
            this.collide = true;
            this.size = new Vector2(512 * scale, 24 * scale);
            this.boost_power = 1.0f;
        }
    }

    class WeakPlatform : PlatformObject
    {
        public WeakPlatform(Vector2 pos, float scale, float rot) : base("./Images/WeakPlatform.png", pos, scale, rot)
        {
            this.boost_power = 1.0f;
        }
    }

    class BasePlatform : PlatformObject
    {
        public BasePlatform(Vector2 pos, float scale, float rot) : base("./Images/BasePlatform.png", pos, scale, rot)
        {
            this.boost_power = 1.0f;
        }
    }

    class BoostedPlatform : PlatformObject
    {
        public BoostedPlatform(Vector2 pos, float scale, float rot) : base("./Images/BoostPlatform.png", pos, scale, rot)
        {
            this.boost_power = 1.5f;
        }
    }

    class ExtraBoostedPlatform : PlatformObject
    {
        public ExtraBoostedPlatform(Vector2 pos, float scale, float rot) : base("./Images/ExtraBoostPlatform.png", pos, scale, rot)
        {
            this.boost_power = 3f;
        }
    }
}
