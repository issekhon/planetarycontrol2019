using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAttributes : MonoBehaviour
{
    public string attributeType;
    public int unit_num;
    
    GameObject[] units;
    GameObject unit;
    
    void Start(){
        //find all units object
		units = GameObject.FindGameObjectsWithTag("Player");
        unit = units[unit_num-1];
    }
    
    //handle toggling to show/hide UI 
    public void buttonClicked()
	{
		unit.GetComponent<PlayerController>().updateStatus(attributeType);
    }
}
