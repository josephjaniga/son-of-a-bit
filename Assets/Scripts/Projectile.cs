using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public Vector3	targetDirection = Vector3.zero;
	public float 	speed = 11.0f;
	public bool 	destroyOnImpact = false;
	public bool		useLifeSpan		= true;
	public bool 	useGravity		= false;

	public float	birthTime;
	public float	lifeSpan = 3f;
	public float	ROF = 0.5f;

	public int 		numProjectiles = 5;
	public int		projectileDamage = 1;

	public bool		sparkOnCollision = true;
	public Color	colorLow 	= new Color(0.6f,0.1f,0.0f,0.1f);
	public Color	colorHigh	= new Color(1.0f,0.5f,0.0f,0.5f);

	public GameObject owner = null;

	public Bit bit;

	public string projectileName;
	
	// Use this for initialization
	void Start () {

		bit = gameObject.GetComponent<Bit>();

		if ( useGravity ){
			rigidbody.useGravity = useGravity;
		}

		birthTime = Time.time;
		rigidbody.velocity = targetDirection * speed;

	}
	
	// Update is called once per frame
	void Update () {
	
		//rigidbody.velocity = targetDirection * speed;

		// kill it after it expires
		if ( Time.time - birthTime >= lifeSpan && useLifeSpan ){
			Destroy(gameObject);
		}

	}
 
	void OnCollisionEnter(Collision c){	 

		int tempProjectileDamage = projectileDamage;
		
		if ( owner.GetComponent<Bit>().statManager != null ){
			tempProjectileDamage = owner.GetComponent<Bit>().statManager.cProjectileDamage;
		}

		// get the owners Faction & and the collided objects faction
		Faction ownersFaction = null;
			if ( owner.GetComponent<Bit>() != null ) {
				ownersFaction	= owner.GetComponent<Bit>().faction;
			}

		Faction collisionsFaction 	= null;
			if ( c.gameObject.GetComponent<Bit>() != null ) {
				collisionsFaction	= c.gameObject.GetComponent<Bit>().faction;
			}

		// BULLETS DONT COLLIDE WITH THE OBJECT WHO FIRED THEM
		if ( owner != c.gameObject ){

			// if the shooter isnt allied with the target
			if ( collisionsFaction == null || (!ownersFaction.isAllied(collisionsFaction.FactionName) && !ownersFaction.isMyFaction(collisionsFaction.FactionName)) ){
				
				if ( c.gameObject != null && c.gameObject.GetComponent<Bit>() != null ){
					damageTarget(c.gameObject, tempProjectileDamage);
				}
				
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
					if ( useLifeSpan )
						lifeSpan = 0.5f;  
				}
				
			} else {
				Destroy(gameObject);
			}

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
		if ( target != null ){

			Health h = target.GetComponent<Bit>().health;

			if ( h != null && !h.isDead ){
				h.applyDamage(dmg, gameObject);
			}

		}
	}

	public void setOwner(GameObject o){
		owner = o;
	}

}
