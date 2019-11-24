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
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        status = new Enemy();
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

    

    private void EnemyDie()
    {
        //create a die effect
        //Instantiate()
        //Destroy()


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
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            onGround = true;
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
    }
}
