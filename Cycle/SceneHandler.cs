using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cycle
{
    /// <summary>
    /// The scene handler for the game.
    /// This is also the only class that requires Raylib, hopefully making it easy to swap to a different graphics library in the future.
    /// </summary>
    class SceneHandler
    {
        private int fps, text_size;

        // This is a little bit different from how I normally handle speed.
        // This shows how many seconds must have passed since the last move update, for the bikes to be updated again.
        // Because they're not moving in continuous lines, I need discrete update times.
        private float move_speed;

        // The timer since the last update.
        private double timer;
        private TextObject[,] map;
        private Vector2 size, map_size;
        private Vector2 buffer;
        private Color background;
        private Camera2D camera;
        private Random rnd;

        // Objects for keeping track of the score.
        private ScoreObject a_score, b_score;

        // Game Over message.
        private TextObject game_over;

        public SceneHandler(int height, int width, string scene_name, int fps, int h_buffer, int w_buffer, Vector4 background, int text_size, float move_speed)
        {
            this.camera = new Camera2D();
            this.camera.target = new Vector2(0.0f, 0.0f);
            this.rnd = new Random();
            this.size.Y = height;
            this.size.X = width;
            this.fps = fps;
            this.text_size = text_size;
            this.move_speed = move_speed;
            // The buffer is where the UI and stuff will be stuck.
            this.buffer.Y = h_buffer;
            this.buffer.X = w_buffer;
            this.background = new Color((int) background.X, (int) background.Y, (int) background.Z, (int) background.W);
            InitWindow(width + w_buffer, height + h_buffer, scene_name);
            SetTargetFPS(fps);

            // Calculate map size.
            this.map_size.X = (int) (this.size.X / (this.text_size + 0.5f));
            this.map_size.Y = (int) (this.size.Y / (this.text_size + 0.5f));

            // Player scores.
            a_score = new ScoreObject("Player A Score: ", new Vector2(this.buffer.X, 5), 15, new Vector4(255, 0, 0, 255));
            a_score.score = 0;
            b_score = new ScoreObject("Player B Score: ", new Vector2(this.size.X / 2, 5), 15, new Vector4(0, 0, 255, 255));
            b_score.score = 0;

            this.a_score.update();
            this.b_score.update();

            this.timer = 0;
        }

        // Loop to be called externally.
        public void mainLoop()
        {
            bool nextRound = true;
            while (nextRound && !Raylib.WindowShouldClose())
            {
                nextRound = this.gameLoop();
            }
        }

        // The bike objects.
        private Bike player_a, player_b;

        // This starts a new round, and contains the logic for each rounds.
        private bool gameLoop()
        {
            bool round_over = false, cont = false;
            // Generate new map.
            this.map = new TextObject[(int) this.map_size.X, (int) this.map_size.Y];
            for (int x = 0; x < this.map_size.X; x++)
            {
                for (int y = 0; y < this.map_size.Y; y++)
                {
                    this.map[x, y] = null;
                }
            }

            // Create players.
            player_a = new Bike("@", new Vector2(0, 0), this.text_size, new Vector4(255, 0, 0, 255), this.map, this.map_size);
            player_a.move = new Vector2(1, 0);
            player_a.last_move = new Vector2(1, 0);

            player_b = new Bike("@", new Vector2(this.map_size.X - 1, this.map_size.Y - 1), this.text_size, new Vector4(0, 0, 255, 255), this.map, this.map_size);
            player_b.move = new Vector2(-1, 0);
            player_b.last_move = new Vector2(-1, 0);

            // Create game_over text.
            this.game_over = new TextObject("", new Vector2(0, this.size.Y / 2) + this.buffer / 2, 35, new Vector4(0, 0, 0, 0));

            // Game play loop.
            while (!cont && !Raylib.WindowShouldClose())
            {
                // Update Objects.
                this.update();
                // Check to see if one of the players has one.
                if (!round_over && (player_a.AI || player_b.AI))
                {
                    if (player_a.AI && player_b.AI)
                    {
                        // Dang, they both lost.
                        this.game_over.text = "You both lose...";
                        this.game_over.color = new Vector4(255, 0, 255, 255);
                    }
                    else if (player_a.AI)
                    {
                        // Player B wins.
                        player_b.roundOver();
                        this.game_over.text = "Player B Wins";
                        this.game_over.color = new Vector4(0, 0, 255, 255);
                        b_score.incrementScore();
                    }
                    else if (player_b.AI)
                    {
                        // Player A wins.
                        player_a.roundOver();
                        this.game_over.text = "Player A Wins";
                        this.game_over.color = new Vector4(255, 0, 0, 255);
                        a_score.incrementScore();
                    }
                    this.game_over.text += "\nPress Space to Play Again.";
                    round_over = true;
                }
                if (round_over)
                {
                    if (IsKeyPressed(KeyboardKey.KEY_SPACE))
                    {
                        cont = true;
                    }
                }
                // Draw Everything.
                this.draw();
            }

            // If the game loop ends before the round is over, or before they continue, it is because the user tried to close the window.
            // Returning this will allow the earlier loop (in mainLoop) to exit properly.
            return round_over && cont;
        }
        
        // I've decided to use polymorphism in each GameObject to handle update calls.
        // However, I am updating the player movements in here, as to keep all the Raylib class stuff in here.
        private void update()
        {
            // Score updates only need to happen when the score changes.
            // The player's have their movements updated every frame even if they don't move.
            // This ensures that the bike movement doesn't feel "glitchy".

            // Check the player_a keystrokes.
            if (!player_a.AI)
            {
                if (IsKeyPressed(KeyboardKey.KEY_W))
                {
                    player_a.move = new Vector2(0, -1);
                }
                else if (IsKeyPressed(KeyboardKey.KEY_S))
                {
                    player_a.move = new Vector2(0, 1);
                }
                else if (IsKeyPressed(KeyboardKey.KEY_D))
                {
                    player_a.move = new Vector2(1, 0);
                }
                else if (IsKeyPressed(KeyboardKey.KEY_A))
                {
                    player_a.move = new Vector2(-1, 0);
                }
            }
            // Check player_b keystrokes.
            if (!player_b.AI)
            {
                if (IsKeyPressed(KeyboardKey.KEY_UP))
                {
                    player_b.move = new Vector2(0, -1);
                }
                else if (IsKeyPressed(KeyboardKey.KEY_DOWN))
                {
                    player_b.move = new Vector2(0, 1);
                }
                else if (IsKeyPressed(KeyboardKey.KEY_RIGHT))
                {
                    player_b.move = new Vector2(1, 0);
                }
                else if (IsKeyPressed(KeyboardKey.KEY_LEFT))
                {
                    player_b.move = new Vector2(-1, 0);
                }
            }

            // The bike updates are only called when they can actually move.
            this.timer += GetFrameTime();
            if (this.timer > this.move_speed)
            {
                // Update the player's movement.
                this.player_a.update();
                this.player_b.update();
                this.timer = 0;
            }
        }

        // I can do all the drawing in here, that way there is no need for Raylib stuff outside of this class.
        private void draw()
        {
            BeginDrawing();
            ClearBackground(this.background);
            // Draw the player scores.
            this.drawText(a_score, a_score.pos);
            this.drawText(b_score, b_score.pos);
            // Draw everything in the map.
            for (int x = 0; x < this.map_size.X; x++)
            {
                for (int y = 0; y < this.map_size.Y; y++)
                {
                    TextObject cell = this.map[x, y];
                    if (cell != null)
                    {
                        this.drawText(cell, (cell.pos * this.text_size) + (this.buffer / 2));
                    }
                }
            }
            // Draw the game over text.
            drawText(game_over, game_over.pos);
            EndDrawing();
        }

        private void drawText(TextObject obj, Vector2 draw_pos)
        {
            DrawText(obj.text, (int) draw_pos.X, (int) draw_pos.Y, obj.text_size, this.convertColor(obj.color));
        }

        private Color convertColor(Vector4 color)
        {
            return new Color((int) color.X, (int) color.Y, (int) color.Z, (int) color.W);
        }
    }
}