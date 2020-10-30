using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MyBehaviour {

	Text text;
	RawImage panel;

	public Transform followTrans;

	// Use this for initialization
	void Start () {

		Destroy (gameObject, 2);
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation =  Camera.main.transform.rotation;
		if(followTrans)
			transform.position = followTrans.position + Vector3.up*1.7f;
	}

	public void SetText(string msg){
		if (!text)
			text = GetComponentInChildren<Text> ();
		if (!panel)
			panel = GetComponentInChildren<RawImage> ();
		text.text = msg;
		panel.rectTransform.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight) * text.transform.localScale.x+Vector2.one*.2f;
//		panel.rectTransform.
	}
}
