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
    private bool attack = false, spawn = false;
    public PlayerController player;
    public GameObject atk_point;
    public GameObject atk_area;
    private GameObject clone;
    private int layerMask;


    //Draw the line from the piovt point
    [SerializeField]
    private Transform attackAreaPosition, pivotPoint;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Player");
        animator = GetComponent<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        status = GetComponent<Enemy>();
        status.setEnemyHealth(2, "");
        status.setEnemySpeed(+10, "");
    }

     private void Update()
     {	
        if(status.getEnemyHealth() <= 0) 
        {
            Vector3 offset = new Vector3(0, 5, 0);
            enemyBehaviour.EnemyDie(offset);
        }
        enemyBehaviour.EnemyRotation();
        EnemyDetection();
     }
     
     private void EnemyDetection()
     {
        RaycastHit hit;
        if (Physics.SphereCast(pivotPoint.position, 5f, transform.forward, out hit, 20f, layerMask))
        {
            Debug.Log("engadge");
            var hitTarget = hit.transform;
            if (hitTarget.CompareTag("Player"))
            {
            	animator.SetInteger("Status", 2);
                EnemyAttack();
            }
        }
        else
        {
            Debug.Log("walk");
            animator.SetInteger("Status", 1);
            enemyBehaviour.EnemyWalk();
        }
    }

    private void EnemyAttack()
    {
        if(animator.GetInteger("Status") == 2 && animator.IsInTransition(0) && clone == null)
        {
            spawn = true;
            attack = true;
            clone = Instantiate(atk_area, atk_point.transform.position, atk_point.transform.rotation) as GameObject;
            if (this.gameObject != null) Destroy(clone, 2);
            else Destroy(clone);
        }
        else
        {
            spawn = false;
        }

        if(!animator.IsInTransition (0) && animator.GetInteger("Status") == 2)
        {
            attack = false;
            if(attack_pattern == 0 && !attack)
            {
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
                attack_pattern = 0;
            }
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
