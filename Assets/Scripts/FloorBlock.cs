using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBlock : MyBehaviour {
	BoundWall[] boundWall = new BoundWall[4];
	FloorKind floorKind;
	FloorColor _floorColor;
	public FloorColor floorColor{
		get{ return _floorColor;}
		set{if (ren == null)
				return;
			_floorColor = value;
			if(_floorColor == FloorColor.Edit)
				ren.material = MaterialM.self.boundMat;
			else if(_floorColor == FloorColor.OnCursor)
				ren.material = MaterialM.self.onCursorMat;
			else if(_floorColor == FloorColor.Normal)
				ren.material = MaterialM.self.noMat;
		}
	}

	MeshRenderer ren;

	void Start(){
		// Init
		//for (int i = 0; i < boundWall.Length; ++i)
		//	boundWall [i] = new BoundWall ();
		ren = GetComponent<MeshRenderer> ();
	}

	public enum FloorKind{
		Normal,
		ForCustomer,
		ForEmployee
	}
}
