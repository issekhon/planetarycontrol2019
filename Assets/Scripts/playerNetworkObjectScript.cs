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

    public void AttachObjectToAnother(NetworkInstanceId atachee, NetworkInstanceId atacher)
    {
        CmdAttachObject(atachee, atacher);
    }

    [Command]
    public void CmdAttachObject(NetworkInstanceId atachee, NetworkInstanceId atacher)
    {
        //Where the item is locally
        //string location = ClientScene.FindLocalObject(netId).name + "/Character/" + controllerName;
        GameObject atacherObj = ClientScene.FindLocalObject(atacher);
        //Find the item based on the netid
        GameObject atacheeObj = ClientScene.FindLocalObject(atachee);
        //Parent object
        atacherObj.GetComponent<isometricCamera>().cameraTarget = atacheeObj.transform;
        //item.transform.localPosition = Vector3.zero;
        //item.transform.localRotation = Quaternion.identity;
        //Call Rpc
        RpcAttachObject(atachee, atacher);
    }

    [ClientRpc]
    public void RpcAttachObject(NetworkInstanceId atachee, NetworkInstanceId atacher)
    {
        //Tell clients to parent the item correctly
        GameObject atacherObj = ClientScene.FindLocalObject(atacher);
        //Find the item based on the netid
        GameObject atacheeObj = ClientScene.FindLocalObject(atachee);
        //Parent object
        atacherObj.GetComponent<isometricCamera>().cameraTarget = atacheeObj.transform;
    }

    [Command]
    void CmdSpawnSoldier()
    {
        GameObject instSoldier = Instantiate(soldier);

        // Create camera and attach it to scripts where needed
        GameObject instCamParent = Instantiate(myCamParent);
        GameObject instCam = Instantiate(myCam);
        instCam.transform.parent = instCamParent.transform;
        instCamParent.GetComponent<cameraLocScript>().myAttachedCam = instCam;
        instSoldier.GetComponent<moveUnit>().camParent = instCamParent;
        instSoldier.GetComponent<PlayerController>().cameraThirdPerson = instCamParent.GetComponentInChildren<Camera>().transform;
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
        //instSoldier.GetComponent<moveUnit>().InitializeMe();
        NetworkServer.SpawnWithClientAuthority(instCamParent, connectionToClient);
        NetworkServer.SpawnWithClientAuthority(instCam, connectionToClient);
        NetworkServer.SpawnWithClientAuthority(instGun, connectionToClient);
        NetworkServer.SpawnWithClientAuthority(instPointerParent, connectionToClient);

        RpcSpawnSoldier(instSoldier, instCamParent, instCam, instPointerParent, instGun);


    }

    [ClientRpc]
    void RpcSpawnSoldier(GameObject tempSoldier, GameObject tempCamParent, GameObject tempCam, GameObject tempPointerParent, GameObject tempGun)
    {
        tempCam.transform.parent = tempCamParent.transform;
        tempCamParent.GetComponent<cameraLocScript>().myAttachedCam = tempCam;
        tempSoldier.GetComponent<moveUnit>().camParent = tempCamParent;
        tempSoldier.GetComponent<PlayerController>().cameraThirdPerson = tempCamParent.GetComponentInChildren<Camera>().transform;
        tempCamParent.GetComponentInChildren<Camera>().GetComponent<ThirdPersonCamera>().target = tempSoldier.transform.Find("ybotTarget");
        tempCam.GetComponent<isometricCamera>().cameraTarget = tempCamParent.transform;
        tempCam.transform.position = tempCamParent.transform.position;
        tempCam.transform.forward = tempCamParent.transform.forward;
        tempCam.transform.Rotate(45, 0, 0);
        tempGun.transform.parent = tempSoldier.transform.Find("Weapons");
        tempGun.GetComponent<PlayerShoot>().playerRef = tempSoldier;
        tempGun.GetComponent<PlayerShoot>().tpCam = tempCam.GetComponent<Camera>();
        tempGun.GetComponent<RayViewer>().tpCam = tempCam.GetComponent<Camera>();
        tempSoldier.GetComponent<moveUnit>().myPointer = tempPointerParent;
    }
}
