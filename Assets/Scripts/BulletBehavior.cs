using UnityEngine;
using System.Collections;



public class BulletBehavior : MonoBehaviour {

    public Sprite[] laserSprites;


    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float speed;

    private float damage;

    private bool init = false;

    private int lifeTime = 500;

    private Teams team;


    public float Damage
    {
        get { return damage; }
    }

    public float Speed
    {
        get { return speed; }
    }

    public Teams Team
    {
        get { return team; }
    }




    public void Init( Teams team, float damage , float speed )
    {
        if ( !init )
        {
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = laserSprites[ (int) team ];
            switch ( team )
            {
                case Teams.BLUE:
                    gameObject.layer = 8;
                    break;
                case Teams.ORANGE:
                    gameObject.layer = 9;
                    break;
                case Teams.GREEN:
                    gameObject.layer = 10;
                    break;
                case Teams.PURPLE:
                    gameObject.layer = 11;
                    break;
                default:
                    break;
            }


            init = true;
            this.team = team;
            this.damage = damage;
            this.speed = speed;

            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.rotation * new Vector3( 0 , speed , 0 );
        }
    }

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        damage *= 0.995f;
        lifeTime--;

        if ( lifeTime <= 0 )
        {
            Destroy( gameObject );
        }
	}




    public float Collision()
    {
        Destroy( gameObject );
        return damage;
    }



}
