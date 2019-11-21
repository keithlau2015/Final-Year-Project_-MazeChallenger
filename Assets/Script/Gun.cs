using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Transform shotSpwan, bullet;

    [SerializeField]
    private GameObject FlipAnimationGameObject, fireVFX;

    [SerializeField]
    private float nextFire, fireRate;

    private Animator gunAnimator, flipGunAnimator;

    private void Awake()
    {
        gunAnimator = this.GetComponent<Animator>();
        flipGunAnimator = FlipAnimationGameObject.GetComponent<Animator>();
    }

    public void shooting()
    {
        Vector3 pos = shotSpwan.position;
        Quaternion rotation = shotSpwan.rotation;
        FindObjectOfType<PlayerController>().setIsShootingTrue();
        //if (Time.time > nextFire && Time.timeScale != 0)
        //{
        //  nextFire = Time.time + fireRate;
        GameObject vfx = Instantiate(fireVFX, shotSpwan);
        Destroy(vfx, 2);
        GameObject clone = Instantiate(bullet.gameObject, pos, rotation) as GameObject;
        Destroy(clone, 5f);
        //}
    }

    public void animationTrigger()
    {
        flipGunAnimator.SetTrigger("Handgun");
        gunAnimator.SetTrigger("Handgun");
    }
}
