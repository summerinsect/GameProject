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

	// 动画组件引用
	protected CharacterAnimator characterAnimator;

	public bool isAlive => health > 0;

	protected virtual void Awake()
	{
		characterAnimator = GetComponent<CharacterAnimator>();
		if (characterAnimator == null)
			characterAnimator = gameObject.AddComponent<CharacterAnimator>();
	}

	public virtual void IsDamagedBy(int damage)
	{
		health -= damage;
		characterAnimator.PlayDamageEffect();
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
		characterAnimator.StartMoveTo(targetWorldPos);
	}


	public void TeleportTo(Vector3 pos)
	{
		characterAnimator.TeleportTo(pos);
	}

	public void TeleportToPosition()
	{
		TeleportTo(GridManager.instance.ComputeOffset(position));
	}
	public bool IsMovementComplete()
	{
		return !characterAnimator.IsMoving;
	}
}