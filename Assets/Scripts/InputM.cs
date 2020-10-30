using UnityEngine;
using System.Collections;

public class InputM : MyBehaviour
{
	static Vector3 _prevMousePos;
	public static Vector3 prevMousePos{
		get{
			if(_prevMousePos != Vector3.zero)
				return _prevMousePos;
			return Input.mousePosition;
		}
	}
	public static Vector3 _mousePosDelta;
	public static Vector3 mousePosDelta {
		get{ return new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0); }
		set{ _mousePosDelta = value; }
	}

	void Update(){
		if (Input.GetMouseButtonDown (2)) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}else if (Input.GetMouseButtonUp (2)) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		//mousePosDelta = CalculateMouseDalta ();
	}
}

