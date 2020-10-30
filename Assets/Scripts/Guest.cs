using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guest : Person
{
	public Chair ownChair;

	[HideInInspector]public Dish dishForEat;
	public float hungryAmount;

	[HideInInspector] public GuestGroup belongGroup;

	public GuestState state;

	protected new void Start (){
		base.Start ();
		belongGroup = transform.GetComponentInParent<GuestGroup> ();
		state = GuestState.JoinAndWait;
	}

	public void ChangeState(GuestState _state){
		state = _state;
	}


	protected new void FixedUpdate(){
		base.FixedUpdate ();
		//if (state != GuestState.JoinAndWait)
		//	print (state);
		switch(state){
		case GuestState.FirstGo2Chair:
			if(ownChair != null)
				if (ownChair.isFull)
					state = GuestState.Want2Eat;
			break;

		// Check state is 'Want2Eat'
		case GuestState.Want2Eat:
			// if don't have food
			// then take food
			// Start State machine
			StartCoroutine(LetsTakeFood());
			break;
		}

		if (isAngry) {
			belongGroup.StandUpTableWithAngry ();
		}
	}



	// OK!! Let's take some food
	// First take a dish
	IEnumerator LetsTakeFood(){
		state = GuestState.TakeDish;
		Say ("Take Dish!");
		yield return new WaitForSeconds (.5f);

		// Let's take Dish
		var tDishTable = DishTable.allDishTables[0];
		// If Don't have more dishes??
		// This guy will be angry
		GoToSocketAndUse(TakeDishFormDishTable,tDishTable,tDishTable.socket);
		yield return new WaitUntil(()=>isReachToPoint);
		yield return new WaitForSeconds (.5f);
		if (isAngry)
			yield break;
		// Taked dish

		if (dishes.Count == 0) {
			Say ("접시 어디??");
			GoToSocketAndUse(ownChair);
			yield return new WaitUntil(()=>isReachToPoint);
			yield return LetsTakeFood ();
		}
		// Let's take Food
		state = GuestState.TakeFood;
		Say ("음식가져와야지");
		var tFood = PerferFoodM.self.GetRandomFood();
		GoToSocketAndUse(TakeFood, tFood, tFood.socket);
		yield return new WaitUntil(()=>isReachToPoint);
		// Taked Food on dish

		// Get back to your own chair
		// He can Throw dish on Ground
		// ownChair can be null
		GoToSocketAndUse(ownChair);
		yield return new WaitUntil(()=>isReachToPoint);
		dishForEat = ThrowAwayDish (ownChair.GetDishPoint());
		dishForEat.isClean = false;

		yield return StartEat ();
	}



	IEnumerator StartEat(){
		state = GuestState.Eat;
		Say (ScriptM.eatting);
		dishForEat.EatFoodThisGuest (this);
		// Eat all?
		yield return new WaitForSeconds (1);
		Say (ScriptM.doneEatting);
		// Dish place to EatupPoint
		ownChair.rootTable.PlaceEatUpPoint(dishForEat);

		yield return new WaitForSeconds (1);
		// If is hungry yet
		if (hungryAmount > 0)
			yield return LetsTakeFood ();
		// Else
		else
		{	// Leave restaurant
			yield return GoEntryTableThenPayMoney ();
		}
	}

	IEnumerator GoEntryTableThenPayMoney(){
		state = GuestState.Want2Exit;
		Say("갈시간이네");
		belongGroup.StandUpTable (this);
		yield return new WaitUntil(()=>belongGroup.state == GuestGroup.GuestGroupState.Want2Exit);
		GoToSocketAndUse(EntryTable.self);
		yield return new WaitUntil(()=>isReachToPoint);
		Say("Take my money!");
		belongGroup.state = GuestGroup.GuestGroupState.Exit;
		state = GuestState.Exit;
		yield return LeaveRestaurant ();
	}

	protected IEnumerator LeaveRestaurant(){
		yield return new WaitForSeconds (1);
		//RemoveThisFormGroup ();
		if(isAngry)
			Say("접시어딧어 ㅈ같은식당..");
		else
			Say(ScriptM.byebye);
		GoToSocketAndUse(ExitM.self);
		yield return new WaitUntil(()=>isReachToPoint);
	}

	// Called by branch isAngry is true when angry
	// isAngry is change by Person
	bool isLeaving;
	public void Leave(){
		if (isLeaving)
			return;
		isLeaving = true;
		isAngry = true;
		var t = ThrowAwayDish ();
		if (t) {
			t.Drop ();
		}
		EnableMoveThings (true);
		StopAllCoroutines ();
		StartCoroutine (LeaveRestaurant ());
	}

	void RemoveThisFormGroup(){
		belongGroup.StandUpTable (this);
	}

	public override void Sit(bool t, Chair chair){
		base.Sit (t, chair);
		if (!t)
			trans.SetParent (belongGroup.transform);
	}

	public enum GuestState{
		Error = 0,
		JoinAndWait = 1,
		FirstGo2Chair,
		Want2Eat,
		TakeDish,
		TakeFood,
		Eat,
		Want2Exit,
		Exit
	}
}
