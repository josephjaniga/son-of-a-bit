using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public Vector3 offset = new Vector3(0f,1.0f,0f);
	public Vector3 scale = new Vector3(2f,0.1f,0.1f);
	public int maxHP = 100;
	public int currentHP;

	public int regen = 1;
	public float regenRate = 2.0f;
	public float regenTimer = 2.0f;

	public GameObject hpBar;
	public GameObject hpBarBG;
	public bool isDead = false;

	public bool isImmortal = false;

	public Bit bit;

	//public Motion m;
	//public AI ai;

	// Use this for initialization
	void Awake () {

		bit = gameObject.GetComponent<Bit>();

		//ai = gameObject.GetComponent<AI>();
		//m = gameObject.GetComponent<Motion>();

		currentHP = maxHP;

		// health bar background
		hpBarBG = GameObject.CreatePrimitive(PrimitiveType.Cube);
		hpBarBG.transform.localScale = new Vector3(scale.x + 0.05f, scale.y + 0.05f, scale.z + 0.05f);
		hpBarBG.name = "hpbarbg";
		hpBarBG.GetComponent<BoxCollider>().enabled = false;
		hpBarBG.renderer.material.color = new Color(0f,0f,0f,0.25f);
		hpBarBG.transform.parent = GameObject.Find("Widgets").transform;
		// health bar
		hpBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
		hpBar.transform.localScale = scale;
		hpBar.name = "hpbar";
		hpBar.GetComponent<BoxCollider>().enabled = false;
		hpBar.transform.parent = GameObject.Find("Widgets").transform;

	}
	
	// Update is called once per frame
	void Update () {
	

		// health regen timer
		if ( regenTimer > 0 )
			regenTimer -= Time.deltaTime;

		// if timers up reset it and apply regen either calculated if this unit has a stat manager or flat
		if ( regenTimer <= 0 ){
			if ( bit.statManager != null ){
				applyHeal(bit.statManager.cRegen);
			} else {
				applyHeal(regen);
			}
			regenTimer = regenRate;
		}


		//currentHP--; 
		setColor();

		// update health bars
		updateBarSize();

		if ( isImmortal ){
			if ( bit.statManager != null ){
				currentHP = bit.statManager.cMaxHealth;
			} else {
				currentHP = maxHP;
			}
		}

		// if the unit is dead
		if ( isDead ){
			bit.motion.userControlled = false;
			bit.motion.shouldLock = false;
			// drop it to the ground
			rigidbody.useGravity = true;
			
			// set its dead color
			if ( bit != null ){
				bit.setColor(bit.deadColor);
			}
			
			// destroy its healthbar
			if ( hpBar != null ){
				Destroy(hpBar);
			}
			
			if ( hpBarBG != null ){
				Destroy(hpBarBG);
			}
		}


	}

	void LateUpdate(){
		
		setPosition();

	}

	public void setPosition(){
		if ( hpBar != null && hpBarBG != null ){
			// move the health bar above the player
			hpBar.transform.position = new Vector3(
					transform.position.x + offset.x,
					transform.position.y + offset.y,
					-2.1f
				);
			hpBarBG.transform.position = new Vector3(
					transform.position.x + offset.x,
					transform.position.y + offset.y,
					-2f
				);
		}
	}

	public void setColor(){

		int tempMaxHP = maxHP;
		
		if ( bit.statManager != null ){
			tempMaxHP = bit.statManager.cMaxHealth;
		}

		if ( hpBar != null && hpBarBG != null ){
			// set the colors
			if ( (float)currentHP / tempMaxHP >= 0.7f ) { // green >= 70%
				hpBar.renderer.material.color = new Color(0f,1f,0f,0.25f);
			} else if ( (float)currentHP / tempMaxHP >= 0.35f ) { // yellow >= 35%
				hpBar.renderer.material.color = new Color(1f,1f,0f,0.25f);
			} else { // red < 35%
				hpBar.renderer.material.color = new Color(1f,0f,0f,0.25f);
			}
		}

	}

	public void updateBarSize(){
		if ( hpBar != null && hpBarBG != null ){
			if ( hpPercent() == 1f ){
				hpBar.gameObject.SetActive(false);
				hpBarBG.gameObject.SetActive(false);
			} else if ( hpPercent() > 0f ) {
				hpBar.gameObject.SetActive(true);
				hpBarBG.gameObject.SetActive(true);
				hpBar.transform.localScale = new Vector3((hpPercent() * scale.x),scale.y,scale.z);
			} else if ( !isDead ) { // if health <= 0 u ded
				isDead = true;
				transform.name = transform.name + " (Dead)";
			}
		}
	}
	
	public float hpPercent(){
		int tempMaxHP = maxHP;
		
		if ( bit.statManager != null ){
			tempMaxHP = bit.statManager.cMaxHealth;
		}

		return (float)currentHP / tempMaxHP;
	}
	
	public void applyDamage(int dmg, GameObject dmgSource){
		if ( currentHP - dmg > 0 ){
			currentHP -= dmg;
		} else {
			currentHP = 0;
			isDead = true;
			// apply Damage killed the target
			if ( isDead && bit != null && bit.artificialInteligence != null ){
				// if source is a projectile
				Projectile p = dmgSource.GetComponent<Bit>().projectile;
				if ( p != null && p.owner != null ){
					// if the owner has an inventory
					Inventory i = p.owner.GetComponent<Bit>().inventory;;
					if ( i != null ){
						// call owner inventory grant reward
						i.addCredits(bit.artificialInteligence.dropRate);
					}
				}
			}
		}
	}


	public void applyHeal(int heal){

		int tempMaxHP = maxHP;
		
		if ( bit.statManager != null ){
			tempMaxHP = bit.statManager.cMaxHealth;
		}

		if ( currentHP + heal > tempMaxHP ){
			currentHP = tempMaxHP;
		} else {
			currentHP += heal;
		}

	}


}
