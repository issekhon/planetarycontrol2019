using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class gameModeManager : MonoBehaviour
{
    public int turn;
    public int battleCountdown;
    public float transitionDuration = 5f;
    public float fightDuration = 2f;
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
        Debug.Log(this.transform.name + ": Transitioning to mode " + currentMode);
        if (currentMode == Mode.strategy)
        {
            modeNum = 0;
        }
        else if (currentMode == Mode.thirdperson)
        {
            modeNum = 1;
            StartCoroutine(TransitionStrategyAfterTime(fightDuration));
        }
        else if (currentMode == Mode.transitionToThirdPerson)
        {
            //StartCoroutine(ThirdPersonAfterTime(transitionDuration));
        } else if (currentMode == Mode.transitionToStrategy) {
            //StartCoroutine(ExecuteAfterTime(transitionDuration));
        }

    }

    IEnumerator TransitionStrategyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        ChangeMode(Mode.transitionToStrategy);
        //Debug.Log("CHANGE MODE TO TRANSITION STRATEGY");
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        ChangeMode(Mode.strategy);
        //Debug.Log("CHANGE MODE TO STRATEGY");
    }

    IEnumerator ThirdPersonAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        ChangeMode(Mode.thirdperson);
        //Debug.Log("CHANGE MODE TO THIRD PERSON");
    }
}
