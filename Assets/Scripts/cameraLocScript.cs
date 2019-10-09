using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cameraLocScript : NetworkBehaviour
{
    public GameObject myAttachedCam;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority == false)
        {
            if (hasAuthority == false)
            {
                Debug.Log("I don't have authority over: " + gameObject.name);
            }
            if (myAttachedCam != null)
            {


                myAttachedCam.GetComponent<AudioListener>().enabled = false;
                myAttachedCam.GetComponent<Camera>().enabled = false;
            } else
            {
                Debug.Log(gameObject.name + " has no camera for myAttachedCam ;-;");
            }
        }
        else if (hasAuthority)
        {
            if (myAttachedCam != null)
            {
                myAttachedCam.GetComponent<AudioListener>().enabled = true;
                myAttachedCam.GetComponent<Camera>().enabled = true;
                Debug.Log(gameObject.name + ": Has authority");
            } else
            {
                Debug.Log(gameObject.name + " has authority but no camera for myAttachedCam ;-;");
            }
        }
    }
}
