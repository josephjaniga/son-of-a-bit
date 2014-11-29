using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	public Bit bit;

	public enum MovementType {
		None,
		Mindless,
		Following,
	};

	public enum AttackType {
		None
	};

	public int movementAI = (int)MovementType.Mindless;
	public int attackAI = (int)AttackType.None;

	public float duration = 1.0f;
	public float delay = 2.0f;
	public float startTime = 0.0f;
	private Vector3 targetDirection = Vector3.zero;

	private float speed = 1.0f;

	public float followDistance = 0.0f;
	public GameObject followTarget;

	public int collisionDamage = 11;

	public int dropRate = 1;


	// Use this for initialization
	void Start () {
	
		bit = gameObject.GetComponent<Bit>();

		if( bit.motion != null){
			speed = bit.motion.speed;
		}

		startTime = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
		// if the units not dead or doesnt have health
		if ( !bit.health.isDead || bit.health == null ){

			if ( movementAI == (int)MovementType.Mindless ) {
				mindless ();
			}

			if ( movementAI == (int)MovementType.Following ) {
				following ();
			}

		}

	}


	void OnCollisionEnter(Collision c){	 
		// check that it has a bit
		if ( c.gameObject != null && c.gameObject.GetComponent<Bit>() != null ){
			// dont damage
			if ( c.gameObject.tag != "Enemy" ){

				if ( ( bit.health != null && !bit.health.isDead ) || bit.health == null ) {
					damageTarget(c.gameObject, collisionDamage);
				}
			}
		}
	}

	
	public void mindless(){
		if ( Time.time - startTime >= duration ){ // reset

			//rigidbody.velocity = Vector3.zero;
			
			// pick a direction
			targetDirection = new Vector3 (
					Random.Range(-1f,1f) * rigidbody.mass * speed,
					Random.Range(-1f,1f) * rigidbody.mass * speed,
					0f
				);
			// wait
			if ( Time.time - startTime >= duration + delay ){
				startTime = Time.time;
			}
				
		} else { // do stuff

			// move for duration
			rigidbody.AddForce(targetDirection);
			// repeat

		}
	}

	public void following(){

		if ( followTarget != null ){

			float y = 0f;
			float x = 0f;
			float z = 0f;

			// target is above
			if ( transform.position.y < followTarget.transform.position.y )
				y = 1f;
			
			// target is below center		
			if ( transform.position.y > followTarget.transform.position.y )
				y = -1f;

			// target is right
			if ( transform.position.x < followTarget.transform.position.x )
				x = 1f;
			
			// target is left		
			if ( transform.position.x > followTarget.transform.position.x )
				x = -1f;

			targetDirection = new Vector3 (
					x * rigidbody.mass * speed,
					y * rigidbody.mass * speed,
					z * rigidbody.mass * speed
				);

			rigidbody.AddForce(targetDirection);

		}
	
	}

	public void damageTarget(GameObject target, int dmg){

		if ( target != null ) {

			Health targetHealth = target.GetComponent<Bit>().health;

			if ( targetHealth != null ){
				targetHealth.applyDamage(dmg, gameObject);
			}

		}

	}

	
}
