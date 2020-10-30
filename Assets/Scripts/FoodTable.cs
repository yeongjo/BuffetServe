using UnityEngine;
using System.Collections.Generic;

public class FoodTable : MyBehaviour{
	List<Food> food;
	public List<Food> Food{
		get{ 
			UpdateFoodStatus ();
			return food; 
		}
	}

	float[] prevFoodRemainAmount;

	FoodTableDish[] foodTableDish;

	StatusNotice[] statusNotices;

	void Start(){
		food = new List<Food>(GetComponentsInChildren<Food>());
		foodTableDish = new FoodTableDish[food.Count];
		prevFoodRemainAmount = new float[food.Count];

		for (int i = 0; i < food.Count; i++) {
			foodTableDish[i] = food [i].transform.GetComponentInParent<FoodTableDish> ();
			foodTableDish [i].childFood = food [i];
			prevFoodRemainAmount [i] = food [i].remainFoodAmount;
		}

		statusNotices = new StatusNotice[food.Count];
		for (int i = 0; i < food.Count; i++) {
			statusNotices[i] = NoticeM.UseStaticNotice (food[i].remainFoodAmount.ToString(), food[i].socket.position);
		}

	}


	void FixedUpdate(){
		UpdateFoodStatus ();
	}

	public void UpdateFoodStatus(){
		for (int i = 0; i < food.Count; i++) {
			if (food [i] != null) {
				if (food [i].remainFoodAmount != prevFoodRemainAmount [i]) {
					statusNotices [i].SetText (food [i].remainFoodAmount.ToString ());
					prevFoodRemainAmount [i] = food [i].remainFoodAmount;
				}
			} else {
				if (prevFoodRemainAmount [i] != 0) {
					statusNotices [i].SetText ("0");
					prevFoodRemainAmount [i] = 0;
				}
			}
		}
	}
}