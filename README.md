# JetBoxer2D

**JetBox2D** is both a lightweight _2D engine_ for creating C# games, based on the Monogame framework (https://github.com/MonoGame/MonoGame), and a game sample called JetBoxer that showcases its usage.

## The JetBox2D engine contains the following: ##

- Dynamic Input System that supports concurrent input from both gamepad, keyboard, and mouse, via a single InputManager with methods such as GetButtonDown(InputAction). Each InputAction (e.g. ShootRight) is created in such a way that you can map to it Gamepad-Right Trigger, Gamepad-A Button, Keyboard-Space Key, and Mouse-Right Click *at the same time*, meaning that any one of those input types will trigger the ShootRight InputAction.

- Particle System, which you can use to create custom game effects such as explosions or fire trails.

- Physics Colliders, for detecting the collisions between the game objects in the game.

- Objects and States, which are necessary for creating the game's underlying systems (e.g. transition from SplashState to GameplayState).

- Custom Extensions, that can be used to ease the development hassle during the production of the game engine. For example, a wrapper class for Monogame's mouse input, called MouseInput, is used by the Input System to help in mouse input handling.
- 

https://github.com/Rossakis/JetBoxer2D/assets/70864643/c3277572-b19d-4cc9-b7fa-f66a01e6e0c1


## SAST Tools

[PVS-Studio](https://pvs-studio.com/en/pvs-studio/?utm_source=website&utm_medium=github&utm_campaign=open_source) - static analyzer for C, C++, C#, and Java code.
