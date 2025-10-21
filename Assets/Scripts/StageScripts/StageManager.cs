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

	public bool isStarted;
    public bool isFinished;

	public void StageInit() // init the stage
	{
		isStarted = false;
		isFinished = false;
	}
	public void StartBattle() // start the battle
	{
		isStarted = true;
	}

	public void FinishBattle() {
		foreach (var character in BattleManager.instance.GetTeamMember(0)) {
			if (character.isAlive)
				BagManager.instance.AddMember(character);
			else
				Destroy(character.gameObject);
            //character.gameObject.SetActive(false);
        }
		foreach (var character in BattleManager.instance.GetTeamMember(1)) {
			Destroy(character.gameObject);
		}

		GridManager.instance.CleanUp();
		UI_BagManager.instance.CleanUp();
		UI_BattleSceneManager.instance.ChangeButton();
    }

	void Update()
    {
        if (isStarted && !isFinished) {
            isFinished = BattleManager.instance.Battle();
			if (isFinished) {
				int winner = BattleManager.instance.GetWinner();
				Debug.Log($"Battle Finished! {winner} wins!");
				FinishBattle();
			}
		}
    }
}
