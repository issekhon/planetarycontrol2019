using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonScript : MonoBehaviour
{
    public void StartMenu()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("WorldMapTest");
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
}
