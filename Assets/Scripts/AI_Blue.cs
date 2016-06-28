using UnityEngine;
using System.Collections;


public class AI_Blue : BotAI {

	
	// Update is called once per frame
	void FixedUpdate () {
        base.Update();


        //MoveToward( FindClosestBot() , 1f , .3f);

        //RotateRight( 1f );
        //MoveForward( 1f );
        BotAI enemy = FindWeakestEnemy();

        if ( enemy != null )
        {
            MoveRight( .5f );
            Attack( enemy );
        }
	}
}
