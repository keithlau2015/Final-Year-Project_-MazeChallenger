using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_sound_effect : MonoBehaviour
{
	Animator anim_status;
	float timer;
    // Start is called before the first frame update
    void Start()
    {
        anim_status = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    	timer += Time.deltaTime;
        if(timer > 5f && anim_status.GetBool("idle"))
        {
        	FindObjectOfType<soundcontrol>().character("Idle");
        }
        if(anim_status.GetBool("attack"))
        {
        	FindObjectOfType<soundcontrol>().wepon_atk("Attack");
        }
        if(anim_status.GetBool("hurt"))
        {
        	
        }
        if(anim_status.GetBool("walk"))
        {
        	FindObjectOfType<soundcontrol>().wepon_atk("Walking");
        }

    }
}
