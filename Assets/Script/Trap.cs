using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Animator[] trapAnimator;

    // Start is called before the first frame update
    void Start()
    {
        trapAnimator = GetComponentsInChildren<Animator>();   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Enemy")
        {
            foreach(Animator animator in trapAnimator){
                animator.SetBool("active", true);
            }

            /*
            for (int i = 0; i < trapAnimator.Length; i++)
            {
                trapAnimator[i].SetBool("active", true);
            }
            */
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag != "Enemy")
        {
            foreach (Animator animator in trapAnimator)
            {
                animator.SetBool("active", false);
            }

            /*
            for (int i = 0; i < trapAnimator.Length; i++)
            {
                trapAnimator[i].SetBool("active", false);
            }
            */
        }
    }
}
