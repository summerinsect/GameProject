using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public List<Character> members = new List<Character>();
	public int memberCount => members.Count;

    public void Init()
    {

    }

    public List<Character> GetTeamMember()
    {
        return members;
	}

    public Character GetMember(int index)
    {
        return members[index];
	}
}
