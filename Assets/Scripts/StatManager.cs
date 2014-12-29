using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatManager : MonoBehaviour {

	public Bit bit;

	public Equipment e;

	public int cMaxHealth;
	public int cRegen;
	public float cMovementSpeed;
	public float cRateOfFire;
	public int cNumberOfProjectiles;
	public int cProjectileDamage;
	public float cCriticalChance;
	public float cCriticalDamage;

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

		e = bit.equipment;

	}

	// Use this for initialization
	void Start () {
		
		e = bit.equipment;

	}
	
	// Update is called once per frame
	void Update () {

		if ( bit != null && bit.weapon != null ){
			if ( bit.weapon.bullet != null ){
				unitsProjectile = bit.weapon.bullet.GetComponent<Projectile>();
			} else {
				unitsProjectile = bit.weapon.defaultBullet.GetComponent<Projectile>();
			}
		}

 		
		cMaxHealth 				= Mathf.RoundToInt( ( bit.health.maxHP + calculateMaxHealthBoost() ) * ( 1.0f + calculateMaxHealthScale() ) );
		cRegen	 				= bit.health.regen + calculateHealthRegenBoost();
		cMovementSpeed 			= bit.motion.speed + calculateMovementSpeedBoost();

		if ( unitsProjectile != null ){
			cRateOfFire				= unitsProjectile.ROF / ( 1.0f + calculateRateOfFireScale() );
			cNumberOfProjectiles 	= unitsProjectile.numProjectiles + calculateNumberOfProjectilesBoost();
			cCriticalChance			= unitsProjectile.critChance + calculateCriticalChanceBoost();
			cCriticalDamage			= unitsProjectile.critDamage + calculateCriticalDamageBoost();
			cProjectileDamage		= Mathf.RoundToInt( ( unitsProjectile.projectileDamage + calculateProjectileDamageBoost() ) * ( 1.0f + calculateProjectileDamageScale() ) );
		}

		// calculated stats
		//		cMaxHealth 				= 111;
		//		cRegen	 				= 0;
		//		cMovementSpeed 			= 4f;
		//		cRateOfFire				= 0.1f;
		//		cNumberOfProjectiles 	= 1;
		//		cCriticalChance			= 0.1f;
		//		cCriticalDamage			= 0.1f;
		//		cProjectileDamage		= 9;

	}


	public int calculateMaxHealthBoost(){
		int tempBoost = 0;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.maxHealthBoost;
		}
		return tempBoost;
	}

	public float calculateMaxHealthScale(){
		float tempBoost = 0f;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.maxHealthScale;
		}
		return tempBoost;
	}

	public float calculateMovementSpeedBoost(){
		float tempBoost = 0;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.movementSpeedBoost;
		}
		return tempBoost;
	}

	public float calculateRateOfFireScale(){
		float tempBoost = 0f;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.rateOfFireScale;
		}
		return tempBoost;
	}

	public int calculateNumberOfProjectilesBoost(){
		int tempBoost = 0;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.numberOfProjectilesBoost;
		}
		return tempBoost;
	}

	public int calculateProjectileDamageBoost(){
		int tempBoost = 0;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.projectileDamageBoost;
		}
		return tempBoost;
	}

	public float calculateProjectileDamageScale(){
		float tempBoost = 0;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.projectileDamageScale;
		}
		return tempBoost;
	}

	public float calculateCriticalChanceBoost(){
		float tempBoost = 0;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.critChanceBoost;
		}
		return tempBoost;
	}

	public float calculateCriticalDamageBoost(){
		float tempBoost = 0;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.critDamageBoost;
		}
		return tempBoost;
	}

	public int calculateHealthRegenBoost(){
		int tempBoost = 0;
		for ( int i = 0; i < bit.equipment.genericSlotList.Count; i++ ){
			if ( bit.equipment.genericSlotList[i].item != null )
				tempBoost += bit.equipment.genericSlotList[i].item.healthRegenBoost;
		}
		return tempBoost;
	}

}
