using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MyBehaviour, IUse
{
	public Chair[] chairs;
	[HideInInspector]public Transform[] dishPoint;
	[HideInInspector]public Transform EatUpAllDishPoint;
	List<Dish> eatUpDishes = new List<Dish>();

	bool _isUsing;
	public bool isUsing{
		get{ return _isUsing; }
		set{ 
			if(_isUsing != value)
			if(value)
				statusNotice.SetText ("Use");
			else
				statusNotice.SetText ("Not Use");
			_isUsing = value; 
		}
	}

	public StatusNotice statusNotice;

	Transform _socket;
	public Transform socket {
		get{ 
			if (_socket == null)
				_socket = transform;
			return _socket;
		}
		set{ _socket = value; }
	}

	void Start(){
		chairs = new Chair[transform.childCount];
		chairs = transform.GetComponentsInChildren<Chair> ();

		var tt = transform.GetChild (4);
		dishPoint = new Transform[tt.childCount];
		for (int i = 0; i < tt.childCount; i++) {
			dishPoint[i] = tt.GetChild (i);
		}

		EatUpAllDishPoint = transform.GetChild (3);

		statusNotice = NoticeM.UseStaticNotice ("Not Use", transform.position+Vector3.up*1.3f);
	}

	public void SitGroup(GuestGroup guestGroup){
		isUsing = true;
		int smallValue = Mathf.Min (chairs.Length, guestGroup.guestList.Count);
		for (int i = 0; i < smallValue; i++) {
			chairs [i].Use (guestGroup.guestList [i]);
		}
	}


	// TODO Add for staff
	// Staff can take dishes
	// Can get Order form guest
	public bool Use(Person person){

		var t1 = person as Staff;

		// Is Guest
		if (!t1) {
			Chair tChair;
			// If already sit person then standup
			if (tChair = IsAlreadySit (person)) {
				if (CheckRemainChair () <= 1)
					isUsing = false;
				return tChair.Use (person);
			} else {
				return Sit (person);
			}
		}
		// Is Staff
		else{
			// Take Dishes
			var dishes = TakeEatUpDishes();
			t1.TakeDishes (dishes);
			return true;
		}
	}

	Chair IsAlreadySit(Person person){
		for (int i = 0; i < chairs.Length; i++) {
			if (chairs [i].sittingPerson == person) {
				return chairs [i];
			}
		}
		return null;
	}

	// Just use for sit
	public bool Sit(Person person){
		var chair = GetEmptyChair ();
		chair.Use (person);
		isUsing = true;
		return chair == null ? false : true;
	}

	public int CheckRemainChair(){
		int t = 0;
		for (int i = 0; i < chairs.Length; i++) {
			if (chairs [i].isFull) {
				++t;
			}
		}
		return t;
	}

	public Chair GetEmptyChair(){
		for (int i = 0; i < chairs.Length; i++) {
			if (!chairs [i].isFull) {
				return chairs [i];
			}
		}
		return null;
	}

	public Transform GetDishPoint(Chair chair){
		for (int i = 0; i < chairs.Length; i++) {
			if (chairs[i].Equals (chair)) {
				return dishPoint [i];
			}
		}
		return null;
	}

	float eatUpPointYOffset = 0;
	public float _eatUpPointYOffset = .1f;
	public void PlaceEatUpPoint(Dish dish){
		dish.transform.position = Vector3.up * eatUpPointYOffset + EatUpAllDishPoint.position;
		eatUpPointYOffset += _eatUpPointYOffset;
		eatUpDishes.Add (dish);
	}

	public Dish[] TakeEatUpDishes(){
		var t = eatUpDishes.ToArray ();
		eatUpDishes.Clear ();
		eatUpPointYOffset = 0;
		return t;
	}
}

