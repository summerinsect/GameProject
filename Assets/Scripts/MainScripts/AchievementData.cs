using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementData : MonoBehaviour {
    public static AchievementData instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public Sprite[] images = new Sprite[14];
    [TextArea(1, 3)]
    public string[] achievements = new string[14];
}
