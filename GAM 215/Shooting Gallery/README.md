# Shooting Gallery (GAM 215, Module 9, Assignment 9.1: Shooting Gallery (Part 3))

## Main file locations:

* Location of script files (Asset/Scripts):
  * BulletController.cs
  * CameraController.cs
  * GameController.cs
  * PlayerController.cs
  * SeekingEnemyController.cs
  * SpreadShotController.cs
  * StartButtonController.cs
* Location of scene files (Asset/Scenes):
  * Start\ Scene.unity
  * Game\ Scene.unity

## Project information

### Unity Scene

1. Created a start scene that:
  1. Explains the objective of the game
  2. Explains the controls
  3. Allows you to start the game (through clicking a button)
2. Created a game scene that:
  1. Has a playable character
  2. Locks the mouse to the center of the screen
  3. Has 3 enemy characters that take damage and changes colors when hit with a raycast
  4. Has a base that will eventually have triggers for losing the game (when enemies destroy the base)
  5. Has no friction on the walls so the character can slide along the wall easily

### Script Info

1. Added controller for the player to move
2. Added camera controller to operate the camera
3. Added raycasting to trigger damage and color change
4. Added some placeholder comments/values to add more complex interactions with enemies
5. Added a game controller to handle more interactions that should be tied to the game instead of to the player/enemy
6. Added functions to spawn enemies at random spawn points
7. Added waves of enemies that are loading into the scene using a coroutine
8. Added bullet controller to facilitate moving spawned bullets, sending `OnShot()` events, and sending messages to let spread shot know it is being destroyed
9. Added spread shot controller to know when child bullets are all destroyed and then proceed to destroy itself

Author: Jv Chen

## How does my project satisfy the conditions of the assignment?

1. Created a new game level scene containing at least 3 game objects that react to being clicked on
2. Added at least 2 functions that are declared with one or more parameters (ChangeHealth() and ChangeColor())
3. I utilized raycasting to send "damage" events to the enemy
4. I have actual win/lose conditions that can be triggered
5. I am using the new UI system
6. Used a coroutine to slowly trickle in enemies
7. Spawned objects under at least 2 conditions via instantiation (spread shot bullets and enemies)

## Miscellaneous

This README is a markdown file that can be generated into an HTML page at http://daringfireball.net/projects/markdown/dingus or
in my Github page (https://github.com/bluejv7/AAU-Projects/tree/master/GAM%20215/Shooting%20Gallery).  You can always check my Github account to see if anyone
plagiarized my work or if I have done the same to others.  Academic integrity is important!

Also thanks to:

* Paul Dela Cruz for providing the free animated crosshair on Unity asset store (https://www.assetstore.unity3d.com/en/#!/content/54897)!  Much appreciated!
* Daniel Brauer and Adrian for their code using raycasting with bullets (http://wiki.unity3d.com/index.php?title=DontGoThroughThings).  I referenced the code
to see what I would like to implement and only tried to implement that parts that made sense to me.