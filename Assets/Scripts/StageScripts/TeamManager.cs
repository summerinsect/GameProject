using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeamManager
{
    public List<Character> members = new List<Character>();
	public int memberCount => members.Count;

    public void BattleInit()
    {

    }

    public void AddMember(Character character)
    {
        members.Add(character);
		character.TeleportToPosition();
	}

	public int MemberCount()
	{
		return members.Count;
	}	

	public List<Character> GetAliveMembers()
    {
		List<Character> characters = new List<Character>();
		foreach (Character character in members)
			if(character.isAlive)
				characters.Add(character);
		return characters;
	}

    public Character GetMember(int index)
    {
        return members[index];
	}
	public bool AllDead()
	{
		foreach (Character character in members)
			if (character.isAlive)
				return false;
		return true;
	}
}
