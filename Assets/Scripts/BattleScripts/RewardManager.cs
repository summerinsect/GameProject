using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardManager : MonoBehaviour {
    public static RewardManager instance;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public RewardSlot[] rewardSlots = new RewardSlot[9];
    public int[,] data = new int[6, 3];
    public int[] weight = new int[5];
    public int playerDepth;
    public TextMeshProUGUI coinNumber;

    private void Start() {
        data[0, 0] = 10; data[0, 1] = 15; data[0, 2] = 20; // 金币
        data[1, 0] = 5; data[1, 1] = 10; data[1, 2] = 15; // 回复生命值
        data[2, 0] = 3; data[2, 1] = 6; data[2, 2] = 9; // 提升攻击力
        data[3, 0] = 3; data[3, 1] = 6; data[3, 2] = 9; // 提升最大生命值
        data[4, 0] = 3; data[4, 1] = 6; data[4, 2] = 9; // 提升速度
        data[5, 0] = 0; data[5, 1] = 1; data[5, 2] = 1; // 修改下一关
        playerDepth = GameManager.instance.playerDepth;
        coinNumber.text = BagManager.instance.coin.ToString();
        GenerateRewards();
    }

    private int GetDataIndex(bool special = false) {
        if (special) {
            return Random.Range(0, 2);
        }
        int w0, w1;
        if (playerDepth <= 3) {
            w0 = 65;
            w1 = 95;
        }
        else if (playerDepth <= 6) {
            w0 = 30;
            w1 = 80;
        }
        else {
            w0 = 10;
            w1 = 50;
        }
        int r = Random.Range(0, 100);
        if (r < w0) {
            return 0;
        }
        else if (r < w1) {
            return 1;
        }
        else {
            return 2;
        }
    }

    public void GenerateRewards() {
        weight[0] = 2; weight[1] = 2; weight[2] = 1; weight[3] = 1; weight[4] = 1;
        if (playerDepth == 10)
            weight[4] = 0;
        rewardSlots[0].SetReward(0, data[0, GetDataIndex()]);
        rewardSlots[1].SetReward(0, data[0, GetDataIndex()]);
        rewardSlots[2].SetReward(0, data[0, GetDataIndex()]);
        rewardSlots[3].SetReward(0, data[0, GetDataIndex()]);
        rewardSlots[4].SetReward(1, data[1, GetDataIndex()]);
        rewardSlots[5].SetReward(2, data[2, GetDataIndex()]);
        for (int i = 6; i < 9; i++) {
            int totalWeight = 0;
            for (int j = 0; j < weight.Length; j++) {
                totalWeight += weight[j];
            }
            int r = Random.Range(0, totalWeight);
            int pre = 0, id = 0;
            for (int j = 0; j < weight.Length; j++) {
                pre += weight[j];
                if (r < pre) {
                    id = j;
                    break;
                }
            }
            weight[id] = 0;
            rewardSlots[i].SetReward(id + 1, data[id + 1, GetDataIndex(id == 4)]);
        }
        Extensions.Shuffle(rewardSlots);
        for (int i = 0; i < rewardSlots.Length; i++) {
            RectTransform rect = rewardSlots[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(175 + (i % 3) * 400, -100 - (i / 3) * 250);
            rewardSlots[i].slotId = i;
        }
    }

    public void HideInvalidReward(int id) {
        for (int i = 0; i < rewardSlots.Length; i++) {
            if (i % 3 == id % 3 || i / 3 == id / 3) {
                rewardSlots[i].Hide();
            }
        }
        coinNumber.text = BagManager.instance.coin.ToString();
    }
}
