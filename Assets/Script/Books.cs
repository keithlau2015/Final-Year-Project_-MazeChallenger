using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : MonoBehaviour
{
    public int randTip;
    private string readingMaterials;

    // Start is called before the first frame update
    private void Start()
    {
        randTip = Random.Range(0, 100);
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerStatus.Instance.getPlayerCanInteractWithReadingMaterial())
        {
            if (this.randTip <= 5) readingMaterials = "Tip 1: The scroll is used to enhance your sanity";
            else if (this.randTip > 5 && this.randTip <= 10) readingMaterials = "Tip 2: At the starting point, there is a wooden sword on a table nearby";
            else if (this.randTip > 10 && this.randTip <= 15) readingMaterials = "Tip 3: Try breaking some crates, maybe there is something useful inside";
            else if (this.randTip > 15 && this.randTip <= 20) readingMaterials = "Tip 4: Careful about the floor, there may be traps";
            else if (this.randTip > 20 && this.randTip <= 25) readingMaterials = "Tip 5: If you break the skull, you will be feeling much better";
            else if (this.randTip > 25 && this.randTip <= 30) readingMaterials = "Tip 6: Everyone loves donut, it will help you restore your sanity & hunger";
            else if (this.randTip > 30 && this.randTip <= 35) readingMaterials = "Tip 7: If you have bad luck when breaking a crate, it will pop out a head skull, and it will make you feel creepy";
            else if (this.randTip > 35 && this.randTip <= 40) readingMaterials = "Tip 8: Barrel is your best friend, try using them at your advantage while walking over the trap area";
            else if (this.randTip > 40 && this.randTip <= 45) readingMaterials = "Tip 9: Breaking crates will reduce your weapon durability";
            else if (this.randTip > 45 && this.randTip <= 50) readingMaterials = "Tip 10: I would suggest you not trying to read the book in black";
            else if (this.randTip > 50 && this.randTip <= 55) readingMaterials = "Captain Diary: 12/12/2109 Our orders is to find a suitable planet to colonial";
            else if (this.randTip > 55 && this.randTip <= 60) readingMaterials = "Captain Diary: 5/12/2110 We are leaving the solar system now, we will be sleeping in the space pod, hope everything will be fine, because I have a bad feeling about this trip";
            else if (this.randTip > 60 && this.randTip <= 65) readingMaterials = "Captain Diary: 5/7/2170 The AI wake all crew up, what I worry is the god damn engine have over 100 years old ready, and those mother fucker don’t want to spend more cost to made one, and now things fucked up.";
            else if (this.randTip > 65 && this.randTip <= 70) readingMaterials = "Captain Diary: 7/8/2170 Seems we got to find somewhere to land or else we got 0 chance to survive in space without our engine, our navigation system also broken.";
            else if (this.randTip > 70 && this.randTip <= 75) readingMaterials = "Captain Diary: 10/11/2170 We floating in space for almost 3 months, luckily we found a planet seems suitable to landing, we decide sending some drones to look around the planet";
            else if (this.randTip > 75 && this.randTip <= 80) readingMaterials = "Captain Diary: 19/1/2171 After the drone scouting, we decide to land on the planet, there is almost out of food and water in here, we got to decide what we should do next.";
            else if (this.randTip > 80 && this.randTip <= 85) readingMaterials = "Captain Diary: 28/1/2171 We all agree to landing there by using my explosion to add force to our ship";
            else if (this.randTip > 85 && this.randTip <= 90) readingMaterials = "Captain Diary: 30/1/2171 Our ship landed in the planet, unfortunately our landing points have some creatures are keep attacking us, and the surface also change, we got to find somewhere else to settle down";
            else if (this.randTip > 90 && this.randTip <= 95) readingMaterials = "Captain Diary: 5/2/2171 The is the last log, I decide to stay in here the hold back those creatures with our last explosion, and cover all the people can evacuate to the safe place";
            else if(this.randTip > 95) readingMaterials = "Captain Diary: ??/??/???? DON’T TRUST EYES IN HERE, ALL ARE FAKE";
        }
    }

    public string getReadMaterial()
    {
        return this.readingMaterials;
    }
}
