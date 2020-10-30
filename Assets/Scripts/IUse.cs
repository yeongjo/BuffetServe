using UnityEngine;
using System.Collections;

public interface IUse
{
	bool Use (Person person);

	Transform socket {
		get;
	}
}

public enum RequestType{
	Table, Account, AddtionalAccount
}