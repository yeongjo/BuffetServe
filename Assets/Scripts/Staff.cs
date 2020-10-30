using UnityEngine;
using System.Collections;

public class Staff : Person
{
	public bool isBusy;

	bool bIsAddedWaitingStaff;


	protected new void FixedUpdate (){
		base.FixedUpdate ();
		if (!isBusy && !bIsAddedWaitingStaff) {
			bIsAddedWaitingStaff = true;
			StaffM.self.AddWaitingStaff (this);
		}
	}


	// Go to Entry table first
	// Then take guest to table 


	// First gudie for guest
	// Middle gudie to table with guest
	public void Guidance2Table(GuestGroup _guestGroup, Table emptyTable){
		emptyTable.isUsing = true;
		isBusy = true;
		//GoToSocketAndUse (StaffM.self.entryTable);
		_guestGroup.TakeTable (emptyTable);
		isBusy = false;
	}

	// Called by Table
	// Take Dishes On my hand
	public void TakeDishes(Dish[] dishes){
		foreach (var item in dishes) {
			TakeDish (item);
		}
	}


	// List do
	void Account(){
		
	}
}

