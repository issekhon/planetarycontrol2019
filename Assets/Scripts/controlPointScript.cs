using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlPointScript : MonoBehaviour
{
    [Header("Tunable Variables")]
    public ControlType controlPointType = ControlType.General;
    public Material controlPointNeutral, controlPointPlayerControlled, controlPointEnemyControlled;

    [Header("Non-tunable variables")]
    public List<GameObject> boostedUnits;
    [HideInInspector] public enum ControlType { General, Random}
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = boostedUnits.Count - 1; i >= 0; i--)
            {
                if (boostedUnits[i] == other.gameObject)
                {
                    Debug.Log(gameObject.name + ": " + other.gameObject.name + " Already boosted!");
                    return;
                } else if (boostedUnits[i].tag == "Enemy")
                {
                    boostedUnits.Remove(boostedUnits[i]);
                } 
            }

            boostedUnits.Add(other.gameObject);
            other.gameObject.GetComponent<PlayerController>().BoostStatsFixed();

            GetComponent<Renderer>().material = controlPointPlayerControlled;

        } else if (other.tag == "Enemy")
        {
            for(int i = boostedUnits.Count - 1; i >= 0; i--)
            {
                if (boostedUnits[i] == other.gameObject)
                {
                    Debug.Log(gameObject.name + ": " + other.gameObject.name + " Already boosted!");
                    return;
                } else if (boostedUnits[i].tag == "Player")
                {
                    boostedUnits[i].GetComponent<PlayerController>().DebuffStats();
                    boostedUnits.RemoveAt(i);
                }
            }

            boostedUnits.Add(other.gameObject);
            GetComponent<Renderer>().material = controlPointEnemyControlled;
        }
    }

    private void BoostMe()
    {
        
    }

    private void UnBoostMe()
    {

    }
}
