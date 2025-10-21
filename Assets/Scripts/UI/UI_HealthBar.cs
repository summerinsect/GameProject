using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    public Character character;
    public Slider slider;

    private void Awake() {
        character = GetComponentInParent<Character>();
        slider = GetComponentInChildren<Slider>();
    }

    public void UpdateHealthUI() {
        slider.maxValue = character.maxHealth;
        slider.value = character.health;
    }
}
