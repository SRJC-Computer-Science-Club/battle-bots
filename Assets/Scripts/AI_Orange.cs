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
        //AI_Orange ally = (AI_Orange) FindClosestAlly();

        //Rotate(1);
        Shoot();
        //MoveToward( enemy , -1f );
        // End Example
    }



    // DO NOT MODIFY THIS FUNCTION
    new void FixedUpdate()
    {
        AI_Routine();
        base.FixedUpdate();
    }
}
