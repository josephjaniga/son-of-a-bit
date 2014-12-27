using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public Bit bit;
	public GameObject bullet = null;
	public float lastFired = 0.0f;

	public float critChance = 0.0f;
	public float critDamage = 0.5f;
	
	public Joystick rightJoystick;

	// Use this for initialization
	void Start () {
	
		bit = gameObject.GetComponent<Bit>();

		if ( GameObject.Find("RightJoystick") != null ){
			rightJoystick = GameObject.Find("RightJoystick").GetComponent<Joystick>();
		}

	}
	
	// Update is called once per frame
	void Update () {

		// get the correct rate of fire / flat or calculated
		float tempROF = bullet.GetComponent<Projectile>().ROF;
		
		if ( bit != null && bit.statManager != null ){
			tempROF = bit.statManager.cRateOfFire;
		}
	
		// if weapon owner is user controlled
		if ( bit != null && bit.motion != null && bit.motion.userControlled ){
			if 	( Input.GetMouseButton(0) && Time.time - tempROF >= lastFired ){
				
				//Debug.DrawRay(transform.position, aimAtMouse()- transform.position, Color.red);
				//Debug.DrawRay(transform.position, aimAtMouse(), Color.blue);

				if ( ( rightJoystick != null && rightJoystick.position.x != 0.0f && rightJoystick.position.y != 0.0f ) || rightJoystick == null ){

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

			}	

		} else if ( 	// if the weapon is AI controlled
		           		bit != null
		           		&& bit.artificialInteligence != null
		           		&& bit.artificialInteligence.attackTarget != null
		          		&& Time.time - tempROF >= lastFired ) {

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
		temp = temp - transform.position;

		if ( rightJoystick != null ){
			temp = new Vector3(rightJoystick.position.x, rightJoystick.position.y, 0f);
			temp = temp;
		}

		return temp;

	}

	Vector3 aimAI(){

		//Debug.Log("aimed AI");

		Vector3 temp = Vector3.zero;

		if ( bit.artificialInteligence != null && bit.artificialInteligence.attackTarget != null ){

			Vector3 offset = bit.artificialInteligence.attackTarget.transform.position;
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

		//Debug.Log (target);

		// get the correct rate of fire / flat or calculated
		int tempNumberOfProjectiles = bullet.GetComponent<Projectile>().numProjectiles;
		
		if ( bit.statManager != null ){
			tempNumberOfProjectiles = bit.statManager.cNumberOfProjectiles;
		}

		lastFired = Time.time;

		Vector3 offset = target.normalized;
		offset.x *= transform.localScale.x;
		offset.y *= transform.localScale.y;
		offset.z = 0.0f;

		Pool slowBulletPool = GameObject.Find("SLOW_BULLET_POOL").GetComponent<Pool>();
		Pool ffBulletPool = GameObject.Find("FF_Bullet_POOL").GetComponent<Pool>();


		if ( slowBulletPool != null && bullet.GetComponent<Projectile>().projectileName == slowBulletPool.goInstance.GetComponent<Projectile>().projectileName ){
			
			for ( var x = 0; x < tempNumberOfProjectiles; x++ ){
				GameObject round = slowBulletPool.popFromStack();
				round.transform.position = transform.position + offset;
				round.SetActive(true);
				round.GetComponent<Projectile>().birthTime = Time.time;
				round.GetComponent<Projectile>().setDirection(target - offset);
				round.transform.parent = GameObject.Find("Projectiles").transform;
				round.GetComponent<Projectile>().setOwner(gameObject);
			}

		} else if ( ffBulletPool != null && bullet.GetComponent<Projectile>().projectileName == ffBulletPool.goInstance.GetComponent<Projectile>().projectileName ) {

			for ( var x = 0; x < tempNumberOfProjectiles; x++ ){
				GameObject round = ffBulletPool.popFromStack();
				round.transform.position = transform.position + offset;
				round.SetActive(true);
				round.GetComponent<Projectile>().birthTime = Time.time;
				round.GetComponent<Projectile>().setDirection(target - offset);
				round.transform.parent = GameObject.Find("Projectiles").transform;
				round.GetComponent<Projectile>().setOwner(gameObject);
			}

		} else {
			
			for ( var x = 0; x < tempNumberOfProjectiles; x++ ){
				GameObject round = Instantiate(bullet, transform.position + offset, Quaternion.identity) as GameObject;
				round.GetComponent<Projectile>().setDirection(target - offset);
				round.transform.parent = GameObject.Find("Projectiles").transform;
				round.GetComponent<Projectile>().setOwner(gameObject);
			}

		}

		Debug.DrawRay(transform.position + offset, target - offset, Color.yellow);

	}

 
}
