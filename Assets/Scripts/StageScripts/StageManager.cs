using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour // Manages the stage setup and battle flow
{
	public static StageManager instance { get; private set; }
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
	}
	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	private bool isStarted;
	private bool isFinished;

	public void StageInit() // init the stage
	{
		isStarted = false;
		isFinished = false;
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateCharacter("melee", 1, new Vector3Int(1, 1, -2)));
		BattleManager.instance.AddMember(0, CharacterCreater.instance.CreateCharacter("melee", 0, new Vector3Int(-1, -1, 2)));
	}
	public void StartBattle() // start the battle
	{
		isStarted = true;
	}

	void Update()
    {
        if(isStarted && !isFinished)
        {
            isFinished = BattleManager.instance.Battle();
			if(isFinished)
				Debug.Log($"Battle Finished! {BattleManager.instance.GetWinner()} wins!");

		}
		else if(isFinished)
		{

		}

    }
}
