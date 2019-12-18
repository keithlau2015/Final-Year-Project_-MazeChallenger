using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpPower;
    private Rigidbody rigidbody;
    private bool isShooting ,isGrounded, 
                 /*check holding what*/
                 holdingShield, holdingSword, holdingSpear, holdingGreatSword, holdingBattleAxe, holdingHandgun, holdingKatana, holdingPan, holdingLance;
    /*skill to have double jump*/
    private int countJump;
    private Camera cam;
    private Vector3 camOffset;
    private Animator leftHandAnimator, rightHandAnimator;
    private bool checkIsMoving;
    //public EnemyBehaviour enemy;
    [SerializeField]
    private GameObject[] weapons, dissolve_weapons;

    [SerializeField]
    private Transform rightHand, leftHand;

    [SerializeField]
    private Transform[] descriptionUISpawningPoint;

    [SerializeField]
    private Text description;

    // set the trigger
    private bool ismoving, running, walking;
    // Raycast collider
    private Collider firstItem = null;
    private Dictionary<string, int> itemPriority = new Dictionary<string, int>()
    {
        {"Item", 0}, {"VendingMachine", 1}, {"Readable", 2}
    };

    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
        isShooting = false;
        countJump = 0;   
        camOffset = new Vector3(0, 4.5f, 0);
        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        leftHandAnimator = leftHand.GetComponent<Animator>();
        rightHandAnimator = rightHand.GetComponent<Animator>();
        InvokeRepeating ("walkandrun", 0.0f, 0.5f);
        PlayerStatus.Instance.setPlayerAttacking(false);
    }


    // Update is called once per frame
    private void Update()
    {
        Vector3 movenment = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 velocity = transform.TransformDirection(movenment) * PlayerStatus.Instance.getSpeed();
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
        cam.transform.position = gameObject.transform.position + camOffset;
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

        float x = rigidbody.velocity.x;
        float z = rigidbody.velocity.z;

        if (rightHandAnimator.GetNextAnimatorStateInfo(0).IsName("idle"))
        {
            PlayerStatus.Instance.setPlayerAttacking(false);
        }

        //Check Player is Attacking or not
        if (Input.GetMouseButtonDown(1) && holdingShield)
        {
            leftHandAnimator.SetBool("Blocking", true);
            PlayerStatus.Instance.setPlayerIsBlocking(true);
        }
        else if (Input.GetMouseButtonUp(1) && holdingShield)
        {
            leftHandAnimator.SetBool("Blocking", false);
            PlayerStatus.Instance.setPlayerIsBlocking(false);
        }

        if (Input.GetMouseButton(0) && holdingSword)
        {
            rightHandAnimator.SetBool("Sword_Swing", true);
            PlayerStatus.Instance.setPlayerAttacking(true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingSword)
        {
            rightHandAnimator.SetBool("Sword_Swing", false);
        }

        if (Input.GetMouseButton(0) && holdingSpear)
        {
            rightHandAnimator.SetBool("Sting", true);
            PlayerStatus.Instance.setPlayerAttacking(true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingSpear)
        {
            rightHandAnimator.SetBool("Sting", false);
        }

        if (Input.GetMouseButton(0) && holdingLance)
        {
            rightHandAnimator.SetBool("Sting", true);
            PlayerStatus.Instance.setPlayerAttacking(true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingLance)
        {
            rightHandAnimator.SetBool("Sting", false);
        }

        if (Input.GetMouseButton(0) && holdingGreatSword)
        {
            rightHandAnimator.SetBool("GreatSword_Swing", true);
            PlayerStatus.Instance.setPlayerAttacking(true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingGreatSword)
        {
            rightHandAnimator.SetBool("GreatSword_Swing", false);
        }

        if (Input.GetMouseButton(0) && holdingBattleAxe)
        {
            rightHandAnimator.SetBool("BattleAxe_Swing", true);
            PlayerStatus.Instance.setPlayerAttacking(true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingBattleAxe)
        {
            rightHandAnimator.SetBool("BattleAxe_Swing", false);
        }

        if (Input.GetMouseButton(0) && holdingPan)
        {
            rightHandAnimator.SetBool("Sword_Swing", true);
            PlayerStatus.Instance.setPlayerAttacking(true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingPan)
        {
            rightHandAnimator.SetBool("Sword_Swing", false);
        }

        if (Input.GetMouseButton(0) && holdingKatana)
        {
            rightHandAnimator.SetBool("Sword_Swing", true);
            PlayerStatus.Instance.setPlayerAttacking(true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingPan)
        {
            rightHandAnimator.SetBool("Sword_Swing", false);
        }

        if (Input.GetMouseButtonDown(0) && holdingHandgun)
        {
            rightHandAnimator.SetBool("Shot", true);
            if (rightHand.GetChild(0).name == "Handgun(Clone)")
            {
                rightHand.GetChild(0).GetComponent<Gun>().animationTrigger();
            }
        }
        else if (Input.GetMouseButtonUp(0) && holdingHandgun)
        {
            rightHandAnimator.SetBool("Shot", false);
            isShooting = false;
        }

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            ismoving = true;
            walking = true;
            running = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (z > 0 || x > 0 || z < 0 || x < 0)
            {
                ismoving = true;
                running = true;
                walking = false;
                PlayerStatus.Instance.setSpeed(+10, "");
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PlayerStatus.Instance.setSpeed(-10, "");
            ismoving = true;
            walking = true;
            running = false;
        }

        if (Mathf.Approximately(rigidbody.velocity.x, 0) && Mathf.Approximately(rigidbody.velocity.z, 0))
        {
            ismoving = false;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(3);
            //Double jump
            /*
            countJump++;
            if (countJump<2)
            {
                rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
            */
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        //Weapon are 0 durability
        if (rightHand.childCount > 0)
        {
            if (rightHand.GetChild(0).GetComponent<Weapon>().durability <= 0)
            {
                description.text = "weapon broken!";
                cleaRightHandObject();
                FindObjectOfType<SoundManager>().PlaySoundEffect(13);
                //set animator to idle
                foreach(AnimatorControllerParameter parameter in rightHandAnimator.parameters)
                {
                    rightHandAnimator.SetBool(parameter.name, false);
                }
                //clear all holding boolean to false
                holdingShield = holdingSword = holdingSpear = holdingGreatSword = holdingBattleAxe = holdingHandgun = holdingKatana = holdingPan = false;
            }
        }

        if (leftHand.childCount > 0)
        {       
            if (leftHand.GetChild(0).GetComponent<Shield>().durability <= 0)
            {
                clearLeftHandObject();
                FindObjectOfType<SoundManager>().PlaySoundEffect(13);
                //set animator to idle
                foreach (AnimatorControllerParameter parameter in leftHandAnimator.parameters)
                {
                    leftHandAnimator.SetBool(parameter.name, false);
                }
                //clear all holding boolean to false
                holdingShield = holdingSword = holdingSpear = holdingGreatSword = holdingBattleAxe = holdingHandgun = holdingKatana = false;
            }
        }
    }
    private void LateUpdate()
    {
        DrawRayCastLine();
        //skill
        if (PlayerStatus.Instance.getActiveHealingSkill())
        {
            PlayerStatus.Instance.HealingSkill();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Barrel_fbx")
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(4);
        }

        if(collision.gameObject.tag == "attack_point")
        {
            PlayerStatus.Instance.setHealth(-1, "");
            Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
            if(PlayerStatus.Instance.getHealth() == 0)
            {
                PlayerStatus.Instance.setPlayerKilledBy(collision.gameObject.name + "Tear you apart");
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Floor
        if(collision.collider.tag == "Ground")
        {
            countJump = 0;
            isGrounded = true;
        }

        if(collision.collider.tag == "Trap")
        {
            countJump = 0;
            isGrounded = true;
        }

        //Ladder
        if (collision.gameObject.tag == "Ladder")
        {
            Vector3 climbMovement = new Vector3(0, Input.GetAxis("Vertical"), 0);
            Vector3 velocity = transform.TransformDirection(climbMovement) * PlayerStatus.Instance.getSpeed();
            //velocity.y = rigidbody.velocity.y;
            rigidbody.velocity = velocity;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }

        if(collision.collider.tag == "Trap")
        {
            isGrounded = false;
        }

        //Ladder
        if(collision.collider.tag == "Ladder")
        {
            Vector3 movenment = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 velocity = transform.TransformDirection(movenment) * PlayerStatus.Instance.getSpeed();
            //velocity.y = rigidbody.velocity.y;
            rigidbody.velocity = velocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Teleporter")
        {
            //upgrade player status & monster status
            PlayerStatus.Instance.setPlayerGetIntoNextLevel(true);
            PlayerStatus.Instance.setPlayerReachLevels(+1);
        }
        if(other.gameObject.tag == "Back2MainMenu")
        {
            PlayerStatus.Instance.resetData();
            SceneManager.LoadScene(1);            
        }
        if (other.gameObject.tag == "LadderBottom")
        {
            rigidbody.AddForce(Vector3.up * 1000, ForceMode.Impulse);
        }
        if(other.gameObject.tag == "Spike")
        {
            PlayerStatus.Instance.setHealth(-1, "");
            popDescriptionText("- Sanity");
            PlayerStatus.Instance.setSanity(-5);
            popDescriptionText("- Sanity");
            if (PlayerStatus.Instance.getHealth() == 0)
            {
                PlayerStatus.Instance.setPlayerKilledBy("Jump into the Spike");
            }
        }
        if(other.gameObject.tag == "attack_point" && !PlayerStatus.Instance.getPlayerIsBlocking())
        {
            PlayerStatus.Instance.setHealth(-1, "");
            popDescriptionText("- Sanity");
            PlayerStatus.Instance.setSanity(-5);
            popDescriptionText("- Sanity");
            if (PlayerStatus.Instance.getHealth() == 0)
            {
                PlayerStatus.Instance.setPlayerKilledBy(other.transform.parent.parent.name + "Tear you apart");
            }
        }
        if(other.gameObject.tag == "DeadZone")
        {
            PlayerStatus.Instance.setHealth(-PlayerStatus.Instance.getTotalHealth(), "");
            PlayerStatus.Instance.setPlayerKilledBy("Fall into spikes");
        }
    }

    private void DrawRayCastLine() {
        //const
        const int accuracy = 5;
        const float radius = 1f;
        const float length = 20f;
        
        //getting camera transformation
        Vector3 unitForward = cam.transform.TransformDirection(Vector3.forward);
        
        //setting offset
        Vector3 offset = unitForward * 1f;
        Vector3 upCenter = cam.transform.position + offset;
        Vector3 downCenter = upCenter + unitForward * length;

        float gap = 0;

        List<RaycastHit> touchItems = new List<RaycastHit>();
        for (int level = 1; level <= accuracy; ++level)
        {
            float angle = 360 / (level * level);
            for (float degree = 0; degree < 360; degree += angle)
            {
                float radians = degree * Mathf.PI / 180f;
                Vector3 xyPoint = new Vector3(
                    Mathf.Cos(radius) * gap,
                    Mathf.Sin(radius) * gap,
                    0
                    );
          
            RaycastHit hit;
            bool isHit = Physics.Raycast(
                upCenter + xyPoint,
                unitForward,
                out hit,
                length
            );
            //debug
            Debug.DrawLine(
            upCenter + xyPoint,
            downCenter + xyPoint,
            Color.green
            );
            if (isHit)
            {
                touchItems.Add(hit);
            }
            };
            gap += radius / accuracy;
        }
        if (touchItems.Count > 0)
        {
            touchItems.Sort(ObjectDistanceDetection);
            Collider firstHit = touchItems.ElementAt(0).collider;
            firstItem = itemPriority.ContainsKey(firstHit.tag) ? firstHit : null;
            onTriggerLineStay();
        }
        else
        {
            PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
            PlayerStatus.Instance.setPlayerCanInteractWithVendingMachine(false);
            PlayerStatus.Instance.setPlayerCanInteractReadingMaterial(false);
            firstItem = null;
        }
    }
    private int ObjectDistanceDetection(RaycastHit xHit, RaycastHit yHit)
    {
        Collider x = xHit.collider, y = yHit.collider;
        var xd = Vector3.Distance(cam.transform.position, xHit.point);
        var yd = Vector3.Distance(cam.transform.position, yHit.point);
        var cp = xd - yd;

        int xPriority, yPriority;
        xPriority = itemPriority.TryGetValue(x.tag, out xPriority) ? xPriority : int.MaxValue;
        yPriority = itemPriority.TryGetValue(y.tag, out yPriority) ? yPriority : int.MaxValue;
        return cp < 0 ? -1 : cp > 0 ? 1 : xPriority < yPriority ? -1 : xPriority > yPriority ? 1 : 0;
    }

    private void walkandrun()
    {
        if(ismoving && walking)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(5);
        }
    }

    private void cleaRightHandObject()
    {
        Vector3 pos = rightHand.transform.position;
        Quaternion rotation = rightHand.transform.rotation;
        if (rightHand.transform.childCount > 0)
        {
            if (rightHand.GetChild(0).gameObject.name == "Sword_1(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[1], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if (rightHand.GetChild(0).gameObject.name == "Sword_0(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[0], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if(rightHand.GetChild(0).gameObject.name == "Katana(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[2], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if(rightHand.GetChild(0).gameObject.name == "GreatSword(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[4], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if(rightHand.GetChild(0).gameObject.name == "BattleAxe(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[5], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if (rightHand.GetChild(0).gameObject.name == "Pan(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[5], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if (rightHand.GetChild(0).gameObject.name == "Spear(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[5], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if (rightHand.GetChild(0).gameObject.name == "Lance(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[5], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            Destroy(rightHand.GetChild(0).gameObject);
            PlayerStatus.Instance.setPlayerAttacking(false);
        }
    }

    private void clearLeftHandObject()
    {
        if (leftHand.transform.childCount > 0)
        {
            Destroy(leftHand.GetChild(0).gameObject);
        }
    }

    //just like onTriggerStay function
    private void onTriggerLineStay()
    {
        if (!firstItem)
        {
            return;
        }
        if(firstItem.tag == "Item")
        {
            PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (firstItem.name)
                {
                    //Weapon
                    case "Spear(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[8], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSword = holdingHandgun = holdingKatana = holdingPan = holdingLance = false;
                        holdingSpear = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(6);
                        Debug.Log("pick up Spear");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Lance(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[9], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSword = holdingHandgun = holdingKatana = holdingPan = holdingSpear = false;
                        holdingSpear = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(6);
                        Debug.Log("pick up Spear");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Sword_1(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[1], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingSpear = holdingGreatSword = holdingBattleAxe = holdingHandgun = holdingKatana = holdingPan = holdingLance = false;
                        holdingSword = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        Debug.Log("pick up Sword_1");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Sword_0(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[0], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSpear = holdingHandgun = holdingKatana = holdingPan = holdingLance = false;
                        holdingSword = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        Debug.Log("pick up Sword_0");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "GreatSword(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[4], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingSpear = holdingSword = holdingBattleAxe = holdingHandgun = holdingKatana = holdingPan = holdingLance = false;
                        holdingGreatSword = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        Debug.Log("pick up GreatSword");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "BattleAxe(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[5], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = holdingHandgun = holdingKatana = holdingPan = holdingLance = false;
                        holdingBattleAxe = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        Debug.Log("pick up BattleAxe");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Katana(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[6], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSpear = holdingHandgun = holdingSword = holdingPan = holdingLance = false;
                        holdingKatana = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Pan(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[7], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = holdingBattleAxe = holdingKatana = holdingHandgun = holdingLance = false;
                        holdingPan = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;

                    //Shield
                    case "Shield_0(Clone)":
                        clearLeftHandObject();
                        Instantiate(weapons[2], leftHand);
                        Destroy(firstItem.gameObject);
                        holdingShield = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(8);
                        Debug.Log("pick up Shield_0");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Shield_1(Clone)":
                        clearLeftHandObject();
                        Instantiate(weapons[3], leftHand);
                        Destroy(firstItem.gameObject);
                        holdingShield = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(8);
                        Debug.Log("pick up Shield_1");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;

                    /*
                    case "Handgun(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[7], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = holdingBattleAxe = holdingKatana = false;
                        holdingHandgun = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(9);
                        Debug.Log("pick up Handgun");
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    */

                    //Food
                    case "Bread(Clone)":
                        popDescriptionText("+ Hunger");
                        if (holdingPan) PlayerStatus.Instance.setHunger(25, "");
                        else PlayerStatus.Instance.setHunger(20, "");
                        FindObjectOfType<SoundManager>().PlaySoundEffect(10);
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Soup(Clone)":
                        popDescriptionText("+ Hunger");
                        if (holdingPan) PlayerStatus.Instance.setHunger(20, "");
                        else PlayerStatus.Instance.setHunger(15, "");
                        FindObjectOfType<SoundManager>().PlaySoundEffect(11);
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Apple(Clone)":
                        popDescriptionText("+ Hunger");
                        if (holdingPan) PlayerStatus.Instance.setHunger(15, "");
                        else PlayerStatus.Instance.setHunger(10, "");
                        FindObjectOfType<SoundManager>().PlaySoundEffect(10);
                        Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Banana(Clone)":
                        popDescriptionText("+ Hunger");
                        if (holdingPan) PlayerStatus.Instance.setHunger(15, "");
                        else PlayerStatus.Instance.setHunger(10, "");
                        FindObjectOfType<SoundManager>().PlaySoundEffect(10);
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    case "Donut(Clone)":
                        popDescriptionText("+ Hunger");
                        popDescriptionText("+ Sanity"); 
                        if (holdingPan) PlayerStatus.Instance.setHunger(15, "");
                        else PlayerStatus.Instance.setHunger(10, "");
                        PlayerStatus.Instance.setSanity(1);
                        FindObjectOfType<SoundManager>().PlaySoundEffect(10);
                        Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;

                    //Coins
                    /*
                    case "Coin(Clone)":
                        PlayerStatus.Instance.setCoins(+1);
                        FindObjectOfType<SoundManager>().PlaySoundEffect(15);
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    */

                    case "Coins(Clone)":
                        popDescriptionText("+ Coins");
                        int rand = Random.Range(5,15);
                        FindObjectOfType<SoundManager>().PlaySoundEffect(15);
                        PlayerStatus.Instance.setCoins(+rand);
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;

                    //Well
                    case "well":
                        popDescriptionText("+ Sanity");
                        PlayerStatus.Instance.setSanity(+15);
                        if (firstItem.GetComponent<Well>().useableTime <= 0)
                        {
                            firstItem.transform.GetChild(1).GetComponent<Animator>().SetBool("Dry", true);
                            firstItem.gameObject.tag = "Untagged";
                            PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        }
                        else
                        {
                            firstItem.GetComponent<Well>().useableTime--;
                        }
                        break;

                    //Healing potion
                    case "RestoreHealthBottle(Clone)":
                        popDescriptionText("+ Health");
                        PlayerStatus.Instance.setHealth(+2, "");
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                    
                    //Little Statue
                    case "LittleStatue":
                        popDescriptionText("- Hunger Speed");
                        PlayerStatus.Instance.setHungerTime();
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;

                    //Scroll
                    case "Scroll(Clone)":
                        popDescriptionText("+ Exp");
                        PlayerStatus.Instance.setReinforcement(+1);
                        Destroy(firstItem.gameObject);
                        PlayerStatus.Instance.setPlayerCanInteractWithOtherObject(false);
                        break;
                }
            }
        }
        else if(firstItem.tag == "VendingMachine")
        {
            int rand = Random.Range(0, 2);
            PlayerStatus.Instance.setPlayerCanInteractWithVendingMachine(true);
            if (!firstItem.GetComponent<VendingMachine>().getCheckIsPurchase()) PlayerStatus.Instance.setPriceUIText("$" + firstItem.GetComponent<VendingMachine>().getPrice());
            else if (rand == 1) PlayerStatus.Instance.setPriceUIText("Empty");
            else if (rand == 2) PlayerStatus.Instance.setPriceUIText("What are you looking for?");
            else PlayerStatus.Instance.setPriceUIText("SOLD OUT");
            if (Input.GetKeyDown(KeyCode.E))
            {
                //buying
                if(PlayerStatus.Instance.getCoins() < firstItem.GetComponent<VendingMachine>().getPrice())
                {
                    //play error sound effects
                }
                else
                {
                    PlayerStatus.Instance.setCoins(-firstItem.GetComponent<VendingMachine>().getPrice());
                    firstItem.GetComponent<VendingMachine>().purchaseSuccess();
                }
            }
        }
        else if(firstItem.tag == "Readable")
        {
            PlayerStatus.Instance.setPlayerCanInteractReadingMaterial(true);
            PlayerStatus.Instance.setReadingMaterials(firstItem.GetComponent<Books>().getReadMaterial());
        }
    }

    public void popDescriptionText(string text)
    {
        Text clone;
        description.text = text;
        switch (text)
        {
            case "+ Health":
                description.color = new Color(0.9339623f, 0.3568441f, 0.4483268f);
                break;

            case "- Health":
                description.color = new Color(0.9339623f, 0.3568441f, 0.4483268f);
                break;

            case "+ Hunger":
                description.color = new Color(1f, 0.7567568f, 0f);
                break;

            case "- Hunger":
                description.color = new Color(1f, 0.7567568f, 0f);
                break;

            case "+ Sanity":
                description.color = new Color(0.3170612f, 0.8076482f, 0.8962264f);
                break;

            case "- Sanity":
                description.color = new Color(0.3170612f, 0.8076482f, 0.8962264f);
                break;

            case "- Hunger Speed":
                description.color = new Color(1f, 1f, 1f);
                break;

            case "+ Coins":
                description.color = new Color(0.9803922f, 1f, 0.4103774f);
                break;

            case "+ Coin":
                description.color = new Color(0.9803922f, 1f, 0.4103774f);
                break;

            case "+ Exp":
                description.color = new Color(0.3568627f, 0.9333333f, 0.5708299f);
                break;
        }
        clone = Instantiate(description, descriptionUISpawningPoint[Random.Range(0, descriptionUISpawningPoint.Length)]) as Text;
        Destroy(clone, 2);
    }

    public bool getIsShooting()
    {
        return isShooting;
    }

    public void setIsShootingTrue()
    {
        isShooting = true;
    }
}
