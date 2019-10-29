using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_sound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<soundcontrol>().music_playing("game_bg");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
