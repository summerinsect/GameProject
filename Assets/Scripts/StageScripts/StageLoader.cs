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
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateBattleCharacter("melee", 1, new Vector3Int(0, 0, 0)));
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateBattleCharacter("melee", 1, new Vector3Int(0, 1, -1)));
    }

}
