using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace RFK
{
    class SceneHandler
    {
        // Variables.
        private int fps;
        private Vector2 size;
        private Vector2 buffer;
        private Color background;
        private Camera2D camera;
        // All objects to update and render.
        private List<Artifact> artifacts;
        private Player player;
        // Constructor.
        public SceneHandler(int height, int width, string scene_name, int fps, int h_buffer, int w_buffer, Color background, Camera2D camera)
        {
            this.artifacts = new List<Artifact>();
            this.size.Y = height;
            this.size.X = width;
            this.fps = fps;
            // The buffer is where the UI and stuff will be stuck.
            this.buffer.Y = h_buffer;
            this.buffer.X = w_buffer;
            this.background = background;
            this.camera = camera;
            InitWindow(width + w_buffer, height + h_buffer, scene_name);
            SetTargetFPS(fps);
        }
        public Vector2 getCameraTarget()
        {
            return this.camera.target;
        }
        public List<Artifact> GetArtifacts()
        {
            return this.artifacts;
        }
        // Get functions.
        public Vector2 getSize()
        {
            return this.size;
        }
        public Vector2 getBuffer()
        {
            return this.buffer;
        }
        // Start Game Main Loop.
        public void mainLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                this.updateObjects();
                BeginDrawing();
                ClearBackground(this.background);
                this.drawObjects();
                EndDrawing();
            }
            CloseWindow();
        }
        public void addPlayer(Player player)
        {
            this.player = player;
        }
        // Add object to the SceneHandler's list of game_objects.
        public void addArtifact(Artifact artifact)
        {
            this.artifacts.Add(artifact);
        }

        // Call each game_object's update function, passing the needed information.
        private void updateObjects()
        {
            // Player gets updated first.
            this.player.update();
            // Only collide with the first object touched.
            bool collided = false;
            foreach (Artifact artifact in this.artifacts)
            {
                // Check for collisions between the player and artifacts.
                if (!collided)
                {
                    // Simple rectangle bounds math.
                    Vector2 pos = artifact.getPosition();
                    Vector2 ppos = player.getPosition();
                    if (Raylib.CheckCollisionRecs(new Rectangle(pos.X, pos.Y, 10, 10), new Rectangle(ppos.X, ppos.Y, 15, 15)))
                    {
                        artifact.touched = true;
                        collided = true;
                    }
                    else
                    {
                        artifact.touched = false;
                    }
                }
                else
                {
                    artifact.touched = false;
                }
                artifact.update();
            }
        }

        // Draw all objects.
        private void drawObjects()
        {
            foreach (GameObject game_object in this.artifacts)
            {
                game_object.draw();
            }
            // Player gets drawn last.
            this.player.draw();
        }
    }
}
