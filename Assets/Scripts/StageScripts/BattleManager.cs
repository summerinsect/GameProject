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
	
	[Header("Battle Timing")]
	public float waitMoveTime;
	public float waitAttackTime;
	private float waitTimer;

	private Character currentCharacter;


	public void BattleInit()
	{
		team[0] = new TeamManager();
		team[1] = new TeamManager();
		turn = 1; // player first
		current[0] = -1;
		current[1] = -1;
	}

	public void AddMember(int teamId, Character character)
	{
		Debug.Log($"Add member {character.uid} to team {teamId}");
		team[teamId].AddMember(character);
	}

	public List<Character> GetTeamMember(int teamId)
	{
		return team[teamId].GetAliveMembers();
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
	void NextAlive()
	{
		current[turn]++;
		while (current[turn] < team[turn].MemberCount() && !team[turn].GetMember(current[turn]).isAlive)
			current[turn]++;
	}
	bool FindedNextMember()
	{
		NextAlive();
		return current[turn] < team[turn].MemberCount();
	}
	void NextMember()
	{
		turn ^= 1;
		while (!FindedNextMember())
		{
			turn ^= 1;
			if (current[0] >= team[0].MemberCount() && current[1] >= team[1].MemberCount())
			{
				turn = 0; // player first
				current[0] = -1;
				current[1] = -1;
				Debug.Log("New Round Started");
			}
		}
		currentCharacter = team[turn].GetMember(current[turn]);
	}
	public bool Battle() // bool -> is finished
	{
		DealTimer();
		if (isMoving)
		{
			if(currentCharacter.IsMovementComplete())
			{
				isMoving = false;
				WaitSomeTime(waitMoveTime);
			}
		}
		else if (isAttacking)
		{
			isAttacking = false;
			WaitSomeTime(waitAttackTime);
		}
		else
		{
			if(StillWaiting())
			{
				return false;
			}

			if (team[0].AllDead()|| team[1].AllDead())
			{
				return true;
			}

			NextMember();

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
		if (team[0].AllDead())
			return 1;
		else if(team[1].AllDead())
			return 0;
		else
		{
			Debug.LogError("Battle not finished yet!");
			return -1;
		}
	}
}