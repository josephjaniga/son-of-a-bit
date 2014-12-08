using UnityEngine;
using System.Collections;

public class MiniMapCameraFollow : MonoBehaviour {

	public Transform target;
	public bool Wireframe = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if ( target != null )
			transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
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
