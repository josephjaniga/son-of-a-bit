using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

	public GameObject slowBullet;

	public List<GameObject> slowBulletList;
	public Stack<GameObject> slowBulletStack;

	// Use this for initialization
	void Start () {
	
		slowBulletList = new List<GameObject>();
		slowBulletStack = new Stack<GameObject>();

		for ( int i=0; i<5000; i++ ){

			GameObject go = Instantiate(slowBullet, new Vector3(-999f, -999f, -999f), Quaternion.identity) as GameObject;
			pushToStack(go);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject popFromStack(){

		GameObject temp = null;
		if ( slowBulletList.Count > 0 ){
			temp = slowBulletList[0];
			slowBulletList.RemoveAt(0);
		}
		return temp;

	}

	public void pushToStack(GameObject go){

		go.transform.SetParent(GameObject.Find ("Pool").transform);
		go.SetActive(false);
		slowBulletList.Add(go);

	}

}
