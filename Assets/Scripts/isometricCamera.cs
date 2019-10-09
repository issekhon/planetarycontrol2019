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
    public bool disabledCameraMotion;


    // Start is called before the first frame update
    void Awake()
    {
        modeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameModeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            if (cameraTarget == null)
            {
                Debug.Log("Have auth but No camera target for :" + gameObject.name);
                Debug.Log(gameObject.name + ": Searching for camera with command");
                
                
                return;
            }
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            if (modeManager.currentMode == gameModeManager.Mode.strategy)
            {
                if (!disabledCameraMotion) MoveCam();
            }
        } else
        {
            Debug.Log("I don't have authority over: " + gameObject.name);
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
