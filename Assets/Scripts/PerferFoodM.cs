using UnityEngine;
using System.Collections.Generic;

public class PerferFoodM : MyBehaviour{
	
	public List<Food> foodList = new List<Food>();
	
	public static PerferFoodM self;
	
	void Awake(){
		self = this;
	}
	
	public Food GetRandomFood(){

		// TODO change later
		return FoodTableM.self.GetRandomFood();
	}
}