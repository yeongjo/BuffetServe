using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisableM : MyBehaviour {

	MeshRenderer meshRenderer;
	void Start(){
		meshRenderer = GetComponent<MeshRenderer> ();
	}
	void FixedUpdate () {
		if (Vector3.Dot (Camera.main.transform.forward, transform.forward) < 0)
			meshRenderer.enabled = false;
		else
			meshRenderer.enabled = true;
	}
}
