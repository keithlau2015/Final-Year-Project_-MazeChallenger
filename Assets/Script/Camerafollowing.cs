using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Camerafollowing : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    //滑鼠靈敏度
    public float Sensitive_V = 10f;
    public float Sensitive_H = 10f;


    //最大視野(單位度)
    public float ViewLimit_V = 120f;

    public float ViewLimit_H = 190f;



    private float Rotation_V = 0f;

    void Start()
    {
        offset = target.position - this.transform.position;
        this.transform.Rotate(0, ViewLimit_H / 2, 0);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //請最好將camera的inspector中的Rotation也改為(0, ViewLimit_H / 2, 0),以免覆蓋

    }
    void Update()
    {

        this.transform.position = target.position - offset;
        Rotation_V -= Input.GetAxis("Mouse Y") * Sensitive_V;

        Rotation_V = Mathf.Clamp(Rotation_V, -ViewLimit_V / 2, ViewLimit_V / 2);
        float Rotation_H = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Sensitive_H;

        Rotation_H = Mathf.Clamp(Rotation_H, 0, ViewLimit_H);
        transform.localEulerAngles = new Vector3(Rotation_V, Rotation_H, 0);
    }
}
