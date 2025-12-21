using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

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
                return; // UI 已拦截
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
            UI_ShopManager.instance.ShowInfo("没有足够的金币来刷新");
            return;
		}
		BagManager.instance.coin -= reloadCharacterCost;
        UI_ShopManager.instance.UpdateCoinText();
        foreach (var character in shopCharacter)
			Destroy(character.gameObject);
		shopCharacter.Clear();
        UI_ShopManager.instance.ShowInfo("刷新商店成功");
        shopCharacterInit();
	}

	public void shopCharacterInit()
	{
		int sellCount = 6;
		int depth = GameManager.instance.playerDepth;
        List<Character> possibleCharacters = GameManager.instance.shopCharacters;
		int characterCount = possibleCharacters.Count;
        int[] weight = new int [characterCount];
		for (int j = 0; j < sellCount; j++) {
			int totalWeight = 0;
			for (int i = 0; i < characterCount; i++) {
				bool alreadyInShop = false;
				foreach (var character in shopCharacter) {
					if (character.characterName == possibleCharacters[i].characterName) {
						alreadyInShop = true;
					}
                }
				if (alreadyInShop) {
					weight[i] = 0;
					continue;
				}
                weight[i] = (100 - possibleCharacters[i].price) * 50 + (depth - 1) * 100;
				int level = BagManager.instance.FindCharacterLevel(possibleCharacters[i].characterName) + 1;
				if (depth >= 8) {
					weight[i] /= level;
				}
				else if (depth >= 5) {
					weight[i] /= level * level;
				}
				else if (depth >= 2) {
					weight[i] /= level * level * level;
				}
				else {
					if (level >= 2)
						weight[i] = 0;
				}
				if (level == 4)
					weight[i] = 0;
				totalWeight += weight[i];
            }
			if (totalWeight == 0) break;
			int r = Random.Range(0, totalWeight);
			for (int i = 0; i < characterCount; i++) {
				r -= weight[i];
				if (r < 0) {
					Character newCharacter = CharacterCreater.instance.CreateCharacter(possibleCharacters[i].characterName);
					int level = BagManager.instance.FindCharacterLevel(newCharacter.characterName) + 1;
					if (level >= 2) newCharacter.LevelUp();
					if (level >= 3) newCharacter.LevelUp();
					Debug.Assert(level <= 3, "Character level in shop should not exceed 3");
                    shopCharacter.Add(newCharacter);
					Debug.Log($"Add character {newCharacter.characterName} with level {level}");
					break;
				}
            }
        }
        UI_ShopManager.instance.UpdateSlotUI();
    }
	public void ShopInit()
	{
		reloadCharacterCost = 5;
		shopCharacterInit();
        UI_ShopManager.instance.UpdateCoinText();
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
		if (selectedCharacter == null)
			return;
		if(BagManager.instance.coin < selectedCharacter.price)
		{
			Debug.Log("Not enough coin!");
			UI_ShopManager.instance.ShowInfo("没有足够的金币来购买");
			return;
		} 
		else
		{
			BagManager.instance.coin -= selectedCharacter.price;
			UI_ShopManager.instance.UpdateCoinText();
			if (selectedCharacter.level == 1) {
				BagManager.instance.AddMember(selectedCharacter);
				GameManager.instance.characterCount += 1;
				UI_ShopManager.instance.ShowInfo($"购买 {selectedCharacter.characterName} 成功");
				shopCharacter.Remove(selectedCharacter);
				string newName = selectedCharacter.characterName;
				if (GameManager.instance.characterAppear.ContainsKey(newName) == false) {
					GameManager.instance.characterAppear[newName] = 1;
					if (GameManager.instance.characterAppear.Count >= 14) {
						AchievementManager.instance.Achieve(6);
					}
				}
            }
			else {
				List<Character> bagCharacters = BagManager.instance.members;
				foreach (var character in bagCharacters) {
					if (character.characterName == selectedCharacter.characterName) {
						Debug.Assert(character.level == selectedCharacter.level - 1, "Character levels do not match for upgrade");
						character.LevelUp();
					}
				}
				GameManager.instance.levelUpCount += 1;
				UI_ShopManager.instance.ShowInfo($"升级 {selectedCharacter.characterName} 成功");
				shopCharacter.Remove(selectedCharacter);
				Destroy(selectedCharacter.gameObject);
			}
            selectedCharacter = null;
            UI_StatsPanel.instance.Clear();
            UI_ShopManager.instance.UpdateSlotUI();
        }
	}

	public void ExitShop() {
		GameManager.instance.NextLevel();
	}
}
