using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = this.transform.right * -speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerStatus.Instance.setHealth(-1, "");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().popDescriptionText("- Health");
            PlayerStatus.Instance.setSanity(-5);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().popDescriptionText("- Sanity");
            if (PlayerStatus.Instance.getHealth() == 0) PlayerStatus.Instance.setPlayerKilledBy("You are brought knife to a gun fight");
        }
    }
}
