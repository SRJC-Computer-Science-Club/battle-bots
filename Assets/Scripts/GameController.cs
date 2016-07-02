using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public static class Game {
    private static bool gameStart = false;

    public static bool GameStart
    {
        get
        {
            return gameStart;
        }
    }

    public static void StartGame()
    {
        gameStart = true;
    }
}


public class GameController : MonoBehaviour
{
    public Text text;

    public float countDown = 4f;

    void Start()
    {
        text = GetComponent<Text>();
        text.text = AI_BlueTeamSettings.TEAM_NAME + " VS " + AI_OrangeTeamSettings.TEAM_NAME;

        
    }

    void Update()
    {
        countDown -= Time.deltaTime;

        string minutes = Mathf.Floor( -countDown / 60 ).ToString( "00" );
        string seconds = Mathf.Floor( -countDown % 60 ).ToString( "00" );
        string hundreds = Mathf.Floor( -countDown * 100 % 100 ).ToString( "00" );

        if ( countDown <= 0f )
        {
            Game.StartGame();
            text.rectTransform.position = new Vector3( 0 , 19f , 0 );
            text.fontSize = 18;
            text.text = minutes + ":" + seconds + "." + hundreds;
        }
        else if( countDown <= 1f )
        {
            text.text = "1";
        }
        else if ( countDown <= 2f )
        {
            text.text = "2";
        }
        else if ( countDown <= 3f )
        {
            text.text = "3";
        }
    }
}