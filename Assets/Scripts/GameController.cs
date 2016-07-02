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

    public float countDown = 6f;

    void Start()
    {
        text = GetComponent<Text>();
        text.text = AI_BlueTeamSettings.TEAM_NAME + " VS " + AI_OrangeTeamSettings.TEAM_NAME;
    }

    void Update()
    {
        countDown -= Time.deltaTime;

        if ( countDown <= 0f )
        {
            Game.StartGame();
            Destroy(gameObject);
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