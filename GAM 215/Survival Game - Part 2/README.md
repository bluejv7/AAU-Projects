# Survival Game (Part 2) (GAM 215, Module 6, Assignment 6.1: Survival Game)

## Main file locations:

* The C# scripts are located in `Assets/Scripts/CharacterRigidBodyController.cs`, `Assets/Scripts/PyramidObstacle.cs`,
`Assets/Scripts/SharkController.cs`, and `Assets/Scripts/SharkPatrollingController.cs`
* The Unity scene is located in `Assets/Scenes/PlatformScene.unity`

## Project information

### Unity Scene

1. Added enemy objects (3 patrolling enemies and 2 seek and destroy enemies)

### Script Info

1. Added `SharkController.cs`, which tries to follow the player if the player is within certain distance of the shark
2. Added `SharkPatrollingController.cs`, which tries to follow a patrol path (an array of `Vector3` positions)
3. Added `PyramidObstacle.cs`, which calls the player's `Damage()` function to attempt to apply damage on trigger
4. Sharks on collision will call the player's `Damage()` function to attempt to apply damage
5. Sharks have a bobbing animation that is done through the script (should be done with an animation component, but I still need to learn how to use that)
6. There are some hacky blocks of code made to correct the bobbing animation of the sharks and to adjust how my model looks at the player or patrol points
7. Public variables are now `[SerializeField] private ...` variables; preventing other classes from modifying the private variables, but still giving control to the inspector in Unity

Author: Jv Chen

## How does my project satisfy the conditions of the assignment?

1. I made a designer-friendly player script
2. I made a designer-friendly enemy behavior script (2 of them!)
3. I have modified my old level scene (so it's technically new... but not really -- I asked for permission to reuse and modify!)
4. I replaced the public variables with serialized private variables
5. I have removed the lose condition check in the `Update()` function and applied it in its own function that is not triggered
by other `Update()` functions or similar functions (they are all triggered by events rather than update cycles)

## Miscellaneous

This README is a markdown file that can be generated into an HTML page at http://daringfireball.net/projects/markdown/dingus or
in my Github page (https://github.com/bluejv7/AAU-Projects/tree/master/GAM%20215/Survival%20Game).  You can always check my Github account to see if anyone
plagiarized my work or if I have done the same to others.  Academic integrity is important!