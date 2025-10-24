using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return; // UI ÒÑÀ¹½Ø
			HandleOtherClick();
        }
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
		BagManager.instance.coin -= reloadCharacterCost;
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
		UI_ShopManager.instance.UpdateSlotUI();
	}
	public void ShopInit()
	{
		reloadCharacterCost = 2;
		shopCharacterInit();
	}

	public Character selectedCharacter;

	public void HandleBagSlotClick(Character _character) {
		selectedCharacter = null;
		if (_character == null)
			UI_StatsPanel.instance.Clear();
		else
			UI_StatsPanel.instance.ShowStats(_character);
	}

	public void HandleShopSlotClick(Character _character) {
		if (selectedCharacter == _character) {
			selectedCharacter = null;
			UI_StatsPanel.instance.Clear();
		}
		else {
			selectedCharacter = _character;
			UI_StatsPanel.instance.ShowStats(selectedCharacter);
		}
    }

	public void HandleOtherClick() {
		selectedCharacter = null;
		UI_StatsPanel.instance.Clear();
	}

	public void BuyCharacter()
	{
		if(BagManager.instance.coin < selectedCharacter.price)
		{
			Debug.Log("Not enough coin!");
			// TODO: show info
			return;
		} 
		else
		{
			BagManager.instance.coin -= selectedCharacter.price;
			BagManager.instance.AddMember(selectedCharacter);
			shopCharacter.Remove(selectedCharacter);
			selectedCharacter = null;
			UI_StatsPanel.instance.Clear();
			UI_ShopManager.instance.UpdateSlotUI();
		}
	}

	public void ExitShop() {
		GameManager.instance.NextLevel();
	}
}
