using UnityEngine;

public class Food : MyBehaviour, IUse{
	float _remainFoodAmount;
	public float remainFoodAmount{
		get{ return _remainFoodAmount; }
		set{ _remainFoodAmount = value; 
			ChangeMeshWithAmount ();
		}
	}
	public float perferedLevel;

	MeshFilter meshFilter;
	MeshFilter MeshFilter{get{ 
			if (!meshFilter)
				meshFilter = GetComponent<MeshFilter> ();
			return meshFilter; 
		}}
	MeshRenderer meshRenderer;
	MeshRenderer MeshRenderer{get{
			if (!meshRenderer)
				meshRenderer = GetComponent<MeshRenderer> ();
			return meshRenderer; 
		}
	}


	// Zero will be destory;
	GameObject[] objsByAmount;

	public TypeOfFood type;
	public enum TypeOfFood
	{
		Error = 0,
		First = 1,
		Secend = 2,
		Third = 3
	}

	public Transform socket{
		get{ return transform; }
	}

	void Start(){
		SetMeshes(FoodRecipeM.self.FindMeshFromType(type));
	}

	public bool Use(Person person){
		GiveFood (person.gameObject, 1);
		return false;
	}

	public void Init(TypeOfFood type){
		this.type = type;
		remainFoodAmount = 8;
	}

	void CopyWithoutRemainAmount(Food food){
		//remainFoodAmount = food.remainFoodAmount;
		type = food.type;
		perferedLevel = food.perferedLevel;
	}

	// Take some food form FoodTable food
	// With Object moving
	public Food GiveFood(GameObject obj, float takenFoodAmount){
		var ts = obj.GetComponentsInChildren<Food> ();
		Food tFood = null;
		if (ts.Length > 0) {
			foreach (var item in ts) {
				if(item.type == type)
					tFood = item;
			}
		}
		if(!tFood)
			tFood = Instantiate(KitchenTable.self.foodTemplate, obj.transform.position,obj.transform.rotation, obj.transform);
				//obj.AddComponent<Food> ();
		tFood.CopyWithoutRemainAmount(this);
		tFood.SetMeshes(FoodRecipeM.self.FindMeshFromType(type));
		if(remainFoodAmount - takenFoodAmount <= 0){
			float lastFood = remainFoodAmount;
			remainFoodAmount = 0;
			tFood.remainFoodAmount += lastFood;
			Destroy (this.gameObject);
			return tFood;
		}
		remainFoodAmount -= takenFoodAmount;
		tFood.remainFoodAmount += takenFoodAmount;
		return tFood;
	}

	// just Take some food
	// Without object moving
	public float GiveFood(float takenFoodAmount){
		if(remainFoodAmount - takenFoodAmount < 0){
			float lastFood = remainFoodAmount;
			remainFoodAmount = 0;
			Destroy (this.gameObject);
			return (lastFood);
		}
		remainFoodAmount -= takenFoodAmount;
		return remainFoodAmount;
	}

	/// <summary>
	/// If same type then Add food
	/// No more use
	/// </summary>
	public void CombineFood(Food food){
		if (type != food.type)
			return;
		remainFoodAmount += food.remainFoodAmount;
		remainFoodAmount = Mathf.Min (remainFoodAmount, GM.foodMax);
		perferedLevel = food.perferedLevel;


		food.remainFoodAmount = 0;
		food.perferedLevel=0;
	}

	void ChangeMeshWithAmount(){
		// y = 4 * (i / 8.0f)
		Transform childTrans;
		if (remainFoodAmount > 0 && objsByAmount != null && objsByAmount.Length > 0) {
			if (transform.childCount > 0) {
				GameObject t = new GameObject ();
				childTrans = t.transform;
				childTrans.SetParent (transform);
				childTrans.localPosition = Vector3.zero;
			}
			var i = (objsByAmount.Length) * (remainFoodAmount / GM.foodMax);
			var t1 = objsByAmount [Mathf.FloorToInt (i)];
			var copyFilter = t1.GetComponent<MeshFilter> ();
			var copyRenderer = t1.GetComponent<MeshRenderer>();
			MeshFilter.mesh = copyFilter.mesh;
			MeshRenderer.materials = copyRenderer.materials;
		}
	}

	public void SetMeshes(GameObject[] objs){
		// TODO sharedMesh diff??
		if (objs == null || objs.Length == 0) {
			Debug.LogError ("Can't find from RecipeM. you need to add recipe first");

			//return;
		}
		objsByAmount = objs;
		ChangeMeshWithAmount ();
		//MeshFilter.sharedMesh = objs[objs.Length-1];
	}
}