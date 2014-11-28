using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public Vector3	targetDirection = Vector3.zero;
	public float 	speed = 11.0f;
	public bool 	destroyOnImpact = false;
	public float	birthTime;
	public float	lifeSpan = 3f;
	public float	ROF = 0.5f;

	public int 		numProjectiles = 5;
	public int		projectileDamage = 1;

	public bool		sparkOnCollision = true;
	public Color	colorLow 	= new Color(0.6f,0.1f,0.0f,0.1f);
	public Color	colorHigh	= new Color(1.0f,0.5f,0.0f,0.5f);

	// Use this for initialization
	void Start () {

		birthTime = Time.time;
		rigidbody.velocity = targetDirection * speed;

		Vector3 av;
		av.x = Random.Range (-3f, 3f);
		av.y = Random.Range (-3f, 3f);
		av.z = Random.Range (-3f, 3f);
		rigidbody.angularVelocity = av;

	}
	
	// Update is called once per frame
	void Update () {
	
		//rigidbody.velocity = targetDirection * speed;

		// kill it after it expires
		if ( Time.time - birthTime >= lifeSpan ){
			Destroy(gameObject);
		}

	}
 
	void OnCollisionEnter(Collision c){	 

		damageTarget(c.gameObject, projectileDamage);

		if ( destroyOnImpact ){
			Destroy(gameObject);
		} else {

			if ( sparkOnCollision ){
				// change color to sparks
				gameObject.GetComponent<Bit>().setColor(sparks());

				Vector3 dir;
				dir.x = Random.Range (-3f, 3f);
				dir.y = Random.Range (-3f, 3f);
				dir.z = 0.0f;
				rigidbody.velocity = dir;

			}

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
		return new Color(
				Random.Range(colorLow.r,colorHigh.r),
				Random.Range(colorLow.g,colorHigh.g),
				Random.Range(colorLow.b,colorHigh.b),
				Random.Range(colorLow.a,colorHigh.a)
			);
	}
	
	public void damageTarget(GameObject target, int dmg){

		Health h = target.GetComponent<Health>();

		if ( h != null ){
			h.applyDamage(dmg);
		}

	}


}
