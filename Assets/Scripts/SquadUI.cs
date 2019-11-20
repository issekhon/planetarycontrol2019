using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadUI : MonoBehaviour
{
    //UI components
	Text toggle_text;
	bool if_show_squad_info = false;
	RectTransform squad_ui;
	GameObject ui_panel;

	//unit related
	GameObject[] units;
    
    public float maxPower = 0;
	public float maxDefense = 0;
	public float maxSpeed = 0;
	public float maxJump = 0;
    public bool if_updated = false;


	// Start is called before the first frame update
	void Start()
    {
		//text change
		toggle_text = transform.FindChild("toggleButton").FindChild("Text").GetComponent<Text>();

		squad_ui = transform.FindChild("squadUI").FindChild("Panel").GetComponent<RectTransform>();
		ui_panel = GameObject.Find("Panel");
		ui_panel.SetActive(false);

		//find all units object
		units = GameObject.FindGameObjectsWithTag("Player");
	}
    
    void Update()
    {
        update_max_values(units);
        
    }


    //handle toggling to show/hide UI 
    public void buttonClicked()
	{
		if_show_squad_info = !if_show_squad_info;
		if (if_show_squad_info)
		{
			toggle_text.text = "<";
			ui_panel.SetActive(true);
		}
		else
		{
			ui_panel.SetActive(false);
			toggle_text.text = ">";
			
		}
	}
    
    // update max values
    void update_max_values(GameObject[] units)
	{
		for (int i = 0; i < units.Length; i++)
		{
			GameObject current_unit = units[i];
            if (current_unit)
            {
                Dictionary<string, float> unit_attributes = current_unit.GetComponent<PlayerController>().getAttributesDic();

                if (unit_attributes["attackPower"] > maxPower)
                {
                    maxPower = unit_attributes["attackPower"];
                    if_updated = true;
                }
                if (unit_attributes["armor"] > maxDefense)
                {
                    maxDefense = unit_attributes["armor"];
                    if_updated = true;
                }
                if (unit_attributes["speed"] > maxSpeed)
                {
                    maxSpeed = unit_attributes["speed"];
                    if_updated = true;
                }
                if (unit_attributes["jumpHeight"] > maxJump)
                {
                    maxJump = unit_attributes["jumpHeight"];
                    if_updated = true;
                }
            }
            
//			Debug.Log("" + maxPower + " power," + maxDefense + " armor," + maxSpeed + " speed," + maxJump + ", jump");
			
		}
	}
    
    public bool get_if_updated_bool(){
    
        return if_updated;
    }
    
    public void set_if_updated_bool(bool val){
        if_updated = val;
    }
    
    public float[] get_max_values(){

        return new float[] { maxPower, maxDefense, maxSpeed, maxJump };
        
    }

}