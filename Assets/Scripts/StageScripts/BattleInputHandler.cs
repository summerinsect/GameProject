using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour // Handles input specific to the battle scene
{
	public static BattleInputHandler instance { get; private set; }
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		instance = this;
	}
	public void BattleInit()
	{

	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			StartBattle();
	}

	private void StartBattle()
	{
		Debug.Log("Battle Start!");
		StageManager.instance.StartBattle();
		// unity is sb
	}

}
