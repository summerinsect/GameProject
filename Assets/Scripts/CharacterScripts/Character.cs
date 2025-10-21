using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour // Base class for all characters in the game
{
	[Header("Attributes")]
	public int level;
	public int maxHealth;
	public int attackRange;
	public int attackDamage;
	public int speed;

	public int health;

	public string characterName;
	public string uid;
	public int teamId;
	public Vector3Int position;

	public CharacterBattleAnimator characterBattleAnimator;
	public UI_HealthBar healthBarUI;

    public bool isAlive => health > 0;

	protected virtual void Awake()
	{
		characterBattleAnimator = GetComponent<CharacterBattleAnimator>();
		if (characterBattleAnimator == null)
			characterBattleAnimator = gameObject.AddComponent<CharacterBattleAnimator>();
		healthBarUI = GetComponentInChildren<UI_HealthBar>();
        health = maxHealth;
    }

	public virtual void IsDamagedBy(int damage)
	{
		health -= damage;
		characterBattleAnimator.PlayDamageEffect();
		healthBarUI.UpdateHealthUI();
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

}