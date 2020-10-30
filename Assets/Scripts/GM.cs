using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GM : MyBehaviour
{
	public static bool isGameEnd = false;

	private int _money;
	public int money{
		get{ return _money; }
		// TODO add animation
		set{ _money = value;
			UpdateMoney ();}
	}

	public static float foodMax;

	public Text moneyText;

	public static Player localPlayer;

	public static GM self;

	void Awake(){
		self = this;
		UpdateMoney ();
	}

	void UpdateMoney(){
		if (moneyText)
			moneyText.text = _money.ToString ();
	}
}

