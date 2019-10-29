using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : MonoBehaviour {
    private gameModeManager modeManager;

    public GameObject playerRef;
    [SerializeField] Transform hand;

    [Header("Weapon Stats")]
    public int gunDamage = 1;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;

    public Transform gunEnd;
    public Camera tpCam;
    public GameObject projectile;
    public GameObject crosshair;

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

        if (playerRef.tag == "Player")
        {
            if (modeManager.currentMode == gameModeManager.Mode.thirdperson && playerRef.GetComponent<moveUnit>().selected)
            {
                if (crosshair.activeSelf == false) crosshair.SetActive(true);
                if (Input.GetButton("Fire1") && Time.time > nextFire)
                {
                    Debug.Log(gameObject.name + ": SHOOT PLEASE");
                    nextFire = Time.time + fireRate;

                    StartCoroutine(ShotEffect());

                    Vector3 rayOrigin = tpCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                    RaycastHit hit;

                    //laserLine.SetPosition(0, gunEnd.position);

                    if (Physics.Raycast(rayOrigin, tpCam.transform.forward, out hit, weaponRange))
                    {
                        //    laserLine.SetPosition(1, hit.point);
                        gunEnd.LookAt(hit.point);
                    }
                    else
                    {
                        //    laserLine.SetPosition(1, rayOrigin + (tpCam.transform.forward * weaponRange));
                        gunEnd.LookAt(rayOrigin + tpCam.transform.forward * weaponRange);
                    }

                    GameObject tempProjectile = Instantiate(projectile, gunEnd.position + gunEnd.transform.forward * 2f, gunEnd.rotation);
                    tempProjectile.GetComponent<laserBulletScript>().SetDamage(gunDamage);
                }
            }
        } 
        else if (playerRef.tag == "Enemy")
        {
            // Enemy weapon behaviour
            
        }
        //}
	}

    public void shootForEnemy(Transform targetLoc)
    {
        if (Time.time > nextFire)
        {
            gunEnd.LookAt(targetLoc);
            GameObject tempProjectile = Instantiate(projectile, gunEnd.position + gunEnd.transform.forward * 2f, gunEnd.rotation);
            tempProjectile.GetComponent<laserBulletScript>().SetDamage(gunDamage);
            nextFire = Time.time + fireRate;
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;

    }
}
