using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public enum Teams
{
    BLUE, ORANGE, GREEN, PURPLE
}

public class BotAI : MonoBehaviour
{
    public GameObject LaserBullet;

    public GameObject StatusBar;

    private Image healthBar;
    private Image shieldBar;

    private Rigidbody2D rb;
    private float radius;

    private Teams team;

    public int ID = 0;

    // Health
    private float health = 100;
    private int maxHealth = 100;

    // Shield
    private float shield = 100;
    private int maxShield = 100;
    private float shieldChargeRate = 0.05f;
    private int shieldChargeDelay = 250;
    private int shieldChargeTimer = 0;

    // Shots
    private float shotDamage = 10f;
    private float shotSpeed = 15f;
    private int shotDelay = 40;
    private int shotTimer = 0;

    // Movement and turning
    private Vector2 force;
    private float forceScale = 50f;
    private float turn;
    private float turnScale = 30f;

    // Arena size
    private float halfArenaWidth;
    private float halfArenaHeight;
    private float arenaWidth;
    private float arenaHeight;




    /*************************************************************************/
    // GETTERS


    public Teams Team
    {
        get { return team; }
    }



    public float Health
    {
        get { return health; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }



    public float Shield
    {
        get { return shield; }
    }

    public int MaxShield
    {
        get { return maxShield; }
    }

    public float ShieldChargeRate
    {
        get { return shieldChargeRate; }
    }

    public int ShieldChargeDelay
    {
        get { return shieldChargeDelay; }
    }

    public int ShieldChargeTimer
    {
        get { return shieldChargeTimer; }
    }



    public float ShotDamage
    {
        get { return shotDamage; }
    }

    public float ShotSpeed
    {
        get { return ShotSpeed; }
    }

    public int ShotDelay
    {
        get { return shotDelay; }
    }

    public int ShotTimer
    {
        get { return shotTimer; }
    }



    public Vector2 Force
    {
        get { return force; }
    }

    public float ForceScale
    {
        get { return forceScale; }
    }

    public float Turn
    {
        get { return turn; }
    }

    public float TurnScale
    {
        get { return turnScale; }
    }



    public float ArenaWidth
    {
        get { return arenaWidth; }
    }

    public float ArenaHeight
    {
        get { return arenaHeight; }
    }



    public float Radius
    {
        get { return radius; }
    }


    public float X
    {
        get { return transform.position.x; }
    }


    public float Y
    {
        get { return transform.position.y; }
    }


    public float Direction
    {
        get { return Mathf.Atan2( transform.rotation.y , transform.rotation.x ) * Mathf.Rad2Deg; }
    }


    /*************************************************************************/


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        radius  = GetComponent<CircleCollider2D>().radius;

        StatusBar = Instantiate( StatusBar , transform.position + new Vector3( 0 , 0.8f , 0 )  , Quaternion.identity ) as GameObject;
        healthBar = StatusBar.transform.FindChild( "healthBar" ).GetComponent<Image>();
        shieldBar = StatusBar.transform.FindChild( "shieldBar" ).GetComponent<Image>();

        halfArenaWidth = Camera.main.orthographicSize * Screen.width / ( float ) Screen.height - radius;
        halfArenaHeight = Camera.main.orthographicSize - radius;

        arenaWidth = 2 * halfArenaWidth;
        arenaHeight = 2 * halfArenaHeight;


        switch( gameObject.layer )
        {
            case 8:
                team = Teams.BLUE;
                break;
            case 9:
                team = Teams.ORANGE;
                break;
            case 10:
                team = Teams.GREEN;
                break;
            case 11:
                team = Teams.PURPLE;
                break;
        }
    }



    // Update is called once per frame
    protected void Update()
    {
        if ( shieldChargeTimer <= 0 && shield < maxShield )
        {
            shield += shieldChargeRate;

            if ( shield > maxShield )
            {
                shield = maxShield;
            }

            shieldBar.fillAmount = shield / maxShield;
        }
        
        shotTimer--;
        shieldChargeTimer--;
    }


    protected void FixedUpdate()
    {
        Mathf.Clamp( turn , -1f , 1f );
        transform.Rotate( Vector3.forward * turn * turnScale );
        turn = 0f;

        force = Vector2.ClampMagnitude( force , 1f );
        force *= forceScale;
        rb.AddForce( force );
        force = new Vector2( 0 , 0 );
    }



    void LateUpdate()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp( pos.x , -halfArenaWidth, halfArenaWidth );
        pos.y = Mathf.Clamp( pos.y , -halfArenaHeight , halfArenaHeight );

        transform.position = pos;
        StatusBar.transform.position = pos + new Vector3( 0 , 0.8f , 0 );
    }


    /*************************************************************************/
    // Collision and Damage



    private void OnTriggerEnter2D( Collider2D other )
    {
        if ( other.tag == "Bullet" && other.gameObject.layer != gameObject.layer )
        {
            ApplyDamage( other.GetComponent<BulletBehavior>().Collision() );
        }
    }

    private void ApplyDamage( float damage )
    {
        shieldChargeTimer = shieldChargeDelay;

        healthBar.fillAmount = health / maxHealth;
        shieldBar.fillAmount = shield / maxShield;

        if ( shield >= damage )
        {
            shield -= damage;
        }
        else
        {
            damage -= shield;
            shield = 0;

            health -= damage;

            if ( health <= 0 )
            {
                Destroy( StatusBar );
                Destroy( gameObject );
            }
        }
    }




    /*************************************************************************/
    // Shooting and Attacking



    protected bool Shoot()
    {
        if ( shotTimer <= 0 )
        {
            GameObject bullet = Instantiate( LaserBullet , transform.position + ( transform.rotation * new Vector3( 0 , 0.4f , 0 ) ) , transform.rotation ) as GameObject;
            bullet.GetComponent<BulletBehavior>().Init( team , shotDamage , shotSpeed );
            shotTimer = shotDelay;
            return true;
        }
        else
        {
            return false;
        }
    }


    protected bool Attack( BotAI bot )
    {
        bool success = RotateTowards( bot , 1f );
        Shoot();

        return success;
    }



    /*************************************************************************/
    // Roatation



    protected void RotateLeft( float speed )
    {
        Rotate( speed );
    }


    protected void RotateRight( float speed )
    {
        Rotate( -speed );
    }


    protected void Rotate( float speed )
    {
        Mathf.Clamp( speed , -1f , 1f );
        turn += speed;
    }


    protected void RotateBy( float angle , float speed )
    {
        //TODO
    }


    protected bool RotateTowards( BotAI bot , float speed = 1.0f )
    {
        if ( bot != null )
        {
            Vector3 vectorToTarget = bot.transform.position - transform.position;
            RotateTowards( Mathf.Atan2( vectorToTarget.y , vectorToTarget.x ) * Mathf.Rad2Deg , speed );

            return true;
        }

        return false;
    }


    protected void RotateTowards( float direction , float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Quaternion q = Quaternion.AngleAxis( direction - 90 , Vector3.forward );
        transform.rotation = Quaternion.Slerp( transform.rotation , q , speed * turnScale );
    }



    /*************************************************************************/
    // Movement



    protected void MoveForward( float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Vector3 movement = new Vector3( 0 , speed , 0 );
        Move( movement , speed );
    }

    

    protected void MoveBackward( float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Vector3 movement = new Vector3( 0 , -speed , 0 );
        Move( movement , speed );
    }


    protected void MoveRight( float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Vector3 movement = new Vector3( speed , 0 , 0 );
        Move( movement , speed );
    }


    protected void MoveLeft( float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Vector3 movement = new Vector3( -speed , 0 , 0 );
        Move( movement , speed );
    }


    protected bool MoveToward( BotAI bot , float speed = 1f , float turnSpeed = 1f )
    {
        bool success = RotateTowards( bot , turnSpeed );
        MoveForward( speed );

        return success;
    }


    protected void MoveToward( float direction , float speed = 1f , float turnSpeed = 1f )
    {
        //TODO
    }


    private void Move( Vector3 movement , float speed )
    {
        force += (Vector2) (transform.rotation * ( movement * speed * Time.deltaTime ));
    }



    /*************************************************************************/
    // FindBots


    protected BotAI[] FindBots()
    {
        return GameObject.FindObjectsOfType<BotAI>();
    }


    protected BotAI[] FindEnemies()
    {
        return FilterEnemies( FindBots() );
    }


    protected BotAI[] FindAllies()
    {
        return FilterAllies( FindBots() );
    }


    /*************************************************************************/
    // Superlatives


    // Closest

    protected BotAI FindClosestBot()
    {
        return Closest( FindBots() );
    }

    protected BotAI FindClosestEnemy()
    {   
        return Closest( FilterEnemies( FindBots() ) );
    }

    protected BotAI FindClosestAlly()
    {
        return Closest( FilterAllies( FindBots() ) );
    }



    // Furthest

    protected BotAI FindFurthestBot()
    {
        return Furthest( FindBots() );
    }

    protected BotAI FindFurthestEnemy()
    {
        return Furthest( FilterEnemies( FindBots() ) );
    }

    protected BotAI FindFurthestAlly()
    {
        return Furthest( FilterAllies( FindBots() ) );
    }



    // HighestHealth

    protected BotAI FindHighestHealthBot()
    {
        return HighestHealth( FindBots() );
    }

    protected BotAI FindHighestHealthEnemy()
    {
        return HighestHealth( FilterEnemies( FindBots() ) );
    }

    protected BotAI FindHighestHealthAlly()
    {
        return HighestHealth( FilterAllies( FindBots() ) );
    }



    // LowestHealth

    protected BotAI FindLowestHealthBot()
    {
        return LowestHealth( FindBots() );
    }

    protected BotAI FindLowestHealthEnemy()
    {
        return LowestHealth( FilterEnemies( FindBots() ) );
    }

    protected BotAI FindLowestHealthAlly()
    {
        return LowestHealth( FilterAllies( FindBots() ) );
    }



    // HighestShield

    protected BotAI FindHighestShieldBot()
    {
        return HighestShield( FindBots() );
    }

    protected BotAI FindHighestShieldEnemy()
    {
        return HighestShield( FilterEnemies( FindBots() ) );
    }

    protected BotAI FindHighestShieldAlly()
    {
        return HighestShield( FilterAllies( FindBots() ) );
    }



    // LowestShield

    protected BotAI FindLowestShieldBot()
    {
        return LowestShield( FindBots() );
    }

    protected BotAI FindLowestShieldEnemy()
    {
        return LowestShield( FilterEnemies( FindBots() ) );
    }

    protected BotAI FindLowestShieldAlly()
    {
        return LowestShield( FilterAllies( FindBots() ) );
    }



    // Strongest

    protected BotAI FindStrongestBot()
    {
        return Strongest( FindBots() );
    }

    protected BotAI FindStrongestEnemy()
    {
        return Strongest( FilterEnemies( FindBots() ) );
    }

    protected BotAI FindStrongestAlly()
    {
        return Strongest( FilterAllies( FindBots() ) );
    }



    // Weakest

    protected BotAI FindWeakestBot()
    {
        return Weakest( FindBots() );
    }

    protected BotAI FindWeakestEnemy()
    {
        return Weakest( FilterEnemies( FindBots() ) );
    }

    protected BotAI FindWeakestAlly()
    {
        return Weakest( FilterAllies( FindBots() ) );
    }





    /*************************************************************************/


    protected float DistanceToBot( BotAI bot )
    {
        if ( bot == null )
        {
            return float.PositiveInfinity;
        }
        else
        {
            return Vector2.Distance( bot.transform.position , transform.position );
        }
    }



    /*************************************************************************/
    // Helpers


    private BotAI[] FilterEnemies( BotAI[] bots )
    {
        int enemyCount = 0;

        foreach ( BotAI bot in bots )
        {
            if ( bot.gameObject.layer != gameObject.layer )
            {
                enemyCount++;
            }
        }

        BotAI[] enemies = new BotAI[ enemyCount ];
        int i = 0;

        foreach ( BotAI bot in bots )
        {
            if ( bot.gameObject.layer != gameObject.layer )
            {
                enemies[ i ] = bot;
                i++;
            }
        }

        return enemies;
    }


    private BotAI[] FilterAllies( BotAI[] bots )
    {
        int allyCount = 0;

        foreach ( BotAI bot in bots )
        {
            if ( bot.gameObject.layer == gameObject.layer )
            {
                allyCount++;
            }
        }

        BotAI[] allies = new BotAI[ allyCount ];
        int i = 0;

        foreach ( BotAI bot in bots )
        {
            if ( bot.gameObject.layer == gameObject.layer )
            {
                allies[ i ] = bot;
                i++;
            }
        }

        return allies;
    }


    private BotAI Closest( BotAI[] bots )
    {
        BotAI closest = null;
        float bestDistance = float.PositiveInfinity;

        foreach ( BotAI bot in bots )
        {
            if ( bot == null || bot == this ) continue;

            float tempDistance = Vector2.Distance( bot.transform.position , transform.position );

            if ( tempDistance < bestDistance )
            {
                bestDistance = tempDistance;
                closest = bot;
            }
        }

        return closest;
    }


    private BotAI Furthest( BotAI[] bots )
    {
        BotAI furthest = null;
        float bestDistance = 0f;

        foreach ( BotAI bot in bots )
        {
            if ( bot == null || bot == this ) continue;

            float tempDistance = Vector2.Distance( bot.transform.position , transform.position );

            if ( tempDistance > bestDistance )
            {
                bestDistance = tempDistance;
                furthest = bot;
            }
        }

        return furthest;
    }


    private BotAI HighestHealth( BotAI[] bots )
    {
        BotAI strongest = null;
        float bestHealth = 0f;

        foreach ( BotAI bot in bots )
        {
            if ( bot != null && bot.Health > bestHealth )
            {
                bestHealth = bot.Health;
                strongest = bot;
            }
        }

        return strongest;
    }


    private BotAI LowestHealth( BotAI[] bots )
    {
        BotAI weakest = null;
        float worstHealth = float.PositiveInfinity;

        foreach ( BotAI bot in bots )
        {
            if ( bot != null && bot.Health < worstHealth )
            {
                worstHealth = bot.Health;
                weakest = bot;
            }
        }

        return weakest;
    }


    private BotAI HighestShield( BotAI[] bots )
    {
        BotAI strongest = null;
        float Shield = 0f;

        foreach ( BotAI bot in bots )
        {
            if ( bot != null && bot.Shield > Shield )
            {
                Shield = bot.Shield;
                strongest = bot;
            }
        }

        return strongest;
    }


    private BotAI LowestShield( BotAI[] bots )
    {
        BotAI weakest = null;
        float worstShield = float.PositiveInfinity;

        foreach ( BotAI bot in bots )
        {
            if ( bot != null && bot.Shield < worstShield )
            {
                worstShield = bot.Shield;
                weakest = bot;
            }
        }

        return weakest;
    }


    private BotAI Strongest( BotAI[] bots )
    {
        BotAI strongest = null;
        float bestStrength = 0f;

        foreach ( BotAI bot in bots )
        {
            if ( bot != null && bot.Health + bot.Shield > bestStrength )
            {
                bestStrength = bot.Health + bot.Shield;
                strongest = bot;
            }
        }

        return strongest;
    }


    private BotAI Weakest( BotAI[] bots )
    {
        BotAI weakest = null;
        float worstStrength = float.PositiveInfinity;

        foreach ( BotAI bot in bots )
        {
            if ( bot != null && bot.Health + bot.Shield < worstStrength )
            {
                worstStrength = bot.Health + bot.Shield;
                weakest = bot;
            }
        }

        return weakest;
    }

}
