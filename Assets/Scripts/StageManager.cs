using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private bool isStarted = false;
    BattleManager battleManager;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarted)
        {
            battleManager.Battle();
        }

    }
}
