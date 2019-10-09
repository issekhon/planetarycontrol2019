using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class gameModeManager : NetworkBehaviour
{
    [SyncVar] public int turn;
    [SyncVar] public int battleCountdown;
    [HideInInspector] public enum Mode { strategy, thirdperson, transitionToThirdPerson, transitionToStrategy}
    [SyncVar] public int modeNum;
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
        if (modeNum == 0)
        {
            currentMode = Mode.strategy;
        } else if (modeNum == 1)
        {
            currentMode = Mode.thirdperson;
        }
    }

    [Client]
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

    [Client]
    public void ChangeMode(Mode newMode)
    {
            CmdChangeMode(newMode);
        
    }

    [Command]
    void CmdChangeMode(Mode newMode)
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
