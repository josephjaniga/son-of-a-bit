using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject bullet = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if ( Input.GetMouseButton(0) ){

			//Debug.DrawRay(transform.position, aim()- transform.position, Color.red);
			//Debug.DrawRay(transform.position, aim(), Color.blue);

			GameObject round = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
			//round.GetComponent<Projectile>().targetDirection =  (aim() - transform.position).normalized;
			round.GetComponent<Projectile>().setDirection(aim() - transform.position);

		}

	}

	Vector3 aim(){
	
		Vector3 temp = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		temp.z = 0.0f;
		return temp;

	}

 
}
