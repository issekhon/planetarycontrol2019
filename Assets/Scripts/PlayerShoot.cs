using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : MonoBehaviour {
    private gameModeManager modeManager;

    public GameObject playerRef;
    [SerializeField] Transform hand;
    private PlayerController myUnitsController;

    [Header("Weapon Stats")]
    public float gunDamage = 1;
    public float powerConversionRate = 2f;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public int shotgunPellets = 4;
    public int shotgunSpread = 5;

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
        if (playerRef.tag == "Player") myUnitsController = playerRef.GetComponent<PlayerController>();
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
            float attackPower = myUnitsController.attackPower;

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

                    if (myUnitsController.unit_type == "soldier")
                    {
                        GameObject tempProjectile = Instantiate(projectile, gunEnd.position + gunEnd.transform.forward * 2f, gunEnd.rotation);
                        tempProjectile.GetComponent<laserBulletScript>().SetDamage(attackPower);
                    }
                    else if (myUnitsController.unit_type == "tank")
                    {
                        for (int i = 0; i < shotgunPellets; i++)
                        {
                            GameObject tempProjectile = Instantiate(projectile, gunEnd.position + gunEnd.transform.forward * 2f, gunEnd.rotation);
                            tempProjectile.GetComponent<laserBulletScript>().SetDamage(attackPower);
                            tempProjectile.transform.Rotate(new Vector3(Random.Range(-shotgunSpread, shotgunSpread), Random.Range(-shotgunSpread, shotgunSpread), Random.Range(-shotgunSpread, shotgunSpread)), Space.Self);
                        }
                    }
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
            StartCoroutine(ShotEffect());
            gunEnd.LookAt(targetLoc);
            if (playerRef.GetComponent<enemySoldierAI>().unit_type == "soldier")
            {
                GameObject tempProjectile = Instantiate(projectile, gunEnd.position + gunEnd.transform.forward * 2f, gunEnd.rotation);
                tempProjectile.GetComponent<laserBulletScript>().SetDamage(gunDamage);
                Debug.Log(gunDamage);
            }
            else if (playerRef.GetComponent<enemySoldierAI>().unit_type == "tank")
            {
                for (int i = 0; i < shotgunPellets; i++)
                {
                    GameObject tempProjectile = Instantiate(projectile, gunEnd.position + gunEnd.transform.forward * 2f, gunEnd.rotation);
                    tempProjectile.GetComponent<laserBulletScript>().SetDamage(gunDamage);
                    tempProjectile.transform.Rotate(new Vector3(Random.Range(-shotgunSpread, shotgunSpread), Random.Range(-shotgunSpread, shotgunSpread), Random.Range(-shotgunSpread, shotgunSpread)), Space.Self);
                }
            }
            nextFire = Time.time + fireRate;
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        //laserLine.enabled = true;
        yield return shotDuration;
        //laserLine.enabled = false;

    }
}
