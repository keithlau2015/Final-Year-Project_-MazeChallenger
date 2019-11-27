using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class is only for Golem Behaviour
*/
public class Golem : MonoBehaviour
{
	private Animator animator;
	private int attack_pattern = 0;
	private EnemyBehaviour enemyBehaviour;
    private Enemy status;
    private Rigidbody rigidbody;
    private bool attack = false;
    public PlayerController player;
    public GameObject atk_point;
    public GameObject atk_area;


    //Draw the line from the piovt point
    [SerializeField]
    private Transform attackAreaPosition, pivotPoint;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        status = GetComponent<Enemy>();
    }

     private void Update()
     {	
        if(status.getEnemyHealth() == 0) 
        {
            Vector3 offset = new Vector3(0, 5, 0);
            enemyBehaviour.EnemyDie(offset);
        }
        EnemyDetection();
        enemyBehaviour.EnemyRotation();
     }
     
     private void EnemyDetection()
     {
        RaycastHit hit;
        if (Physics.SphereCast(pivotPoint.position, 5f, transform.forward, out hit, 20f))
        {
            var hitTarget = hit.transform;
            if (hitTarget.CompareTag("Player"))
            {
            	animator.SetInteger("Satus", 2);
                EnemyAttack();
            }
        }
        else
        {
        	animator.SetInteger("Satus", 1);
            enemyBehaviour.EnemyWalk();
        }
    }


  // Tom 呢度你整, 整完你同我講, 我要問你個animation
    private void EnemyAttack()
    {
    	AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if(!animator.IsInTransition (0) && currentState.fullPathHash == Animator.StringToHash ("Base Layer.Armature|walking"))
        {
        	player.already_atk = false;
            if(attack_pattern == 0 && !attack)
            {
            	attack = true;
                if(Random.value > 0.7)
                {
                	attack_pattern = 2;
                }
                if(Random.value > 0.3)
                {
                	attack_pattern = 1;
                }
                if(attack_pattern == 1)
                {
                    animator.SetInteger("attackpattern", 1);
                }
                if(attack_pattern == 2)
                {
                    animator.SetInteger("attackpattern", 2);
                }
            }
            else
            {
            	attack = false;
                attack_pattern = 0;  
            }
        }
        else if(attack && animator.IsInTransition(0))
        {
        	GameObject clone = Instantiate(atk_area, atk_point.transform.position, atk_point.transform.rotation) as GameObject;
        	Destroy(clone, 1);
        	Debug.Log("Spawn sccess");
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //increase speed
            status.setEnemySpeed(+10, "");
            Debug.Log("Enemy speed" + status.getEnemySpeed());
        }
    }
        
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //increase speed
            status.setEnemySpeed(-10, "");
            Debug.Log("Enemy speed" + status.getEnemySpeed());
        }
    }
}
