using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour // Base class for all characters in the game
{
	[Header("Attributes")]
	public int health;
	public string characterName;
	public string uid;
	public int teamId;
	public Vector3Int position;

	protected CharacterBattleAnimator characterBattleAnimator;

	public bool isAlive => health > 0;

	protected virtual void Awake()
	{
		characterBattleAnimator = GetComponent<CharacterBattleAnimator>();
		if (characterBattleAnimator == null)
			characterBattleAnimator = gameObject.AddComponent<CharacterBattleAnimator>();
	}

	public virtual void IsDamagedBy(int damage)
	{
		health -= damage;
		characterBattleAnimator.PlayDamageEffect();
	}

	public virtual int SingleRound() 
	{
		Debug.Assert(isAlive, $"{characterName} is dead and cannot battle.");
		return ProcessSingleRound();
	}

	protected abstract int ProcessSingleRound();

	public void InitMove()
	{
		Vector3 targetWorldPos = GridManager.instance.ComputeOffset(position);
		characterBattleAnimator.StartMoveTo(targetWorldPos);
	}

	// Animation related methods
	public void TeleportTo(Vector3 pos)
	{
		characterBattleAnimator.TeleportTo(pos);
	}

	public void TeleportToPosition()
	{
		TeleportTo(GridManager.instance.ComputeOffset(position));
	}
	public bool IsMovementComplete()
	{
		return !characterBattleAnimator.IsMoving;
	}
}