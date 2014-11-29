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
	
		if ( Input.GetMouseButton(0) && Time.time - bullet.GetComponent<Projectile>().ROF >= lastFired ){

			//Debug.DrawRay(transform.position, aim()- transform.position, Color.red);
			//Debug.DrawRay(transform.position, aim(), Color.blue);

			// if the unit has health
			if ( bit.health != null ){
				// and the unit is still alive
				if ( !bit.health.isDead ){
					fire();
				}
			} else {
				fire();
			}

		}


		if ( Input.GetKeyDown(KeyCode.Alpha1) ){
			bullet = (GameObject)Resources.Load("Projectiles/FF_Bullet"); 
		}

		if ( Input.GetKeyDown(KeyCode.Alpha2) ){
			bullet = (GameObject)Resources.Load("Projectiles/FF_Rocket"); 
		}


	}

	Vector3 aim(){
	
		Vector3 temp = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		temp.z = 0.0f;
		return temp;

	}

	public void fire(){

		lastFired = Time.time;
		
		for ( var x = 0; x < bullet.GetComponent<Projectile>().numProjectiles; x++ ){
			GameObject round = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
			round.GetComponent<Projectile>().setDirection(aim() - transform.position);
			round.transform.parent = GameObject.Find("Projectiles").transform;
			round.GetComponent<Projectile>().setOwner(gameObject);
		}

	}

 
}
