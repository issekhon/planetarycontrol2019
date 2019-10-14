using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class moveUnit : MonoBehaviour
{
    private gameModeManager modeManager;
    public GameObject myPointer;
    private GameObject pointer;
    public Material defaultPointerMat;
    public Material enemyPointerMat;

    public GameObject camParent;
    public Camera cam;
    public bool selected = false;

    [SerializeField] Transform _destination;

    NavMeshAgent _navMeshAgent;

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

    // Start is called before the first frame update
    void Start()
    {
        modeManager = GameObject.FindWithTag("GameManager").GetComponent<gameModeManager>();

        
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
        
        
    }

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

            if (modeManager.currentMode == gameModeManager.Mode.strategy && _navMeshAgent != null)
            {
                if (_navMeshAgent.isStopped)
                {
                    preDrawPath();

                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.tag == "Player")
                            {
                                Debug.Log("Fight engaged!");
                                modeManager.ChangeMode(gameModeManager.Mode.thirdperson);
                                return;
                            }

                            NavMeshHit closestPoint;

                            if (NavMesh.SamplePosition(hit.point, out closestPoint, 1.0f, NavMesh.AllAreas))
                            {
                                _navMeshAgent.SetDestination(closestPoint.position);
                                _navMeshAgent.isStopped = false;
                            }
                        }
                    }
                }
                else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        _navMeshAgent.isStopped = true;
                    }
                }
            }
        //}
    }

    private Vector3 prevMousPos;
    private Vector3 currentMousPos;
    public float minMouseChangeDist = 5f;

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
                    if (hit.transform.tag == "Player")
                    {
                        //modeManager.ChangeMode(gameModeManager.Mode.thirdperson);
                        Debug.Log("Hovering on enemy, change pointer color!");
                        pointer.GetComponent<Renderer>().material = enemyPointerMat;

                    }
                    else
                    {
                        pointer.GetComponent<Renderer>().material = defaultPointerMat;
                    }

                    NavMeshHit closestPoint;

                    if (NavMesh.SamplePosition(hit.point, out closestPoint, 1.0f, NavMesh.AllAreas))
                    {
                        _navMeshAgent.SetDestination(closestPoint.position);
                        myPointer.transform.position = closestPoint.position;
                        
                        _navMeshAgent.isStopped = true;
                    }
                }
            }
        }
    }
}
