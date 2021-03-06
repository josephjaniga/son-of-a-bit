﻿using UnityEngine;
using System.Collections;

public class Bit : MonoBehaviour {

	public Color 	c = Color.green;
	public Color	deadColor = new Color(0.25f, 0.25f, 0.25f, 0.25f);

	public AI 			artificialInteligence;
	public Health 		health;
	public Item 		item;
	public Inventory 	inventory;
	public Equipment	equipment;
	public Motion 		motion;
	public Projectile 	projectile;
	public StatManager	statManager;
	public Weapon 		weapon;
	public Faction		faction;
	public WaveSpawner  waveSpawner;

	public bool inUse = false;

	void Awake () {
		
		setColor(c);

		artificialInteligence 	= gameObject.GetComponent<AI>();
		health 					= gameObject.GetComponent<Health>();
		equipment 				= gameObject.GetComponent<Equipment>();
		inventory 				= gameObject.GetComponent<Inventory>();
		item 					= gameObject.GetComponent<Item>();
		motion				 	= gameObject.GetComponent<Motion>();
		projectile			 	= gameObject.GetComponent<Projectile>();
		statManager				= gameObject.GetComponent<StatManager>();
		weapon				 	= gameObject.GetComponent<Weapon>();
		
		// get the parent faction
		faction 				= gameObject.GetComponentInParent<Faction>();

		// wave spawner???
		waveSpawner				= gameObject.GetComponentInParent<WaveSpawner>();
	}

	// Use this for initialization
	void Start () {
		
		artificialInteligence 	= gameObject.GetComponent<AI>();
		health 					= gameObject.GetComponent<Health>();
		equipment 				= gameObject.GetComponent<Equipment>();
		inventory 				= gameObject.GetComponent<Inventory>();
		item 					= gameObject.GetComponent<Item>();
		motion				 	= gameObject.GetComponent<Motion>();
		projectile			 	= gameObject.GetComponent<Projectile>();
		statManager				= gameObject.GetComponent<StatManager>();
		weapon				 	= gameObject.GetComponent<Weapon>();
		// get the parent faction
		faction 				= gameObject.GetComponentInParent<Faction>();

		// wave spawner???
		waveSpawner				= gameObject.GetComponentInParent<WaveSpawner>();

		// set a random spin
		Vector3 av;
		av.x = Random.Range (-3f, 3f);
		av.y = Random.Range (-3f, 3f);
		av.z = Random.Range (-3f, 3f);
		rigidbody.angularVelocity = av;

	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public void setColor(Color input){
		transform.renderer.material.color = input;
	}


}
