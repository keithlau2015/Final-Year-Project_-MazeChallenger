using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{
    public int health, price, rand;
    public Transform displayPoint, spawnPickablePoint;
    public List<GameObject> effect = new List<GameObject>();
    public List<GameObject> sellableObjects = new List<GameObject>();
    public List<GameObject> vendingMachine = new List<GameObject>();
    public GameObject  glass;
    public float spawningRate;
    private bool checkIsPurchase;

    // Start is called before the first frame update
    private void Start()
    {
        price = Random.Range(50, 200);
        Debug.Log(price);
        health = 100;
        rand = Random.Range(0, sellableObjects.Capacity);
        Instantiate(sellableObjects[rand], displayPoint);
    }

    private void Update()
    {
        if (health == 0)
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
                    Instantiate(sellableObjects[rand], displayPoint);
                    break;

                case 2:
                    PlayerStatus.Instance.setPriceUIText("You think you can destory me?");
                    break;
            }
        }
    }

    public void Jammed()
    {
        //PlayerStatus.Instance.setCoins(-sellObject);
        PlayerStatus.Instance.setPriceUIText("ERROR");
    }

    public int getPrice()
    {
        return price;
    }

    public void purchaseSuccess()
    {
        if (displayPoint.gameObject.activeInHierarchy)
        {
            displayPoint.gameObject.SetActive(false);
            Vector3 pos = spawnPickablePoint.position;
            Quaternion rotation = spawnPickablePoint.rotation;
            Instantiate(sellableObjects[rand], pos, rotation);
            checkIsPurchase = true;
        }
    }

    public bool getCheckIsPurchase()
    {
        return checkIsPurchase;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            health--;
        }
    }

}
