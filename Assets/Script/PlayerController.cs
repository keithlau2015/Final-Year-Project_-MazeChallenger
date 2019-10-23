using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed, jumpPower;
    private Rigidbody rigidbody;
    private bool isGrounded, holdingShield, holdingSword, holdingSpear, holdingGreatSword, holdingBattleAxe, holdingBow;
    private int countJump;
    private Camera cam;
    private Vector3 camOffset;
    private Animator leftHandAnimator, rightHandAnimator;
    private bool checkIsMoving;

    [SerializeField]
    private GameObject[] weapons;

    [SerializeField]
    private Transform rightHand, leftHand;

    // set the trigger
    private bool soundawake, ismoving, running, walking;
    // Raycast collider
    private Collider firstItem = null;
    private Dictionary<string, int> itemPriority = new Dictionary<string, int>()
    {
        {"Item", 0}, {"Intecractable Item", 1}
    };

    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
        soundawake = false;
        isGrounded = false;
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
            soundawake = true;
            StartCoroutine(soundeffect("sword_atk"));
            Debug.Log("Success");
            
        }
        else if (Input.GetMouseButtonUp(0) && holdingSword)
        {
            rightHandAnimator.SetBool("Sword_Swing", false);
            soundawake = false;
        }

        if (Input.GetMouseButton(0) && holdingSpear)
        {
            rightHandAnimator.SetBool("Sting", true);
            soundawake = true;
            StartCoroutine(soundeffect("spear_atk_sound"));
            
        }
        else if (Input.GetMouseButtonUp(0) && holdingSpear)
        {
            rightHandAnimator.SetBool("Sting", false);
            soundawake = false;
        }

        if (Input.GetMouseButton(0) && holdingGreatSword) // change the sound effect, make the sound longer
        {
            rightHandAnimator.SetBool("GreatSword_Swing", true);
            soundawake = true;
            StartCoroutine(soundeffect("great_sword_atk"));

        }
        else if (Input.GetMouseButtonUp(0) && holdingGreatSword)
        {
            rightHandAnimator.SetBool("GreatSword_Swing", false);
            soundawake = false;
        }

        if (Input.GetMouseButton(0) && holdingBattleAxe)
        {
            rightHandAnimator.SetBool("BattleAxe_Swing", true);
            soundawake = true;
            StartCoroutine(soundeffect("sword_atk"));

        }
        else if (Input.GetMouseButtonUp(0) && holdingBattleAxe)
        {
            rightHandAnimator.SetBool("BattleAxe_Swing", false);
            soundawake = false;
        }

        if (Input.GetMouseButton(0) && holdingBow)
        {
            rightHandAnimator.SetBool("Shot", true);
            soundawake = true;
            StartCoroutine(soundeffect("arrow_atk"));
            
        }
        else if (Input.GetMouseButtonUp(0) && holdingBow)
        {
            rightHandAnimator.SetBool("Shot", false);
            soundawake = false;
        }

        if(Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
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
            FindObjectOfType<soundcontrol>().character("jumpping_effect");
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
            SceneManager.LoadScene(1);
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
            StartCoroutine(soundeffect("wood_touch"));
        }
        if(collision.gameObject.tag == "Enemy")
        {
            PlayerStatus.Instance.setHealth(-2, "");
            Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            countJump = 0;
            isGrounded = true;
        }
        if(collision.collider.tag == "Enemy")
        {
            PlayerStatus.Instance.setHealth(-1, "");
            Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Teleporter")
        {
            //upgrade player status & monster status
            PlayerStatus.Instance.setPlayerGetIntoNextLevel(true);
            PlayerStatus.Instance.resetData();
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
            soundawake = true;
            FindObjectOfType<soundcontrol>().character("walking_effect");
            
        }
        else if(ismoving && running)
        {
            soundawake = true;
            FindObjectOfType<soundcontrol>().character("running");
            Debug.Log("The sound played");
        }
        else{
            soundawake = false;
        }

    }

    private IEnumerator soundeffect(string name)
    {
        if(soundawake)
        {
            FindObjectOfType<soundcontrol>().wepon_atk(name);
        }
        else{
            Debug.Log("the soundawake did not trigger");
        }
        yield return 0;
    }

    private void ifhitthewood()
    {
        /*if (Physics.Raycast (transform.position, transform.forward, out rigidbody)){
         ReceivingClass rc = rigidbody.transform.GetComponent<ReceivingClass>();
     if(rc != null)
         FindObjectOfType<soundcontrol>().character("walking_effect");
     }*/
    }

    private void cleaRightHandObject()
    {
        if(rightHand.transform.childCount > 0)
        {
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
        if (!firstItem) return;
        if(firstItem.tag == "Item")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (firstItem.name)
                {                    
                    //Weapon
                    case "Spear(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[4], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSword = holdingBow = false;
                        holdingSpear = true;
                        FindObjectOfType<soundcontrol>().wepon_atk("wood_pickup");
                        Debug.Log("pick up Spear"); 
                        break;
                    case "Sword_1(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[0], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingSpear = holdingGreatSword = holdingBattleAxe = holdingBow = false;
                        holdingSword = true;
                        FindObjectOfType<soundcontrol>().wepon_atk("sword_pickup");
                        Debug.Log("pick up Sword_1");
                        break;
                    case "Shield_0(Clone)":
                        clearLeftHandObject();
                        Instantiate(weapons[3], leftHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingShield = true;
                        FindObjectOfType<soundcontrol>().wepon_atk("wood_pickup");
                        Debug.Log("pick up Shield_0");  
                        break;
                    case "Shield_1(Clone)":
                        clearLeftHandObject();
                        Instantiate(weapons[1], leftHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingShield = true;
                        FindObjectOfType<soundcontrol>().wepon_atk("wood_pickup");
                        Debug.Log("pick up Shield_1");
                        break;
                    case "Sword_0(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[2], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSpear = holdingBow = false;
                        holdingSword = true;
                        FindObjectOfType<soundcontrol>().wepon_atk("sword_pickup");
                        Debug.Log("pick up Sword_0");
                        break;
                    case "GreatSword(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[5], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingSpear = holdingSword = holdingBattleAxe = holdingBow = false;
                        holdingGreatSword = true;
                        FindObjectOfType<soundcontrol>().wepon_atk("sword_pickup");
                        Debug.Log("pick up GreatSword");
                        break;
                    case "BattleAxe(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[6], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = holdingBow = false;
                        holdingBattleAxe = true;
                        FindObjectOfType<soundcontrol>().wepon_atk("sword_pickup");
                        Debug.Log("pick up BattleAxe");
                        break;
                    case "Bow(Clone)":
                        cleaRightHandObject();
                        Instantiate(weapons[7], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = false;
                        holdingBattleAxe = true;
                        FindObjectOfType<soundcontrol>().wepon_atk("spear_pick");
                        Debug.Log("pick up Bow");
                        break;

                    //Food
                    case "Bread(Clone)":
                        PlayerStatus.Instance.setHunger(20);
                        //Added Some buff if there have extra buff
                        Destroy(firstItem.transform.gameObject);
                        break;
                    case "Soup(Clone)":
                        PlayerStatus.Instance.setHealth(+5, "");
                        PlayerStatus.Instance.setHunger(15);
                        //Added Some buff if there have extra buff
                        Destroy(firstItem.transform.gameObject);
                        break;
                    case "Apple(Clone)":
                        PlayerStatus.Instance.setHunger(10);
                        PlayerStatus.Instance.setHealth(+1, "");
                        Debug.Log("Player Health: " + PlayerStatus.Instance.getHealth());
                        //Added Some buff if there have extra buff
                        Destroy(firstItem.transform.gameObject);
                        break;
                    case "Banana(Clone)":
                        PlayerStatus.Instance.setHealth(+1, "");
                        PlayerStatus.Instance.setHunger(10);
                        //Added Some buff if there have extra buff
                        Destroy(firstItem.transform.gameObject);
                        break;
                }
            }
        }
    }
}
