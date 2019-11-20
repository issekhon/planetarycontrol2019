using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonScript : MonoBehaviour
{
    public void StartTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void StartLevel1()
    {
        SceneManager.LoadScene("WorldMapTest");
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
}
