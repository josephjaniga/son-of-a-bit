using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public bool displayCharacter = false;
	public bool displayInventory = false;
	public bool displayEquipment = false;

	public GameObject characterPanel;
	public GameObject inventoryPanel;
	public GameObject equipmentPanel;

	// vehicle management
	public GameObject p;
	public GameObject unitInControl;
	public bool inVehicle = false;
	public bool lastInVehicle = false;
	public GameObject vehicleToBoard;
	public Vehicle v;
	public float boardingDistance = 10f;

	// camera lerp
	public float closeCameraSize = 2f;
	public float farCameraSize = 7f;
	public float lerpDuration = 2f;
	public float startTime = 0f;
	public float cameraFrom = 7f;
	public float cameraTo = 1.5f;
	public bool doneLerping = false;

	// Use this for initialization
	void Start () {

		inventoryPanel = GameObject.Find("NewInventory");
		characterPanel = GameObject.Find("CharacterPanel");
		if ( getActiveEquipment() != null )
			equipmentPanel = getActiveEquipment().equipPanel;
		p = GameObject.Find("Player");
		unitInControl = p;

		// start the camera lerp
		startTime = Time.time;
		doneLerping = false;
	}
	
	// Update is called once per frame
	void Update () {

		/*
		 * UI Stuffs
		 ************************************
		 */ 
		// hide all Panels
		if ( Input.GetKeyDown(KeyCode.Escape) ){
			displayCharacter = false;
			displayInventory = false;
			displayEquipment = false;
		}

		// keypress c - toggle Character Panel
		if ( Input.GetKeyDown("c") ){
			displayCharacter = !displayCharacter;
		}

		// keypress i - toggle Inventory Panel
		if ( Input.GetKeyDown("i") ){
			displayInventory = !displayInventory;
		}

		// keypress z - toggle equipment
		if ( Input.GetKeyDown ("z") ){
			displayEquipment = !displayEquipment;
		}

		if ( characterPanel != null ) {
			if ( displayCharacter ){
				characterPanel.SetActive(true);
			} else if ( !displayCharacter ){
				characterPanel.SetActive(false);
			}
		}

		if ( inventoryPanel != null ) {
			if ( displayInventory ){
				inventoryPanel.SetActive(true);
			} else if ( !displayInventory ){
				inventoryPanel.SetActive(false);
			}
		}

		if ( getActiveEquipment() != null )
			equipmentPanel = getActiveEquipment().equipPanel;
		if ( equipmentPanel != null ){
			if ( displayEquipment ){
				equipmentPanel.SetActive(true);
			} else if ( !displayEquipment ){
				foreach( Transform child in GameObject.Find("EquipmentPanels").transform ){
					child.gameObject.SetActive(false);
				}
			}
		}


		/*
		 * Vehicle Stuffs
		 ************************************
		 */ 
		vehicleManagement();
	
		// DEVTOOLS
		devTools();

	}


	private void devTools(){

		GameObject player = GameObject.Find ("Player");

		// 	keypress j - spawn an item
		//	if ( Input.GetKeyDown("j") ){
		//		GameObject go = p.GetComponent<Inventory>().createRandomItem();
		//		p.GetComponent<Inventory>().addItemToInventory(go.GetComponent<Item>());
		//	}

		// keypress k - spawn an item
		if ( Input.GetKeyDown("k") ){
			GameObject go = inventoryPanel.GetComponent<Inventory>().createRandomItem();
			inventoryPanel.GetComponent<Inventory>().addItemToInventory(go.GetComponent<Item>());
		}

		//	if ( Input.GetKeyDown(KeyCode.Alpha1) ){
		//		player.GetComponent<Weapon>().bullet = (GameObject)Resources.Load("Projectiles/FF_Bullet"); 
		//	}
		//
		//	if ( Input.GetKeyDown(KeyCode.Alpha2) ){
		//		player.GetComponent<Weapon>().bullet = (GameObject)Resources.Load("Projectiles/FF_Rocket"); 
		//	}

	}

	public void vehicleManagement(){

		/*
		 * THE CAMERA ZOOMS 
		 */
		if ( inVehicle ){
			cameraTo = farCameraSize;
			cameraFrom = closeCameraSize;
		} else {
			cameraTo = closeCameraSize;
			cameraFrom = farCameraSize;
		}

		if ( inVehicle != lastInVehicle ){
			startTime = Time.time;
			doneLerping = false;
			lastInVehicle = inVehicle;
		}

		if ( !doneLerping ){
			Camera.main.GetComponent<Camera>().orthographicSize = Mathf.Lerp(cameraFrom, cameraTo, (Time.time - startTime)/lerpDuration );
			if ( Camera.main.GetComponent<Camera>().orthographicSize == cameraTo ){
				doneLerping = true;
			}
		}

		if ( !inVehicle && vehicleToBoard != null && v != null ){
			if ( Input.GetKeyDown(KeyCode.E) ){

				float distance = Vector3.Distance(p.transform.position, vehicleToBoard.transform.position);
				// board vehicle
				if ( distance <= boardingDistance ){
					inVehicle = true;
					// put player in the seat
					v.boardVehicle(p);
					// disable player
					p.SetActive(false);
					// set control unit to this
					unitInControl = vehicleToBoard;
					// set the vehicle to player controlled
					unitInControl.GetComponent<Motion>().userControlled = true;
					unitInControl.GetComponent<Motion>().shouldLock = true;
					// set the gravity
					unitInControl.rigidbody.useGravity = false;
					// set the camera
					Camera.main.GetComponent<MiniMapCameraFollow>().target = unitInControl.transform;
				}   
			}
		}

		if ( inVehicle && v != null ){
			if ( Input.GetKeyDown(KeyCode.Q) ){
				inVehicle = false;
				// remove player from the seat
				v.seat = p;
				// enable player and set its position above the vehicle
				p.SetActive(true);
				Vector3 temp = unitInControl.transform.position;
				temp.y += unitInControl.transform.localScale.y/1.9f + p.transform.localScale.y/1.9f;
				p.transform.position = temp;
				// set the vehicle to no longer be user controlled
				unitInControl.GetComponent<Motion>().userControlled = false;
				unitInControl.GetComponent<Motion>().shouldLock = false;
				// set the gravity
				unitInControl.rigidbody.useGravity = true;
				// switch unit in control to player!
				unitInControl = p;
				// set the vehicle to no longer be user controlled
				unitInControl.GetComponent<Motion>().userControlled = true;
				// set the camera
				Camera.main.GetComponent<MiniMapCameraFollow>().target = p.transform;
			}
		}
	}

	public Inventory getActiveInventory(){
		return inventoryPanel.GetComponent<Inventory>();
	}

	public Equipment getActiveEquipment(){
		Equipment temp = null;
		if ( unitInControl != null )
			temp = unitInControl.GetComponent<Equipment>();
		return temp;
	}

}
