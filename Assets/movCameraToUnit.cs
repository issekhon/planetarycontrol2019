using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movCameraToUnit : MonoBehaviour
{
    //units related
    public int unit_num;
    GameObject[] units;
    
    //camera offsets
	Vector3 groundCamOffset;
	Vector3 camTarget;
	Vector3 camSmoothDampV;
    Camera cam;
    GameObject camLoc;
    
    // Compute angle that the camera makes with the ground
	private Vector3 GetWorldPosAtViewportPoint(float vx, float vy, float vz)
	{
		Ray worldRay = cam.ViewportPointToRay(new Vector3(vx, vy, vz));
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float distanceToGround;
		groundPlane.Raycast(worldRay, out distanceToGround);
		return worldRay.GetPoint(distanceToGround);
	}
    
    // Start is called before the first frame update
    void Start()
    {
        //find all units object
		units = GameObject.FindGameObjectsWithTag("Player");
        
        //camera
        camLoc = GameObject.Find("CameraLoc");
        cam = GameObject.Find("myCamera").GetComponent<Camera>();
        
        //intialize camera offset
		Vector3 groundPos = GetWorldPosAtViewportPoint(0.5f, 0.5f,0f);
		groundCamOffset = cam.transform.position - groundPos;
		camTarget = cam.transform.position;
    }
    
    void Update(){
    
    }

    
    // Calulate screen position and move camera to the target
    public void moveCamera()
	{
        // Center whatever position is clicked
        Vector3 worldPos = units[2].transform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        float x = screenPos[0] / Camera.main.pixelWidth;
        float y = screenPos[1] / Camera.main.pixelHeight;

        Vector3 clickPt = GetWorldPosAtViewportPoint(x, y,0f);
        camTarget = clickPt + groundCamOffset;
        Debug.Log(x);
        Debug.Log(y);

        // Move the camera smoothly to the target position
        Camera.main.transform.position = Vector3.SmoothDamp(
        Camera.main.transform.position, camTarget, ref camSmoothDampV, 0.5f);
    }
    
}
