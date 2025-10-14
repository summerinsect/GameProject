using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
	public static BagManager instance { get; private set; }
	private void Awake() // appear in all scenes
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public int coin;
	public List<Character> members = new List<Character>();

}
