using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatsPanel : MonoBehaviour {
    public static UI_StatsPanel instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        instance = this;
    }

    [SerializeField] public TextMeshProUGUI characterName;
    [SerializeField] public Image characterImage;
    [SerializeField] public TextMeshProUGUI health;
    [SerializeField] public TextMeshProUGUI attack;
    [SerializeField] public TextMeshProUGUI attackDistance;
    [SerializeField] public TextMeshProUGUI attackRange;
    [SerializeField] public TextMeshProUGUI speed;
    [SerializeField] public TextMeshProUGUI skillDescription;

    private void Start() {
        Clear();
    }

    public void ShowStats(Character character) {
        characterName.text = character.characterName;
        characterImage.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characterImage.color = character.GetComponent<SpriteRenderer>().color;
        health.text = "������" + character.health.ToString() + " / " + character.maxHealth.ToString();
        attack.text = "��������" + character.attack.ToString();
        attackDistance.text = "�������룺" + character.attackDistance.ToString();
        attackRange.text = "������Χ��" + character.attackRange.ToString();
        speed.text = "�ٶȣ�" + character.speed.ToString();
        skillDescription.text = "���ܣ�" + character.skillDescription;
    }

    public void Clear() {
        characterName.text = "�����ɫ�鿴��Ϣ";
        characterImage.sprite = null;
        characterImage.color = Color.clear;
        health.text = "������";
        attack.text = "��������";
        attackDistance.text = "�������룺";
        attackRange.text = "������Χ��";
        speed.text = "�ٶȣ�";
        skillDescription.text = "���ܣ�";
    }
}
