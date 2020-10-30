using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class DragSelectM : MyBehaviour
{
	
	DragBound dragBound = new DragBound();
	FloorColorArray prevFloorColor;


	public bool bIsDraging = false;
	public Toggle editToggle;

	public static DragSelectM self;


	void Awake ()
	{
		self = this;
	}


	// Called by FloorBlockM
	public void InitFloorColorArrays(){
		prevFloorColor = new FloorColorArray (FloorBlockM.floorBlockSize.x, FloorBlockM.floorBlockSize.y);
	}

	void Update(){
		if (editToggle.isOn) {
			if (Input.GetMouseButtonDown (0)) {

				RaycastHit hit;
				RaycastUnderCursor (out hit);
				if (hit.collider) {
					CopyFloorColorToBuffer ();
					int i, j;
					FloorBlockM.self.GetFloorIndex (out i, out j, Vec3ToVec2 (hit.transform.position));
					dragBound.inputBound [0] = new Vector2(i,j);
					bIsDraging = true;
				}
			}

			if (Input.GetMouseButtonUp (0)) {
				bIsDraging = false;
				// TODO Prev floor color data Add Drag box data
				DrawDragData();
			}
		}
	}


	// Use for Prev Color buffer With Draw drag box
	void CopyFloorColorToBuffer(){
		int blockSize = Mathf.RoundToInt(FloorBlockM.floorBlockSize.x * FloorBlockM.floorBlockSize.y);
		for (int i = 0; i < blockSize; i++) {
			FloorColor tColor = FloorBlockM.self.GetFloor (i).floorColor;
			if (tColor == FloorColor.OnCursor)
				tColor = FloorColor.Edit;
			prevFloorColor.SetItem(i, tColor);
		}
	}

	void DrawDragData(){
		DrawDragBoundWithControlKey ();
		CopyFloorColorToBuffer ();
	}

	void DrawDragBoundWithControlKey(){
		if (Input.GetKey (KeyCode.LeftControl))
			FloorBlockM.self.SetDragColor (dragBound, FloorColor.Normal);
		else
			FloorBlockM.self.SetDragColor (dragBound, FloorColor.Edit);
	}


	bool isInEditModeOnce = true;
	void FixedUpdate ()
	{
		if (editToggle.isOn) {
			if (!isInEditModeOnce) {
				FloorBlockM.self.SetEditMode (true);
				isInEditModeOnce = true;
			}
			// Clear to prevColor
			FloorBlockM.self.SetArrayFloorColor (prevFloorColor);

			RaycastHit hit;
			RaycastUnderCursor (out hit);
			Vector2 blockPos = new Vector2 (float.PositiveInfinity, 0);
			if (hit.collider)
				blockPos = Vec3ToVec2 (hit.transform.position);

			int i, j;
			// Draw Boundary box
			if (Input.GetMouseButton (0) && bIsDraging) {
				if (hit.collider) {
					FloorBlockM.self.GetFloorIndex (out i, out j, Vec3ToVec2 (hit.transform.position));
					dragBound.inputBound [1] = new Vector2(i,j);
					DrawDragBoundWithControlKey ();
				}
			}

			// Draw Cursor
			if (!float.IsPositiveInfinity (blockPos.x)) {
				FloorBlockM.self.GetFloorIndex (out i, out j, blockPos);
				FloorBlockM.self.AddFloorColor (i, j, FloorColor.OnCursor);
			}
		} else {
			FloorBlockM.self.ClearFloorColor (FloorColor.Normal);
			if (isInEditModeOnce) {
				FloorBlockM.self.SetEditMode (false);
				isInEditModeOnce = false;
			}
		}
	}
	// TODO 드래그하는 동안만 드래그중인 박스 모양 저장하는 배열 따로 만들고
	// 드래그 종료시 진짜 블럭들에 칠하게해야할듯

	public static Vector2 Vec3ToVec2(Vector3 a){
		return new Vector2 (a.x, a.z);
	}
	public static Vector3 Vec2ToVec3(Vector2 a){
		return new Vector3 (a.x, 0, a.y);
	}

	public static void RaycastUnderCursor(out RaycastHit hit){
		// TODO You can clean this with projection function
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Physics.Raycast (ray, out hit, 100, 1<<8);
	}


	void OnDrawGizmos(){
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (Vec2ToVec3(dragBound.inputBound [0]), .1f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (Vec2ToVec3(dragBound.inputBound [1]), .1f);
	}
}

[System.Serializable]
public class FloorColorArray : DoubleArray<FloorColor>{
	public FloorColorArray(int x, int y):base(x,y){
	}

	public FloorColorArray(float x, float y):base(x,y){
	}
}


public class DoubleArray<T> where T : new(){
	
	[SerializeField] T[] doubleArray;
	public int xSize, ySize;
	public int size;

	public DoubleArray(int x, int y){
		Init (x, y);
	}

	public DoubleArray(float _x, float _y){
		int x = Mathf.RoundToInt(_x);
		int y = Mathf.RoundToInt(_y);
		Init (x, y);
	}

	void Init(int x, int y){
		size = x * y;
		doubleArray = new T[size];
		ySize = y;
		xSize = x;
	}


	public void Copy(DoubleArray<FloorColor> obj){
		obj.doubleArray.CopyTo (this.doubleArray, this.size);
	}


	public T GetItem(int x, int y){
		return doubleArray [x + y * xSize];
	}
	public T GetItem(int x){
		return doubleArray [x];
	}
	public T SetItem(int x, int y, T item){
		return doubleArray [x + y * xSize] = item;
	}
	public T SetItem(int x, T item){
		return doubleArray [x] = item;
	}
}
