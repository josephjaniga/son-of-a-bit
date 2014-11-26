using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public Vector3	targetDirection = Vector3.zero;
	public float 	speed = 11.0f;
	public bool 	destroyOnImpact = false;
	public float	birthTime = Time.time;
	public float	lifeSpan = 3f;

	// Use this for initialization
	void Start () {

		birthTime = Time.time;
		rigidbody.velocity = targetDirection * speed;

	}
	
	// Update is called once per frame
	void Update () {
	
		//rigidbody.velocity = targetDirection * speed;

		// kill it after it expires
		if ( Time.time - birthTime >= lifeSpan ){
			Destroy(gameObject);
		}

	}
 
	void OnCollisionEnter(){	 

		if ( destroyOnImpact ){
			Destroy(gameObject);
		} else {
			// change color to sparks
			gameObject.GetComponent<Bit>().setColor(sparks());
			// or destroy this object half a second after impact
			birthTime = Time.time;
			lifeSpan = 0.5f;
		}

	}

	public void setDirection( Vector3 v3 ){
		targetDirection = v3.normalized;
		rigidbody.velocity = targetDirection * speed;
	}

	public Color sparks(){
		return new Color(Random.Range(0.6f,1.0f),Random.Range(0.1f,0.5f),0.0f);
	}

}
