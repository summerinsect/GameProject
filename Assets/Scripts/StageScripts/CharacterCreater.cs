using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
public class CharacterCreater : MonoBehaviour
{
	public static CharacterCreater instance { get; private set; }
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	private Dictionary<string, Character> prefabDict = new Dictionary<string, Character>();

	public Character CreateCharacter(string characterName)
	{
		if (prefabDict.TryGetValue(characterName, out Character character))
		{
			Character newCharacter = Instantiate(character);
			newCharacter.gameObject.SetActive(true);
			return character;
		}
		else
		{
			Debug.LogError($"Character {characterName} not found!");
			return null;
		}
	}
}