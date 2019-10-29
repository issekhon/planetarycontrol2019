using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    private gameModeManager modeManager;
    private EnemyControllerAI myControllerAI;

    private moveUnit myMoveUnit;

    [Header("Third Person Movement Variables")]
    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;

	float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    public Vector3 velocity;

    Animator animator;
    public Transform cameraThirdPerson;
    CharacterController controller;
    
    //health bar and action bar

    [Header("UI Variables")]
    public float healthBarHeight = 2.93f;
    public float actionBarHeight = 1.96f;
	public float fullHealth = 100;
    public float currentHealth;
    public float fullActionPoints = 20;
    public float previewActionPoints;
	public float currentActionPoints;
	public GameObject healthBarPrefab;
	public GameObject actionPointsBarPrefab;
    private Image healthBarImg;
    private Image ActionBarImg;
    public GameObject myHealthBar;
    public GameObject myActionPointsBar;

    // Use this for initialization
    void Start () {
        modeManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameModeManager>();
        myControllerAI = GameObject.FindWithTag("EnemyController").GetComponent<EnemyControllerAI>();
        animator = GetComponent<Animator>();
        //cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        velocity = Vector3.zero;
        myMoveUnit = GetComponent<moveUnit>();
        
        //intialize and fill health bar
        //ToDo:Here should find a current game object[this]
        Transform character = transform.gameObject.transform;
		myHealthBar = Instantiate(healthBarPrefab, this.transform.position + new Vector3(0, healthBarHeight, 0), Quaternion.Euler(-45, 45, 0), character);
		myActionPointsBar = Instantiate(actionPointsBarPrefab, this.transform.position + new Vector3(0, actionBarHeight, 0), Quaternion.Euler(-45, 45, 0), character);
		currentHealth = fullHealth;
        currentActionPoints = fullActionPoints;
        previewActionPoints = fullActionPoints;

        healthBarImg = myHealthBar.transform.Find("healthBar").Find("healthFilled").GetComponent<Image>();
        healthBarImg.fillAmount = 1.0f;
        ActionBarImg = myActionPointsBar.transform.Find("ActionPoints").Find("ActionFilled").GetComponent<Image>();
        ActionBarImg.fillAmount = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
        //if (hasAuthority)
        //{

		if (cameraThirdPerson == null)
        {
            return;
        }

        //testing
        if (myHealthBar != null && myActionPointsBar != null)
        {
            //currentHealth -= Time.deltaTime;
            healthBarImg.fillAmount = currentHealth / fullHealth;
            if (previewActionPoints != currentActionPoints)
                ActionBarImg.fillAmount = previewActionPoints / fullActionPoints;
            else
                ActionBarImg.fillAmount = currentActionPoints / fullActionPoints;
            //myHealthBar.transform.LookAt(cameraThirdPerson);
        }

        if (modeManager.currentMode == gameModeManager.Mode.thirdperson && myMoveUnit.selected)
        {
            
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 inputDir = input.normalized;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            //if (inputDir != Vector2.zero)
            //{
            float targetRotation = Mathf.Atan2(0, 1) * Mathf.Rad2Deg + cameraThirdPerson.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            //}

            bool running = Input.GetKey(KeyCode.LeftShift);
            float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

            velocityY += Time.deltaTime * gravity;
            velocity = Vector3.zero;
            if (inputDir.x > 0)
            {
                velocity = transform.right * currentSpeed + Vector3.up * velocityY;
            }
            else if (inputDir.x < 0)
            {
                velocity = -1 * transform.right * currentSpeed + Vector3.up * velocityY;
            }
            if (inputDir.y > 0)
            {
                if (velocity == Vector3.zero)
                {
                    velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
                }
                else
                {
                    velocity /= 2;
                    velocity += transform.forward * currentSpeed / 1.2f;
                }
            }
            else if (inputDir.y < 0)
            {
                if (velocity == Vector3.zero)
                {
                    velocity = -1 * transform.forward * currentSpeed + Vector3.up * velocityY;
                }
                else
                {
                    velocity /= 2;
                    velocity += -1 * transform.forward * currentSpeed / 1.2f;
                }
            }
            if (inputDir.x == 0 && inputDir.y == 0)
            {
                velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            }
            //Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);

            currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

            if (controller.isGrounded)
            {
                velocityY = 0;
            }

            float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
            animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        }
        //}
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentHealth > 0) { 
            currentHealth -= damageAmount;
            Mathf.Clamp(currentHealth, 0, fullHealth);
            if (currentHealth <= 0)
            {
                myControllerAI.RemoveFromList(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
