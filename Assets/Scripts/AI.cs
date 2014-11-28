using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

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

	public Motion m;
	public Health h;
	public Bit b;
	private float speed = 1.0f;

	public float followDistance = 0.0f;
	public GameObject followTarget;

	public int collisionDamage = 11;


	// Use this for initialization
	void Start () {
	
		m = gameObject.GetComponent<Motion>();
		if( m != null){
			speed = m.speed;
		}

		h = gameObject.GetComponent<Health>();

		b = gameObject.GetComponent<Bit>();

		startTime = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
	
		// if the units not dead or doesnt have health
		if ( !h.isDead || h == null ){

			if ( movementAI == (int)MovementType.Mindless ) {
				mindless ();
			}

			if ( movementAI == (int)MovementType.Following ) {
				following ();
			}

		}

		// if the unit is dead
		if ( h != null && h.isDead ){
			// drop it to the ground
			rigidbody.useGravity = true;

			// set its dead color
			if ( b != null){
				b.setColor(b.deadColor);
			}

			// destroy its healthbar
			if ( h.hpBar != null ){
				Destroy(h.hpBar);
			}

			if ( h.hpBarBG != null ){
				Destroy(h.hpBarBG);
			}

		}

	}


	void OnCollisionEnter(Collision c){	 

		if ( c.gameObject.tag != "Enemy" ){

			damageTarget(c.gameObject, collisionDamage);

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

		Health h = target.GetComponent<Health>();

		if ( h != null ){
			h.applyDamage(dmg);
		}

	}

	
}
