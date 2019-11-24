using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
	private bool triggerWithPlayer = false, onGround = false, 
	insideAttackArea = false, isSpawnAttackArea = false, attacking = false;

	private Animator animator;
	private int attack_pattern = 0;
	private EnemyBehaviour behaviour;
    [SerializeField]
    private Transform attackAreaPosition, pivotPoint;
    [SerializeField]
    private GameObject dmgArea;
    private Enemy status;
    private Rigidbody rigidbody;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        
        
    }
     private void update()
     {
     	AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (insideAttackArea)
        {
            rigidbody.velocity = Vector3.zero;
            animator.SetInteger("Satus", 2);
            EnemyAttack(currentState);
            attacking = true;
        }
        else 
        {
            attacking = false;
        }
     }
     
     private void EnemyDetection()
     {
     	animator.SetInteger("Satus", 1);
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
    	private void EnemyAttack(AnimatorStateInfo currentState)
    	{
         onGround = false;
         float TotalSpeed = status.getEnemySpeed();
         if(!animator.IsInTransition (0) && currentState.nameHash == Animator.StringToHash ("Base Layer.attack_mode") && insideAttackArea)
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

            if(attacking)
            {
            //speed is 0
                status.setEnemySpeed(-TotalSpeed);
            }
            else
            {
            //speed is back nonormal
                status.setEnemySpeed(TotalSpeed);
            }       
         }

         if(currentState.nameHash == Animator.StringToHash ("Base Layer.Armature|attack_1") || currentState.nameHash == Animator.StringToHash ("Base Layer.Armature|attack_1"))
         {

            FindObjectOfType<enemySound>().PlaySoundEffect(0);
         }

         if (!isSpawnAttackArea)
            {
                GameObject clone = Instantiate(dmgArea, attackAreaPosition) as GameObject;
                isSpawnAttackArea = true;
                Destroy(clone, 4000);
            }

            if(!insideAttackArea)
            {   
                animator.SetInteger("Satus", 1);
            }
    	}

    	private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            attacking = true;
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
            insideAttackArea = false;
            attacking =false;
            status.setEnemySpeed(-10);
            Debug.Log("Enemy speed" + status.getEnemySpeed());
        }
    }
}
