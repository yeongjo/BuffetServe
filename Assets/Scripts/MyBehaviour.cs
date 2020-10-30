using UnityEngine;
using System.Collections;

public class MyBehaviour : MonoBehaviour
{
	public void Say(string msg){
		NoticeM.UseNotice (msg, transform);
	}
}

