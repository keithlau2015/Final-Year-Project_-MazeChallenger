using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [SerializeField]
    private GameObject[] weapons;

    [SerializeField]
    private Transform rightHand, leftHand;

    // Raycast collider
    private Collider firstItem = null;
    private Dictionary<string, int> itemPriority = new Dictionary<string, int>()
    {
        {"Item", 0}, {"Intecractable Item", 1}
    };

    private void Awake()
    {
        isGrounded = false;
        countJump = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        camOffset = new Vector3(0, 4.5f, 0);
        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        leftHandAnimator = leftHand.GetComponent<Animator>();
        rightHandAnimator = rightHand.GetComponent<Animator>();
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
        if (Input.GetMouseButtonDown(1) && holdingShield)
        {
            leftHandAnimator.SetBool("Blocking", true);
        }
        else if (Input.GetMouseButtonUp(1) && holdingShield)
        {
            leftHandAnimator.SetBool("Blocking", false);
        }

        if(Input.GetMouseButtonDown(0) && holdingSword)
        {
            rightHandAnimator.SetBool("Sword_Swing", true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingSword)
        {
            rightHandAnimator.SetBool("Sword_Swing", false);
        }

        if (Input.GetMouseButtonDown(0) && holdingSpear)
        {
            rightHandAnimator.SetBool("Sting", true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingSpear)
        {
            rightHandAnimator.SetBool("Sting", false);
        }

        if (Input.GetMouseButtonDown(0) && holdingGreatSword)
        {
            rightHandAnimator.SetBool("GreatSword_Swing", true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingGreatSword)
        {
            rightHandAnimator.SetBool("GreatSword_Swing", false);
        }

        if (Input.GetMouseButtonDown(0) && holdingBattleAxe)
        {
            rightHandAnimator.SetBool("BattleAxe_Swing", true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingBattleAxe)
        {
            rightHandAnimator.SetBool("BattleAxe_Swing", false);
        }

        if (Input.GetMouseButtonDown(0) && holdingBow)
        {
            rightHandAnimator.SetBool("Shot", true);
        }
        else if (Input.GetMouseButtonUp(0) && holdingBow)
        {
            rightHandAnimator.SetBool("Shot", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 30;
            soundcontrol.running_effect = true;
            soundcontrol.walking_effect = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 20;
            soundcontrol.running_effect = false;
            soundcontrol.walking_effect = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
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
    }

    private void FixedUpdate()
    {
        DrawRayCastLine();
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            countJump = 0;
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    private void DrawRayCastLine() {
        //const
        const int accuracy = 5;
        const float radius = 0.1f;
        const float length = 13f;
        
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

    private void cleaRightHandObject()
    {
        if (rightHand.transform.childCount > 0)
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
                    case "Spear":
                        cleaRightHandObject();
                        Instantiate(weapons[4], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSword = holdingBow = false;
                        holdingSpear = true;
                        soundcontrol.spear = true;
                        soundcontrol.picking_up = true;
                        Debug.Log("pick up Spear"); 
                        break;
                    case "Sword_1":
                        cleaRightHandObject();
                        Instantiate(weapons[0], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingSpear = holdingGreatSword = holdingBattleAxe = holdingBow = false;
                        holdingSword = true;
                        soundcontrol.sword = true;
                        soundcontrol.picking_up = true;
                        Debug.Log("pick up Sword_1");
                        break;
                    case "Shield_0":
                        clearLeftHandObject();
                        Instantiate(weapons[3], leftHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingShield = true;
                        soundcontrol.picking_up = true;
                        soundcontrol.spear = true;
                        Debug.Log("pick up Shield_0");  
                        break;
                    case "Shield_1":
                        clearLeftHandObject();
                        Instantiate(weapons[1], leftHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingShield = true;
                        soundcontrol.picking_up = true;
                        soundcontrol.spear = true;
                        Debug.Log("pick up Shield_1");
                        break;
                    case "Sword_0":
                        cleaRightHandObject();
                        Instantiate(weapons[2], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingBattleAxe = holdingGreatSword = holdingSpear = holdingBow = false;
                        holdingSword = true;
                        soundcontrol.sword = true;
                        soundcontrol.picking_up = true;
                        Debug.Log("pick up Sword_0");
                        break;
                    case "GreatSword":
                        cleaRightHandObject();
                        Instantiate(weapons[5], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingSpear = holdingSword = holdingBattleAxe = holdingBow = false;
                        holdingGreatSword = true;
                        soundcontrol.sword = true;
                        soundcontrol.picking_up = true;
                        Debug.Log("pick up GreatSword");
                        break;
                    case "BattleAxe":
                        cleaRightHandObject();
                        Instantiate(weapons[6], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = holdingBow = false;
                        holdingBattleAxe = true;
                        Debug.Log("pick up BattleAxe");
                        break;
                    case "Bow":
                        cleaRightHandObject();
                        Instantiate(weapons[7], rightHand);
                        Destroy(firstItem.transform.gameObject);
                        holdingGreatSword = holdingSpear = holdingSword = false;
                        holdingBattleAxe = true;
                        Debug.Log("pick up Bow");
                        break;
                }
            }
        }
    }
}
