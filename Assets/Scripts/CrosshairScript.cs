using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    gameModeManager modeManager;
    // Start is called before the first frame update
    void Start()
    {
        modeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameModeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && modeManager.currentMode != gameModeManager.Mode.thirdperson) gameObject.SetActive(false);
    }
}
