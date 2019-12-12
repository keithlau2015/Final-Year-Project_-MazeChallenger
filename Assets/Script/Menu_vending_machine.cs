using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Menu_vending_machine : MonoBehaviour
{
	public int Reinforcement;
	private int skill_random_value, weapon_random_value;
	public GameObject WarningPanel, Buying;
	private bool ifopened = false, ifbuy = false;
	public Text text;

    // Start is called before the first frame update
    void Start()
    {
        Reinforcement = PlayerStatus.Instance.getReinforcement();
        text.text = "";
    }


    // Update is called once per frame
    public void veding_open()
    {
    	if(ifopened == false)
    	{
    		Buying.SetActive(true);
    	}
    	
    }

    public void choose_weapon()
    {
    	if(Reinforcement >= 5)
    		{
    			WarningPanel.SetActive(true);
    			if(ifbuy == false)
    			{
    			weapon_random_value = Random.Range(1, 7);
    			PlayerStatus.Instance.setReinforcement(-5);
    			Buying.SetActive(false);
    			ifopened = true;
    			text.text = "Perchase";
    			}
    		}
    		else
    		{
    			if(ifbuy)
    			{
    				text.text = "You have already buy it";
    			}
    			else
    			{
    				text.text = "You don't have enough reinforment";
    			}
    		}
    }

    public void choose_skill()
    {
    	if(Reinforcement >= 10)
    		{
    			WarningPanel.SetActive(true);
    			if(ifbuy == false)
    			{
    			skill_random_value = Random.Range(1, 7);
    			PlayerStatus.Instance.setReinforcement(-10);
    			ifopened = true;
    			text.text = "Perchase";
    			}
    		}
    		else
    		{
    			if(ifbuy)
    			{
    				text.text = "You have already buy it";
    			}
    			else
    			{
    				text.text = "You don't have enough reinforment";
    			}
    		}
    }

    public void choose_money()
    {
    	if(Reinforcement >= 2)
    		{
    			WarningPanel.SetActive(true);
    			if(ifbuy == false)
    			{
    				PlayerStatus.Instance.setCoins(100);
					PlayerStatus.Instance.setReinforcement(-2);
					ifopened = true;
					text.text = "Perchase";
    			}
    			
    		}
    		else
    		{
    			if(ifbuy)
    			{
    				text.text = "You have already buy it";
    			}
    			else
    			{
    				text.text = "You don't have enough reinforment";
    			}
    			
    		}
    }
    public void warining_yes()
    {
    	ifbuy = true;
    	WarningPanel.SetActive(false);
  		Buying.SetActive(false);
    }
    public void warining_no()
    {
    	ifbuy = false;
    	WarningPanel.SetActive(false);
  		Buying.SetActive(true);
    }

    public int skill_random()
    {
    	return skill_random_value;
    }
    public int weapon_random()
    {
    	return weapon_random_value;
    }
}
