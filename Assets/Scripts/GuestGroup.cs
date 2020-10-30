using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuestGroup : MyBehaviour{
	public List<Guest> guestList = new List<Guest>();

	Table ownTable;
	public int Count{
		get{ return guestList.Count; }
	}
	public GuestGroupState state;

	void Start(){
		guestList = new List<Guest>(GetComponentsInChildren<Guest> ());
		state = GuestGroupState.JoinAndWait;
		ChangeAllGuestState(Guest.GuestState.JoinAndWait);
		CallStaffWhenEnter ();
	}

	void FixedUpdate (){
		
		switch(state){
		// Check state is 'Eat'
		case GuestGroupState.Eat:

			break;
		}

		if (guestList.Count == 0) {
			Destroy (gameObject, 30);
		}
	}


	// It call guest enter to restaurant
	// Should to call Once!!
	void CallStaffWhenEnter(){
		// Add Queue to EntryTable
		// And Call Staff to EntryTable
		EntryTable.self.AddEntryWaitingGuestGroup (this);
	}

	// Guidance to Table
	public bool TakeTable(Table table){
		bool t = CheckThisGroupCanSitOnTableChairs (table);
		if (t) {
			state = GuestGroupState.Guidance2Table;
			ownTable = table;
			TakeTableForeach (table);
		} else {
			Debug.LogError ("what the fuck? Can't sit?? check StaffM.CheckEmptyTable..");
		}
		return t;
	}

	// Every guest in this group
	// They take one table
	void TakeTableForeach(Table table){
		for (int i = 0; i < guestList.Count; i++) {
			// Order to each
			guestList [i].Say(ScriptM.thankYou);
			guestList [i].GoToSocketAndUse(table.chairs[i]);
			guestList [i].ownChair = table.chairs [i];
		}
		state = GuestGroupState.Guidance2Table;

		// Should to call this after sit on chair
		ChangeAllGuestState (Guest.GuestState.FirstGo2Chair);
	}

	// Angry stand up
	// Just use everything then stand up

	// Called by RemainGuestCheck
	public void StandUpTable(Guest guest){
		guestList.Remove (guest);
		if (guestList.Count == 0) {
			ownTable.isUsing = false;
			state = GuestGroupState.Want2Exit;
		}
	}

	public void StandUpTableWithAngry(){
		GoOutEveryBodyWithAngry ();
		guestList.Clear ();
		ownTable.isUsing = false;
	}

	void GoOutEveryBodyWithAngry(){
		foreach (var item in guestList) {
			item.Leave ();
		}
	}

	void ChangeAllGuestState(Guest.GuestState _state){
		foreach (var item in guestList) {
			item.ChangeState (_state);
		}
	}

	public bool CheckThisGroupCanSitOnTableChairs(Table table){
		return table.chairs.Length >= guestList.Count ? true : false;
	}

	public enum GuestGroupState{
		Error = 0,
		JoinAndWait = 1,
		Guidance2Table,
		Eat,
		Want2Exit = 3,
		Exit = 4
	}
}

