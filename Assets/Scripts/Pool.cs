using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

	public string poolName;
	public GameObject goInstance;
	public List<GameObject> list;

	public int listCount = 0;

	public int poolMax;

	// Use this for initialization
	void Start () {
	
		list = new List<GameObject>();
		//slowBulletStack = new Stack<GameObject>();

		if ( poolMax == 0 ){
			poolMax = 1000;		
		}

		for ( int i=0; i<poolMax; i++ ){

			GameObject go = Instantiate(goInstance, new Vector3(-999f, -999f, -999f), Quaternion.identity) as GameObject;
			pushToStack(go);

		}

		listCount = countAvailable();
		
	}

	public GameObject popFromStack(){

		GameObject temp = getFirstAvailable();
		Debug.Log (temp);
		temp.GetComponent<Bit>().inUse = true;
		//listCount = countAvailable();
		return temp;

	}

	public void pushToStack(GameObject go){

		go.GetComponent<Bit>().inUse = false;
		go.transform.SetParent( gameObject.transform );
		go.SetActive(false);
		//listCount = countAvailable();

	}

	public GameObject getFirstAvailable(){
		GameObject temp = null;

		/*
		for( int i=0; i<list.Count; i++ ){
			GameObject go = list[i];
			Debug.Log (go);
			if ( go.GetComponent<Bit>() != null && !go.GetComponent<Bit>().inUse  ){
				temp = go;
				break;
			}
		}
		*/

		foreach ( Transform child in transform ) {
			if ( !child.gameObject.activeSelf  ){
				temp = child.gameObject;
				break;
			}
		}

		return temp;
	}


	public int countAvailable(){
		int temp = 0;
		foreach ( Transform child in transform ) {
			if ( !child.gameObject.activeSelf  ){
				temp++;
			}
		}
		return temp;
	}

}
