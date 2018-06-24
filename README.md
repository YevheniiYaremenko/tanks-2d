# Tanks-2d
This repository is Unity2D project of simple top-down pixel-art game.

## System requirements:
- Unity3D 2018.1+;
- TexturePacker;

## Used tools:
- TexturePacker;
- TextMeshPro;
- Navigation2D;

## Latest releases:
- [Windows](https://drive.google.com/open?id=1JV4_JfL6FIybo-84hX6Y4tKFQZXFyBG2);
- [MacOs](https://drive.google.com/open?id=1DiXPVqGmBoSCUWb23tayVNgWn8uqgr0u);

## Game`s overview
The main goal of the game: survive until the timer expires.
Player should shot on enemies or run away by controlling selected tank.

There are 4 types of player tanks: simple, fast, heavy and double-armed. Each of them has some values of max health, movement and rotation speed, tower rotation speed and list of available weapons: gun, machine gun, laser. Each of weapon type has unique behaviour.

Enemies in levels are spawned randomly in pre-installed points on the map. Enemies directly go on the controllable tank and will destroy when collide with player.

Each game the system counts enemy kills and calculates level score. Best score is saved in memory.

## Project`s overview
All created assets and script are placed on Assets/Game directory.

The main class of the project is [MainController](https://github.com/YevheniiYaremenko/tanks-2d/blob/master/Assets/Game/Scripts/Singletons/MainController.cs). It handles game events and user input events.

Game UI consists of independent UI screens. Navigation between screens is implemented using MainController. Each UI screen is created using Unity UI system and TextMeshPro plugin.

There are 3 unity scenes:
- Start - contains main controllers, UI, tank and enemy factories, etc.
- Menu;
- Level1 - generated using Tile Palette map of the first level;

Navigation between scenes is implemented by [SceneNavigationManager](https://github.com/YevheniiYaremenko/tanks-2d/blob/master/Assets/Game/Scripts/Singletons/SceneNavigationManager.cs).

Information about game session (enemy kills, score, etc) is stores into the memory using [Data](https://github.com/YevheniiYaremenko/tanks-2d/blob/master/Assets/Game/Scripts/Model/Data.cs). 

Enemies spawns in the game using [Factory](https://github.com/YevheniiYaremenko/tanks-2d/blob/master/Assets/Game/Scripts/Factory/Factory.cs) and [Spawner](https://github.com/YevheniiYaremenko/tanks-2d/blob/master/Assets/Game/Scripts/Spawner/Spawner.cs) classes.
