using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownFloatingEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    private Animator animator;
    private EnemyBehaviour enemyBehaviour;
    private Enemy status;


    //Draw the line from the piovt point
    [SerializeField]
    private Transform shotSpawn;

    [SerializeField]
    private float nextFire, fireRate;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        status = GetComponent<Enemy>();
    }

    private void Start()
    {
        status.setEnemySpeed(+10, "");
    }

    // Update is called once per frame
    void Update()
    {
        if (status.getEnemyHealth() <= 0)
        {
            Vector3 offset = new Vector3(0, 2, 0);
            enemyBehaviour.EnemyDie(offset);
        }
        enemyBehaviour.EnemyRotation();
        EnemyDetection();
    }

    private void EnemyDetection()
    {
        RaycastHit hit;
        if (Physics.SphereCast(shotSpawn.position, 5f, transform.forward, out hit, 50f))
        {
            var hitTarget = hit.transform;
            if (hitTarget.CompareTag("Player"))
            {
                Vector3 pos = shotSpawn.position;
                Quaternion rotation = shotSpawn.rotation;
                animator.SetInteger("Status", 1);
                if (Time.time > nextFire && Time.timeScale != 0)
                {
                    nextFire = Time.time + fireRate;
                    GameObject clone = Instantiate(projectile, pos, rotation) as GameObject;
                    Destroy(clone, 5);
                }
            }
            else
            {
                enemyBehaviour.EnemyWalk();
                animator.SetInteger("Status", 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
