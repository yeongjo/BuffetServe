using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackUpObjectM : MyBehaviour
{
	//Transform rootTrans;
	public float offsetY = .1f;
	public float startOffsetY = .1f;
	float _offsetY;
	Vector3 direc = Vector3.up;

	List<Transform> stackTrans = new List<Transform>();

	/*
	public StackUpObjectM(Transform root, float offY, Vector3 direc = Vector3.up){
		rootTrans = root;
		offsetY = offY;
		this.direc = direc;
	}
	*/

	public void Add(Transform trans){
		trans.position = transform.position + direc * _offsetY + direc*startOffsetY;
		stackTrans.Add (trans);
		trans.SetParent (transform);
		_offsetY += offsetY;
	}

	public Transform Remove(){
		if (stackTrans.Count == 0)
			return null;
		_offsetY -= offsetY;
		var t = stackTrans [stackTrans.Count - 1];
		stackTrans.RemoveAt (stackTrans.Count - 1);
		t.SetParent (null);
		return t;
	}

	public void Clear(){
		_offsetY = 0;
		stackTrans.Clear ();
	}
}

