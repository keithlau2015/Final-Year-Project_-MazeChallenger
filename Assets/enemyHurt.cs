using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHurt : MonoBehaviour
{
    [SerializeField]
    private Material hurt, normalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().material = normalMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<EnemyBehaviour>().getBeingAttack())
        {
            this.GetComponent<Renderer>().material = hurt;            
            StartCoroutine(Delay(1.2f));
        }
        else
        {
            this.GetComponent<Renderer>().material = normalMaterial;
        }
    }


    private IEnumerator Delay(float time)
    {
        yield return time;
    }
}
