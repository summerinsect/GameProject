using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
	public static BagManager instance { get; private set; }
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

	public int coin;
	public List<Character> members = new List<Character>();

	public void Start()
	{

	}

	private void Update()
	{
		
	}
}
