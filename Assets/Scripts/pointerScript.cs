﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class pointerScript : NetworkBehaviour
{
    private gameModeManager modeManager;

    // Start is called before the first frame update
    void Start()
    {
        modeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameModeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (modeManager.currentMode == gameModeManager.Mode.strategy)
        {
            this.transform.Find("pointer").gameObject.SetActive(true);
        }
        else
        {
            this.transform.Find("pointer").gameObject.SetActive(false);
        }
    }
}
