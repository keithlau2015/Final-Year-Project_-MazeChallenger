using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{    
    private float[] rotationDir = { 90, -90, 180, 45, -45 };
    //private int rotationDirCounter;
    private Animator animator;
    private Enemy status;
    private Rigidbody rigidbody;
    [SerializeField]
    private Transform attackAreaPosition, pivotPoint;
    [SerializeField]
    private GameObject deadEffect;

    private bool beingAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        status = GetComponent<Enemy>();
        beingAttack = false;
    }
    
    public void EnemyWalk()
    {
        //Animation
         animator.SetInteger("Satus", 1);
                
        rigidbody.velocity = transform.forward * status.getEnemySpeed();

        //Detect the player is in the attack range
        RaycastHit hit;

        if (Physics.SphereCast(pivotPoint.position, 5f, transform.forward, out hit, 20f))
        {
            var hitTarget = hit.transform;
            if (hitTarget.CompareTag("Player"))
            {

                status.setInsideAttackArea(true);
            }
            else
            {
                status.setInsideAttackArea(false);
            }
        }
    }

    public void EnemyDie(Vector3 offset)
    {
        //create a die effect
        Vector3 pos = this.transform.position;
        Quaternion rotation = this.transform.rotation;
        GameObject clone = Instantiate(deadEffect, pos+ offset, rotation) as GameObject;
        Destroy(clone, 10);
        Destroy(this.gameObject);
    }

    public void EnemyRotation()
    {
        if (status.getEnemyCollisionWithObject())
        {
            int rand = Random.Range(0, rotationDir.Length);
            Vector3 targetYAxis = new Vector3(0, rotationDir[rand], 0);
            transform.rotation = Quaternion.Euler(targetYAxis);
        }
        if (status.getEnemyTriggerWithPlayer())
        {
            Vector3 targetYAxis = new Vector3(PlayerRef.instance.player.transform.position.x, this.transform.position.y, PlayerRef.instance.player.transform.position.z);
            transform.LookAt(targetYAxis);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //increase speed
            status.setEnemyTriggerWithPlayer(true);
            status.setInsideAttackArea(true);
            status.setEnemySpeed(+10, "");
            Debug.Log("Enemy speed" + status.getEnemySpeed());
        }
    }
        
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //increase speed
            status.setEnemyTriggerWithPlayer(false);
            status.setInsideAttackArea(false);
            status.setEnemySpeed(-10, "");
            Debug.Log("Enemy speed" + status.getEnemySpeed());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            status.setEnemyCollisionWithObject(true);
        }
        if (collision.gameObject.tag == "Ground")
        {
            status.setOnGround(true);
        }
        if(collision.gameObject.tag == "Weapon" && !beingAttack)
        {
            beingAttack = true;
            status.setEnemyHealth(-1, "");
            Debug.Log("Enemy Health: " + status.getEnemyHealth());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            status.setOnGround(true);
        }
        if(collision.gameObject.tag == "Weapon")
        {
            beingAttack = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            status.setEnemyCollisionWithObject(false);
        }
        if (collision.gameObject.tag == "Ground")
        {
            status.setOnGround(false);
        }
        if(collision.gameObject.tag == "Weapon")
        {
            beingAttack = false;
        }
    }
}
