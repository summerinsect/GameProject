using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleSceneManager : MonoBehaviour {
    public static UI_BattleSceneManager instance;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public Button startButton;
    public Button endButton;

    public void Start() {
        if (startButton != null) {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StageInputHandler.instance.StartBattle);
        }
        if (endButton != null) {
            endButton.onClick.RemoveAllListeners();
            endButton.onClick.AddListener(StageInputHandler.instance.EndBattle);
        }
    }

    public void ChangeButton() {
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);
    }
}
