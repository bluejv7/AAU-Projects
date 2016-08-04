# Final Project (GAM 215, Module 13, Assignment 13.1: Final Project Part 4 -- Scripted Audio)

## Credits

* "all-animation" for providing the free ninja/zombie animation free of charge at
https://www.assetstore.unity3d.com/en/#!/content/19256
* "dustyroom" for providing free sound effects for free at
https://www.assetstore.unity3d.com/en/#!/content/54116
* "Muz Station Productions" for providing free music tracks for free at
https://www.assetstore.unity3d.com/en/#!/content/10623

## Main file locations

### Script files (Assets/Scripts)

* BossController.cs
* BulletController.cs
* CameraController.cs
* DamageBoostController.cs
* GameController.cs
* HealthPackController.cs
* PlayerAnimatorController.cs
* PlayerController.cs
* SeekingEnemyController.cs
* ShootingEnemyController.cs

### Scene files (Assets/Scenes)

* Game Scene.unity

## Project information

This is a top-down gauntlet type game where the main character tries to escape by fighting through enemies
and eventually defeating a final boss blocking the path to freedom.

## Controls

* Move up/left/down/right: WASD or up/left/down/right arrow keys
* Fire weapon: Left click or left ctrl

### Unity Scene

1. Added 11 rooms (one is a starting room and another is a boss room)
2. Added player
3. Added bullets
4. Added enemy
5. Added winning trigger
6. Added UI text (health, win/lose message)
7. Added animated ninja
8. Added health pack
9. Added damage boost power up
10. Added shooting enemy
11. Added boss
12. Added animations for the player
13. Added looping music

### Script Info

1. Implemented projectile pooling
2. Added limited sphere of vision using raycasting
(although I say "sphere", it's just a raycast that always aims at the player,
which can be potentially a sphere in 3D space)
3. Added configurations for the player (health, speed, damage delay, etc...)
4. Added event handlers for the player (movement, on hit, on goal trigger, update, etc...)
5. Added message sending for the player (tell game controller to spawn bullets or when finish line reached)
6. Added configurations for the enemy
7. Added event handlers for the enemy
8. Added message sending for the enemy (notify player when attacking/colliding)
9. Added seek and destroy AI for the enemy
  1. Prevent AI from moving when it loses track of player
  (I intentionally want the AI to behave a little dumb)
  2. Improve the AI a bit... noticed that the AI would stop following if the player shoots bullets
  to block the raycast and if enemies block each other
10. Added bullet movement
11. Added bullet message sending (for when it hits something)
12. Added game controller pooling of projectiles to reduce CPU usage for spawning projectiles
(at the cost of memory)
13. Added game controller public methods for instantiating projectiles
or consuming projectiles from the pool
14. Added game controller events and methods for winning/losing the game
15. Added game controller event handler for updating player health
16. Added health pack, healing, and modified player's `ChangeHealth()` param to optionally overheal
the player (provide more current health than max health)
17. Added damage boost power up and make use of a function to increase player's damage
18. Added shooting enemy AI, which flees when player is too close and shoots when it can see the player
19. Added animation controller for handling player animation state
20. Added sounds triggered by player damage, enemy damage, enemy attack, bullet spawning,
and winning the game

Author: Jv Chen

## How does my project satisfy the conditions of the assignment?

1. My level demonstrates some of the gameplay that will be available for the final project (incomplete)
2. Made use of at least one array (pooling)
3. Used a loop to instantiate the pool
4. Used animations for the player and had `PlayerAnimatorController` handle the variables that trigger
animation states changing
5. Imported 6 sound files
  * 5 of them are activated by events through scripting
  (player damage, enemy damage, enemy attack, bullet firing, and winning the game)
  * 1 is done through just playing it on an audio source component
  (doesn't count towards required 4 sound files)

## Miscellaneous

This README is a markdown file that can be generated into an HTML page at http://daringfireball.net/projects/markdown/dingus or
in my Github page (https://github.com/bluejv7/AAU-Projects/tree/master/GAM%20215/Marble%20Dungeon).  You can always check my Github account to see if anyone
plagiarized my work or if I have done the same to others.  Academic integrity is important!