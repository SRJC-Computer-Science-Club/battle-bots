using UnityEngine;
using System.Collections;


public class AI_Orange : BotAI
{

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        BotAI enemy = FindClosestEnemy();

        if ( DistanceToBot( enemy ) > 10f )
        {
            MoveToward( enemy );
        }
        else
        {
            MoveBackward();
        }

        Shoot();


    }
}
