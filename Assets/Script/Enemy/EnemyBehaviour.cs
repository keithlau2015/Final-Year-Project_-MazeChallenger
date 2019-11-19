using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private bool collisionWithObject = false, triggerWithPlayer = false, onGround = false, insideAttackArea = false, isSpawnAttackArea = false;
    private float[] rotationDir = { 90, -90, 180, 45, -45 };
    //private int rotationDirCounter;
    private Animator animator;
    private Enemy status;
    private Rigidbody rigidbody;

    [SerializeField]
    private Transform attackAreaPosition, pivotPoint;
    [SerializeField]
    private GameObject dmgArea;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        status = new Enemy();
    }

    // Start is called before the first frame update
    private void Start()
    {
        //rotationDirCounter = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        /*
        else
        { 
            EnemyIdle();
            Debug.Log("idle");
        }
        */
        EnemyRotation();
        if (insideAttackArea)
        {
            rigidbody.velocity = Vector3.zero;
            EnemyAttack();
        }
        else if(onGround)
        {
            EnemyWalk();
        }
    }

    private void EnemyIdle()
    {
        //Animation
        animator.SetBool("idle", true);
        animator.SetBool("walk", false);
        animator.SetBool("attack", false);
    }

    private void EnemyWalk()
    {
        //Animation
        animator.SetBool("idle", false);
        animator.SetBool("attack", false);
        animator.SetBool("walk", true);
                
        rigidbody.velocity = transform.forward * status.getEnemySpeed();

        //Detect the player is in the attack range
        RaycastHit hit;

        if (Physics.SphereCast(pivotPoint.position, 5f, transform.forward, out hit, 20f))
        {
            var hitTarget = hit.transform;
            if (hitTarget.CompareTag("Player"))
            {

                insideAttackArea = true;
            }
            else
            {
                insideAttackArea = false;
            }
        }
    }
    private void EnemyAttack()
    {
        onGround = false;
        animator.SetBool("walk", false);
        animator.SetBool("idle", false);
        animator.SetBool("attack", true);
        if (!isSpawnAttackArea )
        {
            GameObject clone = Instantiate(dmgArea, attackAreaPosition) as GameObject;
            isSpawnAttackArea = true;
            Destroy(clone, 4000);
        }
        if (!insideAttackArea)
        {
            animator.SetBool("attack", false);
            animator.SetBool("idle", false);
            animator.SetBool("walk", true);
        }

    }
    private void EnemyDie()
    {
        //create a die effect
        //Instantiate()
        //Destroy()


        Destroy(this.gameObject);
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
