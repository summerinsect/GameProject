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
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateCharacter("melee", 1, new Vector3Int(2, 3, -5)));
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateCharacter("melee", 1, new Vector3Int(1, 3, -4)));
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateCharacter("melee", 1, new Vector3Int(-2, 2, 0)));
		BattleManager.instance.AddMember(1, CharacterCreater.instance.CreateCharacter("melee", 1, new Vector3Int(-1, 2, -1)));
		BattleManager.instance.AddMember(0, CharacterCreater.instance.CreateCharacter("melee", 0, new Vector3Int(-2, -3, 5)));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("melee"));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("YouKnowWho"));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("melee"));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("YouKnowWho"));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("melee"));
    }

}
