using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace FinalProject
{
    class Player : TexturedObject
    {
        private float speed, jump_force, gravity;
        private bool can_jump;
        private Vector2 movement;
        private float boost_power;
        private PlatformObject to_break;
        private Texture2D[] texs = new Texture2D[5];
        private Sound jump_sound;
        public Player(Vector2 pos, float scale, float rot, float speed, float jump_force, float gravity) : base("./Images/Person1.png", pos, scale, rot)
        {
            this.speed = speed;
            this.jump_force = jump_force;
            this.gravity = gravity;
            this.can_jump = true;
            this.boost_power = 1.0f;
            this.to_break = null;
            this.texs[0] = this.tex;
            this.texs[1] = LoadTexture("./Images/Person2.png");
            this.texs[2] = LoadTexture("./Images/Person3.png");
            this.texs[3] = LoadTexture("./Images/Person4.png");
            this.texs[4] = LoadTexture("./Images/Person5.png");
            this.jump_sound = LoadSound("./Images/Jump.wav");
        }

        public void update(float deltaTime, List<PlatformObject> platforms)
        {
            base.update(deltaTime);
            // The player moves.
            // I could have the SceneHandler deal with collisions, but I've decided to do that here so things are grouped just a little bit better.
            if (IsKeyPressed(KeyboardKey.KEY_SPACE) && this.can_jump)
            {
                this.movement.Y -= this.jump_force * this.boost_power;
                this.can_jump = false;
                platforms.Remove(this.to_break);
                this.tex = this.texs[2];
                PlaySound(this.jump_sound);
            }

            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                this.movement.X = -this.speed * deltaTime;
            }
            else if (IsKeyDown(KeyboardKey.KEY_D))
            {
                this.movement.X = this.speed * deltaTime;
            }
            else
            {
                this.movement.X = 0;
            }

            bool hit_obstacle = false;
            if (this.movement.Y >= 0)
            {
                // Very simple animation states.
                if (this.movement.Y > 1)
                {

                    this.tex = this.texs[3];
                }
                else
                {
                    this.tex = this.texs[0];
                }
                // Check for collisions with platforms.
                foreach (PlatformObject platform in platforms)
                {
                    int player_offset = 15;
                    if (platform.collide && 
                    platform.pos.X <= this.pos.X + 40 &&
                    platform.pos.X + platform.size.X >= this.pos.X + 15 &&
                    platform.pos.Y >= this.pos.Y + player_offset && 
                    platform.pos.Y - (platform.size.Y * 3f) <= this.pos.Y + player_offset)
                    {
                        this.tex = this.texs[1];
                        this.movement.Y = 0;
                        this.can_jump = true;
                        this.pos.Y = (platform.pos.Y - (platform.size.Y * 3f)) - player_offset;
                        hit_obstacle = true;
                        this.boost_power = platform.boost_power;
                        if (platform is WeakPlatform)
                        {
                            this.to_break = platform;
                        }
                        else
                        {
                            this.to_break = null;
                        }
                    }
                }
            }
            else
            {
                // Jumping up animation state.
                if (Math.Abs(this.pos.Y) % 100 < 50)
                {
                    this.tex = this.texs[4];
                }
                else
                {
                    this.tex = this.texs[2];
                }
            }
            if (!hit_obstacle)
            {
                this.movement.Y += this.gravity * deltaTime;
            }
            this.pos += this.movement;
        }
    }
}