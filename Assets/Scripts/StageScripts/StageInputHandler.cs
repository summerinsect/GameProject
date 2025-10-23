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
            // if (Input.GetKeyDown(KeyCode.K))
            //     StartBattle();
            // already add button to start battle
            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return; // UI �����أ���Ҫ�Ǳ�����
                if (selected)
                    ClickWhenSelected(); // ���û��볡������
                else
                    ClickWhenUnselected(); // ���ѡ�񳡾�����ȣ�δѡ��ʱ�Ĳ�����
            }
        }
		else if (!StageManager.instance.isFinished) {
            // Battle in progress
        }
        // else if (StageManager.instance.isFinished) {
        // 	if (Input.GetKeyDown(KeyCode.N)) {
        //         EndBattle();
        //     }
        // }
        // already add button to end battle
    }

    public void EndBattle() {
        if (BattleManager.instance.GetWinner() == 0) {
            GameManager.instance.NextLevel();
        }
        else {
            Debug.Log("Game Over. You lost the battle.");
            GameManager.instance.GameOver();
        }
    }

    public void StartBattle()
	{
		if (StageManager.instance.isStarted)
			return;	
        Debug.Log("Battle Start!");
		StageManager.instance.StartBattle();
	}

    public void HandleSlotClick(Character _character) {
        // ����ڱ���������ĳ����ɫ��
        // ����Ѿ�ѡ����һ����ɫ
        if (selectedCharacter != null) {
            // ����˿ղۣ��Żر���
            if (_character == null) {
                BagManager.instance.AddMember(selectedCharacter);
                RemoveMember();
                DeSelectCharacter();
            }
            // �ٴε��ͬһ��ɫ��ȡ��ѡ��
            else if (_character == selectedCharacter) {
                DeSelectCharacter();
            }
            // ����˲�ͬ��ɫ��ѡ�иý�ɫ
            else {
                SelectCharacter(_character, true);
            }
        }
        // ���û��ѡ�н�ɫ��ѡ�иý�ɫ
        else if (_character != null) {
            SelectCharacter(_character, true);
        }
    }

    public void SelectCharacter(Character _character, bool inBag) {
		selectedCharacter = _character;
        selected = true;
		selectedInBag = inBag;
        UI_StatsPanel.instance.ShowStats(_character);
    }

    public void RemoveMember() {
		if (!selected)
			return;
        // ����Ǳ�����Ľ�ɫ��ֱ�Ӵӱ����Ƴ�
        if (selectedInBag) {
			BagManager.instance.RemoveMember(selectedCharacter);
		}
        // ����ǳ��Ͻ�ɫ����ս���������Ƴ�
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
		selectedInBag = false;
		UI_StatsPanel.instance.Clear();
    }

    // ��û��ѡ�н�ɫ������µ���������Ǳ����ۣ�
	public void ClickWhenUnselected() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, gridLayerMask);
        // ��������ĳ������
        if (hit.collider != null) {
            GameObject hitObject = hit.collider.gameObject;
            GridScript gridScript = hitObject.GetComponent<GridScript>();
            if (gridScript != null) {
                Vector3Int coordinate = gridScript.coordinate;
                if (GridManager.instance.HasCharacter(coordinate)) {
                    Character characterInGrid = GridManager.instance.FindCharacter(coordinate);
                    // ����������н�ɫ���������Ӫ��ѡ�У��ǵз���Ӫ����ʾ��Ϣ
                    int teamID = BattleManager.instance.GetTeamID(characterInGrid);
                    if (teamID == 0) {
                        SelectCharacter(characterInGrid, false);
                    }
                    else {
                        UI_StatsPanel.instance.ShowStats(characterInGrid);
                    }
                }
            }
		    // ������û�����ӣ�ȡ��ѡ��
            else {
				DeSelectCharacter();
            }
        }
        // û��������ӣ�ȡ��ѡ��
        else {
            DeSelectCharacter();
        }
    }	
    
    // ��ѡ�н�ɫ������µ������Ӧ���Ǳ����ۣ�
    public void ClickWhenSelected() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, gridLayerMask);
	    // ��������ĳ������
		if (hit.collider != null) {
			GameObject hitObject = hit.collider.gameObject;
			GridScript gridScript = hitObject.GetComponent<GridScript>();
			if (gridScript != null) {
				Vector3Int coordinate = gridScript.coordinate;
                // ���������û�н�ɫ������ѡ�еĽ�ɫ
                if (!GridManager.instance.HasCharacter(coordinate)) {
					RemoveMember();
                    selectedCharacter.gameObject.SetActive(true);
                    selectedCharacter.position = coordinate;
					BattleManager.instance.AddMember(0, selectedCharacter);
					DeSelectCharacter();
				}
                // ����������н�ɫ���������Ӫ��ѡ�У��ǵз���Ӫ����ʾ��Ϣ
                else {
					Character characterHere = GridManager.instance.FindCharacter(coordinate);
                    int teamID = BattleManager.instance.GetTeamID(characterHere);
					if (teamID == 0) {
						SelectCharacter(characterHere, false);
                    }
                    else {
						DeSelectCharacter();
                        UI_StatsPanel.instance.ShowStats(characterHere);
                    }
                }
			}
			// ��������ӵ�û�е�������ӣ�����
			else {
                Debug.Assert(false, "Why are you here?");
			}
		} 
		// û��������ӣ�ȡ��ѡ��
        else {
			DeSelectCharacter();
		}
	}
}
