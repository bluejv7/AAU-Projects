# Platformer (Part 2) (GAM 215, Module 3, Assignment 3.1: Platformer)

## Main file locations:

* The C# script is located in `Assets/Scripts/CharacterRigidBodyController.cs`
* The Unity scene is located in `Assets/Scenes/PlatformScene.unity`

## Project information

### Unity Scene (Assignment 3)

1. Added triggers to enemy/hazard objects
2. Added trigger to goal
3. Added health `Text` game object
4. Added end game `Text` game object

### Script Info

1. Added conditions to check if player has triggered the goal.  If player triggered goal, display win text and ignore loss conditions.
2. Added conditions to check if player has hit a non-goal trigger.  If so, player takes damage.
3. Subtract health when player takes damage and update health display
4. If player takes enough damages to go to/below 0 health, display loss message, detach player object's children, and destroy player object

Author: Jv Chen

## How does my project satisfy the conditions of the assignment

1. I have declared at least 5 variables (14 variables)
2. I have at least 2 if statements checking a user defined variable (3 if statements)
3. I have programmed a lose condition (when health <= 0) and informed player when game is over
4. I have added at least 5 triggers, with 1 goal trigger (16 triggers, 1 of them being a goal trigger)
5. The goal trigger causes the game to end in a win
6. Assignment update: destroy the player `GameObject` on lose condition, initialize reference variables with `null`,
and check trigger identities using tag instead of name

## Recap from Assignment 2

### Unity Scene

1. A player-controlled (player) `GameObject`
2. Multiple platform, obstacle, and enemy game objects
3. A plane for the player and other game objects to land/rest on
4. There are at least 10 game objects
5. I attached the main camera as a child of the player and tweaked the transform values so I could get a proper view of the game environment

### Script Info

I made one script that handles controlling the player through modifying the `Rigidbody` component.
I used the `Physics.checkSphere()` method to detect whether the player is grounded or not based on the example code written by Scott Berkenkotter.
I added 4 directional inputs (W,A,S,D) to move the player (up, down, look left, look right; respectively).
I also added a jump input (spacebar) so the player can hop onto platforms or use it to avoid obstacles/enemies.