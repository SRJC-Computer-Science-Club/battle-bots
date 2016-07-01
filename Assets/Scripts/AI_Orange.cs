using UnityEngine;
using System.Collections;


public class AI_Orange : BotAI
{
    // Initialize class variables here


    // called once per frame
    // This is will most of the AI logic will go
    void AI_Routine()
    {

        // Example
        BotAI enemy = FindClosestEnemy();
        AI_Orange ally = (AI_Orange) FindClosestAlly();

    if ( DistanceToBot( enemy ) > 10f )
        {
            // MoveToward( enemy );
            //Rotate( 1f );
           // enemy.Rotate( 1f );
            ally.Rotate( 1f );
        }
        else
        {
            MoveBackward();
        }

        Shoot();
        // End Example
    }



    // DO NOT MODIFY THIS FUNCTION
    new void FixedUpdate()
    {
        AI_Routine();
        base.FixedUpdate();
    }
}
