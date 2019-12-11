using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //rotation angles
    private float[] rotationDir = { 90, -90, 180, 45, -45 };

    //The Dead effect
    [SerializeField]
    private GameObject deadEffect;

    [SerializeField]
    private GameObject[] reward;

    //check about is this enemy being attacked
    private bool collisionWithObject, triggerWithPlayer, onGround, insideAttackArea, beingAttack;

    //Need component 
    private Enemy status;
    private Animator animator;
    private enemySound soundcontrol;
    private Rigidbody rigidbody;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        status = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {

    }

    public void EnemyWalk()
    {
        //The animation of the walking
        rigidbody.velocity = transform.forward * status.getEnemySpeed();
    }

    public void EnemyDie(Vector3 offset)
    {
        //create a die effect & delete this obj and the effect obj
        Vector3 pos = this.transform.position;
        Quaternion rotation = this.transform.rotation;
        GameObject clone = Instantiate(deadEffect, pos+ offset, rotation) as GameObject;
        GameObject rewardClone = Instantiate(reward[Random.Range(0, reward.Length)], pos, rotation) as GameObject;
        Destroy(clone, 10);
        Destroy(this.gameObject);
    }

     public void EnemyRotation()
    {
        //Whenever this obj is collision sth tag is wall
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
        RaycastHit hit;
        if (Physics.Raycast(this.gameObject.transform.position, transform.forward, out hit, 10f))
        {
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Breakable") || hit.collider.CompareTag("VendingMachine"))
            {
                int rand = Random.Range(0, rotationDir.Length);
                Vector3 targetYAxis = new Vector3(0, rotationDir[rand], 0);
                transform.rotation = Quaternion.Euler(targetYAxis);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //increase speed
            triggerWithPlayer = true;
            insideAttackArea = true;
            status.setEnemySpeed(+10, "");
            Debug.Log("Enemy speed" + status.getEnemySpeed());
        }
    }
        
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //increase speed
            triggerWithPlayer = false;
            insideAttackArea = false;
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
        if(collision.gameObject.tag == "Weapon" && !beingAttack && PlayerStatus.Instance.getPlayerAttacking())
        {
            beingAttack = true;
            status.setEnemyHealth(-(collision.gameObject.GetComponent<Weapon>().damge), "");
            Debug.Log("Enemy Health: " + status.getEnemyHealth());
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
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Breakable") || collision.gameObject.CompareTag("VendingMachine"))
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

    public void setBeingAttack(bool beingAttack)
    {
        this.beingAttack = beingAttack;
    }

    public bool getBeingAttack()
    {
        return this.beingAttack;
    }

}
