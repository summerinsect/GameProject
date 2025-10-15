using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BagManager : MonoBehaviour
{
	public static BagManager instance { get; private set; }
	private void Awake() // appear in all scenes
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public int coin;
	public List<Character> members = new List<Character>();
	

	public void AddMember(Character _character) {
		Debug.Log($"Add member {_character.uid} to bag");
		members.Add(_character);
	}

	public void RemoveMember(Character _character) {
		foreach (Character _char in members) {
			if (_char.uid == _character.uid) {
				members.Remove(_char);
				return;
            }
        }
	}

}
