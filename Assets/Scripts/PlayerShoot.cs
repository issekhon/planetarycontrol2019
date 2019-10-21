using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : MonoBehaviour {
    private gameModeManager modeManager;

    public GameObject playerRef;
    [SerializeField] Transform hand;

    public int gunDamage = 1;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public Camera tpCam;

    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;

	// Use this for initialization
	void Start () {
        laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        modeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameModeManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        //if (hasAuthority)
        //{
            if (playerRef == null)
            {
                Debug.LogError(gameObject.name + ": PLAYER REF NULL");
            }
            //if (hand == null && playerRef != null)
            //{
            //    hand = playerRef.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");
            //    transform.SetParent(hand);
            //    transform.position = hand.transform.position;
            //    //transform.Rotate(180, 0, 180);
            //    //transform.Translate(0, 0.1f, -0.5f);
            //} else if (hand != null)
            //{
            //    transform.SetParent(hand);
            //}

            if (modeManager.currentMode == gameModeManager.Mode.thirdperson && playerRef.GetComponent<moveUnit>().selected)
            {
                if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
                {
                    Debug.Log(gameObject.name + ": SHOOT PLEASE");
                    nextFire = Time.time + fireRate;

                    StartCoroutine(ShotEffect());

                    Vector3 rayOrigin = tpCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                    RaycastHit hit;

                    laserLine.SetPosition(0, gunEnd.position);

                    if (Physics.Raycast(rayOrigin, tpCam.transform.forward, out hit, weaponRange))
                    {
                        laserLine.SetPosition(1, hit.point);
                    }
                    else
                    {
                        laserLine.SetPosition(1, rayOrigin + (tpCam.transform.forward * weaponRange));
                    }
                }
            }
        //}
	}

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;

    }
}
