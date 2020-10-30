using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Food Recipe")]
public class FoodRecipe : ScriptableObject
{
	public Food.TypeOfFood makingType;
	public GameObject[] makingFoodObjByAmount;

	/// <summary>
	/// The request types. If types length is zero then it's can't make by kitchen table
	/// </summary>
	public Food.TypeOfFood[] requestTypes;
}

