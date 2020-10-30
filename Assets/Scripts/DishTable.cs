using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DishTable : MyBehaviour, IUse
{
	public Dish dishTemplate;
	public StackUpObjectM dishSocket;
	public List<Dish> dishes = new List<Dish>();


	public static List<DishTable> allDishTables = new List<DishTable> ();

	public Transform socket {
		get{ return transform;}
	}

	// Use this for initialization
	void Start ()
	{
		dishSocket = GetComponentInChildren<StackUpObjectM> ();
		allDishTables.Add (this);
		CreateDish ();
		CreateDish ();
		CreateDish ();
		CreateDish ();
	}


	public Dish CreateDish(){
		var t = Instantiate (dishTemplate,dishSocket.transform.position,Quaternion.identity, transform);
		dishSocket.Add (t.transform);
		dishes.Add (t);
		return t;
	}


	public void PutDishes(Dish[] dishes){
		this.dishes.AddRange (dishes);
		foreach (var item in dishes) {
			dishSocket.Add (item.transform);
		}
	}

	// Put dish on the table
	public void PutDish(Dish dish){
		var t = dish;
		if (t != null) {
			if (!t.isClean)
				Debug.LogError ("Dirty Dish can't put here");
			t.transform.SetParent (transform);
			t.transform.position = socket.position;
			dishes.Add (t);
			dishSocket.Add (t.transform);
		}
	}

	public Dish TakeDish(){
		if (dishes.Count == 0)
			return null;
		Dish t = dishes [dishes.Count - 1];
		dishes.RemoveAt(dishes.Count-1);
		dishSocket.Remove ();
		t.transform.SetParent (null);
		return t;
	}

	public void TakeDishToPerson(Person person){
		person.TakeDish (TakeDish ());
	}

	public bool Use(Person person){
		var t1 = person as Guest;
		var t2 = person as Staff;
		if (t1 != null) {
			TakeDish ();
			return true;
		} else if (t2 != null) {

			// Put dish
			// If Just one dish was dirty
			var tDishes = t2.GetHavingDishes ();
			if (tDishes.Count > 0) {
				bool isDirty = false;
				foreach (var item in tDishes) {
					if (!item.isClean) {
						isDirty = true;
						break;
					}
				}
				if (!isDirty)
					PutDishes (t2.GiveHavingDishes ());
				else
					NoticeM.UseNotice ("Dirty! Wash First", transform.position);
				return true;
			} else {
				// Take Dish
				TakeDishToPerson (person);
			}
		}
		return false;
	}

}

