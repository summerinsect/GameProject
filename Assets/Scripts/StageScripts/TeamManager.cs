using System.Collections;
using System.Collections.Generic;
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


	public List<Character> GetTeamMember()
    {
        return members;
	}

    public Character GetMember(int index)
    {
        return members[index];
	}
	public int GetNextAlive(int cur)
	{
		int count = members.Count;

		bool hasAlive = false;
		for (int i = 0; i < count; i++)
			if (members[i].isAlive)
			{
				hasAlive = true;
				break;
			}

		if (!hasAlive)
			return -1;

		int next = (cur + 1) % count;
		while (!members[next].isAlive)
			next = (next + 1) % count;

		return next;
	}
}
