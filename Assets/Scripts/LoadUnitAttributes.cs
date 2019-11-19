  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUnitAttributes : MonoBehaviour
{
	//attributes
	Text attackPower;
	Text defense;
	Text speed;
	Text jump;
	Text health;
	Text actionPoints;
	//    Text vision;


	//unit info
	Text className;
	Text weaponName;

	//units related
	public int unit_num;
	GameObject[] units;
	GameObject unit;
	float maxActionPoints = 20;
	float maxHealth;
	float currentActionPoints;
	float currentHealth;
    float healthUpgradeVal = 5;
	bool updateAttributes;
    
    //UI related
    GameObject squadUI;

	//sliders 
	Slider attackPowerSlider;
	Slider defenseSlider;
	Slider speedSlider;
	Slider jumpSlider;
	Slider healthSlider;
	Slider actionPointsSlider;



	// Start is called before the first frame update
	void Start()
	{
		//find all texts
		if (transform.FindChild("inner_ui_bottom").FindChild("lineAttack").FindChild("value") != null)
		{
			attackPower = transform.FindChild("inner_ui_bottom").FindChild("lineAttack").FindChild("value").GetComponent<Text>();
			attackPowerSlider = transform.FindChild("inner_ui_bottom").FindChild("lineAttack").FindChild("Slider").GetComponent<Slider>();
		}

		if (transform.FindChild("inner_ui_bottom").FindChild("lineDefense").FindChild("value") != null)
		{
			defense = transform.FindChild("inner_ui_bottom").FindChild("lineDefense").FindChild("value").GetComponent<Text>();
			defenseSlider = transform.FindChild("inner_ui_bottom").FindChild("lineDefense").FindChild("Slider").GetComponent<Slider>();
		}

		if (transform.FindChild("inner_ui_bottom").FindChild("lineSpeed").FindChild("value") != null)
		{
			speed = transform.FindChild("inner_ui_bottom").FindChild("lineSpeed").FindChild("value").GetComponent<Text>();
			speedSlider = transform.FindChild("inner_ui_bottom").FindChild("lineSpeed").FindChild("Slider").GetComponent<Slider>();
		}

		if (transform.FindChild("inner_ui_bottom").FindChild("lineJump").FindChild("value") != null)
		{
			jump = transform.FindChild("inner_ui_bottom").FindChild("lineJump").FindChild("value").GetComponent<Text>();
			jumpSlider = transform.FindChild("inner_ui_bottom").FindChild("lineJump").FindChild("Slider").GetComponent<Slider>();
		}

		if (transform.FindChild("inner_ui_top").FindChild("lineHealth").FindChild("value") != null)
		{
			health = transform.FindChild("inner_ui_top").FindChild("lineHealth").FindChild("value").GetComponent<Text>();
			healthSlider = transform.FindChild("inner_ui_top").FindChild("lineHealth").FindChild("Slider").GetComponent<Slider>();
		}
		if (transform.FindChild("inner_ui_top").FindChild("lineActionPoints").FindChild("value") != null)
		{
			actionPoints = transform.FindChild("inner_ui_top").FindChild("lineActionPoints").FindChild("value").GetComponent<Text>();
			actionPointsSlider =
			transform.FindChild("inner_ui_top").FindChild("lineActionPoints").FindChild("Slider").GetComponent<Slider>();
		}

		if (transform.FindChild("top_ui").FindChild("unitClass") != null)
		{
			className = transform.FindChild("top_ui").FindChild("unitClass").GetComponent<Text>();
		}
		if (transform.FindChild("top_ui").FindChild("weapon").FindChild("weaponName") != null)
		{
			weaponName = transform.FindChild("top_ui").FindChild("weapon").FindChild("weaponName").GetComponent<Text>();
		}

		//find all units object
		units = GameObject.FindGameObjectsWithTag("Player");
		unit = units[unit_num - 1];
        squadUI = GameObject.Find("UIcontainer");
		Dictionary<string, float> attributes = unit.GetComponent<PlayerController>().initStatus();
		string unit_type = unit.GetComponent<PlayerController>().get_unit_type();


		if (className) className.text = "" + unit_type;
		if (attackPower) attackPower.text = "" + attributes["attackPower"];
		if (defense) defense.text = "" + attributes["armor"] + "%";
		if (speed) speed.text = "" + attributes["speed"];
		if (jump) jump.text = "" + attributes["jumpHeight"];
		if (health) health.text = attributes["fullHealth"] + "/" + attributes["fullHealth"];
		if (actionPoints) actionPoints.text = maxActionPoints + "/" + maxActionPoints;

		maxHealth = attributes["fullHealth"];
		currentHealth = maxHealth;
		currentActionPoints = maxActionPoints;

		//        vision.text = "Vision: "+attributes["vision"];

	}

	// Update is called once per frame
	void Update()
	{

        float current_game_health = unit.GetComponent<PlayerController>().get_current_health();
		float current_game_action_points = unit.GetComponent<PlayerController>().get_action_points();

		// update when any of attributes get upgraded
		bool if_need_update = unit.GetComponent<PlayerController>().check_if_need_update();

		if (if_need_update)
		{
			Dictionary<string, float> new_attributes = unit.GetComponent<PlayerController>().getAttributesDic();
			if (attackPower) attackPower.text = "" + new_attributes["attackPower"];
			if (defense) defense.text = "" + new_attributes["armor"] + "%";
			if (speed) speed.text = "" + new_attributes["speed"];
			if (jump) jump.text = "" + new_attributes["jumpHeight"];
			if (health)
			{
				if (new_attributes["fullHealth"] > maxHealth)
				{
                    unit.GetComponent<PlayerController>().set_current_health(healthUpgradeVal);
					maxHealth = new_attributes["fullHealth"];
                    health.text = current_game_health + "/" + maxHealth;
				}
			}
		}

		//update health and action points in real time
		
		if (health)
		{
			if (current_game_health != currentHealth || (current_game_health / maxHealth != currentHealth / maxHealth))
			{
				health.text = System.Math.Round(current_game_health, 2) + "/" + maxHealth;
				healthSlider.value = current_game_health / maxHealth;
			}
		}
		if (actionPoints)
		{
			if (current_game_action_points != currentActionPoints)
			{
				actionPoints.text = System.Math.Round(current_game_action_points, 2) + "/" + maxActionPoints;
				actionPointsSlider.value = current_game_action_points / maxActionPoints;
			}
		}
        
        //update arrtibutes slider in the percentage of highest value
        if(squadUI.GetComponent<SquadUI>().get_if_updated_bool()){
            List<int> updated_units = new List<int>() ;
            if(updated_units.Count < units.Length ){
                float[] max_values = squadUI.GetComponent<SquadUI>().get_max_values();

                Dictionary<string, float> unit_attributes = unit.GetComponent<PlayerController>().getAttributesDic();

                attackPowerSlider.value = unit_attributes["attackPower"] / max_values[0];
                defenseSlider.value = unit_attributes["armor"] / max_values[1];
                speedSlider.value = unit_attributes["speed"] / max_values[2];
                jumpSlider.value = unit_attributes["jumpHeight"] / max_values[3];
                if(!updated_units.Contains(unit_num)){
                    updated_units.Add(unit_num);
                }
            }else{
                squadUI.GetComponent<SquadUI>().set_if_updated_bool(false);
            }
        }
        
	}

}