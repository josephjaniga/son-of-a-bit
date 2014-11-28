using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject bullet = null;
	public float lastFired = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if ( Input.GetMouseButton(0) && Time.time - bullet.GetComponent<Projectile>().ROF >= lastFired ){

			//Debug.DrawRay(transform.position, aim()- transform.position, Color.red);
			//Debug.DrawRay(transform.position, aim(), Color.blue);

			lastFired = Time.time;

			for ( var x = 0; x < bullet.GetComponent<Projectile>().numProjectiles; x++ ){
				GameObject round = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
				round.GetComponent<Projectile>().setDirection(aim() - transform.position);
				round.transform.parent = GameObject.Find("Projectiles").transform;
			}

		}

	}

	Vector3 aim(){
	
		Vector3 temp = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		temp.z = 0.0f;
		return temp;

	}

 
}
