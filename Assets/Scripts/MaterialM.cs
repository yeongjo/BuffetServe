using UnityEngine;
using System.Collections;

public class MaterialM : MyBehaviour
{
	public Material boundMat;
	public Material floorMat;
	public Material noMat;
	public Material onCursorMat;

	public Material dishDirtyMat;
	public Material dishCleanMat;

	public static MaterialM self;

	void Awake(){
		self = this;
	}
}

