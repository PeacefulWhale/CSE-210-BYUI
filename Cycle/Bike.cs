using System;
using System.Numerics;
using System.Collections.Generic;

// At first I thought I could implement two classes for the bike, the "head" and the "tail", but then I realized that I wouldn't actually get much out of that.
// The tail never actually moves, only the head.
// In my mind, doing this procedurally seems to be both easier, and more efficient...
// I've taken a hybrid approach, where the map / drawing is done procedurally, but each player handles their own update calls.

namespace Cycle
{
    // The base TextObject class.
    // Most of my classes extend this.
    class TextObject : GameObject
    {
        public string text;
        public Vector2 pos;
        public int text_size;
        public Vector4 color;

        public TextObject(string text, Vector2 pos, int text_size, Vector4 color)
        {
            this.text = text;
            this.pos = pos;
            this.text_size = text_size;
            this.color = color;
        }

        public virtual void update() { }
    }

    class ScoreObject : TextObject
    {
        private string base_text;
        public int score = 0;
        public ScoreObject(string text, Vector2 pos, int text_size, Vector4 color) : base(text, pos, text_size, color)
        {
            this.base_text = text;
        }

        public override void update()
        {
            this.text = this.base_text + score.ToString();
        }

        public void incrementScore()
        {
            this.score += 1;
            this.update();
        }
    }

    // The bike class.
    class Bike : TextObject
    {
        // Probability of the AI bike turning.
        private const float turn_chance = 0.1f;

        // I didn't like how pure white looked, so now it's slightly grey.
        private Vector4 white = new Vector4(255, 255, 255, 175);

        public TextObject[,] map;

        private Vector2 map_size;

        public bool AI = false;

        public Vector2 move, last_move;

        private Random rnd = new Random();

        private List<TextObject> tail = new List<TextObject>();

        public Bike(string text, Vector2 pos, int text_size, Vector4 color, TextObject[,] map, Vector2 map_size) : base(text, pos, text_size, color)
        {
            this.map = map;
            this.map_size = map_size;
            // The bikes should always be moving.
        }

        public override void update()
        {
            // Originally I was tempted to put the keyboard handling in here, but then I realized that I could keep nearly all of the Raylib stuff within the Scene_handler, which seemed like a better choice.

            // Also C# has a garbage handler right?
            // I should be able to just replace the objects in the map with new text objects without causing *too* many problems.

            // Calculate tail_text;
            string tail_text = "#";
            // I tried to use unicode box drawing characters earlier.
            // Sadly Raylib doesn't seem to support those...
            // I had this whole logic tree to select the right one...
            // I might come back to this and draw them with Raylib's other drawing methods.

            // Create a new tail object here.
            TextObject tailObj = new TextObject(tail_text, this.pos, this.text_size, this.color);
            this.tail.Add(tailObj);
            this.map[(int)this.pos.X, (int)this.pos.Y] = tailObj;

            if (this.AI)
            {
                this.AIMove();
            }
            // Move the head.
            this.pos += this.move;

            // Clamp the position of the bike to be within the map.
            // Players that go out of bounds loop back around.
            this.pos = this.clampPos(this.pos);
            // Check for collision.
            TextObject temp = this.map[(int)this.pos.X, (int)this.pos.Y];
            if (temp != null && temp.color != this.color)
            {
                this.roundOver();
            }
            // Update the head.
            this.map[(int)this.pos.X, (int)this.pos.Y] = new TextObject(this.text, this.pos, this.text_size, this.color);
            // Update last_move
            last_move = move;
        }

        // They're not supposed to turn into each other.
        // But I am lazy, and don't want to have to write an actual pathfinder algorithm for them to follow.
        // The pathfinder algorithm would only have to run once, and it would just generate the longest possible paths for both cycles.
        // But still, I think that might be a little bit beyond the scope of this assignment, so instead I'll just have the cycles try their best to not touch white walls, and if they can't avoid it they just ignore them (with a single cell lookahead).
        private void AIMove()
        {
            // AI moves randomly.
            if (this.rnd.NextDouble() < turn_chance)
            {
                // Select a new random direction to move in.
                // That isn't the opposite direction... That just looks weird.

                // If they don't sometimes move randomly, they can get "trapped" within blocks.
                double turn = this.rnd.NextDouble();
                if (turn < 0.25f && this.move != new Vector2(-1, 0))
                {
                    this.move = new Vector2(1, 0);
                }
                else if (turn < 0.5f && this.move != new Vector2(0, -1))
                {
                    this.move = new Vector2(0, 1);
                }
                else if (turn < 0.75f && this.move != new Vector2(1, 0))
                {
                    this.move = new Vector2(-1, 0);
                }
                else if (this.move != new Vector2(0, 1))
                {
                    this.move = new Vector2(0, -1);
                }
            }
            Vector2 new_pos = this.clampPos(this.pos + this.move);
            // Clamp the position of the bike to be within the map.
            // Players that go out of bounds loop back around.
            if (map[(int)new_pos.X, (int)new_pos.Y] != null)
            {
                Vector2 new_move;
                bool good_move = false;
                for (int x = -1; x <= 1 && !good_move; x++)
                {
                    for (int y = -1; y <= 1 && !good_move; y++)
                    {
                        if (x + y == 1 || x + y == -1)
                        {
                            new_move = new Vector2(x, y);
                            new_pos = this.clampPos(this.pos + new_move);
                            if (map[(int)new_pos.X, (int)new_pos.Y] == null)
                            {
                                this.move = new_move;
                                good_move = true;
                            }
                        }
                    }
                }
            }
        }

        private Vector2 clampPos(Vector2 temp)
        {
            // Clamp the position of the bike to be within the map.
            // Players that go out of bounds loop back around.
            if (temp.X < 0)
            {
                temp.X = this.map_size.X - 1;
            }
            else if (temp.X >= this.map_size.X)
            {
                temp.X = 0;
            }
            if (temp.Y < 0)
            {
                temp.Y = this.map_size.Y - 1;
            }
            else if (temp.Y >= this.map_size.Y)
            {
                temp.Y = 0;
            }
            return temp;
        }

        public void roundOver()
        {
            // Head is collided. Change color to white. Turn AI mode on.
            this.color = this.white;
            this.AI = true;
            // Change all the tails to white.
            foreach (TextObject tempTail in this.tail)
            {
                tempTail.color = this.white;
            }
        }
    }
}