
// These variables are read only
### Variables
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

### Functions
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
