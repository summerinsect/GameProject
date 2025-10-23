using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
	public static DamageCalculator instance { get; private set; }
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public int CalculateDamage(string attackerId, string defenderId)
	{
		return BattleManager.instance.FindCharacter(attackerId).attack;
	}
}
