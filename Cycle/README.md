# Cycle

This is a simple game that replicates TRON / Light Cycle games that I've seen before.

It does have some features that I disagree with (like being able to turn around 180 degrees, and being able to touch your own line), but in the end it works fine.

It made me really nostalgic for a game I used to play on my iPod touch a while back called "Light Cycle 2"...

The "AI" moving is very simple. It just avoids running directly into walls and sometimes moves randomly. It doesn't look ahead more than one block, so it often "traps" itself in small pockets, but once it gets out it will try to stay out.

## Requirements

- Main Class / Entrypoint
  - Calls SceneHandler
  - Stores values regarding SceneHandler window construction.
- SceneHandler
  - Handles all RayLib code.
  - Draws TextObjects.
  - Calls TextObject update functions.
  - GameLoop / Logic.
  - Collision Checking.
- GameObject
  - Interface that includes the update function.
- TextObject : GameObject
  - Includes all shared values and methods for classes that extend TextObject
  - Position
  - String to print
  - Text size
  - Text color
  - Virtual Update
- CycleObject : TextObject
  - Speed
  - Head / Dead-Head / Tail text.
  - Stores the movement method in update
  - Updates the map of TextObjects with it's head and trail.
  - Contains a flag that can be set which will turn the players into autonomous mode while the game ends.
  - Collision Checking: Will set it's own head to a separate text symbol when it collides with an opposite player's tail. SceneHandler will watch for this.
  - No need for a TailObject class because tails can be TextObjects stored in the map, as they don't update or do anything.
  - I did add a list of tails, so they could be changed to white easily.
  - As well as grouping the "death" functionality together in a public function so it could be called by the SceneManger when the game ends.

## Notes on Polymorphism

Polymorphism will mainly be utilized in the update method of classes that extend the GameObject. This will allow all the unique objects that extend GameObject to have update functions that are unique. This also allows the update loop in the SceneHandler to just call the update function from all of the GameObjects, allowing for better encapsulation.
