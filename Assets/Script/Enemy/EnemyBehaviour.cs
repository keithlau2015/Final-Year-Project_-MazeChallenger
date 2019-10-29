using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private bool collisionWithObject = false, triggerWithPlayer = false, onGround = false, insideAttackArea = false;
    private float[] rotationDir = { 90, -90, 180, -180 };
    //private int rotationDirCounter;
    private Animator animator;
    private Enemy status;
    private Rigidbody rigidbody;
    [SerializeField]
    private Transform attackAreaPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        status = new Enemy();
    }

    // Start is called before the first frame update
    private void Start()
    {
        status.setEnemySpeed(+10);
        //rotationDirCounter = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyRotation();
        /*
        else
        { 
            EnemyIdle();
            Debug.Log("idle");
        }
        */
        if(onGround) EnemyWalk();
        //EnemyAttack();
    }

    private void EnemyIdle()
    {
        //Animation
        animator.SetBool("idle", true);
        animator.SetBool("walk", false);
    }

    private void EnemyWalk()
    {
        //Animation
        animator.SetBool("idle", false);
        animator.SetBool("walk", true);
                
        rigidbody.velocity = transform.forward * status.getEnemySpeed();

        //Detect the player is in the attack range
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            insideAttackArea = true;
        }
        else
        {
            insideAttackArea = false;
        }
    }
    /*
    private void EnemyAttack()
    {
        if (insideAttackArea)
        {
            attackAreaPosition.gameObject.AddComponent<SphereCollider>();
            attackAreaPosition.gameObject.GetComponent<SphereCollider>().radius = 5f;
        }
        else
        {
            Destroy(attackAreaPosition.gameObject.GetComponent<SphereCollider>());
        }
    }
    */
    private void EnemyDie()
    {

    }

    private void DrawRayCast()
    {

    }

    private void EnemyRotation()
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
            triggerWithPlayer = true;
            status.setEnemySpeed(+10);
            Debug.Log("Enemy speed" + status.getEnemySpeed());
        }
    }
        
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //increase speed
            triggerWithPlayer = false;
            status.setEnemySpeed(-10);
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
