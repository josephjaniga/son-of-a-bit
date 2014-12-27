using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DraggableWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public Transform window;
	public RectTransform rt;

	public bool dragging = false;

	// Use this for initialization
	void Start () {
		window = transform.parent;
		rt = window.GetComponent<RectTransform>();
	}
	
	void Update(){
		if ( dragging ){
			Vector3 temp = Input.mousePosition;
			temp.y -= rt.rect.height/2 - 16;
			window.position = temp;
		}
	}

	public void OnPointerDown(PointerEventData data){
		dragging = true;
	}
	
	public void OnPointerUp(PointerEventData data){
		dragging = false;
	}

}
