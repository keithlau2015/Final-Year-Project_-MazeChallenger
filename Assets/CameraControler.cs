using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
	public Transform Camera;
	public Transform start_game;
	public Transform setting;
	public Transform brunner;

	//private Vector3 start_game_pos = new Vector3(-103.17f, 14.69f, -62.84f);
	//private Vector3 setting_pos = new Vector3(-116.4f, 13.3f,-110.3f);
	//private Vector3 brunner_pos = new Vector3(-22.47f, 11.44f, -88.4f);

	private float panSpeed = 20f;
	private float time = 1f;

	//public float smooth = 2.0F;
	void Start()
	{
		Camera.transform.position = start_game.transform.position;
	}
	
    void Update()
    {

    	if (Input.GetKeyDown(KeyCode.A))
    	{
    		pos_change(start_game, setting);
    	}
    	if(Input.GetKeyDown(KeyCode.D))
    	{
    		pos_change(setting, start_game);
    	}
    }

    private void pos_change(Transform from, Transform to)
    {
             transform.position = Vector3.Lerp(from.position, to.position, time);
             transform.rotation = Quaternion.Slerp (from.rotation,to.rotation, panSpeed*Time.deltaTime);
             
    }
}
