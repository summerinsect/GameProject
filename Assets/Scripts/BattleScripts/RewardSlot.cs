using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardSlot : MonoBehaviour, IPointerClickHandler {
    public TextMeshProUGUI rewardText;
    public int slotId;
    public int rewardType;
    public int rewardData;
    public bool canBeApplied;
    public void SetReward(int _rewardType, int _rewardData) {
        rewardType = _rewardType;
        rewardData = _rewardData;
        canBeApplied = true;
        switch (rewardType) {
            case 0: 
                rewardText.text = $"获得 {rewardData} 金币";
                Color gold = Color.clear;
                ColorUtility.TryParseHtmlString("#D6A95A", out gold);
                rewardText.color = gold;
                break;
            case 1: 
                rewardText.text = $"全队回复 {rewardData}%\n生命值";
                rewardText.color = Color.green;
                break;
            case 2: 
                rewardText.text = $"全队提升 {rewardData}\n攻击力"; 
                rewardText.color = Color.red;
                break;
            case 3: 
                rewardText.text = $"全队提升 {rewardData}%\n最大生命值"; 
                rewardText.color = Color.green;
                break;
            case 4: 
                rewardText.text = $"全队提升 {rewardData}\n速度"; 
                rewardText.color = Color.yellow;
                break;
            case 5: 
                if (rewardData == 0)
                    rewardText.text = $"将下一关改为\n事件";
                else
                    rewardText.text = $"将下一关改为\n商店";
                rewardText.color = Color.blue;
                break;
            default: 
                Debug.LogError("Invalid reward type"); 
                break;
        }
    }

    public void ApplyReward() {
        switch (rewardType) {
            case 0:
                BagManager.instance.coin += rewardData;
                GameManager.instance.coinCount += rewardData;
                break;
            case 1:
                foreach (var character in BagManager.instance.members) {
                    int addHealth = character.maxHealth * rewardData / 100;
                    character.currentHealth = Mathf.Min(addHealth + character.currentHealth, character.maxHealth);
                }
                break;
            case 2:
                foreach (var character in BagManager.instance.members) {
                    character.attack += rewardData;
                }
                break;
            case 3:
                foreach (var character in BagManager.instance.members) {
                    int addHealth = character.maxHealth * rewardData / 100;
                    character.maxHealth += addHealth;
                    character.currentHealth += addHealth;
                }
                break;
            case 4:
                foreach (var character in BagManager.instance.members) {
                    character.speed += rewardData;
                }
                break;
            case 5:
                if (rewardData == 0)
                    MapManager.instance.ModifySuccessors(MapSlotType.Event);
                else
                    MapManager.instance.ModifySuccessors(MapSlotType.Shop);
                break;
            default: Debug.LogError("Invalid reward type"); break;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (canBeApplied == false)
            return;
        if (eventData.button == PointerEventData.InputButton.Left) {
            ApplyReward();
            RewardManager.instance.HideInvalidReward(slotId);
        }
    }

    public void Hide() {
        if (canBeApplied == false)
            return;
        canBeApplied = false;
        StartCoroutine(FadeOut(1f));
    }

    private IEnumerator FadeOut(float fadeDuration) {
        rewardText.color = new Color(rewardText.color.r, rewardText.color.g, rewardText.color.b, 1f);
        Color currentColor = rewardText.color;
        float targetAlpha = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, elapsedTime / fadeDuration);
            rewardText.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }
        rewardText.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}
