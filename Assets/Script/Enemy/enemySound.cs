using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySound : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> source = new List<AudioClip>();

    private AudioSource soundEffect;
    private float soundEffect_volume;

    private void Awake()
    {
        this.gameObject.AddComponent<AudioSource>();
        soundEffect = GetComponent<AudioSource>();
        soundEffect_volume = 1f;
    }
    
    public void PlaySoundEffect(int id)
    {       
        soundEffect.volume = soundEffect_volume;
        soundEffect.PlayOneShot(source[id]);
    }

    public void AdjustSoundEffectVolume(float volume)
    {
        soundEffect_volume = volume;
    }

    public bool CheckAudioIsPlaying(int id)
    {
        return !soundEffect.isPlaying;
    }
}
