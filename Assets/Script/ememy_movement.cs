using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
	public class ememy_movement : MonoBehaviour
{
	public float lookRadius = 10f;
	Transform target;
	NavMeshAgent nav;
	Rigidbody m_Rigidbody;
	//set up state
	public enum State   {CHASE, PATROL, DIE, INVESTIGATE}
    public State state;
	//investigate
    private Vector3 investigatespot;
    private float timer = 0f;
    public float InvestigateWait = 10f;
	//insight
	public float heightMultiplier;
	public float sightDist = 10f;
	//patrol time
	private float waittime;
	private float startwaittime = 3f;

	//speed
	float chase_speed = 10f;
	float nevigation_speed = 3f;
	float patrol_speed = 5f;
	// moving spot
	public Transform[] movespot;
	private int random_spot;


void Start()
{
	target = PlayerController.instance.player.transform;
	nav = GetComponent<NavMeshAgent>();
	m_Rigidbody = GetComponent<Rigidbody>();
	state = State.PATROL;
	heightMultiplier = 1.36f;
	waittime = startwaittime;
	random_spot = Random.Range(0, movespot.Length);
}

void Update()
{
	float distance = Vector3.Distance(target.position, transform.position);
	if()
	{
		
	}
	switch(state)
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

	if(distance >= lookRadius)
	{
		state = State.PATROL;
	}
	
}

void OnDrawGizmosSelected()
{
	Gizmos.color = Color.red;
	Gizmos.DrawWireSphere(transform.position, lookRadius);
}

void FaceTarget()
{
	Vector3 direction = (target.position - transform.position).normalized;
	Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
	transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * 3f);
}

void Chase(float distances)
{
	if(state == State.CHASE)
	{
		nav.SetDestination(target.position);
		nav.speed = chase_speed;
		if(nav.remainingDistance < 2f)
		{
			Debug.Log("Has been attack!");
			//GetComponent<Animator>().SetTrigger("Attack");
		}
		FaceTarget();
	}
}

void Patrol(float distances)
{
	Vector3 movingPoint = movespot[random_spot].position;
	Vector3 relativePos = movingPoint - transform.position;
	float distance_way = Vector3.Distance(movingPoint, transform.position);

	nav.speed = patrol_speed;
	nav.SetDestination(movingPoint);
	transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
	if(distances <= lookRadius)
		{
		state = State.INVESTIGATE;
		Invistigate(distances);
		}


	if(waittime <= 0)
	{	
		random_spot = Random.Range(0, movespot.Length);
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

void Die()
{
	//GetComponent<Animator>().SetTrigger("die");
	Destroy(gameObject);
}

void Invistigate(float distances)
{
	timer += Time.deltaTime;	
	nav.SetDestination(target.position);
	transform.LookAt(investigatespot);
	if(timer >= InvestigateWait)
	{
		state = State.PATROL;
		timer = 0;
	}

}

void FixedUpdate()
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

}

}





