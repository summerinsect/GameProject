using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
	public static ShopManager instance { get; private set; }
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
		foreach (var character in shopCharacter)
			Destroy(character.gameObject);

		if (instance == this)
			instance = null;
	}

	public int reloadCharacterCost;
	public List<Character> shopCharacter = new List<Character>();

	void AddCharacter(string name)
	{
		shopCharacter.Add(CharacterCreater.instance.CreateCharacter(name));
	}
	public void ReloadCharacter()
	{
		if (BagManager.instance.coin < reloadCharacterCost)
		{
			Debug.Log("Not enough coin to reload shop!");
			// TODO: show info
			return;
		}

		foreach (var character in shopCharacter)
			Destroy(character.gameObject);
		shopCharacter.Clear();

		shopCharacterInit();
	}

	public void shopCharacterInit()
	{
		AddCharacter("melee");
		AddCharacter("melee");
		AddCharacter("melee");
		AddCharacter("melee");
		AddCharacter("melee");
		AddCharacter("melee");
		// TODO: UI update
	}
	public void ShopInit()
	{
		reloadCharacterCost = 2;
		shopCharacterInit();
	}

	public void BuyCharacter(Character c)
	{
		if(BagManager.instance.coin < c.price)
		{
			Debug.Log("Not enough coin!");
			// TODO: show info
			return;
		} 
		else
		{
			BagManager.instance.coin -= c.price;
			BagManager.instance.AddMember(c);
			shopCharacter.Remove(c);
			// TODO: UI update
		}
	}
}
