using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Camera))]
public class MouseFollow : MonoBehaviour
{
    private const int Y_ROT = 50; //up 70 + down 70 = 140
    
    private const int X_ROT = 180; //left 180 + right 180 = 360
    public float rotationSpeed = 10; //cursor speed
    
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        Rotate(new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * rotationSpeed);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        CursorControl();
    }

    public static void CursorControl()
    {
        if(!PlayerStatus.Instance.getPlayerGetIntoNextLevel() && !PlayerStatus.Instance.getPlayerAtTheMenu() && Time.timeScale != 0) 
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Rotate(Vector2 distance)
    {
        Vector3 rot = transform.eulerAngles;
        float rot_x = rot.x + distance.y;
        float rot_y = rot.y + distance.x;
        rot_x = Mathf.Clamp(rot_x > 180 ? rot_x - 360 : rot_x, -Y_ROT, Y_ROT);
        rot_y = Mathf.Clamp(rot_y > 180 ? rot_y - 360 : rot_y, -X_ROT, X_ROT);
        if (FindObjectOfType<PlayerController>().getIsShooting() && Input.GetMouseButtonDown(0))
        {
            transform.rotation = Quaternion.Euler(rot_x-3, rot_y, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(rot_x, rot_y, 0);
        }
    }

}