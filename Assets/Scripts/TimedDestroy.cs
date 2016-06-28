using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {

    public float lifeTime = 5f;

	// Use this for initialization
	void Start () {
        Destroy( gameObject , lifeTime );
	}
	
}
