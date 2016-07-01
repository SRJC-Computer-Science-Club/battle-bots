using UnityEngine;
using System.Collections;

public static class AI_BlueTeamSettings
{
    public const string TEAM_NAME = "BLUE";
    public const string AUTHOR = "";
    public const string VERSION = "0.0";
}


public class AI_Blue : BotAI
{
    // Initialize class variables here


    // This is will most of the AI logic will go
    // It is called once per frame
    void AI_Routine()
    {

        // Example
        BotAI enemy = FindWeakestEnemy();

        if ( enemy != null )
        {
            if ( ID <= 2 )
            {
                MoveRight( 1.5f );
            }
            else
            {
                MoveForward( .5f );
            }

            ShootAt( enemy );
        }
        // End Example
    }



    // DO NOT MODIFY THIS FUNCTION
    new void FixedUpdate()
    {
        if ( Game.GameStart )
        {
            AI_Routine();
            base.FixedUpdate();
        }
    }
}
