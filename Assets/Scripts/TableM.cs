using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableM : MyBehaviour
{
	public GameObject[] tableTemplate;

	public List<Table> tableList = new List<Table>();

	public static TableM self;

	void Awake(){
		self = this;
	}

	void Start ()
	{
		FindTableInScene ();
	}

	// Init
	void FindTableInScene(){
		tableList = new List<Table>(GameObject.FindObjectsOfType<Table> ());
	}

	public Table GetEmptyTable(int count){
		foreach (var item in tableList) {
			if(!item.isUsing && item.chairs.Length >= count)
				return item;
		}
		return null;
	}

	GameObject CreateTable(){
		var t = Instantiate (tableTemplate[0]);
		tableList.Add(t.GetComponent<Table> ());
		return t;
	}
}

