using UnityEngine;
using System.Collections;

public class FoodTableDish : MyBehaviour, IUse, IRightUse
{
	FoodTable foodTable;
	public Food childFood;

	public Transform socket{ get { return transform; } }

	void Start(){
		foodTable = GetComponentInParent<FoodTable> ();
	}

	/// <summary>
	/// Take Food amount 1
	/// </summary>
	/// <param name="person">taken person.</param>
	public bool Use(Person person){

		person.TakeFood (childFood);
		if (childFood == null) {
			childFood = null;
			var tFood = foodTable.Food;
			tFood[tFood.FindIndex ((t)=>t==childFood)] = null;
		}
		return true;
	}

	/// <summary>
	/// Called by player only with right click use
	/// </summary>
	/// <param name="person">using person.</param>
	public void RightUse(Player player){
		TakeFoodFromPerson (player);
	}

	void TakeFoodFromPerson(Player player){
		var t = player.GetHavingFoods ();
		bool isTaken = false;

		if (t.Count > 0) {
			foreach (var item in t) {
				var t1 = item;
				if (t1.type == childFood.type) {
					t1.GiveFood(childFood.gameObject, 1000);
					//childFood.CombineFood ();
					isTaken = true;
				}
			}
		}

		if (isTaken) {  
			player.RemoveAllFood ();
			// TODO 음식두가지 이상 들고있었을땐 하나만 내려놓고 배열에선 지워져버리는듯함
			Say ("음식 놓여짐");
		} else
			Say ("놓여지지 않음");
	}
}

