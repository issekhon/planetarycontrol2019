using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUIScript : MonoBehaviour
{
    private gameModeManager modeManager;
    private Text buttonText;
    public string endTurntext = "End Turn";
    public string otherPlayerTurnText = "Enemy Turn";
    // Start is called before the first frame update
    void Start()
    {
        modeManager = GameObject.FindWithTag("GameManager").GetComponent<gameModeManager>();
        buttonText = transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (modeManager.turn == 0)
        {
            if (modeManager.currentMode == gameModeManager.Mode.strategy)
            {
                if (this.GetComponent<Button>().interactable == false)
                {
                    this.GetComponent<Button>().interactable = true;
                    if (!buttonText.text.Equals(endTurntext)) buttonText.text = endTurntext;
                }
            }
        } 
        else if (modeManager.turn == 1)
        {
            if (!buttonText.text.Equals(otherPlayerTurnText)) buttonText.text = otherPlayerTurnText;
        }
    }

    public void ButtonEndTurn()
    {
        if (modeManager.turn == 0)
        {
            modeManager.EndTurn();
            this.GetComponent<Button>().interactable = false;
        }
    }
}
