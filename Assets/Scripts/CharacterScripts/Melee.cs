using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Melee : Character // A melee character that can attack and move
{

	protected override int ProcessSingleRound() 
	{
		string targetId = FindTarget();
		if (targetId != null)
		{
			//Debug.Log($"{uid} found target {targetId} in range and prepares to attack, distance: {GridManager.instance.Distance(position, BattleManager.instance.FindCharacter(targetId).position)}");
			//Debug.Log($"position: {position}, target position: {BattleManager.instance.FindCharacter(targetId).position}");
			Attack(targetId);
			return 1;
		}
		else
		{
			Move();
			return 0;
		}
	}

	protected string FindTarget()
	{
		List<Character> targets = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);

		foreach (Character target in targets)
			if(GridManager.instance.Distance(position, target.position) <= attackRange)
				return target.uid;


		return null;
	}

	public void Attack(string targetId)
	{
		Debug.Log($"{uid} attacks {targetId} for {attackDamage} damage.");
		BattleManager.instance.DamageCharacter(targetId, DamageCalculator.instance.CalculateDamage(uid, targetId));
	}
	public void Move()
	{
		List<Character> targets = BattleManager.instance.GetAliveTeamMember(teamId ^ 1);
		List<Vector3Int> validPositions = GridManager.instance.ValidPositions(position);

		int minDist = int.MaxValue;
		foreach (var pos in validPositions)
			minDist = Mathf.Min(minDist, GridManager.instance.GetMinDistInTargets(pos, targets));
		foreach (var pos in validPositions)
			if(GridManager.instance.GetMinDistInTargets(pos, targets) == minDist)
			{
				Debug.Log($"{uid} moves from {position} to {pos}.");
				position = pos;
				break;
			}

	}
}
