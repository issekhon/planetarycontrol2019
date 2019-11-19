using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class gameModeManager : MonoBehaviour
{
    public int turn;
    public int battleCountdown;
    public float transitionDuration = 5f;
    public float defaultFightDuration = 20f;
    public float fightDuration = 20f;
    public float currentTime;
    public Text timerUI;
    public Button turnEndButton;
    [SerializeField] public GameObject playerManager;
    [SerializeField] public GameObject enemyManager;
    [HideInInspector] public enum Mode { strategy, thirdperson, transitionToThirdPerson, transitionToStrategy}
    public int modeNum;
    public Mode currentMode = Mode.strategy;

    public Image youWinImage;
    public Image youLoseImage;

    // Start is called before the first frame update
    void Start()
    {
        turn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerUI();
        UpdateEndTurnButton();
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
            playerManager.GetComponent<PlayerManager>().AddCurrency();
        }
        ResetTurnvariables();
    }

    public void ResetTurnvariables()
    {
        enemyManager.GetComponent<EnemyControllerAI>().ResetMe();
        playerManager.GetComponent<PlayerManager>().ResetMe();
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
            currentTime = fightDuration;
            modeNum = 1;
            foreach(Transform playerUnit in playerManager.transform)
            {
                if (!playerUnit.GetComponent<moveUnit>().selected)
                {
                    playerUnit.gameObject.SetActive(false);
                }
            }
            foreach (Transform enemyUnit in enemyManager.transform)
            {
                if (!enemyUnit.GetComponent<enemySoldierAI>().selected)
                {
                    enemyUnit.gameObject.SetActive(false);
                }
            }
            //StartCoroutine(TransitionStrategyAfterTime(fightDuration));
        }
        else if (currentMode == Mode.transitionToThirdPerson)
        {
            //StartCoroutine(ThirdPersonAfterTime(transitionDuration));
        }
        else if (currentMode == Mode.transitionToStrategy)
        {
            //StartCoroutine(ExecuteAfterTime(transitionDuration));

            foreach (Transform playerUnit in playerManager.transform)
            {
                playerUnit.gameObject.SetActive(true);
                playerUnit.GetComponent<moveUnit>().selected = false;
            }
            foreach (Transform enemyUnit in enemyManager.transform)
            {
                enemyUnit.gameObject.SetActive(true);
                enemyUnit.GetComponent<enemySoldierAI>().selected = false;
            }
        }

    }

    public void UpdateTimerUI()
    {
        // Update timer UI
        if (currentMode == Mode.thirdperson)
        {
            if (currentTime > 0)
            {
                currentTime -= 1 * Time.deltaTime;
                timerUI.text = "Fight Time: " + currentTime.ToString("F2");
            }
            else
            {
                ChangeMode(Mode.transitionToStrategy);
            }
        }
        else
        {
            if(timerUI) timerUI.text = "";
        }
    }

    public void UpdateEndTurnButton()
    {
        if (turn == 0)
        {
            if (currentMode != Mode.strategy)
            {
                turnEndButton.gameObject.SetActive(false);
            }
            else
            {
                turnEndButton.gameObject.SetActive(true);
            }
        }
    }

    public void GameWon()
    {
        youWinImage.gameObject.SetActive(true);
    }

    public void GameLost()
    {
        youLoseImage.gameObject.SetActive(true);
    }







    // NOT CURRENTLY IN USE



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
