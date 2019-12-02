using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject deadEffect;

    [SerializeField]
    private List<GameObject> Object = new List<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon") && PlayerStatus.Instance.getPlayerAttacking())
        {
            Vector3 pos = this.transform.position;
            Quaternion rotation = this.transform.rotation;
            GameObject clone = Instantiate(deadEffect, pos, rotation) as GameObject;
            clone.transform.localScale = this.gameObject.transform.localScale;
            if(Object.Capacity > 0)Instantiate(Object[Random.Range(0, Object.Capacity)], pos, rotation);
            Destroy(clone, 10);
            Destroy(this.gameObject);
        }
    }
}