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

	public int levelId;

	public void StageInit()
	{
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateBattleCharacter("A", 1, new Vector3Int(2, 3, -5)));
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateBattleCharacter("B", 1, new Vector3Int(3, 1, -4)));
        BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateBattleCharacter("C", 1, new Vector3Int(4, -1, -3)));
        BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateBattleCharacter("D", 1, new Vector3Int(5, -3, -2)));

    }

}
