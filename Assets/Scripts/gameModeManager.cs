using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameModeManager : MonoBehaviour
{
    public int turn;
    [HideInInspector] public enum Mode { strategy, thirdperson, transitionToThirdPerson, transitionToStrategy}
    public Mode currentMode = Mode.strategy;
    // Possible options 
    // strategy
    // thirdperson
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
    }
}
