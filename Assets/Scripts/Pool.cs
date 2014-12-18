using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

	public string poolName;
	public GameObject goInstance;
	public List<GameObject> list;

	public int listCount = 0;

	// Use this for initialization
	void Start () {
	
		list = new List<GameObject>();
		//slowBulletStack = new Stack<GameObject>();

		for ( int i=0; i<1000; i++ ){

			GameObject go = Instantiate(goInstance, new Vector3(-999f, -999f, -999f), Quaternion.identity) as GameObject;
			pushToStack(go);

		}

		listCount = list.Count;
		
	}

	public GameObject popFromStack(){

		GameObject temp = null;
		if ( list.Count > 0 ){
			temp = list[0];
			list.RemoveAt(0);
			listCount = list.Count;
		}
		return temp;

	}

	public void pushToStack(GameObject go){

		go.transform.SetParent( gameObject.transform );
		go.SetActive(false);
		list.Add(go);
		listCount = list.Count;

		/*
		if ( go.name == goInstance.name ){

		} else {
			Destroy(go);
		}
		*/

	}

}
