using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class gameModeManager : MonoBehaviour
{
    public int turn;
    public int battleCountdown;
    [HideInInspector] public enum Mode { strategy, thirdperson, transitionToThirdPerson, transitionToStrategy}
    public int modeNum;
    public Mode currentMode = Mode.strategy;
    // Possible options 
    // 0 strategy
    // 1 thirdperson
    // transitionToThirdPerson
    // transitionToStrategy

    // Start is called before the first frame update
    void Start()
    {
        turn = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndTurn()
    {
        if (turn == 0)
        {
            turn = 1;
        } 
        else
        {
            turn = 0;
        }
    }

    public void ChangeMode(Mode newMode)
    {
        currentMode = newMode;
        if (currentMode == Mode.strategy)
        {
            modeNum = 0;
        }
        else if (currentMode == Mode.thirdperson)
        {
            modeNum = 1;
            StartCoroutine(ExecuteAfterTime(5));
        }

    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        ChangeMode(Mode.strategy);
        Debug.Log("CHANGE MODE BACK TO STRAT");
    }
}
