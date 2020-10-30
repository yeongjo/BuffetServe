using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KitchenTable : MyBehaviour, IUse
{

	/*
	 * 음식만들때 무슨 음식만들지 고르라는 메세지가 뜨고 선택시 만들지
	 * 레시피가 있어서 그 레시피대로 조합해서 음식이 나오게 할건지
	 */

	/*
	가져갈때 접시랑 같이 가져갈지 음식만 가져갈지 고민임
	접시도 있어야 맞는데 불편할거같기도
	*/

	/*
	음식테이블에 있는 음식가져와서 음식만들수도 있음
	*/

	Dish foodPlateDish;

	public Food foodTemplate;

	public Transform madeFoodSocket;
	Food madeFood;

	List<Food> requestFood = new List<Food>();
	public StackUpObjectM requestFoodObj;


	public Transform socket{get{ return transform; }}

	public static KitchenTable self;




	void Awake(){
		self = this;
	}


	public void AddRequestFood(Food food){
		Say ("음식추가됨");
		requestFood.Add (food);
		requestFoodObj.Add (food.transform);
	}


	// 
	public bool Use(Person person){

		// If person has any food
		// Then food is added by AddRequestFood
		var tDish = person.GetHavingDishes ();
		bool isHasAnyFood = false;
		if(tDish.Count>0)
			foreach (var item in tDish) {
				foreach (var t in item.Foods) {
					if (t != null) {
						isHasAnyFood = true;
						AddRequestFood (t);
					}
				}
			}
		if (isHasAnyFood) {
			person.RemoveAllFood ();
			return true;
		}


		if (madeFood == null)
			MakeFood ();
		else
			// TODO 음식가져가는게 안됨
			TakeMadeFood (person);
		return true;
	}

	// TODO Should to have delay time
	public Food MakeFood(){
		if (requestFood.Count == 0)
			return null;
		
		var foundRecipe = FoodRecipeM.self.FindFoodFormRecipe (requestFood);
		Food.TypeOfFood foodType;
		if (foundRecipe == null) {
			Say ("맞지않는 레시피");
			foodType = Food.TypeOfFood.Error;
		} else {
			Say ("음식만들어짐");
			foodType = foundRecipe.makingType;
		}

		var t = Instantiate (foodTemplate);
		t.Init (foodType);
		madeFood = t;
		SetMakingFoodPos (t.transform);
		ClearRequestFoods ();
		return t;
	}


	// Called by MakeFood
	void ClearRequestFoods(){
		foreach (var item in requestFood) {
			Destroy (item.gameObject);
		}
		requestFood.Clear ();
		requestFoodObj.Clear ();
	}

	public void TakeMadeFood(Person person){
		Say ("음식가져감");
		person.TakeFood (madeFood, 1000);
	}

	void SetMakingFoodPos(Transform trans){
		trans.position = madeFoodSocket.position;
	}
}

