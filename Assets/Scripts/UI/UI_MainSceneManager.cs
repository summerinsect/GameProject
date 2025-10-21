using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainSceneManager : MonoBehaviour {
    public Button startButton;

    private void Start() {
        if (startButton != null) {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(GameManager.instance.StartGame);
        }
    }
}
