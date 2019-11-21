using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed, jumpPower;
    private Rigidbody rigidbody;
    private bool isShooting ,isGrounded, holdingShield, holdingSword, holdingSpear, holdingGreatSword, holdingBattleAxe, holdingHandgun;
    private int countJump;
    private Camera cam;
    private Vector3 camOffset;
    private Animator leftHandAnimator, rightHandAnimator;
    private bool checkIsMoving;

    [SerializeField]
    private GameObject[] weapons, dissolve_weapons;

    [SerializeField]
    private Transform rightHand, leftHand;

    // set the trigger
    private bool ismoving, running, walking;
    // Raycast collider
    private Collider firstItem = null;
    private Dictionary<string, int> itemPriority = new Dictionary<string, int>()
    {
        {"Item", 0}, {"VendingMachine", 1}
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
    }
   

    // Update is called once per frame
    private void Update()
    {
        Vector3 movenment = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
        Vector3 velocity = transform.TransformDirection(movenment) * speed;
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
        cam.transform.position = gameObject.transform.position + camOffset;
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

        float x = rigidbody.velocity.x;
        float z = rigidbody.velocity.z;

        if (Input.GetMouseButtonDown(1) && holdingShield)
        {
            leftHandAnimator.SetBool("Blocking", true);
        }
        else if (Input.GetMouseButtonUp(1) && holdingShield)
        {
            leftHandAnimator.SetBool("Blocking", false);
        }

        if(Input.GetMouseButton(0) && holdingSword)
        {
            rightHandAnimator.SetBool("Sword_Swing", true);
            Debug.Log("Success");
            
        }
        else if (Input.GetMouseButtonUp(0) && holdingSword)
        {
            rightHandAnimator.SetBool("Sword_Swing", false);
        }

        if (Input.GetMouseButton(0) && holdingSpear)
        {
            rightHandAnimator.SetBool("Sting", true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingSpear)
        {
            rightHandAnimator.SetBool("Sting", false);
        }

        if (Input.GetMouseButton(0) && holdingGreatSword) // change the sound effect, make the sound longer
        {
            rightHandAnimator.SetBool("GreatSword_Swing", true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingGreatSword)
        {
            rightHandAnimator.SetBool("GreatSword_Swing", false);
        }

        if (Input.GetMouseButton(0) && holdingBattleAxe)
        {
            rightHandAnimator.SetBool("BattleAxe_Swing", true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingBattleAxe)
        {
            rightHandAnimator.SetBool("BattleAxe_Swing", false);
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

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(z>0 || x>0)
            {
                ismoving = true;
                running = true;
                walking = false;
                speed = 40;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 20;
            ismoving = true;
            walking = true;
            running = false;
        }
        
        if(Mathf.Approximately(rigidbody.velocity.x, 0) && Mathf.Approximately(rigidbody.velocity.z, 0))
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerStatus.Instance.setPlayerAtTheMenu(true);
            PlayerStatus.Instance.setPlayerGetIntoNextLevel(false);
        }
    }

    private void FixedUpdate()
    {
        DrawRayCastLine();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Barrel_fbx (2)")
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect(4);
        }

        if(collision.gameObject.tag == "Enemy")
        {
            PlayerStatus.Instance.setHealth(-2, "");
            Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
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

        //Enemy
        if(collision.collider.tag == "Enemy")
        {
            PlayerStatus.Instance.setHealth(-1, "");
            Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
        }

        //Ladder
        if (collision.gameObject.tag == "Ladder")
        {
            Vector3 climbMovement = new Vector3(0, Input.GetAxis("Vertical"), 0);
            Vector3 velocity = transform.TransformDirection(climbMovement) * speed;
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

        //Ladder
        if(collision.collider.tag == "Ladder")
        {
            Vector3 movenment = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 velocity = transform.TransformDirection(movenment) * speed;
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
            PlayerStatus.Instance.setPlayerStagePass(1);
            Debug.Log("The stage player pass: " + PlayerStatus.Instance.getPlayerStagePass());
        }
        if (other.gameObject.tag == "LadderBottom")
        {
            rigidbody.AddForce(Vector3.up * 1000, ForceMode.Impulse);
        }
        if(other.gameObject.tag == "Spike")
        {
            PlayerStatus.Instance.setHealth(-1, "");
        }
    }

    private void DrawRayCastLine() {
        //const
        const int accuracy = 5;
        const float radius = 0.1f;
        const float length = 15f;
        
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
                GameObject dissolve = Instantiate(dissolve_weapons[0], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if (rightHand.GetChild(0).gameObject.name == "Sword_0(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[0], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if(rightHand.GetChild(0).gameObject.name == "Spear(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[0], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if(rightHand.GetChild(0).gameObject.name == "GreatSword(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[0], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            else if(rightHand.GetChild(0).gameObject.name == "BattleAxe(Clone)")
            {
                GameObject dissolve = Instantiate(dissolve_weapons[0], pos, rotation) as GameObject;
                Destroy(dissolve, 5f);
            }
            Destroy(rightHand.GetChild(0).gameObject);
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
                        Instantiate(weapons[4], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSword = holdingHandgun = false;
                        holdingSpear = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(6);
                        Debug.Log("pick up Spear"); 
                        break;
                    case "Sword_1(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[0], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingSpear = holdingGreatSword = holdingBattleAxe = holdingHandgun = false;
                        holdingSword = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        Debug.Log("pick up Sword_1");
                        break;
                    case "Shield_0(Clone)":
                        clearLeftHandObject();
                        Instantiate(weapons[3], leftHand);
                        Destroy(firstItem.gameObject);
                        holdingShield = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(8);
                        Debug.Log("pick up Shield_0");  
                        break;
                    case "Shield_1(Clone)":
                        clearLeftHandObject();
                        Instantiate(weapons[1], leftHand);
                        Destroy(firstItem.gameObject);
                        holdingShield = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(8);
                        Debug.Log("pick up Shield_1");
                        break;
                    case "Sword_0(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[2], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSpear = holdingHandgun = false;
                        holdingSword = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        Debug.Log("pick up Sword_0");
                        break;
                    case "GreatSword(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[5], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingSpear = holdingSword = holdingBattleAxe = holdingHandgun = false;
                        holdingGreatSword = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        Debug.Log("pick up GreatSword");
                        break;
                    case "BattleAxe(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[6], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = holdingHandgun = false;
                        holdingBattleAxe = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(7);
                        Debug.Log("pick up BattleAxe");
                        break;
                    case "Handgun(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[7], rightHand);
                        Destroy(firstItem.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = holdingBattleAxe = false;
                        holdingHandgun = true;
                        FindObjectOfType<SoundManager>().PlaySoundEffect(9);
                        Debug.Log("pick up Handgun");
                        break;

                    //Food
                    case "Bread(Clone)":
                        PlayerStatus.Instance.setHunger(20);
                        FindObjectOfType<SoundManager>().PlaySoundEffect(10);
                        //Added Some buff if there have extra buff
                        Destroy(firstItem.gameObject);
                        break;
                    case "Soup(Clone)":
                        PlayerStatus.Instance.setHealth(+5, "");
                        PlayerStatus.Instance.setHunger(15);
                        FindObjectOfType<SoundManager>().PlaySoundEffect(11);
                        //Added Some buff if there have extra buff
                        Destroy(firstItem.gameObject);
                        break;
                    case "Apple(Clone)":
                        PlayerStatus.Instance.setHunger(10);
                        PlayerStatus.Instance.setHealth(+1, "");
                        FindObjectOfType<SoundManager>().PlaySoundEffect(10);
                        Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
                        //Added Some buff if there have extra buff
                        Destroy(firstItem.gameObject);
                        break;
                    case "Banana(Clone)":
                        PlayerStatus.Instance.setHealth(+1, "");
                        PlayerStatus.Instance.setHunger(10);
                        FindObjectOfType<SoundManager>().PlaySoundEffect(11);
                        //Added Some buff if there have extra buff
                        Destroy(firstItem.gameObject);
                        break;

                    //Coins
                    case "Coin(Clone)":
                        PlayerStatus.Instance.setCoins(+1);
                        Destroy(firstItem.gameObject);
                        break;

                    case "Coins(Clone)":
                        int rand = Random.Range(5,15);
                        PlayerStatus.Instance.setCoins(+rand);
                        Destroy(firstItem.gameObject);
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
