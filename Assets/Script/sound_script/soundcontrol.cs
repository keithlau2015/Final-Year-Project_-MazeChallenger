using UnityEngine.Audio;
using System;
using UnityEngine;

public class soundcontrol : MonoBehaviour
{
    public Sound[] sounds;
    public soundcontrol instance;
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

     public void Play(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			Debug.Log("warnning, there is a error, name not found");
			return;
		}
		s.source.Play();
	}

    //FindObjectOfType<AudioManager>().Play("spear_atk_sound");
    // For other script using
}
