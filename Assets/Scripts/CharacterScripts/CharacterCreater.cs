using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
public class CharacterCreater : MonoBehaviour
{
	public static CharacterCreater instance { get; private set; }
	[SerializeField] private List<Character> characterPrefabs = new List<Character>();
	[SerializeField] private Dictionary<string, Character> prefabDict = new Dictionary<string, Character>();
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);

		foreach (Character prefab in characterPrefabs)
			if (prefab != null && !prefabDict.ContainsKey(prefab.characterName))
			{
				Debug.Log($"Register Character Prefab: {prefab.characterName}");
				prefabDict.Add(prefab.characterName, prefab);
			}
	}

	private void InitCharacter(Character character, int teamId, Vector3Int pos)
	{
		character.uid = System.Guid.NewGuid().ToString();
		character.teamId = teamId;
		character.position = pos;
	}

	public Character CreateCharacter(string characterName, int teamId = 0, Vector3Int pos = new Vector3Int())
	{
		if (prefabDict.TryGetValue(characterName, out Character character))
		{
			Character newCharacter = Instantiate(character, transform);
			InitCharacter(newCharacter, teamId, pos);
			return newCharacter;
		}
		else
		{
			Debug.LogError($"Character {characterName} not found!");
			return null;
		}
	}
}