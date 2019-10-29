using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRoundReset : MonoBehaviour
{
    private gameModeManager modeManager;
    private moveUnit myMoveUnit;
    private PlayerController myPlayerController;
    // Start is called before the first frame update
    void Start()
    {
        modeManager = GameObject.FindWithTag("GameManager").GetComponent<gameModeManager>();
        myMoveUnit = GetComponent<moveUnit>();
        myPlayerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Reset stuff for this unit between rounds
    public void ResetMe()
    {
        myPlayerController.currentActionPoints = myPlayerController.fullActionPoints;
        myPlayerController.previewActionPoints = myPlayerController.currentActionPoints;
        myMoveUnit.attackedThisTurn = false;
        myMoveUnit.selected = false;
    }
}
