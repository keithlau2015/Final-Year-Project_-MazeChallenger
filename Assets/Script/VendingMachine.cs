using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    public int health;
    public Transform displayPoint;
    public List<GameObject> effect = new List<GameObject>();
    public List<GameObject> sellableObjects = new List<GameObject>();
    public List<GameObject> vendingMachine = new List<GameObject>();
    public GameObject sellObject, glass;
    public TextMesh text;

    // Start is called before the first frame update
    private void Start()
    {
        health = 100;
        int rand = Random.Range(0, sellableObjects.Capacity);
        sellObject = Instantiate(sellableObjects[rand], displayPoint) as GameObject;
    }

    private void Update()
    {
        if(health == 0)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    Destroy(this.gameObject);
                    GameObject effectClone = Instantiate(effect[Random.Range(0, effect.Capacity)], this.transform) as GameObject;
                    Destroy(effectClone, 5);
                    Instantiate(vendingMachine[2], this.transform);
                    break;

                case 1:
                    Destroy(this.gameObject);
                    GameObject effectClone_1 = Instantiate(effect[Random.Range(0, effect.Capacity)], this.transform) as GameObject;
                    Destroy(effectClone_1, 5);
                    Instantiate(vendingMachine[2], this.transform);
                    Instantiate(sellObject, displayPoint);
                    break;

                case 2:
                    text.text = "You think you can destory me?";
                    break;
            }
        }
    }

    public void Jammed()
    {
        //PlayerStatus.Instance.setCoins(-sellObject);
        text.color = Color.red;
        text.text = "ERROR";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            health--;
        }
    }

}
