using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clocker : MonoBehaviour {
    public TextMeshProUGUI timeText;

    private void Awake() {
        timeText = transform.Find("Time").GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        DateTime currentTime = DateTime.Now;
        DateTime startTime = GameManager.instance.startTime;
        int totalSeconds = (int)(currentTime - startTime).TotalSeconds;
        if (totalSeconds % 60 < 10)
            timeText.text = $"{totalSeconds / 60}:0{totalSeconds % 60}";
        else 
            timeText.text = $"{totalSeconds / 60}:{totalSeconds % 60}";
    }
}
