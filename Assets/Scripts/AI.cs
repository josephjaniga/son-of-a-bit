using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	public Bit bit;

	public enum MovementType {
		None,
		Mindless,
		Follow,
		Chase
	};

	public enum AttackType {
		None,
		Shoot
	};

	public enum TargettingType {
		None,
		NearestUnit,
		NearestEnemy
	};

	public int movementAI = (int)MovementType.Mindless;
	public int attackAI = (int)AttackType.None;
	public int targetAI = (int)TargettingType.None;

	public float duration = 1.0f;
	public float delay = 2.0f;
	public float startTime = 0.0f;
	private Vector3 targetDirection = Vector3.zero;

	private float speed = 1.0f;

	public float followDistance = 0.0f;
	public GameObject followTarget;
	public GameObject attackTarget;

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
		if ( bit.health == null || !bit.health.isDead  ){


			// MOTION TYPES
			if ( movementAI == (int)MovementType.Mindless ) {
				mindless();
			}

			if ( movementAI == (int)MovementType.Follow ) {
				follow();
			}

			if ( movementAI == (int)MovementType.Chase ) {
				chase();
			}


			// TARGETING TYPES
			if ( targetAI == (int)TargettingType.NearestUnit ){
				nearestUnit();
			}

			if ( targetAI == (int)TargettingType.NearestEnemy ){
				nearestEnemy();
			}


			// ATTACK TYPES
			if ( attackAI == (int)AttackType.Shoot ){
				shoot();

			}

		}

		if ( attackTarget != null && !bit.health.isDead){
			Debug.DrawLine(transform.position, attackTarget.transform.position, Color.red);
		}

	}


	void OnCollisionEnter(Collision c){	 

		// check that it has a bit
		if ( c.gameObject != null && c.gameObject.GetComponent<Bit>() != null ){
			
			Faction collisionsFaction 	= null;
			if ( c.gameObject.GetComponent<Bit>() != null ) {
				collisionsFaction	= c.gameObject.GetComponent<Bit>().faction;
			}
			
			// if the rammer isnt friendly with the target
			if 	( 
				    collisionsFaction == null 
				    || (
						!bit.faction.isAllied(collisionsFaction.FactionName) 
						&& !bit.faction.isMyFaction(collisionsFaction.FactionName)
					) 
			    ){

				// if this bit is still alive or capable of dealing damage
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

	public void follow(){

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

	public void chase(){
		
		if ( attackTarget != null ){
			
			float y = 0f;
			float x = 0f;
			float z = 0f;
			
			// target is above
			if ( transform.position.y < attackTarget.transform.position.y )
				y = 1f;
			
			// target is below center		
			if ( transform.position.y > attackTarget.transform.position.y )
				y = -1f;
			
			// target is right
			if ( transform.position.x < attackTarget.transform.position.x )
				x = 1f;
			
			// target is left		
			if ( transform.position.x > attackTarget.transform.position.x )
				x = -1f;
			
			targetDirection = new Vector3 (
				x * rigidbody.mass * speed,
				y * rigidbody.mass * speed,
				z * rigidbody.mass * speed
				);
			
			rigidbody.AddForce(targetDirection);
			
		}
		
	}

	public void nearestUnit(){
		GameObject closest = null;
		GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
		
		foreach (GameObject unit in allUnits) {
			
			// if not self
			if ( unit != gameObject ){
				float closestDistance = 9999.9f;
				if ( closest != null ){
					closestDistance = Vector3.Distance(closest.transform.position, transform.position);
				}
				
				float testDistance = Vector3.Distance(unit.transform.position, transform.position);
				
				if ( closest == null || testDistance <= closestDistance ){
					closest = unit;
				}
			}
			
		}
		
		attackTarget = closest;
	}
	
	public void nearestEnemy(){
		GameObject closest = null;
		GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
	
		foreach (GameObject unit in allUnits) {

			Faction parentFaction = unit.GetComponentInParent<Faction>();

			if ( 
				    unit != gameObject 											// ensure not targeting myself 
				    && parentFaction != null									// target belongs to a faction
				    && !bit.faction.isAllied(parentFaction.FactionName) 		// not an ally
				    && !bit.faction.isMyFaction(parentFaction.FactionName)		// not belonging to my faction
			    	&& !unit.GetComponent<Bit>().health.isDead					// target is not dead
			    ){
				float closestDistance = 9999.9f;
				if ( closest != null ){
					closestDistance = Vector3.Distance(closest.transform.position, transform.position);
				}
				
				float testDistance = Vector3.Distance(unit.transform.position, transform.position);
				
				if ( closest == null || testDistance <= closestDistance ){
					closest = unit;
				}
			}
			
		}
		
		attackTarget = closest;
	}


	public void damageTarget(GameObject target, int dmg){

		if ( target != null ) {

			Health targetHealth = target.GetComponent<Bit>().health;

			if ( targetHealth != null ){
				targetHealth.applyDamage(dmg, gameObject);
			}

		}

	}

	public void shoot(){
		if ( Time.time - bit.weapon.bullet.GetComponent<Projectile>().ROF >= bit.weapon.lastFired ){
			if ( attackTarget != null ) {
				bit.weapon.fire ( attackTarget.transform.position );
				//Debug.Log("fired");
			}
		}
	}

	
}
