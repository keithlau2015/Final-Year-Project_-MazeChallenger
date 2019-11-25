﻿using System.Collections;
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
                //Attack
            }
        }
        else
        {
            enemyBehaviour.EnemyWalk();
        }
    }

/*
 *  Tom 呢度你整, 整完你同我講, 我要問你個animation
    private void EnemyAttack(AnimatorStateInfo currentState)
    {
        if(!animator.IsInTransition (0) && currentState.fullPathHash == Animator.StringToHash ("Base Layer.attack_mode") && insideAttackArea)
         {
            if(attack_pattern == 0)
            {
                attack_pattern = Random.Range(1,3);
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

            if()
            {
            //speed is 0
                status.setEnemySpeed(,"");
            }
            else
            {
            //speed is back nonormal
                status.setEnemySpeed(,"");
            }       
        }
        if()
        {   
            animator.SetInteger("Satus", 1);
        }
    }
*/

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
