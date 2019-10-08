using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class isometricCamera : NetworkBehaviour
{
    private gameModeManager modeManager;

    public Transform cameraTarget;
    [SerializeField] public float speed = 5;
    [SerializeField] private bool isCamMoving = false;
    [SerializeField] private int screenWidth;
    [SerializeField] private int screenHeight;
    [SerializeField] public int horizontalBound = 30;
    [SerializeField] public int verticalBound = 30;
    

    // Start is called before the first frame update
    void Awake()
    {
        modeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameModeManager>();

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            if (modeManager.currentMode == gameModeManager.Mode.strategy)
            {
                MoveCam();
            }
        }
    }

    void MoveCam()
    {
        if (Input.mousePosition.x > screenWidth - horizontalBound)
        {
            isCamMoving = true;
            cameraTarget.transform.Translate(cameraTarget.transform.right * speed * Time.deltaTime, Space.World);
        }
        else if (Input.mousePosition.x < horizontalBound)
        {
            isCamMoving = true;
            cameraTarget.transform.Translate(-cameraTarget.transform.right * speed * Time.deltaTime, Space.World);
        }

        else if (Input.mousePosition.y > screenHeight - verticalBound)
        {
            isCamMoving = true;
            cameraTarget.transform.Translate(cameraTarget.transform.forward * speed * Time.deltaTime, Space.World);
        }
        else if (Input.mousePosition.y < verticalBound)
        {
            isCamMoving = true;
            cameraTarget.transform.Translate(-cameraTarget.transform.forward * speed * Time.deltaTime, Space.World);
        }
        else
        {
            isCamMoving = false;
        }
    }
}
