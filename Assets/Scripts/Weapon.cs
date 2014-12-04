using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public Bit bit;
	public GameObject bullet = null;
	public float lastFired = 0.0f;


	// Use this for initialization
	void Start () {
	
		bit = gameObject.GetComponent<Bit>();

	}
	
	// Update is called once per frame
	void Update () {
	
		// if weapon owner is user controlled
		if ( bit.motion != null && bit.motion.userControlled ){
			if 	(
					Input.GetMouseButton(0)
					&& Time.time - bullet.GetComponent<Projectile>().ROF >= lastFired ){
				
				//Debug.DrawRay(transform.position, aimAtMouse()- transform.position, Color.red);
				//Debug.DrawRay(transform.position, aimAtMouse(), Color.blue);
				
				// if the unit has health
				if ( bit.health != null ){
					// and the unit is still alive
					if ( !bit.health.isDead ){
						fire(aimAtMouse());
					}
				} else {
					fire(aimAtMouse());
				}
				
			}	

			if ( Input.GetKeyDown(KeyCode.Alpha1) ){
				bullet = (GameObject)Resources.Load("Projectiles/FF_Bullet"); 
			}
			
			if ( Input.GetKeyDown(KeyCode.Alpha2) ){
				bullet = (GameObject)Resources.Load("Projectiles/FF_Rocket"); 
			}


		} else if ( 	// if the weapon is AI controlled
		           		bit.artificialInteligence != null
		           		&& bit.artificialInteligence.attackTarget != null
		           		&& Time.time - bullet.GetComponent<Projectile>().ROF >= lastFired ) {

			// if the unit has health
			if ( bit.health != null ){
				// and the unit is still alive
				if ( !bit.health.isDead ){
					fire(aimAI());
				}
			} else {
				fire(aimAI());
			}

		}

	}

	Vector3 aimAtMouse(){
	
		Vector3 temp = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		temp.z = 0.0f;
		return temp - transform.position;

	}

	Vector3 aimAI(){

		Vector3 temp = Vector3.zero;
		if ( bit.artificialInteligence != null && bit.artificialInteligence.attackTarget != null ){

			Vector3 offset = bit.artificialInteligence.attackTarget.transform.position.normalized;
			offset.x *= transform.localScale.x;
			offset.y *= transform.localScale.y;
			offset.z = 0.0f;

			temp = bit.artificialInteligence.attackTarget.transform.position;
			temp.z = 0.0f;
			temp = temp - transform.position;
		}

		return temp;
		
	}

	public void fire(Vector3 target){

		lastFired = Time.time;

		Vector3 offset = target.normalized;
		offset.x *= transform.localScale.x;
		offset.y *= transform.localScale.y;
		offset.z = 0.0f;

		for ( var x = 0; x < bullet.GetComponent<Projectile>().numProjectiles; x++ ){
			GameObject round = Instantiate(bullet, transform.position + offset, Quaternion.identity) as GameObject;
			round.GetComponent<Projectile>().setDirection(target - offset);
			round.transform.parent = GameObject.Find("Projectiles").transform;
			round.GetComponent<Projectile>().setOwner(gameObject);
		}

		Debug.DrawRay(transform.position + offset, target - offset, Color.yellow);

	}

 
}
