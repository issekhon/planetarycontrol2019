using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> myUnits;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            myUnits.Add(child.gameObject);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetMe()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<UnitRoundReset>().ResetMe();
        }
    }
}
