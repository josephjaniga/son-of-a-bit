using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public Vector3 scale = new Vector3(2f,0.1f,0.1f);
	public int maxHP = 100;
	public int currentHP;
	public Vector3 offset = new Vector3(0f,1.0f,0f);

	public GameObject hpBar;
	public GameObject hpBarBG;

	public bool isDead = false;

	// Use this for initialization
	void Start () {

		currentHP = maxHP;

		// health bar background
		hpBarBG = GameObject.CreatePrimitive(PrimitiveType.Cube);
		hpBarBG.transform.localScale = new Vector3(scale.x + 0.05f, scale.y + 0.05f, scale.z + 0.05f);
		hpBarBG.name = "hpbarbg";
		hpBarBG.GetComponent<BoxCollider>().enabled = false;
		hpBarBG.renderer.material.color = new Color(0f,0f,0f,0.25f);

		// generate health bar
		hpBar = GameObject.CreatePrimitive(PrimitiveType.Cube);

		// set parent, name, & size
		hpBar.transform.localScale = scale;
		//hpBar.transform.parent = transform;
		hpBar.name = "hpbar";

		// disable the collider
		hpBar.GetComponent<BoxCollider>().enabled = false;

		hpBar.transform.parent = GameObject.Find("Widgets").transform;
		hpBarBG.transform.parent = GameObject.Find("Widgets").transform;
	}
	
	// Update is called once per frame
	void Update () {

		//currentHP--; 
		setColor();

		// update health bars
		updateBarSize();

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
		if ( hpBar != null && hpBarBG != null ){
			// set the colors
			if ( (float)currentHP / maxHP >= 0.7f ) { // green >= 70%
				hpBar.renderer.material.color = new Color(0f,1f,0f,0.25f);
			} else if ( (float)currentHP / maxHP >= 0.35f ) { // yellow >= 35%
				hpBar.renderer.material.color = new Color(1f,1f,0f,0.25f);
			} else { // red < 35%
				hpBar.renderer.material.color = new Color(1f,0f,0f,0.25f);
			}
		}
	}

	public void updateBarSize(){
		if ( hpBar != null && hpBarBG != null ){
			if ( hpPercent() > 0f ) {
				hpBar.transform.localScale = new Vector3((hpPercent() * scale.x),scale.y,scale.z);
			} else if ( !isDead ) { // if health <= 0 u ded
				isDead = true;
				transform.name = transform.name + " (Dead)";
			}
		}
	}
	
	public float hpPercent(){
		return (float)currentHP / maxHP;
	}

	public void applyDamage(int dmg){
		if ( currentHP - dmg > 0 ){
			currentHP -= dmg;
		} else {
			currentHP = 0;
		}
	}

}
