using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	public static BattleManager instance { get; private set; }

	public TeamManager[] team = new TeamManager[2];
	private int turn;
	private int[] current = new int[2];

	public float moveTurnTime = 0.5f;
	private float moveTimer;
	private bool isMoving;

	private bool isAttacking;

	private Character currentCharacter => team[turn].GetMember(current[turn]);

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

	private void Init()
	{

	}


	public List<Character> GetTeamMember(int teamId)
	{
		return team[teamId].GetTeamMember();
	}

	private int GetNextAlive(int cur, int teamId)
	{
		int count = team[teamId].memberCount;
		int next = (cur + 1) % count;
		while (!team[teamId].GetMember(next).isAlive)
		{
			next = (next + 1) % team[teamId].memberCount;
			if (next == cur)
			{
				return -1; // All dead
			}
		}
		return next;
	}

	public bool Battle() // bool: is finished
	{
		if (isMoving)
		{
			moveTimer -= Time.deltaTime;
			if (moveTimer <= 0)
			{
				isMoving = false;
			}
			else
			{
				currentCharacter.TryMove();
			}
		}
		else if (isAttacking)
		{

		}
		else
		{
			current[turn] = GetNextAlive(current[turn], turn);
			if(current[turn] == -1)
			{
				Debug.Log($"Team {turn ^ 1} wins!");
				return true;
			}

			int state = currentCharacter.SingleRound();
			if (state == 0)
			{
				isMoving = true;
				moveTimer = moveTurnTime;
			}
			else if (state == 1)
			{
				isAttacking = true;
			}
			else
			{
				Debug.LogError("Invalid State!");
			}

			turn ^= 1;
		}
		return false;
	}
}