using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodRecipeM : MyBehaviour
{
	FoodRecipe[] foodRecipe;

	// Mesh
	// Type

	public static FoodRecipeM self;

	void Awake(){
		self = this;
		foodRecipe = Resources.LoadAll<FoodRecipe> ("FoodRecipe");
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	public FoodRecipe FindFoodFormRecipe(List<Food> types){
		foreach (var item in foodRecipe) {

			// Check recipe is same
			if(types.Count == item.requestTypes.Length){
				var len = types.Count;
				var isSame = true;
				for (int i = 0; i < len; i++) {
					
					for (int j = 0; j < len; j++) {
						if (!item.requestTypes [i].Equals (types [j].type)) {
							isSame = false;
							break;
						}
					}
					if (!isSame)
						break;
				}
				if (isSame)
					return item;
			}
		}
		return null;
	}


	public GameObject[] FindMeshFromType(Food.TypeOfFood type){
		if (foodRecipe == null)
			return null;
		foreach (var item in foodRecipe) {
			if (item.makingType == type)
				return item.makingFoodObjByAmount;
		}
		return null;
	}
}

