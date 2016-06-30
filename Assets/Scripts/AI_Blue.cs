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
