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

    public Teams team;


    // Health
    private float health = 100;
    private int maxHealth = 100;

    // Shield
    private float shield = 100;
    private int maxShield = 100;
    private float shieldChargeRate = 0.5f;
    private int shieldChargeDelay = 250;
    private int shieldChargeTimer = 0;

    // Shots
    private float shotDamage = 10f;
    private float shotSpeed = 15f;
    private int shotDelay = 30;
    private int shotTimer = 0;

    // Movement
    private float movementSpeed = 0.05f;
    private float turnSpeed = 0.05f;



    /*************************************************************************/
    // GETTERS



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



    public float MovementSpeed
    {
        get { return movementSpeed; }
    }

    public float TurnSpeed
    {
        get { return turnSpeed; }
    }

    
    /*************************************************************************/


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        radius  = GetComponent<CircleCollider2D>().radius;
       // healthBar = transform.FindChild( "Canvas" ).FindChild( "Image" ).GetComponent<Image>();

        StatusBar = Instantiate( StatusBar , transform.position + new Vector3( 0 , 0.8f , 0 )  , Quaternion.identity ) as GameObject;
        healthBar = StatusBar.transform.FindChild( "healthBar" ).GetComponent<Image>();
        shieldBar = StatusBar.transform.FindChild( "shieldBar" ).GetComponent<Image>();
    }



    // Update is called once per frame
    public void Update()
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


    void LateUpdate()
    {
        Vector3 pos = transform.position;

        float screenRatio = Screen.width / ( float ) Screen.height;
        float screenWidth = Camera.main.orthographicSize * screenRatio;

        pos.x = Mathf.Clamp( pos.x , -screenWidth + radius, screenWidth - radius );
        pos.y = Mathf.Clamp( pos.y , -Camera.main.orthographicSize + radius , Camera.main.orthographicSize - radius );



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



    public bool Shoot()
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


    public bool Attack( BotAI bot )
    {
        bool success = RotateTowards( bot , 1f );
        Shoot();

        return success;
    }



    /*************************************************************************/
    // Roatation



    public void RotateLeft( float speed )
    {
        Rotate( speed );
    }


    public void RotateRight( float speed )
    {
        Rotate( -speed );
    }


    public void Rotate( float speed )
    {
        Mathf.Clamp( speed , -1f , 1f );
        rb.AddTorque( speed * turnSpeed );
        //transform.Rotate( Vector3.forward * speed * turnSpeed );
    }


    public void RotateBy( float angle , float speed )
    {
        //TODO
    }


    public bool RotateTowards( BotAI bot , float speed = 1.0f )
    {
        if ( bot != null )
        {
            Vector3 vectorToTarget = bot.transform.position - transform.position;
            RotateTowards( Mathf.Atan2( vectorToTarget.y , vectorToTarget.x ) * Mathf.Rad2Deg , speed );

            return true;
        }

        return false;
    }


    public void RotateTowards( float direction , float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Quaternion q = Quaternion.AngleAxis( direction - 90 , Vector3.forward );
        transform.rotation = Quaternion.Slerp( transform.rotation , q , speed * turnSpeed );
    }



    /*************************************************************************/
    // Movement



    public void MoveForward( float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Vector3 movement = new Vector3( 0 , speed , 0 );
        Move( movement , speed );
    }


    public void MoveBackward( float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Vector3 movement = new Vector3( 0 , -speed , 0 );
        Move( movement , speed );
    }


    public void MoveRight( float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Vector3 movement = new Vector3( speed , 0 , 0 );
        Move( movement , speed );
    }


    public void MoveLeft( float speed = 1.0f )
    {
        Mathf.Clamp( speed , -1f , 1f );
        Vector3 movement = new Vector3( -speed , 0 , 0 );
        Move( movement , speed );
    }


    public bool MoveToward( BotAI bot , float speed = 1f , float turnSpeed = 1f )
    {
        bool success = RotateTowards( bot , turnSpeed );
        MoveForward( speed );

        return success;
    }


    public void MoveToward( float direction , float speed = 1f , float turnSpeed = 1f )
    {
        //TODO
    }


    private void Move( Vector3 movement , float speed )
    {
        movement = transform.rotation * ( movement * speed );
        rb.AddForce( movement );
        //movement += transform.position;
        //transform.position = movement;
    }



    /*************************************************************************/
    // FindBots


    public BotAI[] FindBots()
    {
        return GameObject.FindObjectsOfType<BotAI>();
    }


    public BotAI[] FindEnemies()
    {
        return FilterEnemies( FindBots() );
    }


    public BotAI[] FindAllies()
    {
        return FilterAllies( FindBots() );
    }


    /*************************************************************************/
    // Superlatives


    // Closest

    public BotAI FindClosestBot()
    {
        return Closest( FindBots() );
    }

    public BotAI FindClosestEnemy()
    {   
        return Closest( FilterEnemies( FindBots() ) );
    }

    public BotAI FindClosestAlly()
    {
        return Closest( FilterAllies( FindBots() ) );
    }



    // Furthest

    public BotAI FindFurthestBot()
    {
        return Furthest( FindBots() );
    }

    public BotAI FindFurthestEnemy()
    {
        return Furthest( FilterEnemies( FindBots() ) );
    }

    public BotAI FindFurthestAlly()
    {
        return Furthest( FilterAllies( FindBots() ) );
    }



    // HighestHealth

    public BotAI FindHighestHealthBot()
    {
        return HighestHealth( FindBots() );
    }

    public BotAI FindHighestHealthEnemy()
    {
        return HighestHealth( FilterEnemies( FindBots() ) );
    }

    public BotAI FindHighestHealthAlly()
    {
        return HighestHealth( FilterAllies( FindBots() ) );
    }



    // LowestHealth

    public BotAI FindLowestHealthBot()
    {
        return LowestHealth( FindBots() );
    }

    public BotAI FindLowestHealthEnemy()
    {
        return LowestHealth( FilterEnemies( FindBots() ) );
    }

    public BotAI FindLowestHealthAlly()
    {
        return LowestHealth( FilterAllies( FindBots() ) );
    }



    // HighestShield

    public BotAI FindHighestShieldBot()
    {
        return HighestShield( FindBots() );
    }

    public BotAI FindHighestShieldEnemy()
    {
        return HighestShield( FilterEnemies( FindBots() ) );
    }

    public BotAI FindHighestShieldAlly()
    {
        return HighestShield( FilterAllies( FindBots() ) );
    }



    // LowestShield

    public BotAI FindLowestShieldBot()
    {
        return LowestShield( FindBots() );
    }

    public BotAI FindLowestShieldEnemy()
    {
        return LowestShield( FilterEnemies( FindBots() ) );
    }

    public BotAI FindLowestShieldAlly()
    {
        return LowestShield( FilterAllies( FindBots() ) );
    }



    // Strongest

    public BotAI FindStrongestBot()
    {
        return Strongest( FindBots() );
    }

    public BotAI FindStrongestEnemy()
    {
        return Strongest( FilterEnemies( FindBots() ) );
    }

    public BotAI FindStrongestAlly()
    {
        return Strongest( FilterAllies( FindBots() ) );
    }



    // Weakest

    public BotAI FindWeakestBot()
    {
        return Weakest( FindBots() );
    }

    public BotAI FindWeakestEnemy()
    {
        return Weakest( FilterEnemies( FindBots() ) );
    }

    public BotAI FindWeakestAlly()
    {
        return Weakest( FilterAllies( FindBots() ) );
    }





    /*************************************************************************/


    public float DistanceToBot( BotAI bot )
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
