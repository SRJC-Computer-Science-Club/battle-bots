using UnityEngine;
using System.Collections;

public class AI_Blue : BotAI {

    // Initialize class variables here


	// called once per frame
    // This is will most of the AI logic will go
	void AI_Routine () {

        // Example
        BotAI enemy = FindWeakestEnemy();

        if ( enemy != null )
        {
            MoveRight( 1f );
            Attack( enemy );
        }
        // End Example
	}



    // DO NOT MODIFY THIS FUNCTION
    new void FixedUpdate()
    {
        AI_Routine();
        base.FixedUpdate();
    }
}
