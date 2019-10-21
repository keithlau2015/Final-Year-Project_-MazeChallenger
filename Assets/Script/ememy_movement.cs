using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ememy_movement : MonoBehaviour
{
	public float lookRadius = 10f;
	GameObject player;
	Transform target;
	NavMeshAgent nav;
	Rigidbody m_Rigidbody;
	//set up animation
	private Animator clip;
	Golem status;

	//set up state
	public enum State   {CHASE, PATROL, INVESTIGATE, DIE}
    public State state;

	//investigate
    private Vector3 investigatespot;
    private float timer = 0f;
    public float InvestigateWait = 10f;

	//insight
	public float heightMultiplier;
	public float sightDist = 20f;

	//patrol time
	private float waittime;
	private float startwaittime = 3f;

	//speed
	float chase_speed = 10f;
	float nevigation_speed = 3f;
	float patrol_speed = 5f;

	// moving spot
	public GameObject[] movespot;
	public Transform[] movespot_transform;
	private int random_spot;

    private void Awake()
    {
        status = GetComponent<Golem>();
	    //target = PlayerController.instance.player.transform;
	    player = GameObject.FindWithTag("Player");
	    target = player.transform;
	    movespot = GameObject.FindGameObjectsWithTag("waypoint");
	    nav = GetComponent<NavMeshAgent>();
	    m_Rigidbody = GetComponent<Rigidbody>();
	    movespot_transform = new Transform[movespot.Length];
	    if(movespot == null)
	     {
	        Debug.Log("There is no array");
	     }
	    for(int i=0; i<movespot.Length; i++)
	    {
		    movespot_transform[i] = movespot[i].transform;
		    Debug.Log(i);	
	    }
    }

    private void Start()
    {
	    state = State.PATROL;
	    heightMultiplier = 1.36f;
	    waittime = startwaittime;
	    random_spot = Random.Range(0, movespot_transform.Length);
	    Debug.Log(movespot.Length);
	    Debug.Log(random_spot);
    }

    private void Update()
    {
	    float distance = Vector3.Distance(target.position, transform.position);
	    /*if(status.health == 0)
	    {
		    state = State.DIE;
		    Die();
	    }*/
	    if(distance >= lookRadius)
	    {
		    state = State.PATROL;
	    }

        switch (state)
        {
            case State.CHASE:
                Chase(distance);
                break;
            case State.INVESTIGATE:
                Invistigate(distance);
                break;
            case State.PATROL:
                Patrol(distance);
                break;
            case State.DIE:
                Die();
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
	    Gizmos.color = Color.red;
	    Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void FaceTarget()
    {
	    Vector3 direction = (target.position - transform.position).normalized;
	    Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
	    transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * 3f);
    }

    private void Chase(float distances)
    {
	    if(state == State.CHASE)
	    {
		    nav.SetDestination(target.position);
		    nav.speed = chase_speed;
		    if(nav.remainingDistance < 2f)
		    {
			    Debug.Log("Has been attack!");
			    GetComponent<Animator>().SetBool("attack", true);
		    }
		    FaceTarget();
	    }
    }

    private void Patrol(float distances)
    {
	    Vector3 movingspot = movespot_transform[random_spot].position;
	    Vector3 relativePos = movingspot - transform.position;
	    float distance_way = Vector3.Distance(movingspot, transform.position);

	    nav.speed = patrol_speed;
	    nav.SetDestination(movingspot);
	    transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
	    if(distances <= lookRadius)
	    {
		    state = State.INVESTIGATE;
		    Invistigate(distances);
		}

	    if(waittime <= 0)
	    {	
		    random_spot = Random.Range(0, movespot_transform.Length);
		    waittime = startwaittime;
		    nav.speed = patrol_speed;
	    }
	    else
	    {
		    if(distance_way <= 13f)
		    {
		        nav.speed = 0f;
		        waittime -= Time.deltaTime;
		    }
	    }
    }

    private void Die()
    {
	    GetComponent<Animator>().SetBool("die", true);
	    Debug.Log("Destroy");
	    Destroy(gameObject);
    }

    private void Invistigate(float distances)
    {
	    timer += Time.deltaTime;	
	    nav.SetDestination(target.position);
	    transform.LookAt(target.position);
	    if(timer >= InvestigateWait)
	    {
	    	state = State.PATROL;
		    timer = 0;
	    }
    }

    private void FixedUpdate()
    {
	    RaycastHit hit;
	    Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDist, Color.green);
	    Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized * sightDist, Color.green);
	    Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized * sightDist, Color.green);

	    if(Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, sightDist))
	    {
		    if(hit.collider.gameObject.tag == "Player")
		    {
			    state = State.CHASE;
		    }
	    }

	    if(Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized, out hit, sightDist))
	    {
		    if(hit.collider.gameObject.tag == "Player")
		    {
			    state = State.CHASE;
		    }
	    }

	    if(Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized, out hit, sightDist))
	    {
		    if(hit.collider.gameObject.tag == "Player")
		    {
			    state = State.CHASE;
		    }
	    }

        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized, out hit, sightDist))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                state = State.CHASE;
            }
        }
    }

    private IEnumerator anim_waiting()
    {
        yield return new WaitForSeconds(clip.GetCurrentAnimatorStateInfo(0).length + clip.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
}

