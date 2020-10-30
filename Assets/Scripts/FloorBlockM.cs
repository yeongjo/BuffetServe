using UnityEngine;
using System.Collections;

public class FloorBlockM : MyBehaviour
{
	DoubleArray<FloorBlock> floorBlocks;
	public static Vector2 floorBlockSize = new Vector2 (10, 20);

	//
	Transform rootFloor;
	public GameObject floorTemplate;

	public static FloorBlockM self;

	void Awake(){
		self = this;
	}

	void Start ()
	{
		rootFloor = transform;

		Init ();
	}

	void Init(){
		floorBlocks = new DoubleArray<FloorBlock> (floorBlockSize.x,floorBlockSize.y);

		for (int i = 0; i < floorBlocks.xSize; i++) {
			for (int j = 0; j < floorBlocks.ySize; j++) {
				var floorObj = Instantiate (floorTemplate);
				floorObj.transform.SetParent (rootFloor);
				floorObj.transform.localPosition = new Vector3 (i, 0, j);
				floorBlocks.SetItem(i,j,floorObj.AddComponent<FloorBlock> ());
			}
		}

		DragSelectM.self.InitFloorColorArrays ();
	}

	void ClearDragColor(){
		for (int i = 0; i < floorBlocks.xSize; i++) {
			for (int j = 0; j < floorBlocks.ySize; j++) {
				floorBlocks.GetItem(i,j).floorColor = FloorColor.Normal;
				// don't do clear all
			}
		}
	}

	public void ClearFloorColor(FloorColor color){
		for (int i = 0; i < floorBlocks.size; i++) {
			floorBlocks.GetItem (i).floorColor = color;
		}
	}

	public void SetEditMode(bool isEnable){
		for (int i = 0; i < floorBlocks.size; i++) {
			floorBlocks.GetItem (i).gameObject.SetActive (isEnable);
		}
	}

	// Use for under cursor
	public void SetFloorColor(int i, int j, FloorColor color){
		ClearFloorColor (FloorColor.Normal);
		AddFloorColor (i, j, color);
	}

	public void AddFloorColor(int i, int j, FloorColor color){
		floorBlocks.GetItem(i,j).floorColor = color;
	}

	public void SetArrayFloorColor(DoubleArray<FloorColor> _prevFloorColor){
		for (int i = 0; i < floorBlocks.size; i++) {
			floorBlocks.GetItem (i).floorColor = _prevFloorColor.GetItem(i);
		}
	}


	// Need to do last time
	public void SetDragColor(DragBound dragBound, FloorColor color){
//		if (color == FloorColor.Edit)
//			ClearDragColor ();
		dragBound.SortBound ();
		float minX = dragBound.bound [0].x, maxX = dragBound.bound [1].x;
		float minY = dragBound.bound [0].y, maxY = dragBound.bound [1].y;
		for (; minX <= maxX; ++minX) {
			for (int y = Mathf.RoundToInt (minY); y <= maxY; ++y) {
				int x = Mathf.RoundToInt (minX);
				floorBlocks.GetItem(x,y).floorColor = color;
			}
		}
	}

	public void GetFloorIndex(out int i, out int j, Vector2 point){
		for (i = 0; i < floorBlocks.xSize; ++i)
			for (j = 0; j < floorBlocks.ySize; ++j)
				if (Vector2.SqrMagnitude(DragSelectM.Vec3ToVec2(floorBlocks.GetItem(i,j).transform.position) - point) < .8f)
					return;
		i = -1; j = -1;
	}

	public void GetFloorIndex(out int i, out int j, FloorBlock floor){
		for (i = 0; i < floorBlocks.xSize; ++i)
			for (j = 0; j < floorBlocks.ySize; j++)
				if (floorBlocks.GetItem(i,j).Equals (floor))
					return;
		i = -1; j = -1;
	}

	public FloorBlock GetFloor(int i){
		return floorBlocks.GetItem(i);
	}
}

public enum FloorColor{
	Normal, Edit, OnCursor
}