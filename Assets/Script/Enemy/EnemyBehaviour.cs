using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private bool collisionWithObject = false, triggerWithPlayer = false;
    private float[] rotationDir = { 90, -90, 180, -180 };
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
        //status.setEnemySpeed();
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyRotation();        
        /*
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            EnemyIdle();
            Debug.Log("idle");
        }
        else
        */
        EnemyWalk();
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
                
        rigidbody.velocity = transform.forward * Time.deltaTime * status.getEnemySpeed();
    }

    private void EnemyAttack()
    {

    }

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
            Vector3 targetYAxis = new Vector3( rotationDir[rand], this.transform.position.y, rotationDir[rand]);
            transform.LookAt(targetYAxis);
        }
        else if (triggerWithPlayer)
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //increase speed
            triggerWithPlayer = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            collisionWithObject = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            collisionWithObject = false;
        }
    }
}
