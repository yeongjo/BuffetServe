using UnityEngine;
using System.Collections;

[SelectionBase]
public class Chair : MyBehaviour, IUse
{
	[HideInInspector]public Table rootTable;
	public Person sittingPerson;
	public bool isFull;
	public float pullScale = .6f;

	[HideInInspector] Transform _socket;
	public Transform socket {
		get{ return _socket; }
		set{ _socket = value; }
	}

	void Start(){
		socket = transform.GetChild (1);
		rootTable = GetComponentInParent<Table> ();
	}

	void FixedUpdate(){
		if (sittingPerson) {
			sittingPerson.transform.SetParent (_socket, false);
			sittingPerson.transform.SetPositionAndRotation(_socket.position, _socket.rotation);
		}
	}

	// Called by person
	public bool Use(Person person){
		ChangeisUsing (!isFull);
		person.EnableMoveThings (!isFull);
		if (IsAlreadySit (person)) {
			sittingPerson.trans.Translate (Vector3.back);
			sittingPerson.Sit (false, this);
		}else{
			sittingPerson = person;
			sittingPerson.Sit (true, this);
		}
		// Old way
		//person.GoToSitOnChair (this);

		return true;
	}

	bool IsAlreadySit(Person person){
		return sittingPerson == person ? true : false;
	}

	void ChangeisUsing(bool _isUsing){
		isFull = _isUsing;

		float backAmount = isFull ? pullScale:-pullScale;
		// Move to little back or forward
		transform.Translate (0, 0, backAmount);
	}

	public Transform GetDishPoint(){
		return rootTable.GetDishPoint (this);
	}
}

