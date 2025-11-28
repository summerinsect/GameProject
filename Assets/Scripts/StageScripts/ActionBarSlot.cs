using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ActionBarSlot : MonoBehaviour {
    public int rank;
    public RectTransform rectTransform;
    public Image background;
    public Character character;
    public Image characterImage;
    public TextMeshProUGUI nameText;
    public UI_HealthBarForAction healthBar;

    public void SetCharacter(Character _character) {
        rectTransform = GetComponent<RectTransform>();
        character = _character;
        UpdateSlotUI();
    }

    public void UpdateSlotUI() {
        background.color = Color.green;
        SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
        characterImage.sprite = sr.sprite;
        characterImage.color = sr.color;
        nameText.text = character.characterName;
        healthBar.gameObject.SetActive(true);
        healthBar.UpdateHealthUI(character);
    }

    public void ClearSlotUI() {
        background.color = Color.clear;
        characterImage.sprite = null;
        characterImage.color = Color.clear;
        nameText.text = "";
        healthBar.gameObject.SetActive(false);
    }
}
