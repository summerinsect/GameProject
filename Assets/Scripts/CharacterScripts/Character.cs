using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour // Base class for all characters in the game
{

	private Vector3Int[] directionsArray = new Vector3Int[]{
		new Vector3Int(1, -1, 0),
		new Vector3Int(0, 1, -1),
		new Vector3Int(-1, 0, 1),
		new Vector3Int(-1, 1, 0),
		new Vector3Int(0, -1, 1),
		new Vector3Int(1, 0, -1)
	};

	[Header("Attributes")]
	public int health;

	public string characterName;
	public string uid;
	public int teamId;

	public Vector3Int position;


	public bool isAlive => health > 0;

	protected virtual void BattleInit()
	{

	}

	public virtual void IsDamagedBy(int damage)
	{
		health -= damage;
	}

	public virtual int SingleRound() 
	{
		Debug.Assert(isAlive, $"{characterName} is dead and cannot battle.");
		return ProcessSingleRound();
	}

	protected abstract int ProcessSingleRound();

	public void TryMove() // Animation
	{

	}

}