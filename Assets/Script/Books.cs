using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : MonoBehaviour
{
    private int randTip;

    // Start is called before the first frame update
    void Start()
    {
        randTip = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatus.Instance.getPlayerCanInteractWithReadingMaterial())
        {
            if (randTip <= 10) PlayerStatus.Instance.setReadingMaterials("Tip 1: The scroll is used to enhance your sanity");
            else if (randTip > 10 && randTip <= 20) PlayerStatus.Instance.setReadingMaterials("Tip 2: At the starting point, there have a wooden sword on the table");
            else if (randTip > 20 && randTip <= 30) PlayerStatus.Instance.setReadingMaterials("Tip 3: You can try to break the crate, maybe you can get something useful inside");
            else if (randTip > 30 && randTip <= 40) PlayerStatus.Instance.setReadingMaterials("Tip 4: Careful about the floor, there may have traps");
            else if (randTip > 40 && randTip <= 50) PlayerStatus.Instance.setReadingMaterials("Tip 5: If you break the skull, you will feeling much more better");
            else if (randTip > 50 && randTip <= 60) PlayerStatus.Instance.setReadingMaterials("Tip 6: Everyone love Donut, it will help you restore your sanity & hunger");
            else if (randTip > 60 && randTip <= 70) PlayerStatus.Instance.setReadingMaterials("Tip 7: If you had a bad luck when breaking a crate, it will pop out a head skull, and it will made you feel creepy");
            else if (randTip > 70 && randTip <= 80) PlayerStatus.Instance.setReadingMaterials("Tip 8: Barrel is your best friend, try use them to peek for you while you walking over the trap area");
            else if (randTip > 80 && randTip <= 90) PlayerStatus.Instance.setReadingMaterials("Tip 9: Breaking crate will reduce your weapon durability");
            else PlayerStatus.Instance.setReadingMaterials("Tip 10: I would suggest you try not to read the book which in black");
        }
        else
        {
            PlayerStatus.Instance.setReadingMaterials("");
        }
    }
}
