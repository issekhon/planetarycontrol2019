using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadAttributes : MonoBehaviour
{
    //text
    Text attackPower;
    Text defense;
    Text speed;
    Text jump;
    Text health;
    Text vision;
    
    //units related
    public int unit_num;
    GameObject[] units;
    
    // Start is called before the first frame update
    void Start()
    {
        //find all texts
        attackPower = transform.Find("lineAttack").Find("Text1").GetComponent<Text>();
        defense = transform.Find("lineDefense").Find("Text1").GetComponent<Text>();
        speed = transform.Find("lineSpeed").Find("Text1").GetComponent<Text>();
        jump = transform.Find("lineJump").Find("Text1").GetComponent<Text>();
        health = transform.Find("lineHealth").Find("Text1").GetComponent<Text>();
        vision = transform.Find("lineVision").Find("Text1").GetComponent<Text>();
        
        //find all units object
		units = GameObject.FindGameObjectsWithTag("Player");
        GameObject unit = units[unit_num-1];
        Dictionary<string,float> attributes = unit.GetComponent<PlayerController>().initStatus();
        
        attackPower.text = "Attack: "+attributes["attackPower"];
        defense.text = "Defense: "+attributes["armor"];
        speed.text = "Speed: "+attributes["speed"];
        jump.text = "Jump: "+attributes["jumpHeight"];
        health.text = "Health: "+attributes["fullHealth"];
        vision.text = "Vision: "+attributes["vision"];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
