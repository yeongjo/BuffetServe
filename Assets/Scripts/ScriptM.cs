using UnityEngine;
using System.Collections;

public class ScriptM : MyBehaviour
{
	public static string guestHere = "손님왔어용!";
	public static string thankYou = "고마워요!";
	public static string eatting = "잘먹겠습니다!";
	public static string doneEatting = "다먹었정!";
	public static string byebye = "수고하세용~";

	public static ScriptM self;

	void Awake(){self = this;}
}

