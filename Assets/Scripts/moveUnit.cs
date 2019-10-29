using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class moveUnit : MonoBehaviour
{
    private gameModeManager modeManager;

    public float maxMoveDistance = 10f;
    [SerializeField] private float pathDistance;

    private Animator myAnim;
    private PlayerController myContrl;

    public GameObject myPointer;
    private GameObject pointer;
    public Material defaultPointerMat;
    public Material enemyPointerMat;
    public Material grayedPointerMat;
    private LineRenderer myLineR;
    public Material lineActive;
    public Material lineDeactive;
    private drawNavLine myNavLine;

    public GameObject camParent;
    public Camera cam;
    public bool selected = false;
    public bool mouseHovering = false;
    Outline myOutline;
    public float outlineWidth = 0f;
    public Color outlineHoverColor = Color.white;
    public Color outlineSelectedColor = Color.blue;

    [SerializeField] Transform _destination;

    NavMeshAgent _navMeshAgent;

    // Initialize all the varialbes that are not connected through the editor
    void Start()
    {
        modeManager = GameObject.FindWithTag("GameManager").GetComponent<gameModeManager>();

        myLineR = GetComponent<LineRenderer>();
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        myAnim = GetComponent<Animator>();
        myContrl = GetComponent<PlayerController>();
        myNavLine = GetComponent<drawNavLine>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            _navMeshAgent.SetDestination(this.transform.position);
            _navMeshAgent.isStopped = true;
        }

        myOutline = gameObject.AddComponent<Outline>();
        myOutline.OutlineMode = Outline.Mode.OutlineAll;
        myOutline.OutlineColor = Color.white;
        myOutline.OutlineWidth = 0f;
    }

    // Not currently used
    private void SetDestination()
    {
        if(_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (hasAuthority)
        //{
            // Set camera after it's been spawned
            if (camParent == null) return;
            if (cam == null)
            {
                cam = camParent.GetComponentInChildren<Camera>();
            }
            
            if (pointer == null)
            {
                pointer = myPointer.transform.Find("pointer").gameObject;
            }

            // Cast rays from mouse to see if player clicked on me
            if (modeManager.currentMode == gameModeManager.Mode.strategy && _navMeshAgent != null)
            {
                // If not already selected, check to see if I was clicked on and make me selected
                if (!selected)
                {
                    if (_navMeshAgent.hasPath) _navMeshAgent.ResetPath();    

                    Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitCheck;


                    if (Physics.Raycast(camRay, out hitCheck))
                    {
                        if (hitCheck.transform.gameObject == this.gameObject)
                        {
                            
                            //Debug.Log("Hovering on player select");
                            
                            //myOutline.OutlineMode = Outline.Mode.OutlineAll;
                            //myOutline.OutlineColor = Color.white;
                            if (myOutline.OutlineWidth == 0f) myOutline.OutlineWidth = outlineWidth;
                            if (Input.GetMouseButtonDown(0))
                            {
                                selected = true;
                                cam.GetComponent<isometricCamera>().selectedPlayer = this.gameObject;
                                cam.GetComponent<ThirdPersonCamera>().selectedPlayer = this.gameObject;
                                return;
                            }
                        }
                        else
                        {
                            if (myOutline.OutlineWidth > 0f)
                            {
                                myOutline.OutlineWidth = 0f;
                            }
                        }
                    }
                }

                // If I am selected, then check to see if I click on a location to move me
                if (selected && _navMeshAgent.isStopped)
                {
                    if (myOutline.OutlineWidth == 0f) myOutline.OutlineWidth = outlineWidth;
                    if (myOutline.OutlineColor != outlineSelectedColor) myOutline.OutlineColor = outlineSelectedColor;

                    preDrawPath();

                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.tag == "Enemy")
                            {
                                //Debug.Log("Fight engaged!");
                                modeManager.ChangeMode(gameModeManager.Mode.transitionToThirdPerson);
                                myOutline.OutlineWidth = 0f;
                                return;
                            }
                            else if (hit.transform.tag == "Player")
                            {
                                selected = false;
                                myOutline.OutlineWidth = 0f;
                                myOutline.OutlineColor = outlineHoverColor;
                                return;
                            }

                            NavMeshHit closestPoint;

                            if (NavMesh.SamplePosition(hit.point, out closestPoint, 1.0f, NavMesh.AllAreas))
                            {
                                _navMeshAgent.SetDestination(closestPoint.position);
                                if (myNavLine.pathLength < maxMoveDistance) _navMeshAgent.isStopped = false;
                            }
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.C))
                    {
                        selected = false;
                        myOutline.OutlineWidth = 0f;
                        myOutline.OutlineColor = outlineHoverColor;
                    }
                }
                else if (selected && !_navMeshAgent.isStopped)
                {
                    if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    {
                        if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            _navMeshAgent.isStopped = true;
                        }
                    }
                }
                float animationSpeedPercent = Mathf.Clamp(Mathf.Abs(_navMeshAgent.velocity.magnitude) / myContrl.runSpeed, 0f, 1f);
                myAnim.SetFloat("speedPercent", animationSpeedPercent, myContrl.speedSmoothTime, Time.deltaTime);
            }
        //}
    }

    private Vector3 prevMousPos;
    private Vector3 currentMousPos;
    public float minMouseChangeDist = 5f;
    Vector3 lastViablePath;

    // This is just to draw the path with the pointer and line but does not move the unit
    private void preDrawPath()
    {
        if (currentMousPos == null)
        {
            currentMousPos = Input.mousePosition;
            prevMousPos = currentMousPos;
        }
        else if (Input.mousePosition != prevMousPos)
        {
            currentMousPos = Input.mousePosition;

            if ((Math.Abs(currentMousPos.x - prevMousPos.x) > minMouseChangeDist) || (Math.Abs(currentMousPos.y - prevMousPos.y) > minMouseChangeDist))
            {
                prevMousPos = currentMousPos;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {

                    NavMeshHit closestPoint;

                    if (hit.transform.tag == "Enemy")
                    {
                        //modeManager.ChangeMode(gameModeManager.Mode.thirdperson);
                        //Debug.Log("Hovering on enemy, change pointer color!");
                        pointer.GetComponent<Renderer>().material = enemyPointerMat;
                        myPointer.transform.position = hit.transform.position;
                        myLineR.material = lineDeactive;

                    }
                    else if (NavMesh.SamplePosition(hit.point, out closestPoint, 1.0f, NavMesh.AllAreas))
                    {

                        _navMeshAgent.SetDestination(closestPoint.position);
                        if (myNavLine.pathLength > maxMoveDistance)
                        {
                            pointer.GetComponent<Renderer>().material = grayedPointerMat;
                            myLineR.material = lineDeactive;
                        }
                        else
                        {
                            pointer.GetComponent<Renderer>().material = defaultPointerMat;
                            myLineR.material = lineActive;
                        }

                        myPointer.transform.position = closestPoint.position;

                        _navMeshAgent.isStopped = true;
                        pathDistance = _navMeshAgent.remainingDistance;
                    }
                    else
                    {
                        pointer.GetComponent<Renderer>().material = grayedPointerMat;
                    }
                }
            }
        }
    }


    // For network code, not currently in use
    public void InitializeMe()
    {
        modeManager = GameObject.FindWithTag("GameManager").GetComponent<gameModeManager>();

        //if (hasAuthority)
        //{
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            _navMeshAgent.SetDestination(this.transform.position);
            _navMeshAgent.isStopped = true;
        }
        //}
    }
}
