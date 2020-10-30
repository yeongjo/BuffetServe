//#define FirstKeySetting
//#define SecondKeySetting
#define __Third

using UnityEngine;
using System.Collections;

public class CameraController : MyBehaviour
{
	public float 	moveSpeed = .04f,
					scrollSpeed = 1,
					scrollMax = 20,
					rotateSpeed = .1f,
					scrollDistance = 6;
	public Vector2 rotateVec = new Vector2(26,0);

	Transform trans;
	Quaternion rot;
	Vector3 cameraPos;
	Vector3 offset;

	// Use this for initialization
	void Start ()
	{
		trans = transform;
		cameraPos = trans.position;

		rotateVec = (Vector2)transform.eulerAngles;
		rot = Quaternion.Euler (rotateVec.x, rotateVec.y, 0);
		trans.position = cameraPos + rot * offset;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		Vector3 cameraForward = Vector3.Cross (Vector3.up, trans.right);

		scrollDistance -= Input.mouseScrollDelta.y * scrollSpeed;
		scrollDistance = Mathf.Clamp (scrollDistance, 1, scrollMax);
		offset = new Vector3 (0, 0, -scrollDistance);

		#if FirstKeySetting
		if (Input.GetMouseButton (1)) {
			rot = Quaternion.Euler (rotateVec.x, rotateVec.y, 0);


			if (Input.GetKey (KeyCode.LeftControl)) {
				// With Control key rotate
				rotateVec += new Vector2(-InputM.mousePosDelta.y, InputM.mousePosDelta.x) * rotateSpeed;
				rotateVec.x = Mathf.Clamp (rotateVec.x, 10, 60);
				rot = Quaternion.Euler (rotateVec);
				trans.LookAt (cameraPos);
			} else {
				// Without Control key move position
				cameraPos = cameraPos + (-InputM.mousePosDelta.x*trans.right + InputM.mousePosDelta.y*cameraForward) * moveSpeed;

			}
		}
		#elif SecondKeySetting
		if (Input.GetMouseButton (2)) {
			rot = Quaternion.Euler (rotateVec.x, rotateVec.y, 0);
			//rotate
			rotateVec += new Vector2(-InputM.mousePosDelta.y, InputM.mousePosDelta.x) * rotateSpeed;
			rotateVec.x = Mathf.Clamp (rotateVec.x, 10, 60);
			rot = Quaternion.Euler (rotateVec);
			trans.LookAt (cameraPos);
		}
		// Without Control key move position
		cameraPos = cameraPos + (Input.GetAxis("Horizontal")*trans.right + -Input.GetAxis("Vertical")*cameraForward) * moveSpeed;
		#elif __Third
		if (Input.GetMouseButton (2)) {
			rot = Quaternion.Euler (rotateVec.x, rotateVec.y, 0);
			//rotate
			rotateVec += new Vector2(-InputM.mousePosDelta.y, InputM.mousePosDelta.x) * rotateSpeed;
			rotateVec.x = Mathf.Clamp (rotateVec.x, 10, 60);
			rot = Quaternion.Euler (rotateVec);
			trans.LookAt (cameraPos);
		}
		cameraPos = GM.localPlayer.transform.position;
		#endif
		trans.position = cameraPos + rot * offset;
	}
}

