using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour // Manages scene transitions and initialization
{
	public static GameScene instance { get; private set; }

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	[Header("≥°æ∞√˚≥∆≈‰÷√")]
	public string battleSceneName = "BattleScene";
	public string mainSceneName = "MainScene";

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == mainSceneName)
			InitializeMainScene();
		if (scene.name == battleSceneName)
			InitializeBattleScene();
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}


	// Battle Scene
	public void LoadBattleScene()
	{
		LoadScene(battleSceneName);
	}
	private void InitializeBattleScene()
	{
		Debug.Log("Initialize Battle Scene");
		BattleManager.instance.BattleInit();
		StageManager.instance.StageInit();
		GridManager.instance.GridInit();
		StageLoader.instance.StageInit();
	}

	// Main Scene
	public void LoadMainScene()
	{
		LoadScene(mainSceneName);
	}
	private void InitializeMainScene()
	{
		Debug.Log("Initialize Main Scene");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F6))
			LoadMainScene();

		if (Input.GetKeyDown(KeyCode.F7))
			LoadBattleScene();
	}
}
