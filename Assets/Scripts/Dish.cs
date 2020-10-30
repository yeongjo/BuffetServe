using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SelectionBase]
public class Dish : MyBehaviour, IUse
{
	List<Food> foods = new List<Food>();
	public List<Food> Foods{
		get{ return foods; }
		set{ foods = value; }
	}

	public Transform socket{
		get{ return transform; }
	}

	bool _isClean = true;
	public bool isClean{
		get{ return _isClean; }
		set{

			_isClean = value; 
			if(_isClean)
				_renderer.material = MaterialM.self.dishCleanMat;
			else
				_renderer.material = MaterialM.self.dishDirtyMat;
		}
	}

	public bool isDrop;

	Renderer _renderer;

	// Use this for initialization
	void Start ()
	{
		_renderer = GetComponentInChildren<Renderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Drop(){
		isClean = false;
		isDrop = true;
	}

	// Change guest hungryAmount
	// Can be minus
	public void EatFoodThisGuest(Guest guest){
		float eatAmount = 0;
		foreach (var item in foods) {
			if (item.type != Food.TypeOfFood.Error) {
				eatAmount += item.GiveFood (1000);
			}
			else {
				guest.isAngry = true;
				guest.Say ("이게 음식이야? ㅅㅂ");
			}
		}
		guest.hungryAmount -= eatAmount;
	}

	// Eat food on this dish
	public bool Use(Person person){
		if (isDrop) {
			person.TakeDish (this);
			isDrop = false;
			Destroy (GetComponent<Rigidbody>());
			return true;
		}
		return false;
	}
}

