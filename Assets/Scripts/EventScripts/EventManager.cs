using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {
    public static EventManager instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
	}
	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	public TextMeshProUGUI eventText;

    public async void GenerateEvent() {
        eventText.text = await EventGenerator.instance.GetEvent();
		ReplyGenerator.instance.InitEvent(eventText.text);
	}

    public void ExitEvent() {
        GameManager.instance.NextLevel();
    }

    public void FromEventToBattle() {
        GameManager.instance.FromEventToBattle();
    }

    public TMP_InputField userInputField;
    public TextMeshProUGUI LLMReturnText;
    public Button nextLevelButton;
    public Button endInputButton;

    public async void HandleInputCompletion()
	{
        LLMReturnText.text = "请等待……";
		EventReply eventReply = await ReplyGenerator.instance.GenerateReply(userInputField.text);
        LLMReturnText.text = eventReply.narrative;
		endInputButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(true);
        nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "下一关";
        nextLevelButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.AddListener(ExitEvent);
        Debug.Log("Update the button");
        HandleEventOutcome(eventReply.outcome);
    }

    public void HandleEventOutcome(EventOutcome outcome) {
        GameManager.instance.eventCount++;
        Character selected = null;
        int bagNumber = BagManager.instance.members.Count;
        if (bagNumber > 0)
            selected = BagManager.instance.members[Random.Range(0, bagNumber)];
        if (outcome.gold > 0) {
            LLMReturnText.text += $"\n获得了 {outcome.gold} 金币！";
            BagManager.instance.coin += outcome.gold;
            GameManager.instance.coinCount += outcome.gold;
        }
        if (outcome.gold < 0) {
            LLMReturnText.text += $"\n失去了 {-outcome.gold} 金币……";
            BagManager.instance.coin += outcome.gold;
            if (BagManager.instance.coin < 0) {
                BagManager.instance.coin = 0;
                LLMReturnText.text += "（金币最多减少到0）";
            }
        }
        if (outcome.hp > 0) {
            if (selected == null) {
                LLMReturnText.text += $"\n……但是你的队伍空空如也，所以什么都没有发生。";
            }
            else {
                LLMReturnText.text += $"\n{selected.characterName} 回复了 {outcome.hp} 生命值！";
                selected.currentHealth += outcome.hp;
                if (selected.currentHealth > selected.maxHealth) {
                    selected.currentHealth = selected.maxHealth;
                    LLMReturnText.text += "（生命值不能超过上限）";
                }
            }
        }
        if (outcome.hp < 0) {
            if (selected == null) {
                LLMReturnText.text += $"\n……但是你的队伍空空如也，所以什么都没有发生。";
            }
            else {
                LLMReturnText.text += $"\n{selected.characterName} 失去了 {-outcome.hp} 生命值……";
                selected.currentHealth += outcome.hp;
                if (selected.currentHealth < 1) {
                    selected.currentHealth = 1;
                    LLMReturnText.text += "（生命值至多减少到1）";
                }
            }
        }
        if (outcome.attack > 0) {
            if (selected == null) {
                LLMReturnText.text += $"\n……但是你的队伍空空如也，所以什么都没有发生。";
            }
            else {
                LLMReturnText.text += $"\n{selected.characterName} 提高了 {outcome.attack} 攻击力！";
            }
        }
        if (outcome.attack < 0) {
            if (selected == null) {
                LLMReturnText.text += $"\n……但是你的队伍空空如也，所以什么都没有发生。";
            }
            else {
                LLMReturnText.text += $"\n{selected.characterName} 失去了 {-outcome.attack} 攻击力……";
                selected.attack += outcome.attack;
                if (selected.attack < 1) {
                    selected.attack = 1;
                    LLMReturnText.text += "（攻击力至多减少到1）";
                }
            }
        }
        if (outcome.level == 1) {
            if (selected == null) {
                LLMReturnText.text += $"\n……但是你的队伍空空如也，所以什么都没有发生。";
            }
            else {
                if (selected.level < 3) {
                    LLMReturnText.text += $"\n{selected.characterName} 的等级提升了！";
                    GameManager.instance.levelUpCount += 1;
                    selected.LevelUp();
                }
                else {
                    LLMReturnText.text += $"\n{selected.characterName} 已经达到满级，无法再提升了……";
                }
            }
        }
        if (outcome.events == "battle") {
            nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "进入战斗";
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(GameManager.instance.FromEventToBattle);
        }
    }
}
