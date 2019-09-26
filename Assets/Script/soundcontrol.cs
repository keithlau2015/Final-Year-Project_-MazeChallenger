using UnityEngine.Audio;
using UnityEngine;

public class soundcontrol : MonoBehaviour
{
	AudioSource SoundEffect;
	private AudioClip gun_pick, spear_pick, 
			  walking_sound, running_sound,
			  gun_breaking, spear_breaking, sword_breaking,
			  AK_atk, handgun_atk, shortgun_atk, sword_atk, spear_atk;


	public static bool picking_up, walking_effect, running_effect, broke,
						attack, AK47, handgun, shortgun, sword, spear;
    // Start is called before the first frame update
    void Start()
    {
     SoundEffect = GetComponent<AudioSource>();  	   
    }

    // Update is called once per frame
    void picking()
    {
    	if(picking_up && (AK47 || handgun || shortgun))
    	{
    		SoundEffect.PlayOneShot(gun_pick,0.75f);
    	}

    	if(picking_up && spear)
    	{
    		SoundEffect.PlayOneShot(spear_pick,0.75f);
    	}
    }

    void walking()
    {
    	if(walking_effect)
    	{
    		SoundEffect.PlayOneShot(walking_sound, 0.7f);
    	}
    }

    void running()
    {
    	if(running_effect)
    	{
    		SoundEffect.PlayOneShot(running_sound, 0.7f);
    	}
    }


    void breaking()
    {
    	if(broke && (!AK47 || !handgun || !shortgun))
    	{
    		SoundEffect.PlayOneShot(gun_breaking, 0.7f);
    	}
    	if(broke && !spear)
    	{
    		SoundEffect.PlayOneShot(spear_breaking, 0.7f);
    	}
    	if(broke && !sword)
    	{
    		SoundEffect.PlayOneShot(sword_breaking, 0.7f);
    	}
    }

    void attacking()
    {
    	if(attack && AK47)
    	{
    		SoundEffect.PlayOneShot(AK_atk, 0.7f);
    	}

    	if(attack && handgun)
    	{
    		SoundEffect.PlayOneShot(handgun_atk, 0.7f);
    	}

    	if(attack && shortgun)
    	{
    		SoundEffect.PlayOneShot(shortgun_atk, 0.7f);
    	}

    	if(attack && sword)
    	{
    		SoundEffect.PlayOneShot(sword_atk, 0.7f);
    	}

    	if(attack && spear)
    	{
    		SoundEffect.PlayOneShot(spear_atk, 0.7f);
    	}
    	
    }
}
