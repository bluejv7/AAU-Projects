# Game Name

Marble Dungeon

# Game Description

Our player (a marble) is trapped in a dungeon where she has to escape by navigating to the exit while defeating enemies in her path.
Armed with a mini marble firing weapon, she must brave through rooms of marble enemies that will chase her down or shoot at her.
Will she survive the dungeon and be triumphant against the final boss awaiting at the exit and make her escape from the dungeon?

# Game Functionality and Organization

## Camera

* Mostly top-down, but will be tilted on the x/z axis a little (No rotation)
* (Low Priority) Planned on allowing the player to zoom-in/zoom-out

## Player

* Has two-dimensional movement (up, down, left, right)
* Can fire projectiles
* Has max/current health values
* Has damage values that get increased by power-ups
* (Low Priority) Planning on giving a secondary attack where a sphere of ki emanates from the character and damages enemies
while destroying projectiles, while negating any damage for the duration of the move

## Enemies

### All Enemies

* Have max/current health values
* Will change color based on the amount of damage they receive
* Have damage values
* Have attack delays

### Enemy 1

* Only starts following the player if a raycast of a certain length hits the player (line of sight)

### Enemy 2

* Moves away from the player if the player moves too close
* Fires slow moving projectiles at the player
* Only fires if a raycast of a certain length hits the player (line of sight)

### Boss

* Shoots multiple projectiles periodically
* Will have different patterns of projectiles that the player will need to dodge
* Periodically spawns enemies around it

## Projectile Controller

* Handles collision messages to object that gets hit
* Gets assigned damage numbers when instantiated
* Gets assigned which tagged objects should be sent an `OnShot` message
* Notifies game controller when it collides and adds itself to the resource pool

## Game Controller

* Handles the display for max/current health and damage
* Makes checks for win/lose conditions and triggers the appropriate scenes/ui changes
* Handle spawning of enemies if player triggers a trap room
* Has a pool of projectiles to be recycled so we don't constantly create/destroy projectiles

# Miscellaneous

This file can be viewed in its HTML form at https://github.com/bluejv7/AAU-Projects/blob/master/GAM%20215/10_1_Chen.md