using UnityEngine;
using System.Collections;

public class NoticeM : MyBehaviour
{
	public Notice notice;
	public StatusNotice staticNotice;

	public static NoticeM self;

	void Awake(){
		self = this;
	}

	public static void UseNotice(string msg, Vector3 pos){
		var t = Instantiate (self.notice, pos+Vector3.up*1.7f, Quaternion.identity, self.transform);
		// TODO fix position
		//t.GetComponent<RectTransform>().localPosition = pos + Vector3.up * 1.7f;
		t.SetText (msg);
	}

	public static void UseNotice(string msg, Transform pos){
		var t = Instantiate (self.notice, pos.position+Vector3.up*1.7f, Quaternion.identity, self.transform);
		t.followTrans = pos;
		// TODO fix position
		//t.GetComponent<RectTransform>().localPosition = pos + Vector3.up * 1.7f;
		t.SetText (msg);
	}

	public static StatusNotice UseStaticNotice(string msg, Vector3 pos){
		var t = Instantiate (self.staticNotice, pos+Vector3.up*1, Quaternion.identity, self.transform);
		t.SetText (msg);
		return t;
	}

	IEnumerator CreateTest(){
		yield return new WaitForSeconds (1);
		Instantiate (notice, transform);
	}

	void OnDrawGizmos(){
	}
}

