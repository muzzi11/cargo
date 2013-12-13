Cargo
=====

Cargo is a space oriented game for mobile platforms. The player tries to buy goods and sell them for more by transporting them to different systems. During your travels you will encounter hostile ships that are after your cargo, you can fend these ships off, call for back-up, drop your cargo or try to make a run for it.

---------

How to get it up and running
---------

- Clone the repository: git clone git@github.com:muzzi11/cargo.git
- Download and install Unity from http://unity3d.com/
- Open Unity and choose Open Project and browse to the Cargo folder inside the folder you just cloned
- From the File menu select Build settings
- Choose the desired platform to run on and click Build and Run

Features
--------
- The game starts with a simple title screen.
- After starting or continuing a game, the starmap is shown.
- Tapping a location on the starmap moves the player to that location with a certain speed. If a planet is selected, a new screen will be shown upon reaching that location. The player will be able to transition to the auction house screen from here, or to an engineering bay screen.
- In the auction house the player will be able to buy or sell goods with or for currency. Buy and sell prices will differ from system to system.
- Carrying goods slows down the ship's speed during starmap travel.
- In the engineering bay the player can increase weapon damage, hull armor, shield capacity, shield regeneration rate, sensor range and ship speed by spending currency/goods and repair any hull damage.
- During travels the player will randomly encounter hostile ships. When this happens the screen transitions to the battle screen.
- During battle, the player and AI take turns. The ship with the highest sensor range goes first. For every turn, only one action may be taken. The player can choose to drop the cargo  or flee in an attempt to leave the battle. Dropping the cargo will yield the highest chance of getting away. The player can also send out a distress call, this might result in the player getting aided by friendly shis, or the hostile ship to leave in fear of reinforcements. Lastly, the player may also attack the hostile ship until it retreats or gets destroyed.
- When a ship takes damage, the shield will absorb all or a portion of the damage until it is depleted. Every turn the shield replenishes depending on the shield regeneration rate of the ship. When there's no shield to absorb the damage, the hull starts taking damage. When the hull reaches 0, it's game over and the player is returned to the last planet he/she left from.

Frameworks, language and libraries
---------
- C#
- Unity

Mock-up screens
---------------

![Title screen](https://github.com/muzzi11/cargo/raw/master/Doc/titlescreen.jpg)&nbsp;
![Starmap](https://github.com/muzzi11/cargo/raw/master/Doc/starmap.jpg)&nbsp;

![Auction house](https://raw.github.com/muzzi11/cargo/master/Doc/auction-house.jpg)&nbsp;
![Battle screen](https://raw.github.com/muzzi11/cargo/master/Doc/battlescreen.jpg)