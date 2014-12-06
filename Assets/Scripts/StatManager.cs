using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatManager : MonoBehaviour {

	public Bit bit;

	public int cMaxHealth;
	public int cRegen;
	public float cMovementSpeed;
	public float cRateOfFire;
	public int cNumberOfProjectiles;
	public int cProjectileDamage;

	public Projectile unitsProjectile;

	void Awake() {

		bit = gameObject.GetComponent<Bit>();

		if ( 
		    bit != null 
		    && bit.weapon != null 
		    && bit.weapon.bullet != null 
		    ){
			unitsProjectile = bit.weapon.bullet.GetComponent<Projectile>();
		}

	}

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {

		if ( 
		    bit != null 
		    && bit.weapon != null 
		    && bit.weapon.bullet != null 
		    ){
			unitsProjectile = bit.weapon.bullet.GetComponent<Projectile>();
		}

		// calculated stats
		cMaxHealth 				= Mathf.RoundToInt( ( bit.health.maxHP + calculateMaxHealthBoost() ) * ( 1.0f + calculateMaxHealthScale() ) );
		cRegen	 				= bit.health.regen;
		cMovementSpeed 			= bit.motion.speed + calculateMovementSpeedBoost();
		cRateOfFire				= unitsProjectile.ROF / ( 1.0f + calculateRateOfFireScale() );
		cNumberOfProjectiles 	= unitsProjectile.numProjectiles + calculateNumberOfProjectilesBoost();
		cProjectileDamage		= Mathf.RoundToInt( ( unitsProjectile.projectileDamage + calculateProjectileDamageBoost() ) * ( 1.0f + calculateProjectileDamageScale() ) );

	}


	public int calculateMaxHealthBoost(){
		int tempBoost = 0;

		for ( int i = 0; i < bit.inventory.equippedItems.Count; i++ ){
			tempBoost += bit.inventory.equippedItems[i].maxHealthBoost;
		}

		return tempBoost;
	}

	public float calculateMaxHealthScale(){
		float tempBoost = 0f;

		for ( int i = 0; i < bit.inventory.equippedItems.Count; i++ ){
			tempBoost += bit.inventory.equippedItems[i].maxHealthScale;
		}
		
		return tempBoost;
	}

	public float calculateMovementSpeedBoost(){
		float tempBoost = 0;

		for ( int i = 0; i < bit.inventory.equippedItems.Count; i++ ){
			tempBoost += bit.inventory.equippedItems[i].movementSpeedBoost;
		}
		
		return tempBoost;
	}

	public float calculateRateOfFireScale(){
		float tempBoost = 0f;
		
		for ( int i = 0; i < bit.inventory.equippedItems.Count; i++ ){
			tempBoost += bit.inventory.equippedItems[i].rateOfFireScale;
		}
		
		return tempBoost;
	}

	public int calculateNumberOfProjectilesBoost(){
		int tempBoost = 0;
		
		for ( int i = 0; i < bit.inventory.equippedItems.Count; i++ ){
			tempBoost += bit.inventory.equippedItems[i].numberOfProjectilesBoost;
		}
		
		return tempBoost;
	}

	public int calculateProjectileDamageBoost(){
		int tempBoost = 0;
		
		for ( int i = 0; i < bit.inventory.equippedItems.Count; i++ ){
			tempBoost += bit.inventory.equippedItems[i].projectileDamageBoost;
		}
		
		return tempBoost;
	}

	public float calculateProjectileDamageScale(){
		float tempBoost = 0;
		
		for ( int i = 0; i < bit.inventory.equippedItems.Count; i++ ){
			tempBoost += bit.inventory.equippedItems[i].projectileDamageScale;
		}
		
		return tempBoost;
	}


}
