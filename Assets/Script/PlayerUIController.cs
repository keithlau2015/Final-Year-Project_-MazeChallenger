using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject pickUpSight, frontSight;

    [SerializeField]
    private Text Health, Hunger, Coins, Price;

    // Update is called once per frame
    void FixedUpdate()
    {
        Price.enabled = PlayerStatus.Instance.getPlayerCanInteractWithVendingMachine();
        Price.text = PlayerStatus.Instance.getPriceUIText();
        pickUpSight.SetActive(PlayerStatus.Instance.getPlayerCanInteractWithOtherObject());
        frontSight.SetActive(!PlayerStatus.Instance.getPlayerCanInteractWithVendingMachine());
        Hunger.text = PlayerStatus.Instance.getHunger().ToString();
        Health.text = PlayerStatus.Instance.getHealth().ToString();
        Coins.text = PlayerStatus.Instance.getCoins().ToString();
    }
}
