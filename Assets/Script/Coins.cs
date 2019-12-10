using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public float spawningRate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && this.gameObject.name == "Coin(Clone)")
        {
            PlayerStatus.Instance.setCoins(+1);
            other.GetComponent<PlayerController>().popDescriptionText("+ Coin");
            FindObjectOfType<SoundManager>().PlaySoundEffect(15);
            Destroy(this.gameObject);
        }
    }
}
