using UnityEngine.Audio;
using System;
using UnityEngine;

public class soundcontrol : MonoBehaviour
{
    public Sound[] sounds;
    public soundcontrol instance;
    public float playRate = 1;
    private float nextPlayTime = 0;

    //public static AudioManager instance;

	void Awake()
	{
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


		foreach(Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
		}
        if(sounds == null)
        {
            Debug.Log("Error");
        }
	}

     public void wepon_atk(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);

		if(Time.time > nextPlayTime && !s.source.isPlaying)
        {
        Debug.Log("Played");
        nextPlayTime = Time.time + playRate;
		s.source.PlayOneShot(s.clip);
	    }
     if(s == null)
        {
            Debug.Log("warnning, there is a error, name not found" + "" + name);
            return;
        }
        
    }

    public void character(string name, bool sound_stop)
    {
     Sound s = Array.Find(sounds, sound => sound.name == name);
     if(s == null)
     {
        Debug.Log("warnning, there is a error, name not found" + "" + name);
        return;
     }
     if(sound_stop)
    {
        s.source.Stop();
       
    }

    s.source.Play(); 
    }

    //FindObjectOfType<soundcontrol>().Play("spear_atk_sound");
    // For other script using
}
