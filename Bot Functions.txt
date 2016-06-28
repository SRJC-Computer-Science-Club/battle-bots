
// These variables are read only

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



bool Shoot() // Returns true if bullet was fired that frame, false otherwise
bool Attack( BotAI bot ) // Will turn towards 'bot' and Shoot, returns false if bot does not exist, true otherwise

float DistanceToBot( BotAI bot ) //returns INF if 'bot' does not exist

// speed range [-1f,1f]
void RotateLeft( float speed ) 
void RotateRight( float speed )
void Rotate( float speed ) // CCW is positive speed
void RotateBy( float angle , float speed ) // NOT IMPLEMENTED
bool RotateTowards( BotAI bot , float speed = 1.0f ) // Returns false if bot does not exist, true otherwise
void RotateTowards( float direction , float speed = 1.0f )

void MoveForward( float speed = 1.0f )
void MoveBackward( float speed = 1.0f )
void MoveRight( float speed = 1.0f )
void MoveLeft( float speed = 1.0f )
bool MoveToward( BotAI bot , float speed = 1f , float turnSpeed = 1f ) // Turns towards and moves towards 'bot'. Returns false if bot does not exist, true otherwise
void MoveToward( float direction , float speed = 1f , float turnSpeed = 1f ) // NOT IMPLEMENTED


// Any of the following superlatives can be used in place of "Superlative"
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

BotAI FindSuperlativeBot()
BotAI FindSuperlativeEnemy()
BotAI FindSuperlativeAlly()
//returns null if no such bot exists
