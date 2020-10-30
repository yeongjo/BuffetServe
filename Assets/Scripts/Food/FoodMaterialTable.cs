using UnityEngine;
using System.Collections;

public class FoodMaterialTable : MyBehaviour, IUse
{
	public Food foodTemplate;
	public Transform exitSocket;

	public Food.TypeOfFood makeType;

	Food outFood;

	public Transform socket{ get { return transform; } }

	public bool Use(Person person){
		// Make food and out
		if (person == null)
			return false;
		if (outFood == null)
			MakeFood ();
		else
			TakeFood (person);
		return true;
	}

	void MakeFood(){
		NoticeM.UseNotice ("음식나옴", transform.position);
		var t = Instantiate (foodTemplate);
		t.remainFoodAmount = 1;
		t.type = makeType;
		outFood = t;
		t.transform.position = exitSocket.position;
	}

	void TakeFood(Person person){
		NoticeM.UseNotice ("음식가져감", transform.position);
		person.TakeFood (outFood);
		outFood = null;
	}
}

