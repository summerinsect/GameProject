using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	public static BattleManager instance { get; private set; }
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

	public TeamManager[] team = new TeamManager[2]; // team[0]: player, team[1]: enemy
	private int turn;
	public int[] current = new int[2];

	private bool isMoving;
	private bool isAttacking;
	public float waitMoveTime;
	public float waitAttackTime; 
	public float waitTimer;

	private Character currentCharacter => team[turn].GetMember(current[turn]);


	public void BattleInit()
	{
		team[0] = new TeamManager();
		team[1] = new TeamManager();
		current[0] = 0;
		current[1] = 0;
		waitMoveTime = 0.15f;
		waitAttackTime = 0.5f;
	}

	public void AddMember(int teamId, Character character)
	{
		Debug.Log($"Add member {character.uid} to team {teamId}");
		team[teamId].AddMember(character);
	}

	public List<Character> GetTeamMember(int teamId)
	{
		return team[teamId].GetTeamMember();
	}

	public Character FindCharacter(string uid)
	{
		for (int i = 0; i <= 1; i++)
			foreach (var member in team[i].members)
				if (member.uid == uid)
					return member;

		Debug.LogError($"Character with UID {uid} not found!");
		return null;
	}

	public void DamageCharacter(string uid, int damage)
	{
		Character target = FindCharacter(uid);
		target.IsDamagedBy(damage);
	}

	void WaitSomeTime(float t)
	{
		waitTimer = t;
	}
	bool StillWaiting()
	{
		return waitTimer > 0f;
	}
	void DealTimer()
	{
		waitTimer -= Time.deltaTime;
	}

	public bool Battle() // bool -> is finished
	{
		DealTimer();
		if (isMoving)
		{
			if(currentCharacter.IsMovementComplete())
			{
				isMoving = false;
				turn ^= 1;
				WaitSomeTime(waitMoveTime);
			}
		}
		else if (isAttacking)
		{
			isAttacking = false;
			turn ^= 1;
			WaitSomeTime(waitAttackTime);
		}
		else
		{
			if(StillWaiting())
			{
				return false;
			}
			current[turn] = team[turn].GetNextAlive(current[turn]);
			if(current[turn] == -1)
			{
				Debug.Log($"Team {turn ^ 1} wins!");
				return true;
			}

			int state = currentCharacter.SingleRound();
			if (state == 0)
			{
				isMoving = true;
				currentCharacter.InitMove();
			}
			else if (state == 1)
			{
				isAttacking = true;
			}
			else
			{
				Debug.LogError("Invalid State!");
			}
		}
		return false;
	}

	public int GetWinner()
	{
		if (team[0].GetNextAlive(0) == -1)
			return 1;
		else if(team[1].GetNextAlive(0) == -1)
			return 0;
		else
		{
			Debug.LogError("Battle not finished yet!");
			return -1;
		}
	}
}