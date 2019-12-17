using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField]
    private Transform shotSpawn, player;

    [SerializeField]
    private GameObject projectile;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = player.position * 5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject clone = Instantiate(projectile, shotSpawn) as GameObject;
            Destroy(clone, 3);
        }
    }
}
