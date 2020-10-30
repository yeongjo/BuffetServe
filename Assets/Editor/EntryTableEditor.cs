using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntryTable))]
public class EntryTableEditor : Editor{

	EntryTable entry;

	private void OnEnable(){
		entry = (EntryTable)target;
	}

	public override void OnInspectorGUI(){
		//var t = staff.ToArray ();
//		SerializedProperty prop = serializedObject.FindProperty("");
		serializedObject.Update();
		var t = entry.entryWaitingGuestGroup.ToArray ();
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("Waiting GuestGroups");
		EditorGUI.indentLevel++;
		for (int i = 0; i < t.Length; i++) {
			EditorGUILayout.ObjectField("Waiting"+i, t[i].guestGroup, typeof(GuestGroup), true);
		}
		EditorGUI.indentLevel--;
		if(EditorApplication.isPlaying)
			Repaint ();
	}

	public override bool RequiresConstantRepaint()
	{
		return true;
	}
}
