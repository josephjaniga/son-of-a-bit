using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public Vector3	targetDirection = Vector3.zero;
	public float	birthTime;
	
	public bool 	destroyOnImpact = false;
	public bool		useLifeSpan		= true;
	public bool 	useGravity		= false;
	public float 	speed = 11.0f;
	public float	lifeSpan = 3f;
	public float	ROF = 0.5f;
	public int 		numProjectiles = 5;
	public int		projectileDamage = 1;
	public float 	critChance = 0.0f;
	public float 	critDamage = 0.5f;
	public Color	bulletColor = Color.white;
	public bool		sparkOnCollision = true;
	public Color	colorLow 	= new Color(0.6f,0.1f,0.0f,0.1f);
	public Color	colorHigh	= new Color(1.0f,0.5f,0.0f,0.5f);

	public GameObject owner = null;

	public Bit bit;

	public string projectileName;

	public Pool pool;
	
	// Use this for initialization
	void Awake () {

		//Debug.Log(projectileName);

		pool = GameObject.Find( projectileName + "_POOL").GetComponent<Pool>();

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
			
			if ( pool != null && projectileName == pool.goInstance.GetComponent<Projectile>().projectileName ){
				pool.pushToStack(gameObject);
			} else {
				Debug.Log (gameObject.name + " destroyed");
				Destroy(gameObject);
			}

		}
		
	}
	
	void OnCollisionEnter(Collision c){	 

		if ( owner != null ){
			
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

						if ( pool != null && projectileName == pool.goInstance.GetComponent<Projectile>().projectileName ){
							pool.pushToStack(gameObject);
						} else {
							//Debug.Log (gameObject.name + " destroyed");
							Destroy(gameObject);
						}
						
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

					if ( pool != null && projectileName == pool.goInstance.GetComponent<Projectile>().projectileName ){
						pool.pushToStack(gameObject);
					} else {
						//Debug.Log (gameObject.name + " destroyed");
						Destroy(gameObject);
					}

				}
				
			}

		} else {

			if ( pool != null && projectileName == pool.goInstance.GetComponent<Projectile>().projectileName ){
				pool.pushToStack(gameObject);
			} else {
				Debug.Log (gameObject.name + " destroyed");
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

	public void copyBulletAttributes(Projectile p){
		destroyOnImpact 	= p.destroyOnImpact;
		useLifeSpan			= p.useLifeSpan;
		useGravity			= p.useGravity;
		speed 				= p.speed;
		lifeSpan 			= p.lifeSpan;
		ROF 				= p.ROF;
		numProjectiles 		= p.numProjectiles;
		projectileDamage 	= p.projectileDamage;
		critChance 			= p.critChance;
		critDamage 			= p.critDamage;
		bulletColor 		= p.bulletColor;
		sparkOnCollision 	= p.sparkOnCollision;
		colorLow 			= p.colorLow;
		colorHigh			= p.colorHigh;

		gameObject.GetComponent<Bit>().setColor(bulletColor);
	}

	public void randomizeBullet(int Rarity = 0){

		float r = Random.Range(0f,1f);
		float g = Random.Range(0f,1f);
		float b = Random.Range(0f,1f);
		float a = Random.Range(0f,1f);

		destroyOnImpact 	= true;
		useLifeSpan			= true;
		useGravity			= Random.Range (0, 1) > 0.5f;
		speed 				= 10.0f + 2.5f * Random.Range(0, Rarity+1);
		lifeSpan 			= 2.5f;
		ROF 				= 1f - .05f * Random.Range(0, Rarity+2);
		numProjectiles 		= 1;
		projectileDamage 	= Mathf.RoundToInt((4 * Random.Range(1, Rarity+2) + Random.Range(1, Rarity) ) * Random.Range(0.8f, 1.25f));
		critChance 			= .05f * Random.Range(0, Rarity+1);
		critDamage 			= .25f * Random.Range(0, Rarity+2);
		bulletColor 		= new Color(r, g, b, a);

	}

}
