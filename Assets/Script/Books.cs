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
            else if (this.randTip > 5 && this.randTip <= 10) readingMaterials = "Tip 2: At the start of the game, your hunger will be restore 15, also there is a wooden sword or your starting weapons on a table nearby";
            else if (this.randTip > 10 && this.randTip <= 15) readingMaterials = "Tip 3: Try breaking some crates, maybe there is something useful inside";
            else if (this.randTip > 15 && this.randTip <= 20) readingMaterials = "Tip 4: Try to look carefully about the wall, some of them are breakable";
            else if (this.randTip > 20 && this.randTip <= 25) readingMaterials = "Tip 5: If you break skulls, you will be feeling much better";
            else if (this.randTip > 25 && this.randTip <= 30) readingMaterials = "Tip 6: Everyone loves donut, it will help you restore your sanity & hunger";
            else if (this.randTip > 30 && this.randTip <= 35) readingMaterials = "Tip 7: If you have bad luck when breaking a crate, a head skull will pop out, and it will make you feel creepy";
            else if (this.randTip > 35 && this.randTip <= 40) readingMaterials = "Tip 8: Barrel is your best friend, try using them at your advantage while walking over the trap area";
            else if (this.randTip > 40 && this.randTip <= 45) readingMaterials = "Tip 9: Breaking crates will reduce your weapon durability";
            else if (this.randTip > 45 && this.randTip <= 50) readingMaterials = "Tip 10: When you in the Safe House, your Hunger will not decrease";
            else if (this.randTip > 50 && this.randTip <= 55) readingMaterials = "Captain Diary: 12/12/2109 Our orders is to find a suitable planet to colonize";
            else if (this.randTip > 55 && this.randTip <= 60) readingMaterials = "Captain Diary: 5/12/2110 We are leaving the Solar System now, we will be sleeping in the space pod, hope everything will be fine, I have a bad feeling about this trip";
            else if (this.randTip > 60 && this.randTip <= 65) readingMaterials = "Captain Diary: 5/7/2170 AI woke the crew up, the 100 year old engine got busted, and those mother fucker don’t want to spend more for a new one, and now things are fucked up.";
            else if (this.randTip > 65 && this.randTip <= 70) readingMaterials = "Captain Diary: 7/8/2170 Seems we got to find somewhere to land or else we would have 0 chance to survive in space without our engine, our navigation system is also broken.";
            else if (this.randTip > 70 && this.randTip <= 75) readingMaterials = "Captain Diary: 10/11/2170 We have been floating in space for almost 3 months, luckily we found a planet that seems suitable to land, we decided sending some drones to look around on the planet";
            else if (this.randTip > 75 && this.randTip <= 80) readingMaterials = "Captain Diary: 19/1/2171 After the drone scouting, we decided to land on the planet, there is almost no food and water in here, we got to decide what we should do next.";
            else if (this.randTip > 80 && this.randTip <= 85) readingMaterials = "Captain Diary: 28/1/2171 We all agreed to land there by using explosives to add force to the ship";
            else if (this.randTip > 85 && this.randTip <= 90) readingMaterials = "Captain Diary: 30/1/2171 Our ship landed on the planet, unfortunately our landing points have creatures surrounded and they attacked us, and the surface is constantly changing, we got to find somewhere else to settle down";
            else if (this.randTip > 90 && this.randTip <= 95) readingMaterials = "Captain Diary: 5/2/2171 The is the last log, I decided to stay here to hold back those creatures with our remaining explosives, making sure the others can evacuate to somewhere safe";
            else if(this.randTip > 95) readingMaterials = "Captain Diary: ??/??/???? DON’T TRUST ANYTHINGS IN HERE, ALL ARE FAKE";
        }
    }

    public string getReadMaterial()
    {
        return this.readingMaterials;
    }
}
