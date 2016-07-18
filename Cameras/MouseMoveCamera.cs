using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseMoveCamera : MonoBehaviour {

	public float CameraSpeed = 0.5f;
	public float dragSpeed = 2;

	bool cameraDragging = true;
	Vector3 dragOrigin;
	bool rotating = false;
	bool RightClick = false;
	
	void Update () {
		Vector2 mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);	
		float left = Screen.width * 0.2f;
		float right = Screen.width - (Screen.width * 0.2f);
		float bottom = Screen.height * 0.2f;
		float top = Screen.height - (Screen.height * 0.2f);
		float Y;

		float scroll = Input.GetAxis ("Mouse ScrollWheel");

		transform.Translate (0, scroll * 50.0f, 0);


		// Check if mouse is left / right / top / bottom screen edge

		if ((mousePosition.x < left || mousePosition.x > right || mousePosition.y > top || mousePosition.y < bottom) && !rotating) {
			cameraDragging = true;
		} else 
			cameraDragging = false;
		
		
		if (cameraDragging) {
			Y = transform.position.y;
			if (mousePosition.x < left) {
				transform.Translate (Vector3.left);
			} else if (mousePosition.x > right){
				transform.Translate (Vector3.right);
			} else if (mousePosition.y > top) {
			   transform.Translate(Vector3.forward);			
			} else if (mousePosition.y < bottom) {
				transform.Translate (Vector3.back);
			}

			// Reset Height else camera moved up or down because it's tilting
			Vector3 pos;
			pos = transform.position;
			pos.y = Y;
			transform.position = pos;
		}	
		
		RightClick = Input.GetMouseButtonDown (1);


		if (RightClick && !rotating) {
			rotating = true;
			dragOrigin = mousePosition;

		}

		if (rotating) {
			if (dragOrigin.x < mousePosition.x) {
				transform.RotateAround(transform.position,Vector3.up,.5f);
			} else if (dragOrigin.x > mousePosition.x) {
				transform.RotateAround(transform.position,Vector3.up,-.5f);
			}

			float tilt = Input.GetAxis ("Mouse Y") * 2.0f;
		//	Debug.Log (tilt.ToString());

			transform.Rotate (tilt,0,0);

		}
		
		if (!Input.GetMouseButton (1)) {
			rotating = false;		
		}		
	}
}
