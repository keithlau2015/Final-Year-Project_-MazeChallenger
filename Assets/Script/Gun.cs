using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Transform shotSpwan, bullet;

    [SerializeField]
    private GameObject FlipAnimationGameObject;

    private Animator gunAnimator, flipGunAnimator;

    private void Awake()
    {
        gunAnimator = this.GetComponent<Animator>();
        flipGunAnimator = FlipAnimationGameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if(flipGunAnimator.GetBool("Trigger") || gunAnimator.GetBool("Trigger"))
        {
            flipGunAnimator.SetBool("trigger", false);
            gunAnimator.SetBool("trigger", false);
        }
    }

    public void shooting()
    {
        Vector3 pos = shotSpwan.position;
        Quaternion rotation = shotSpwan.rotation;
        GameObject clone = Instantiate(bullet.gameObject, pos, rotation) as GameObject;
        flipGunAnimator.SetBool("trigger", true);
        gunAnimator.SetBool("trigger", true);
        Destroy(clone, 5f);
    }
}
