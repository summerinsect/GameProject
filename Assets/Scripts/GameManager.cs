using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool isGameStarted = false;
    public bool inBattle = false;
    public bool inShop = false;
    public bool inEvent = false;
    public bool inMap = false;
    public int mapWidth;
    public int mapHeight;
    public int playerDepth;
    public int playerPosition;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            isGameStarted = true;
            playerDepth = 0;
            inBattle = false;
            inShop = false;
            inEvent = false;
            inMap = true;
            AddInitialCharacters();
            MapManager.instance.GenerateMap(mapWidth, mapHeight);
            GameScene.instance.LoadMapScene();
        } // start game
    }

    private void AddInitialCharacters() {
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("YouKnowWho"));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("YouKnowWho"));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("YouKnowWho"));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("YouKnowWho"));
        BagManager.instance.members.Add(CharacterCreater.instance.CreateCharacter("YouKnowWho"));
    }

    public void HandleClickOnMapSlot(MapSlot mapSlot) {
        if (mapSlot == null) {
            Debug.Log("Clicked on null map slot. This should not happen.");
            return;
        }
        if (mapSlot.depth != playerDepth) {
            Debug.Log("Cannot access this slot yet. Incorrect depth.");
            return;
        }
        if (playerDepth != 0 && !MapManager.instance.edges[playerDepth - 1][playerPosition, mapSlot.position]) {
            Debug.Log("Cannot access this slot. No edge.");
            return;
        }
        playerPosition = mapSlot.position;
        switch (mapSlot.slotType) {
            case MapSlotType.Battle:
                inBattle = true;
                inShop = false;
                inEvent = false;
                inMap = false;
                GameScene.instance.LoadBattleScene();
                break;
            case MapSlotType.Shop:
                inBattle = false;
                inShop = true;
                inEvent = false;
                inMap = false;
                // GameScene.instance.LoadShopScene();
                break;
            case MapSlotType.Event:
                inBattle = false;
                inShop = false;
                inEvent = true;
                inMap = false;
                // GameScene.instance.LoadEventScene();
                break;
            default:
                Debug.Log("Unknown map slot type. This should not happen.");
                break;
        }
    }

    public void BattleFinish() {
        inBattle = false;
        inShop = false;
        inEvent = false;
        inMap = true;
        playerDepth += 1;
        if (playerDepth >= mapWidth) {
            Debug.Log("Congratulations! You have completed the game!");
            isGameStarted = false;
            return;
        }
        GameScene.instance.LoadMapScene();
    }
}
