using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntryTable : MyBehaviour, IUse
{
	public Queue<EntryWaitingGuestGroup> entryWaitingGuestGroup = new Queue<EntryWaitingGuestGroup>();

	public static EntryTable self;

	Transform _socket;
	public Transform socket {
		get{ 
			if (_socket == null)
				_socket = transform.GetChild(1);
			return _socket; 
		}
	}

	void Awake(){
		self = this;
	}

	void Start(){
		
	}

	public bool Use(Person person){
		bool t = true;
		var staff = person as Staff;
		var guest = person as Guest;

		if (staff != null)
			StaffUse (staff);
		else if (guest != null)
			CalculationMoney (guest);
		else
			t = false;
		return t;
	}

	// Called by GuestGroup
	public void AddEntryWaitingGuestGroup(GuestGroup guestGroup){
		cakeslice.OutlineAnimation.self.SetActive(true);

		entryWaitingGuestGroup.Enqueue (new EntryWaitingGuestGroup (guestGroup));
		NoticeM.UseNotice (ScriptM.guestHere, EntryTable.self.transform.position);

		// Call Staff to EntryTable
		StaffM.self.CallStaffForGuidance2Table (new WaitingGuestGroup(guestGroup, RequestType.Table));
	}


	void StaffUse(Staff staff){
		if (entryWaitingGuestGroup.Count <= 0)
			return;
		Table temTable = TableM.self.GetEmptyTable (entryWaitingGuestGroup.Peek ().guestGroup.Count);

		// do not have table now
		// Need to wait more
		if (temTable != null) {
			staff.Guidance2Table (entryWaitingGuestGroup.Dequeue ().guestGroup, temTable);
		} else {
			NoticeM.UseNotice ("No Table", transform.position);
		}

		if (entryWaitingGuestGroup.Count <= 0)
			cakeslice.OutlineAnimation.self.SetActive(false);
	}


	// TODO Next to do make ui
	void UpdateEntryWaitingGuestGroupOnUI(){
		
	}

	void CalculationMoney(Guest guest){
		// Add Later
		// take money form guest
		GM.self.money += 10;
	}
}

public class EntryWaitingGuestGroup{
	public GuestGroup guestGroup;

	public EntryWaitingGuestGroup(GuestGroup _guestGroup){
		guestGroup = _guestGroup;
	}
}
