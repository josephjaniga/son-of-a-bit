using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public int credits = 0;

	public Bit bit;

	// Use this for initialization
	void Start () {
	
		bit = gameObject.GetComponent<Bit>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void addCredits(int Amount){
		credits += Amount;
	}


}
