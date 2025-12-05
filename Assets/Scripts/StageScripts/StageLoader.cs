using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
	public static StageLoader instance { get; private set; }
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

	public void StageInit() {
		int playerDepth = GameManager.instance.playerDepth;
		string levelId;
		if (playerDepth <= 4) {
			levelId = "EasyLevel/Easy_0" + Random.Range(1, 7).ToString();
		}
		else if (playerDepth <= 7) {
            levelId = "MediumLevel/Medium_0" + Random.Range(1, 8).ToString();
        }
		else if (playerDepth == 8) {
            levelId = "SmallBoss";
        }
		else if (playerDepth <= 11) {
            levelId = "HardLevel/Hard_0" + Random.Range(1, 6).ToString();
        }
		else {
			// playerDepth = 12
			levelId = "BigBoss";
		}
		Debug.Log(levelId);
		TextAsset jsonAsset = Resources.Load<TextAsset>("Levels/" + levelId);
        string jsonText = jsonAsset.text;
        LevelConfig data = JsonUtility.FromJson<LevelConfig>(jsonText);
        StageManager.instance.maxCharacterCount = data.maxCharacterCount;
		Debug.Log(data.enemyList);
		foreach (var enemy in data.enemyList) {
			BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateBattleCharacter(enemy.name, 1, new Vector3Int(enemy.xPos, enemy.yPos, enemy.zPos)));
		}
    }

}
