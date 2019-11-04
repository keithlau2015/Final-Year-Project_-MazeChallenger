using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject pickUpSight;

    [SerializeField]
    private Text Health, Hunger;

    // Update is called once per frame
    void FixedUpdate()
    {
        pickUpSight.SetActive(PlayerStatus.Instance.getPlayerCanInteractWithOtherObject());
        Hunger.text = PlayerStatus.Instance.getHunger().ToString();
        Health.text = PlayerStatus.Instance.getHealth().ToString();
    }
}
