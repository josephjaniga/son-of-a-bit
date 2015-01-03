using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

	public Vector3 offset = new Vector3(0f,1.0f,0f);
	public Vector3 scale = new Vector3(2f,0.1f,0.1f);
	public int maxHP = 100;
	public int currentHP = 1;

	public int regen = 1;
	public float regenRate = 2.0f;
	public float regenTimer = 2.0f;

	public GameObject hpBar;
	public GameObject hpBarBG;
	public bool isDead = false;
	public bool isTerrain = false;

	public bool isImmortal = false;

	public Bit bit;

	public GameObject sct;

	private float deathClock = 0f;
	private float deathFadeTime = 5f;

	public Inventory inv;

	//public Motion m;
	//public AI ai;

	// Use this for initialization
	void Awake () {

		bit = gameObject.GetComponent<Bit>();

		inv = GameObject.Find ("NewInventory").GetComponent<Inventory>();

		//sct = GameObject.Find("SCT");
		sct = Resources.Load("Prefabs/UI/SCT") as GameObject;
		sct.GetComponent<SCT>().isEnabled = false;

		//ai = gameObject.GetComponent<AI>();
		//m = gameObject.GetComponent<Motion>();

		currentHP = maxHP;

		// health bar background
		hpBarBG = GameObject.CreatePrimitive(PrimitiveType.Cube);
		hpBarBG.transform.localScale = new Vector3(scale.x + 0.05f, scale.y + 0.05f, scale.z + 0.05f);
		hpBarBG.name = "hpbarbg_"+gameObject.name;
		hpBarBG.GetComponent<BoxCollider>().enabled = false;
		hpBarBG.renderer.material.color = new Color(0f,0f,0f,0.25f);
		hpBarBG.transform.parent = GameObject.Find("Widgets").transform;
		// health bar
		hpBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
		hpBar.transform.localScale = scale;
		hpBar.name = "hpbar_"+gameObject.name;;
		hpBar.GetComponent<BoxCollider>().enabled = false;
		hpBar.transform.parent = GameObject.Find("Widgets").transform;

		//currentHP--; 
		setColor();
		
		// update health bars
		updateBarSize();

	}
	
	// Update is called once per frame
	void Update () {
	

		// health regen timer
		if ( regenTimer > 0 && regenRate != 0 )
			regenTimer -= Time.deltaTime;

		// if timers up reset it and apply regen either calculated if this unit has a stat manager or flat
		if ( regenTimer <= 0 && regenRate != 0 ){
			if ( bit.statManager != null ){
				applyHeal(bit.statManager.cRegen);
			} else {
				applyHeal(regen);
			}
			regenTimer = regenRate;
		}


		if ( !isTerrain ){

			//currentHP--; 
			setColor();

			// update health bars
			updateBarSize();

		} else {

			if ( hpBar != null ){
				Destroy(hpBar);
			}
			
			if ( hpBarBG != null ){
				Destroy(hpBarBG);
			}

		}

		if ( isImmortal ){
			if ( bit.statManager != null && bit.statManager.cMaxHealth != 0 ){
				currentHP = bit.statManager.cMaxHealth;
			} else {
				currentHP = maxHP;
			}
		}

		// if the unit is dead
		if ( isDead ){
	
			deathClock += Time.deltaTime;
			if ( deathClock >= deathFadeTime ){

				if ( gameObject.name.Contains("Cylinder") ){
					Pool cylinderPool = GameObject.Find ("Cylinder_POOL").GetComponent<Pool>();
					cylinderPool.pushToStack(gameObject);
				} else {
					//Debug.Log (gameObject.name + " destroyed");
					Destroy(gameObject);
				}

				gameObject.SetActive(false);
			}

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
		
		if ( bit.statManager != null && bit.statManager.cMaxHealth != 0 ){
			tempMaxHP = bit.statManager.cMaxHealth;
		}

		return (float)currentHP / tempMaxHP;
	}
	
	public void applyDamage(int dmg, GameObject dmgSource){

		int tempDmg = dmg;

		if ( dmg > 0 && dmgSource != null ){
			
			Projectile p = dmgSource.GetComponent<Bit>().projectile;
			
			if ( p != null && p.owner != null ){
				StatManager sm = p.owner.GetComponent<StatManager>();
				if ( sm != null ){

					float r = Random.Range(0.0f, 1.0f);
					//Debug.Log("Crit Chance: " + sm.cCriticalChance + " // Crit Roll: " + r );
					if ( sm.cCriticalChance > r ){
						tempDmg += Mathf.RoundToInt(tempDmg * sm.cCriticalDamage);
						//Debug.Log ("CRITICAL HIT " + tempDmg);
						
						GameObject combatText = Instantiate(sct, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity) as GameObject;
						combatText.GetComponent<Text>().text = tempDmg+"!";
						combatText.transform.SetParent(GameObject.Find("Canvas").transform);
						combatText.GetComponent<SCT>().enabled = true;
						
					}
				}
			}
			
			if ( currentHP - tempDmg > 0 ){
				currentHP -= tempDmg;
			} else {
				currentHP = 0;
				isDead = true;
				// apply Damage killed the target
				if ( isDead && bit != null && bit.artificialInteligence != null ){
					// if source is a projectile
					p = dmgSource.GetComponent<Bit>().projectile;
					if ( p != null && p.owner != null ){
						// if the owner has an inventory
						Inventory i = inv;
						if ( i != null ){
							// call owner inventory grant reward
							i.addCredits(bit.artificialInteligence.dropRate);
							
							// do a drop roll
							if ( i != null 
							    && bit.artificialInteligence.calculateLootDrops()
							    ) {

								// clear all the alerts
								foreach (Transform child in GameObject.Find("Alerts").transform) {
									GameObject.Destroy(child.gameObject);
								}

								GameObject go = i.createRandomItem(bit.artificialInteligence.dropRarity);
								i.addItemToInventory(go.GetComponent<Item>());
								
								GameObject alert = Instantiate(sct, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
								alert.GetComponent<Text>().text = "You have picked up: [<color=red>"+ go.GetComponent<Item>().itemName +"</color>]!";
								alert.transform.SetParent(GameObject.Find("Alerts").transform);
								alert.transform.localPosition = new Vector3(0f, -80f, 0f);
								alert.GetComponent<Text>().fontSize = 12;
								alert.GetComponent<SCT>().Timer = 5;
								alert.GetComponent<SCT>().Timeout = 5;

							}
						}
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
