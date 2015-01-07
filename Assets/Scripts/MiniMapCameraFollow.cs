using UnityEngine;
using System.Collections;

public class MiniMapCameraFollow : MonoBehaviour {

	public Transform target;
	public bool Wireframe = false;

	public bool doneLerping = false;
	private float startTime;
	private float lerpDuration = 4f;
	private Vector3 cameraOffsetActual = Vector3.zero;
	private Vector3 cameraFrom = Vector3.zero;
	private Vector3 cameraTarget = Vector3.zero;
	private Vector3 cameraOffsetNormal = Vector3.zero;
	private Vector3 cameraOffsetVehicle = Vector3.zero;
	public Vector3 cameraPlayerOffsetTarget = new Vector3(0f, 1.75f, 0f);
	private Main m;
	private Camera cam;


	// Use this for initialization
	void Start () {
		cam = Camera.main.GetComponent<Camera>();
		m = GameObject.Find ("FatherBit").GetComponent<Main>();
	}
	
	void Update(){

		/*
		 * THE CAMERA ZOOMS 
		 */
		if ( m.inVehicle ){
			cameraTarget = cameraOffsetVehicle;
			cameraFrom = cameraOffsetActual;
		} else {
			cameraTarget = cameraPlayerOffsetTarget;
			cameraFrom = cameraOffsetActual;
		}
		
		if ( m.inVehicle != m.lastInVehicle ){
			startTime = Time.time;
			doneLerping = false;
		}

		if ( !doneLerping ){
			cameraOffsetActual = Vector3.Lerp(cameraFrom, cameraTarget, (Time.time - startTime)/lerpDuration );
			if ( cameraOffsetActual == cameraTarget ){
				doneLerping = true;
			}
		}
	}


	// Update is called once per frame
	void LateUpdate () {

		if ( target != null ){
			if ( m.unitInControl.name == "Player" ){
				transform.position = new Vector3(target.position.x, target.position.y, transform.position.z) + cameraOffsetActual;
			} else {
				transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
			}
		}

	}

	void OnPreRender() {
		if ( Wireframe )
			GL.wireframe = true;
	}

	void OnPostRender() {
		if ( Wireframe )
			GL.wireframe = false;
	}

}
