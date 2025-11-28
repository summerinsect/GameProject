using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

public class UI_HealthBarForAction : MonoBehaviour {
    public Character character;
    public GameObject healthBar;
    public GameObject fillHealth;
    public GameObject fillShield;
    public GameObject healthText;

    public void UpdateHealthUI(Character _character) {
        character = _character;
        if (character.teamId == 0)
            fillHealth.GetComponent<Image>().color = Color.green;
        else
            fillHealth.GetComponent<Image>().color = Color.red;
        fillShield.GetComponent<Image>().color = Color.gray;
        float totalLength = healthBar.GetComponent<RectTransform>().rect.width;
        float height = healthBar.GetComponent<RectTransform>().rect.height;
        float healthPercent = 1.0f * character.currentHealth / character.maxHealth;
        float shieldPercent = 1.0f * character.shield / character.maxHealth;
        shieldPercent = Mathf.Min(shieldPercent, 1.0f);
        fillHealth.GetComponent<RectTransform>().sizeDelta = new Vector2(healthPercent * totalLength, 0);
        fillShield.GetComponent<RectTransform>().sizeDelta = new Vector2(shieldPercent * totalLength, 0);
        healthText.GetComponent<TextMeshProUGUI>().text = character.currentHealth.ToString() + "/" + character.maxHealth.ToString();
    }
}
