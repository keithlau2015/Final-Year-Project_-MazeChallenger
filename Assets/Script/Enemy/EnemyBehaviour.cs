using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private bool collisionWithObject = false, triggerWithPlayer = false, onGround = false;
    private float[] rotationDir = { 90, -90, 180, 45, -45 };
    //private int rotationDirCounter;
    private Animator animator;
    private enemySound soundcontrol;
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
    private void update()
    {
        if(onGround)
        {
            EnemyWalk();
        }
    }

    public void EnemyWalk()
    {
        rigidbody.velocity = transform.forward * status.getEnemySpeed();
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
        if (collisionWithObject)
        {
            int rand = Random.Range(0, rotationDir.Length);
            Vector3 targetYAxis = new Vector3(0, rotationDir[rand], 0);
            transform.rotation = Quaternion.Euler(targetYAxis);
        }
        if (triggerWithPlayer)
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
            collisionWithObject = true;
        }
        if(collision.gameObject.tag == "Ground")
        {
            onGround = true;
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
        if(collision.gameObject.tag == "Ground")
        {
            onGround = true;
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
            collisionWithObject = false;
        }
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
        if(collision.gameObject.tag == "Weapon")
        {
            beingAttack = false;
        }
    }
}
