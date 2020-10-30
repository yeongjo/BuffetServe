using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sink : MyBehaviour, IUse
{
	List<Dish> waitingDishes = new List<Dish>();
	List<Dish> cleanedDishes = new List<Dish>();

	public StackUpObjectM cleanedPoint, dutyPoint;

	public Transform socket{
		get { return transform; }
	}

	void Start(){
		StartCoroutine (SlowUpdate ());
	}

	// Called by Staff
	public bool Use(Person person){
		var t1 = person as Staff;
		if (t1 == null)
			return false;
		// If staff has some dishes?
		// then Put dishes
		// Else
		// Take Dishes

		if (t1.IsHavingDishes())
			PutDishes (t1.GiveHavingDishes ());
		else
			t1.TakeDishes(TakeDishes ());
		return true;
	}

	public Dish[] TakeDishes(){
		var t = cleanedDishes.ToArray ();
		cleanedDishes.Clear ();
		cleanedPoint.Clear ();
		return t;
	}

	// Called by Staff
	public void PutDishes(Dish[] dishes){
		foreach (var item in dishes) {
			dutyPoint.Add (item.transform);
		}

		waitingDishes.AddRange (dishes);
	}

	IEnumerator SlowUpdate(){
		while (!GM.isGameEnd) {

			if (waitingDishes.Count > 0) {
				yield return new WaitForSeconds (1);
				CleanDish ();
			}else
				yield return new WaitForSeconds (.1f);
		}
	}

	// Called by Staff
	public void CleanDish(){
		if (waitingDishes.Count <= 0)
			return;
		NoticeM.UseNotice ("Clean!", transform.position);
		var t = dutyPoint.Remove ();
		cleanedPoint.Add(t.transform);
		// Take Clean
		var i = waitingDishes.Count-1;
		waitingDishes[i].isClean = true;
		cleanedDishes.Add(waitingDishes[i]);
		waitingDishes.RemoveAt(i);
	}
}

