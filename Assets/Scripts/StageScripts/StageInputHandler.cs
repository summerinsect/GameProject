using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageInputHandler : MonoBehaviour // Handles input specific to the battle scene
{
	public static StageInputHandler instance { get; private set; }

	public bool selected;
	public bool selectedInBag;
	public Character selectedCharacter;
	[SerializeField] LayerMask gridLayerMask;
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		instance = this;
	}
	public void BattleInit()
	{

	}

	private void Update()
	{
		if (!StageManager.instance.isStarted) {
            if (Input.GetKeyDown(KeyCode.K))
                StartBattle();

            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return; // UI 已拦截
                if (selected)
                    ClickWhenSelected(); // 放置或与场景交互
                else
                    ClickWhenUnselected(); // 点击选择场景物体等（未选中时的操作）
            }
        }
		else if (!StageManager.instance.isFinished) {
            // Battle in progress
        }
		else if (StageManager.instance.isFinished) {
			if (Input.GetKeyDown(KeyCode.N)) {
				GameManager.instance.BattleFinish();
            }
        }
	}

	private void StartBattle()
	{
		Debug.Log("Battle Start!");
		StageManager.instance.StartBattle();
	}

    public void HandleSlotClick(Character _character) {
		if (selectedCharacter != null) {
			if (_character == null) {
				BagManager.instance.AddMember(selectedCharacter);
				RemoveMember();
				DeSelectCharacter();
			}
			else if (_character == selectedCharacter) // 再次点击同一角色，取消选择
				DeSelectCharacter();
			else
				SelectCharacter(_character, true);
		}
		else if (_character != null)
			SelectCharacter(_character, true);
    }

    public void SelectCharacter(Character _character, bool inBag) {
		selectedCharacter = _character;
        selected = true;
		selectedInBag = inBag;
    }

	public void RemoveMember() {
		if (!selected)
			return;
		if (selectedInBag) {
			BagManager.instance.RemoveMember(selectedCharacter);
		}
		else {
			int teamID = BattleManager.instance.GetTeamID(selectedCharacter);
			Debug.Assert(teamID == 0, "Only player team members can be removed.");
            BattleManager.instance.RemoveMember(selectedCharacter);
			selectedCharacter.gameObject.SetActive(false);
        }
    }

    public void DeSelectCharacter() {
		selectedCharacter = null;
        selected = false;
	}

	public void ClickWhenUnselected() {
		// Debug.Log("Select nothing, click");
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, gridLayerMask);
        if (hit.collider != null) {
            // Debug.Log("hit grid");
            GameObject hitObject = hit.collider.gameObject;
            GridScript gridScript = hitObject.GetComponent<GridScript>();
            if (gridScript != null) {
                Vector3Int coordinate = gridScript.coordinate;
                if (GridManager.instance.HasCharacter(coordinate)) {
                    Character characterInGrid = GridManager.instance.FindCharacter(coordinate);
                    // show stats of characterInGrid
                    int teamID = BattleManager.instance.GetTeamID(characterInGrid);
                    if (teamID == 0) {
                        SelectCharacter(characterInGrid, false);
                    }
                }
            }
            else {
                // Debug.Log("hit grid but not hit grid");
            }
        }
        else {
            // Debug.Log("hit nothing");
            DeSelectCharacter();
        }

    }		
    public void ClickWhenSelected() {
		// Debug.Log("Select " + selectedCharacter.uid + ", click");

		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, gridLayerMask);
		if (hit.collider != null) {
			// Debug.Log("hit grid");
			GameObject hitObject = hit.collider.gameObject;
			GridScript gridScript = hitObject.GetComponent<GridScript>();
			if (gridScript != null) {
				Vector3Int coordinate = gridScript.coordinate;
				if (!GridManager.instance.HasCharacter(coordinate)) {
					RemoveMember();
                    selectedCharacter.gameObject.SetActive(true);
                    selectedCharacter.position = coordinate;
					BattleManager.instance.AddMember(0, selectedCharacter);
					DeSelectCharacter();
				}
				else {
					Character characterHere = GridManager.instance.FindCharacter(coordinate);
                    // show stats of characterInGrid
                    int teamID = BattleManager.instance.GetTeamID(characterHere);
					if (teamID == 0) {
						SelectCharacter(characterHere, false);
                    }
                    else {
						DeSelectCharacter();
					}
                }
			}
			else {
				// Debug.Log("hit grid but not hit grid");
			}
		} else {
			// Debug.Log("hit nothing");
			DeSelectCharacter();
		}
	}
}
