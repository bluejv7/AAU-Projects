# Survival Game (Part 2) (GAM 215, Module 6, Assignment 6.1: Survival Game)

## Main file locations:

* The C# scripts are located in `Assets/Scripts/CharacterRigidBodyController.cs`, `Assets/Scripts/PyramidObstacle.cs`,
`Assets/Scripts/SharkController.cs`, `Assets/Scripts/SharkPatrollingController.cs`, `Assets/Scripts/DroneController.cs`, and
`Assets/Scripts/CameraController.cs`
* The Unity scene is located in `Assets/Scenes/PlatformScene.unity`

## Project information

### Unity Scene

1. Added one more enemy object (a patrolling drone)
2. Added a barrier/wall
3. Added more platforms and made scaling platforms vertically a "requirement" to beat the game
(there is still a glitch that can happen when the player attempts to stick to the barrier wall and somehow jump around it)
4. Added new camera structure to help implement mouse camera controls

### Script Info

1. Added `DroneController.cs` and created a patrolling enemy that circles around the platforms "required" to beat the game
2. Modified `CharacterRigidBodyController.cs` to call non-event functions that perform character movement
3. Changed left/right to be strafing instead of camera movement
4. Implemented camera controls in `CameraController.cs` so the mouse can move the camera in the x/y axes
  1. Added an option to toggle inverted camera controls (because I hate inverted y-axis)
  2. Also added some options to modify the camera rotation speeds on the x/y axes
5. All input is now gathered through the Input Manager

Author: Jv Chen

## How does my project satisfy the conditions of the assignment?

1. My player script gathers all inputs through the Input Manager
2. Added the mouse as an analog control
3. Declared and called at least 2 well-written non-event function (`MoveHorizontally()`, `Jump()`, and `RotateCamera()`)
4. Separated the player's input from its actions
5. Use variables for all input names
6. Attempted to use a mouse-controller camera

## Miscellaneous

This README is a markdown file that can be generated into an HTML page at http://daringfireball.net/projects/markdown/dingus or
in my Github page (https://github.com/bluejv7/AAU-Projects/tree/master/GAM%20215/Survival%20Game%20-%20Part%202).  You can always check my Github account to see if anyone
plagiarized my work or if I have done the same to others.  Academic integrity is important!