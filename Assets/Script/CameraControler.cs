using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Vector3[] Positions;
    [SerializeField] private float[] XRotations;
    [SerializeField] private float[] YRotations;

    //[SerializeField] private Image[] Arrows;
    private int mCurrentIndex = 0;
    
    private void Start()
    {
        transform.position = Positions[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPos = Positions[mCurrentIndex];
        float currentXRot = XRotations[mCurrentIndex];
        float currentYRot = YRotations[mCurrentIndex];

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(0);
            if (mCurrentIndex < 2)
            {
                mCurrentIndex++;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(0);
            if (mCurrentIndex > 0)
            {
                mCurrentIndex--;
            }
        }
        /*
        if (mCurrentIndex == 0)
        {
            Arrows[1].gameObject.SetActive(false);
        }
        else if (mCurrentIndex == 2)
        {
            Arrows[0].gameObject.SetActive(false);
        }
        else
        {
            Arrows[0].gameObject.SetActive(true);
            Arrows[1].gameObject.SetActive(true);
        }
        */

        transform.position = Vector3.Lerp(transform.position, currentPos, 2 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(currentXRot, currentYRot, 0);
    }
}
