using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform[] idleWayPoints, attackAreaPosition;

    private Transform player;

    private List<Transform> walkedPosition;

    private new Rigidbody rigidbody;

    private Animator enemyAnimator;

    private Enemy enemyStatus;

    private GameObject[] attackArea;

    private bool inAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        enemyAnimator = GetComponent<Animator>();
        enemyStatus = GetComponent<Enemy>();
        player = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnemyMovement(Transform target)
    {
        //At the very beginning
        if (walkedPosition.Capacity == 0 && target.Equals(null))
        {
            int rand = Random.Range(0, idleWayPoints.Length);
            walkedPosition.Add(idleWayPoints[rand]);
            Vector3 velocity = transform.TransformDirection(idleWayPoints[rand].position) * enemyStatus.getEnemySpeed();
            rigidbody.velocity = velocity;
            this.transform.rotation = Quaternion.Euler(0, idleWayPoints[rand].rotation.y, 0);

            //Animation part
            enemyAnimator.SetBool("walking", true);
            enemyAnimator.SetBool("idle", false);
        }

        //Walk to another waypoint
        else if(this.transform.position == walkedPosition[0].position && target.Equals(null))
        {
            float idleRate = Random.Range(0f, 1f);
            int rand = Random.Range(0, idleWayPoints.Length);

            //idle
            if(this.transform.position == idleWayPoints[rand].position && idleRate <= 0.3)
            {
                //Animation part
                enemyAnimator.SetBool("walking", false);
                enemyAnimator.SetBool("idle", true);
            }

            //run this function again
            else if (this.transform.position == idleWayPoints[rand].position && idleRate > 0.7)
            {
                EnemyMovement(target);
            }

            //walking to another waypoint
            else
            {
                walkedPosition.Clear();
                walkedPosition.Add(idleWayPoints[rand]);
                Vector3 velocity = transform.TransformDirection(idleWayPoints[rand].position) * enemyStatus.getEnemySpeed();
                rigidbody.velocity = velocity;
                this.transform.rotation = Quaternion.Euler(0, idleWayPoints[rand].rotation.y, 0);

                //Animation part
                enemyAnimator.SetBool("walking", true);
                enemyAnimator.SetBool("idle", false);
            }
        }

        //when detected player
        else if(target.tag == "Player")
        {
            enemyStatus.setEnemySpeed(10);
            Vector3 velocity = transform.TransformDirection(target.position) * enemyStatus.getEnemySpeed();
            this.rigidbody.velocity = velocity;
            this.transform.rotation = Quaternion.Euler(0, target.rotation.y, 0);
        }

        //getting expection 
        else
        {
            Debug.LogError("got exception in Enemy Movement");
        }
    }

    private void EnemyWalking()
    {
        Transform temp = null;
        EnemyMovement(temp);
    }

    private void EnemyAttack()
    {
        EnemyMovement(player);
        enemyAnimator.SetBool("walking", false);
        enemyAnimator.SetBool("attack_1", true);
        Instantiate(attackArea[0], attackAreaPosition[0]);
        if (this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack_1"))
        {
            enemyAnimator.SetBool("attack_1", false);
            enemyAnimator.SetBool("attack_2", true);
            Instantiate(attackArea[1], attackAreaPosition[1]);
            if (this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack_2"))
            {
                enemyAnimator.SetBool("attack_2", false);
                enemyAnimator.SetBool("attack_3", true);
                Instantiate(attackArea[2], attackAreaPosition[2]);
                if (this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack_3"))
                {
                    enemyAnimator.SetBool("attack_3", false);
                }
            }
        }
    }

    private void EnemyDie()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            player = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            PlayerStatus.Instance.setSpeed(-20, "");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            PlayerStatus.Instance.setSpeed(20, "");
        }
    }

}
