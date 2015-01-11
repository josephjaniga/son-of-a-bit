using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {

	public Pool enemyPool;

	public GameObject faction;

	public int waveNumber = 0;
	public int alive = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void LateUpdate () {

		List<GameObject> gos = new List<GameObject>();
		int tempAlive = 0;

		foreach (Transform child in faction.transform){
			if ( !child.GetComponent<Bit>().health.isDead ){
				tempAlive++;
			} else {

				int countChildBullets = 0;

				foreach (Transform bullet in GameObject.Find("Projectiles").transform ){
					if ( bullet.GetComponent<Projectile>().owner == child ){
						countChildBullets++;
					}
				}

				if ( countChildBullets == 0 ){
					gos.Add(child.gameObject);
				}
			}
		}
		alive = tempAlive;

		if ( alive == 0 ){

			waveNumber++;

			if ( enemyPool != null ){
				int dimm = Mathf.RoundToInt(Mathf.Sqrt (waveNumber))+1;

				for ( int i=0; i<dimm; i++ ){
					for ( int j=0; j<dimm; j++){
						if ( i * dimm + j < waveNumber ){
							Vector3 t = new Vector3(i-dimm/2f, j-dimm/2f, 0f);
							GameObject go = enemyPool.popFromStack();
							go.transform.position = transform.position + t;
							go.SetActive(true);
							go.transform.SetParent(faction.transform);
							go.renderer.material.color = faction.GetComponent<Faction>().factionColor;
							// FIXME why do I have to do this to make this work.. "Awake?"
							go.GetComponent<AI>().enabled = false;
							go.GetComponent<AI>().enabled = true;
						}
					}
				}
			}

		}

	}
}
