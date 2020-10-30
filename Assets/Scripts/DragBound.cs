using UnityEngine;
using System.Collections;

public class DragBound
{
	public Vector2[] inputBound = new Vector2[2];
	public Vector2[] bound = new Vector2[2];


	public void SortBound(){
		Vector2 a = Vector2.Min (inputBound [0], inputBound [1]);
		Vector2 b = Vector2.Max (inputBound [0], inputBound [1]);
		bound [0] = a;
		bound [1] = b;
	}


}

