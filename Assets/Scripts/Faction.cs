using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Faction : MonoBehaviour {

	public string FactionName = "";
	public Color factionColor;

	public List<Faction> allies = new List<Faction>();

	// Use this for initialization
	void Start () {
		factionColor = new Color(Random.Range(0,100)/100f,Random.Range(0,100)/100f,Random.Range(0,100)/100f);
		foreach (Transform child in transform)
			child.gameObject.GetComponent<Bit>().setColor(factionColor);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public bool isAllied(string testFactionName){

		bool alleigance = false;

		for ( int i = 0; i < allies.Count; i++ ){
			if ( allies[i].FactionName == testFactionName ){
				alleigance = true;
			}
		}

		return alleigance;

	}

	public bool isMyFaction(string testFactionName){
		
		bool alleigance = false;

		if ( FactionName == testFactionName ){
			alleigance = true;
		}
		
		return alleigance;
		
	}

}
