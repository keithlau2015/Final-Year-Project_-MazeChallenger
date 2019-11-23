using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    private Animator animator;
    private Enemy status;
    private EnemyBehaviour enemyBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        status = this.GetComponent<Enemy>();
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyBehaviour.EnemyRotation();
        if (status.getOnGround())
        {
            enemyBehaviour.EnemyWalk();
        }
        else if (status.getInsideAttackArea())
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            animator.SetInteger("Satus", 2);
            EnemyAttack();
        }

        if(status.getEnemyHealth() == 0)
        {
            enemyBehaviour.EnemyDie();
        }
    }

    private void EnemyAttack()
    {
        /*
                onGround = false;
                animator.SetBool("walk", false);
                animator.SetBool("idle", false);
                animator.SetBool("attack", true);
                if (!isSpawnAttackArea )
                {
                    GameObject clone = Instantiate(dmgArea, attackAreaPosition) as GameObject;
                    isSpawnAttackArea = true;
                    Destroy(clone, 2);
                }
                if (!insideAttackArea)
                {
                    animator.SetBool("attack", false);
                    animator.SetBool("idle", false);
                    animator.SetBool("walk", true);
                }
        */
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (!animator.IsInTransition(0) && currentState.fullPathHash == Animator.StringToHash("Base Layer.attack_mode") && status.getInsideAttackArea())
        {
            if (status.getAttack_pattern() == 0)
            {
                status.setAttack_pattern(Random.Range(1, 3));
                if (status.getAttack_pattern() == 1)
                {
                    animator.SetInteger("attackpattern", 1);
                }
                if (status.getAttack_pattern() == 2)
                {
                    animator.SetInteger("attackpattern", 2);
                }
            }
            else
            {
                status.setAttack_pattern(0);
            }

            Debug.Log(status.getAttack_pattern());

        }


        if (!status.getInsideAttackArea())
        {
            animator.SetInteger("Satus", 1);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
