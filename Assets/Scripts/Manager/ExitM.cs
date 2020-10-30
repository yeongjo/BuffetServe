using UnityEngine;
using System.Collections;

public class ExitM : MyBehaviour, IUse
{
	public static ExitM self;

	public Transform socket{
		get{ return transform; }
	}

	void Awake(){
		self = this;
	}

	public bool Use(Person person){
		var t1 = person as Guest;

		if (t1 != null) {
			Destroy (t1.gameObject);
			return true;
		}
		return false;
	}
}

