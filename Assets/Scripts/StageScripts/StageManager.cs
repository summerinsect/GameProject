using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

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

	public int maxCharacterCount;

	public Button startButton;
	public ActionBar actionBar;
    public GameObject bossUI;

    public bool isStarted;
    public bool isFinished;

	public void StageInit() // init the stage
	{
		isStarted = false;
		isFinished = false;
        if (GameManager.instance.playerDepth == 11)
            bossUI.gameObject.SetActive(true);
    }
	public void StartBattle() // start the battle
	{
		isStarted = true;
        UI_BattleSceneManager.instance.ChangeUIForStart();
		actionBar.InitActionBar();
        foreach (var character in BattleManager.instance.GetAllTeamMember(0))
			character.ActionsWhenStart();
		foreach (var character in BattleManager.instance.GetAllTeamMember(1))
			character.ActionsWhenStart();
    }

	public void FinishBattle(int winner) {
		foreach (var character in BattleManager.instance.GetAllTeamMember(0)) {
			if (character.isAlive) {
				character.ActionsWhenEnd();
				BagManager.instance.AddMember(character);
			}
			else
				Destroy(character.gameObject);
            //character.gameObject.SetActive(false);
        }
		foreach (var character in BattleManager.instance.GetAllTeamMember(1)) {
			Destroy(character.gameObject);
		}

		UI_BattleSceneManager.instance.ChangeUIForFinish(winner);
		GridManager.instance.CleanUp();
    }

	void Update()
    {
        if (isStarted && !isFinished) {
            isFinished = BattleManager.instance.Battle();
			if (isFinished) {
				int winner = BattleManager.instance.GetWinner();
				Debug.Log($"Battle Finished! {winner} wins!");
				FinishBattle(winner);
			}
		}
    }
}
