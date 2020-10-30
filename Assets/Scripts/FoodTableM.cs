using UnityEngine;
using System.Collections.Generic;

public class FoodTableM : MyBehaviour{
	
	List<FoodTable> foodTableList;
	public List<FoodTable> FoodTableList{
		get{ return foodTableList; }
	}

	List<Food> everyFoods = new List<Food>();
	public List<Food> EveryFoods{
		get{ 
			UpdateFoodList ();
			return everyFoods;
		}
	}

	public static FoodTableM self;

	void Awake(){
		self = this;
	}

	void Start(){
		// Find FoodTable
		FindFoodTable();
	}


	void UpdateFoodList(){
		everyFoods.Clear ();
		foreach (var t in foodTableList) {
			everyFoods.AddRange(t.Food);
		}
	}

	void FindFoodTable(){
		foodTableList = new List<FoodTable>(GameObject.FindObjectsOfType<FoodTable>());
	}

	public Food GetRandomFood(){
		var num = Random.Range (0, EveryFoods.Count);
		return everyFoods [num];
	}
}
