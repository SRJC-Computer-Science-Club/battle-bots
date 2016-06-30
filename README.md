
# BattleBots

Inspired by the game [Gladiabots](https://gfx47.itch.io/gladiabots)

This project provides a platform to build AI controlled ships that battle to the death!
There is support for upto 4 teams currently

A live Demo of the project can be seen [here](http://srjcscheduler.com/BattleBots/)  

### Applications Used

###### Required
* [Unity](https://store.unity.com/download?ref=personal) Game Developement Engine

###### Recommended
* [Visual Studio 2015](https://www.visualstudio.com/products/visual-studio-community-vs)
*VS allows for the use of the debugger"


### Getting Started

[Unity Tutorial](https://www.youtube.com/playlist?list=PLbghT7MmckI4IeNHkPm5bFJhY9GQ0anKN)
*(I had actually downloaded the [spaceship sprites](http://kenney.nl/assets/space-shooter-redux) before I saw the tutorial)*

[Unity Manual](https://docs.unity3d.com/Manual/index.html)

[Unity Scripting Reference](https://docs.unity3d.com/ScriptReference/index.html)


### Building the AI

The files you will be changing are
**/Assets/Scripts/AI_Blue.cs**
or
**/Assets/Scripts/AI_Orange.cs**

Each file represents the AI for the corresponding team


##### The Manual

###### Variables
*These are read only*

``` c++
float Health
int MaxHealth

float Shield
int MaxShield
float ShieldChargeRate
int ShieldChargeDelay
int ShieldChargeTimer

float ShotDamage
float ShotSpeed
int ShotDelay
int ShotTimer

float MovementSpeed
float TurnSpeed
```

###### Functions
``` c++
bool Shoot(); 
// Fires in the current direction, 'ShotDelay' is the number of frames between shots
// Returns true if bullet was fired that frame, false otherwise

bool Attack( BotAI bot ); 
// Will turn towards 'bot' and Shoot, returns false if bot does not exist, true otherwise

float DistanceToBot( BotAI bot ); 
// Returns the distance to 'bot'
// Returns INF if 'bot' does not exist

// For the following rotate and move functions 'speed' should be given in the range of -1.0f to 1.0f

void RotateLeft( float speed ); 

void RotateRight( float speed );

void Rotate( float speed ); 
// CCW is positive speed

void RotateBy( float angle , float speed ); 
// NOT IMPLEMENTED

bool RotateTowards( BotAI bot , float speed = 1.0f ); 
// Returns false if bot does not exist, true otherwise

void RotateTowards( float direction , float speed = 1.0f );
// Rotates the ship to match the given 'direction', 0° is to the right and goes CCW

void MoveForward( float speed = 1.0f );
// Using a negative 'speed' as the same as using MoveBackward with positive 'speed'

void MoveBackward( float speed = 1.0f );

void MoveRight( float speed = 1.0f );

void MoveLeft( float speed = 1.0f );

bool MoveToward( BotAI bot , float speed = 1f , float turnSpeed = 1f ); 
// Turns towards and moves towards 'bot'. Returns false if bot does not exist, true otherwise

void MoveToward( float direction , float speed = 1f , float turnSpeed = 1f ); 
// NOT IMPLEMENTED


// Any of the following superlatives can be used in place of "Superlative" for the 3 functions below
/*
Closest  			// will not return itself
Furthest			// will not return itself
Strongest     // Health + Shield
Weakest				// Health + Shield
HighestHealth
LowestHealth
HighestShield
LowestShield
*/
// ex.  FindLowestHealthEnemy()

BotAI FindSuperlativeBot();
//returns null if no such bot exists

BotAI FindSuperlativeEnemy();
//returns null if no such bot exists

BotAI FindSuperlativeAlly();
//returns null if no such bot exists

```
###### The AI

A simple AI file will look like this
``` cs
using UnityEngine;
using System.Collections;

public class AI_Blue : BotAI {

    // Initialize class variables here


	// Update is called once per frame
    // This is will most of the AI logic will go
	void FixedUpdate () {

        // Example
        BotAI enemy = FindWeakestEnemy();

        if ( enemy != null )
        {
            MoveRight( .5f );
            Attack( enemy );
        }
        // End Example
        
	}
}
```
### Roadmap

**v1.0**
* Add Bot ID's
* Fix movement
* Add UI
* Load AI Dynamically
* Provide access to arena size
* Provide access to getBots/Enemies/Allies
* Balance Bot paramters
