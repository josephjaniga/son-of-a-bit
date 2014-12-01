using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Faction : MonoBehaviour {

	public string FactionName = "";

	public List<Faction> allies = new List<Faction>();

	// Use this for initialization
	void Start () {
		
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
