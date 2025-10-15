using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageInputHandler : MonoBehaviour // Handles input specific to the battle scene
{
	public static StageInputHandler instance { get; private set; }

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
		if (Input.GetKeyDown(KeyCode.K))
			StartBattle();

		if (selectedCharacter != null) {
			if (Input.GetMouseButtonDown(0)) {
				HandleWorldClick();
			}
		}
	}

	private void StartBattle()
	{
		Debug.Log("Battle Start!");
		StageManager.instance.StartBattle();
	}

    public void HandleSlotClick(Character _character) {
		if (selectedCharacter == _character)
			DeSelectCharacter();
		else
			SelectCharacter(_character);
    }

    public void SelectCharacter(Character _character) {
        DeSelectCharacter();
		selectedCharacter = _character;
    }

    public void DeSelectCharacter() {
		selectedCharacter = null;
    }

	public void HandleWorldClick() {
		if (EventSystem.current.IsPointerOverGameObject())
			return;
		Debug.Log("handle world click");

		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, gridLayerMask);
		if (hit.collider != null) {
			Debug.Log("hit grid");
			GameObject hitObject = hit.collider.gameObject;
			GridScript gridScript = hitObject.GetComponent<GridScript>();
			if (gridScript != null) {
				Vector3Int coordinate = gridScript.coordinate;
				if (!GridManager.instance.HasCharacter(coordinate)) {
					selectedCharacter.position = coordinate;
					BattleManager.instance.AddMember(0, selectedCharacter);
					BagManager.instance.RemoveMember(selectedCharacter);
					DeSelectCharacter();
				}
			} else if (hit.collider.GetComponent<UI_BagSlot>() == null) {
				DeSelectCharacter();
			}
		} else {
			Debug.Log("hit nothing");
			DeSelectCharacter();
		}
	}
}
