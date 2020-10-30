using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusNotice : MyBehaviour
{
	Animator anim;

	[HideInInspector] Text text;

	void Start(){
		if (!anim)
			anim = GetComponentInChildren<Animator> ();
		//anim.StopPlayback ();
	}

	// Use this for initialization
	public void SetText(string msg)
	{
		if(!text)
			text = GetComponentInChildren<Text> ();
		if (!anim)
			anim = GetComponentInChildren<Animator> ();
		//anim.StartPlayback ();
		anim.Play ("StatusNoticeUpdated",-1,0);
		text.text = msg;
	}

	void FixedUpdate(){
		
		transform.rotation = /*Quaternion.Inverse*/ (Camera.main.transform.rotation);
		//anim.rootRotation= Camera.main.transform.rotation;
	}
}

