using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaffM : MyBehaviour
{
	List<Staff> staffList = new List<Staff>();
	Queue<Staff> waitingStaff = new Queue<Staff>();


	public EntryTable entryTable;

	public GameObject staffTemplate;
	public Transform staffRoot;
	public Transform staffSpawnPoint;

	public Transform entryPoint;

	[SerializeField]Queue<WaitingGuestGroup> waitingGuestGroup = new Queue<WaitingGuestGroup>();

	public static StaffM self;


	void Awake(){
		self = this;
	}

	void Start(){
		StartCoroutine (CheckEmptyTableForWaitingGuest ());
	}

	void FixedUpdate(){
	}

	public void CreateStaff(){
		var t = Instantiate (staffTemplate, staffSpawnPoint.position, Quaternion.identity, staffRoot);
		AddStaff (t.GetComponent<Staff>());
	}

	public void AddStaff(Staff staff){
		staffList.Add (staff);
	}

	public void RemoveStaff(){
		
	}

	public void AddWaitingStaff(Staff item){
		waitingStaff.Enqueue(item);	
	}

	public Staff GetWaitingStaff(){
		return waitingStaff.Dequeue ();
	}

	// Called by Guest
	public void CallStaffForGuidance2Table(WaitingGuestGroup _guestGroup){
		// Need to add Check first
		foreach (var item in waitingGuestGroup)
			if (item == _guestGroup)
				return;
		waitingGuestGroup.Enqueue (_guestGroup);
	}


	// called by this Start()
	IEnumerator CheckEmptyTableForWaitingGuest(){
		while (!GM.isGameEnd) {
			
			// If don't have waiting Staff
			// Or don't have Waiting Guest
			// Check later
			if (waitingGuestGroup.Count > 0 && waitingStaff.Count > 0) {

				var t = waitingGuestGroup.Peek ();

				switch(t.requestType){
				case RequestType.Table:
					CheckTableThenGoToEntryTable (t);
					break;
				}
			}
			yield return new WaitForSeconds (1);
		}
	}

	void CheckTableThenGoToEntryTable(WaitingGuestGroup waitGroup){
		// TODO can make bug. can be null.
		Table temTable = TableM.self.GetEmptyTable (waitGroup.Count);

		// do not have table now
		// Need to wait more
		if (temTable != null) {
			GetWaitingStaff ().GoToSocketAndUse (entryTable);
		}
	}

	void Account(WaitingGuestGroup waitGroup){
		
	}

	void AddtionalAccount(WaitingGuestGroup waitGroup){
		
	}


	// OLD way
	int FindNotBusyStaffIndex(){
		for (int i = 0; i < staffList.Count; i++)
			if (!staffList [i].isBusy)
				return i;
		return -1;
	}
}

public class WaitingGuestGroup{

	public GuestGroup guestGroup;
	public RequestType requestType;

	public WaitingGuestGroup(GuestGroup guestGroup, RequestType requestType){
		this.guestGroup = guestGroup;
		this.requestType = requestType;
	}

	public int Count{
		get{ return guestGroup.Count; }
	}
}
