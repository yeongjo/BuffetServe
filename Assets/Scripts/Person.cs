using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;

[SelectionBase]
public class Person : MyBehaviour
{
	Vector3 _nextMovePoint;
	public Vector3 nextMovePoint{
		get{ return _nextMovePoint; }
		set{
			isReachToPoint = false;
			agent.isStopped = false;
			agent.SetDestination (value);
			_nextMovePoint = value;
		}
	}
	bool bIsChangeNextMovePoint;

	public bool isReachToPoint;

	public PersonState personState;
	[HideInInspector]public Chair sittingChair;

	protected List<Dish> dishes = new List<Dish> ();

	NavMeshAgent agent;
	[HideInInspector]public Transform trans;

	StackUpObjectM handSocket;

	protected void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
		trans = transform;
		handSocket = GetComponentInChildren<StackUpObjectM>();
	}
	
	protected void FixedUpdate(){
		MoveTo ();

	}


	public void EnableMoveThings(bool t){
		var t1 = GetComponent<NavMeshAgent> ();
		if(t1 && t1.enabled != t)
			t1.enabled = t;
	}

	void StandUpFromChair(){
		EnableMoveThings (true);
		if(sittingChair)
			sittingChair.Use (this);
	}

	/********************************/
	// TODO How could use this for food
	// Called by Wat??
	// Move to chair and Sit
	Coroutine goToCoroutine;

	public void GoToSocketAndUse(IUse useObj){
		if (goToCoroutine != null) StopCoroutine (goToCoroutine);
		StandUpFromChair ();
		nextMovePoint = useObj.socket.position;
		goToCoroutine = StartCoroutine (CheckIsReachToSocket (useObj));
	}

	IEnumerator CheckIsReachToSocket(IUse useObj){
		for (int i = 0; i < 10; i++) {
			yield return null;
		}
		bIsChangeNextMovePoint = true;
		yield return new WaitUntil (() => isReachToPoint);
		UseObj (useObj);
	}

	void UseObj(IUse useObj){
		goToCoroutine = null;
		//agent.isStopped = true;
		useObj.Use (this);
	}
	/********************************/
	public void GoToSocketAndUse<T1>(Action<T1> callingFunc, T1 t, Transform _socket){
		if (goToCoroutine != null) StopCoroutine (goToCoroutine);
		StandUpFromChair ();
		nextMovePoint = _socket.position;
		goToCoroutine = StartCoroutine (CheckIsReachToSocket (callingFunc, t));
	}

	IEnumerator CheckIsReachToSocket<T1>(Action<T1> callingFunc, T1 t){
		for (int i = 0; i < 10; i++) {
			yield return null;
		}
		bIsChangeNextMovePoint = true;
		yield return new WaitUntil (() => isReachToPoint);
		UseObj (callingFunc, t);
	}

	void UseObj<T1>(Action<T1> callingFunc, T1 t){
		goToCoroutine = null;
		agent.isStopped = true;
		callingFunc (t);
	}
	/********************************/

	Vector3 pathEnd;
	void MoveTo(){
		if (!bIsChangeNextMovePoint)
			return;
		pathEnd = agent.pathEndPosition;//debug
		//float distance = Vector3.Distance (agent.pathEndPosition, trans.position);
		float distance = agent.remainingDistance;
		if (distance <= agent.stoppingDistance) {
			isReachToPoint = true;
			bIsChangeNextMovePoint = false;
		}else
			isReachToPoint = false;
	}


	bool IsReachToDestination(Vector3 point){
		return false;
	}

	public void TakeDishFormDishTable(DishTable dishTable){
		var dish = dishTable.TakeDish ();
		TakeDish (dish);
	}


	/// <summary>
	/// Removes all food. With object
	/// </summary>
	public void RemoveAllFood(){
		foreach (var item in dishes) {
			/*
			foreach (var t2 in item.Foods) {
				Destroy (t2.gameObject);
			}
			*/
			item.Foods.Clear ();
		}
	}


	public bool isAngry;
	public void TakeDish(Dish dish){
		
		if (dish == null) {
			Say ("No Dish? Fxxk you");
			isAngry = true;
			return;
		}
		//dish.transform.rotation = Quaternion.identity;
		dish.transform.SetParent (handSocket.transform);

		handSocket.Add (dish.transform);
		if (!dish.isClean) {
			foreach (var item in dishes) {
				item.isClean = false;
			}
		}
		dishes.Add (dish);
	}


	/// <summary>
	/// Take food -> guest
	/// It call Food::TakeFood(Amount:1)
	/// Can be angry
	/// </summary>
	/// <param name="food">taking food</param>
	public void TakeFood(Food food){
		if (!food)
			return;
		if (dishes.Count < 1) {
			Say ("접시가 없엉");
			return;
		}
		var t = food.GiveFood (dishes [0].gameObject, 1);
		if (t != null)
		if (dishes.Count > 0) {
			int i = -1;
			if((i = dishes [0].Foods.FindIndex ((a)=>a.type==t.type)) > -1)
				dishes [0].Foods[i] = t;
			else
				dishes [0].Foods.Add (t);
		}
		else
			isAngry = true;
	}

	public void TakeFood(Food food, float amount){
		if (!food)
			return;
		if (dishes.Count < 1) {
			Say ("접시가 없엉");
			return;
		}
		var t = food.GiveFood (dishes [0].gameObject, amount);
		if (t != null)
		if (dishes.Count > 0) {
			int i = -1;
			if((i = dishes [0].Foods.FindIndex ((a)=>a.type==t.type)) > -1)
				dishes [0].Foods[i] = t;
			else
				dishes [0].Foods.Add (t);
		}
		else
			isAngry = true;
	}



	public float throwForce = 2;
	// TODO Something is wrong
	// Not using now
	public Dish ThrowAwayDish(){
		if (dishes.Count == 0)
			return null;
		foreach (var t in dishes) {
			var t1 = handSocket.Remove ();
			t1.position = trans.position + trans.forward * .5f + Vector3.up;
			var t2 = t1.gameObject.AddComponent<Rigidbody> ();
			t2.AddForce (trans.forward * throwForce);
		}
		var tt = dishes[dishes.Count-1];
		dishes.RemoveAt (dishes.Count-1);
		return tt;
	}

	public Dish ThrowAwayDish(Transform point){
		if (point == null || dishes.Count == 0)
			return ThrowAwayDish ();
		foreach (var t in dishes) {
			t.transform.SetParent (null);
			t.transform.position = point.position;
			handSocket.Remove ();
		}
		var tt = dishes[dishes.Count-1];
		dishes.RemoveAt (dishes.Count-1);
		return tt;
	}

	public virtual void Sit(bool t, Chair chair){
		if (t) {
			personState = PersonState.sit;
			sittingChair = chair;
			chair.sittingPerson = this;
		} else {
			personState = PersonState.stand;
			sittingChair = null;
			chair.sittingPerson = null;
		}
	}

	public bool IsHavingDishes(){
		return dishes.Count > 0 ? true : false;
	}

	public Dish[] GiveHavingDishes(){
		var t = dishes.ToArray();
		foreach (var item in t) {
			handSocket.Remove ();
		}
		dishes.Clear();

		return t;
	}

	public List<Dish> GetHavingDishes(){
		return dishes;
	}


	public List<Food> GetHavingFoods(){
		List<Food> tFoodList = new List<Food> ();
		foreach (var item in dishes) {
			foreach (var t2 in item.Foods) {
				tFoodList.Add (t2);
			}
		}
		return tFoodList;
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawSphere (nextMovePoint, .1f);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (pathEnd, .2f);

	}


	public enum PersonState{
		stand,
		sit,
		lay,
		dead
	}
}

