using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainSceneManager : MonoBehaviour {
    public Button startButton;
    public Button ruleButton;

    public void StartGame() {
        GameManager.instance.StartGame();
    }

    public GameObject rules;

    public void ShowRules() {
        rules.SetActive(true);
    }

    public void HideRules() {
        rules.SetActive(false);
    }
}
