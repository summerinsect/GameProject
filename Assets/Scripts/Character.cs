using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
	[Header("ÊôÐÔ")]
	public string characterName;
	public int health = 100;
	public int attackDamage = 10;
	public int attackRange = 2;

	public string uid;
	public int teamId;
	public Vector3Int position;


	public bool isAlive => health > 0;

	protected virtual void Init()
	{

	}

	public virtual int SingleRound() // 
	{
		Debug.Assert(isAlive, $"{characterName} is dead and cannot battle.");

		string targetId = FindTarget();
		if (targetId != null)
		{
			Attack(targetId);
			return 1;
		}
		else
		{
			Move();
			return 0;
		}
	}

	protected virtual string FindTarget()
	{
		List<Character> targets = BattleManager.instance.GetTeamMember(teamId ^ 1);

		return targets[0].uid;
	}

	public virtual void Attack(string targetId)
	{

	}

	public virtual void Move()
	{

	}

	public void TryMove() // Animation
	{

	}

}