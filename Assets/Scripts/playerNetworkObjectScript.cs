using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerNetworkObjectScript : NetworkBehaviour
{

    public GameObject soldier;
    public GameObject myCamParent;
    public GameObject myCam;
    public GameObject myPointerParent;
    public GameObject myGun;


    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer == false)
        {
            // This object belongs to another player, exit function
            return;
        }

        CmdSpawnSoldier();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    void CmdSpawnSoldier()
    {
        GameObject instSoldier = Instantiate(soldier);

        // Create camera and attach it to scripts where needed
        GameObject instCamParent = Instantiate(myCamParent);
        GameObject instCam = Instantiate(myCam);
        instCam.transform.parent = instCamParent.transform;
        instSoldier.GetComponent<moveUnit>().camParent = instCamParent;
        instSoldier.GetComponent<PlayerController>().cameraT = instCamParent.GetComponentInChildren<Camera>().transform;
        instCamParent.GetComponentInChildren<Camera>().GetComponent<ThirdPersonCamera>().target = instSoldier.transform.Find("ybotTarget");
        instCam.GetComponent<isometricCamera>().cameraTarget = instCamParent.transform;
        instCam.transform.position = instCamParent.transform.position;
        instCam.transform.forward = instCamParent.transform.forward;
        instCam.transform.Rotate(45, 0, 0);

        // Create pointer and attach it where necessary
        GameObject instPointerParent = Instantiate(myPointerParent);

        GameObject instGun = Instantiate(myGun);
        instGun.transform.parent = instSoldier.transform.Find("Weapons");
        instGun.GetComponent<PlayerShoot>().playerRef = instSoldier;
        instGun.GetComponent<PlayerShoot>().tpCam = instCam.GetComponent<Camera>();
        instGun.GetComponent<RayViewer>().tpCam = instCam.GetComponent<Camera>();
        //instSoldier.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");

        instSoldier.GetComponent<moveUnit>().myPointer = instPointerParent;

        // Spawns soldier unit
        NetworkServer.SpawnWithClientAuthority(instSoldier, connectionToClient);
        NetworkServer.SpawnWithClientAuthority(instCamParent, connectionToClient);
        NetworkServer.SpawnWithClientAuthority(instCam, connectionToClient);
        NetworkServer.SpawnWithClientAuthority(instGun, connectionToClient);
        NetworkServer.SpawnWithClientAuthority(instPointerParent, connectionToClient);
    }
}
