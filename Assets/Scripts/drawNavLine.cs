using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class drawNavLine : MonoBehaviour
{
    private gameModeManager modeManager;
    private NavMeshAgent agentToDraw;
    private LineRenderer lineR;
    public float lineHeight;

    // Start is called before the first frame update
    void Start()
    {
        modeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameModeManager>();
        agentToDraw = GetComponent<NavMeshAgent>();
        lineR = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (modeManager.currentMode == gameModeManager.Mode.strategy)
        {
            if (agentToDraw.hasPath)
            {
                lineR.positionCount = agentToDraw.path.corners.Length;
                Vector3[] positionsToSet = agentToDraw.path.corners;
                for (int i = 0; i < positionsToSet.Length; i++)
                {
                    positionsToSet[i].y += lineHeight;
                }
                lineR.SetPositions(positionsToSet);
                lineR.enabled = true;
            }
        }
        else
        {           
            lineR.enabled = false;
        }
    }
}
