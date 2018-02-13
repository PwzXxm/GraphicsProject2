# Rolling Rolling
This is a repository for unimelb COMP30019 GRAPHICS AND INTERACTION Project2.



### Description
This game is a puzzle game. The goal of the game is to make a 2x1 cuboid stand on the green 1x1 square.

The player only can roll the cuboid to move and will restart the level if the cuboid fails to stand/lie on the base cubes.

### Gameplay
Link: https://youtu.be/pcggNpw43Ag

### Controls
To start the game, when the game is loaded, press `Start Game`.

To see the introduction, press `Introduction`.

To exit the game, press the button `Exit`.

Player can use arrow keys/swipe gestures to roll the cuboid.

| Key | Swipe | Description |
|-----|-------|-------------|
| UP | From lower to higher | Rolling forwards |
| DOWN | From higher to lower | Rolling backwards |
| LEFT | From right to left | Rolling to the left |
| RIGHT | From left to right | Rolling to the right |



### Graphics

In the Menu scene, fog shader and particle system are used to created a cloud-like effect behind all UI. There is a image with blur effect used as background of the scene.

In the Main scene, player and base cubes are modelled and imported to Unity from Blender. Phong shader are applied to them.

In the Ending Scene, there is a player stand on the ground with shadows.

Between each scene, fading effect is used.



### Reference Code

Fading effect learned from
>https://www.youtube.com/watch?v=0HwZQt94uHQ


Fog shader learned from
> Assets store -> Lazyfog
