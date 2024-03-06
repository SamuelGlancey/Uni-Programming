using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public delegate void diggingEvent(float damage);

public delegate void respawnEvent();

public delegate void ProgressEvent(int item);
public class PlayerController_ : MonoBehaviour
{

    public event diggingEvent digSomething;
    public event respawnEvent died;
    public static event ProgressEvent GoldenCollected;
    [Header("drag in")]
    [SerializeField] private Animator anim;
    [SerializeField] private TMP_Text rollsText;
    Gyroscope gyro;
    Rigidbody rb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera cameraModeCamera;
    [SerializeField] private Camera ToiletCam;
    [SerializeField] private moveButton moveButton;
    [SerializeField] private List<GameObject> attacks;

    [Header("Upgrades")]
    [SerializeField] private upgrade attackUpgrade;
    [SerializeField] private upgrade attackRangeUpgrade;
    [SerializeField] private upgrade healthUpgrade;
    [SerializeField] private upgrade digSpeedUpgrade;


    [Header("adjustables")]
    [SerializeField] private float speed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float groundSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float minimumShake;
    [SerializeField] private float backstepSpeed;
    [SerializeField] private float backstepTime;
    [SerializeField] private float digAmount;

    //variables not shown in inspector
    private bool canShake;
    private bool shook;
    private bool canMove;
    private bool nearToilet;
    private int attackNum;
    private bool canJump;
    public static int rolls;

    private bool isGrounded;
    private bool isMoving;
    private bool isDigging;
    private bool isBackstepping;
    [SerializeField] private Transform nearestToilet;
    // Start is called before the first frame update
    void Start()
    {
        canShake = true;
        gyro = Input.gyro;
        gyro.enabled = true;
        canMove = true;
        rb = GetComponent<Rigidbody>();
        sitOnToilet();
    }
    private void OnEnable()
    {
        died += resetHealth;
        died += sitOnToilet;
        died += resetAttackCombo;
        died += resetAllAttacks;
        pirateBoss.bossDied += invulnerable;
        attackUpgrade.upgraded += UpgradeAttacks;
        attackRangeUpgrade.upgraded += UpgradeAttackRange;
        healthUpgrade.upgraded += UpgradeHealth;
        digSpeedUpgrade.upgraded += UpgradeDigSpeed;

    }
    private void OnDisable()
    {
        died -= resetHealth;
        died -= sitOnToilet;
        died -= resetAttackCombo;
        died -= resetAllAttacks;
        pirateBoss.bossDied -= invulnerable;
        attackUpgrade.upgraded -= UpgradeAttacks;
        attackRangeUpgrade.upgraded -= UpgradeAttackRange;
        healthUpgrade.upgraded -= UpgradeHealth;
        digSpeedUpgrade.upgraded -= UpgradeDigSpeed;
    }

    public void invulnerable()
    {
        healthController hc = GetComponent<healthController>();
        hc.MaxHealth = Mathf.Infinity;
        hc.Heal(hc.MaxHealth);
        hc.isDead = false;
        hc.healthBar.transform.parent.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        //shake is only detected for one frame.
        shook = false;

        //check if the phone has been shook
        if (isPhoneShook())
        {
            //check if the phone has recovered since it was last shaken
            if (canShake)
            {
                //"if(shook)" could go here but I'm doing this incase I set shook to true anywhere else
                shook = true;
            }
        }
        //what to do if the phone has been shook
        if(shook)
        {
            //start the cooldown of shaking so that it doesnt trigger multiple times
            canShake = false;
            StartCoroutine(shakeCoolDown());
        }

        //set the rolls number on UI
        rollsText.text = $"{rolls}";

        //controls are dependent on the current game state.
        //look at the GameManager for additional functions
        switch (GameManager.currentState)
        {

            // the core gameplay controls
            case GameManager.GameState.GAMEPLAY:
                //switch camera
                mainCamera.enabled = true;
                cameraModeCamera.enabled = false;
                ToiletCam.enabled = false;

                //catch the error for no gyro being detected, this was useful for testing on a PC
                if (gyro != null)
                {
                    //tilted to the right
                    if (Input.acceleration.x > 0.2)
                    {
                        rotateRight(Input.acceleration.x * turnSpeed);
                    }
                    //tilted to the left
                    else if (Input.acceleration.x < -0.2)
                    {
                        rotateLeft(-Input.acceleration.x * turnSpeed);
                    }
                    //if the player has tapped the screen with any number of fingers
                    if (Input.touchCount > 0)
                    {
                        int touchID = Input.touches[0].fingerId;
                        //only happens on the first frame of the first finger tap
                        //only attacks if the player didnt tap a button
                        if (Input.touches[0].phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touchID))
                        {
                            attack();
                        }
                    }

                    //makes the player not slippy. stops when not moving
                    if (!isMoving && isGrounded)
                    {
                        stopMoveXZ();
                    }
                }

                //change player speed and allow player to jump while grounded
                //do the opposite while not grounded
                if (isGrounded)
                {
                    speed = groundSpeed;
                    anim.SetBool("isGrounded", true);
                    canJump = true;
                }
                else
                {
                    speed = airSpeed;
                    anim.SetBool("isGrounded", false);
                    canJump = false;
                }
                //while digging, run the dig function every frame
                if (isDigging)
                {
                    dig();
                }
                //while not digging, change the animation
                if (!isDigging)
                {
                    anim.SetBool("isDigging", false);
                }
                //allows player to sit on toilet after shaking near a toilet
                if (shook && nearToilet)
                {
                    shook = false;
                    sitOnToilet();
                }
                //while backstepping, run the backstep function everyframe. 
                if (isBackstepping)
                {
                    backStep();
                }
                break;
                //When you press the camera button
            case GameManager.GameState.CAMERA:
                //stop the player moving
                stopMoveXZ();
                //switch camera
                mainCamera.enabled = false;
                cameraModeCamera.enabled = true;
                //Use the gyro to move the camera around. affected by frame-rate
                cameraModeCamera.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
                break;
                //while the player is looking at the map
            case GameManager.GameState.MAP:
                stopMoveXZ();
                break;
                //while the player is sat on the toilet
            case GameManager.GameState.TOILET:
                stopMoveXZ();
                //shake to exit toilet
                if (shook)
                {
                    shook = false;
                    getOffToilet();
                }
                //switch camera to main camera
                mainCamera.enabled = false;
                ToiletCam.enabled = true;
                break;
        }

        //if the player dies, trigger the died() event
        if (GetComponentInChildren<healthController>().isDead)
        {
            died();
        }
    }

    //checks if the player is shaking the phone
    public bool isPhoneShook()
    {
        return Mathf.Abs(Input.acceleration.x) > minimumShake || Mathf.Abs(Input.acceleration.y) > minimumShake || Mathf.Abs(Input.acceleration.z) > minimumShake;
    }

    //heals the player to full health. 
    //happens when sitting on toilet and after dying.
    public void resetHealth()
    {
        //get the health controller
        healthController hc = GetComponent<healthController>();
        //heal by the player's maxhealth
        hc.Heal(hc.MaxHealth);
        //if the player was dead, revive them
        hc.isDead = false;
        //if the healthbar was turned off, turn it back on
        hc.healthBar.transform.parent.gameObject.SetActive(true);
    }

    //this is a function so that it may be used in the animator
    public void startDigging()
    {
        isDigging = true;
    }

    //makes the player sit on the toilet
    public void sitOnToilet()
    {
        //changes the gameState
        GameManager.sitOnToilet();
        //teleports the player to the most recently visited toilet
        transform.position = nearestToilet.position;
        //sets rotation of player to rotation of most recently visited toilet
        transform.rotation = nearestToilet.rotation;
        //Heals player to full health
        resetHealth();
        //activates the sitting animation
        anim.SetBool("toilet", true);
    }
    
    //reverses the effects of sitting on the toilet
    public void getOffToilet()
    {
        //switches game state back to GAMEPLAY
        GameManager.getOffToilet();
        //exits sitting animation
        anim.SetBool("toilet", false);
    }
    //rotate the player left
    public void rotateLeft(float speed)
    {
        transform.Rotate(new Vector3(0, -speed * Time.deltaTime, 0));
    }

    //rotate the player right
    public void rotateRight(float speed)
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }

    //increases the attack damage of all attacks
    void UpgradeAttacks(int level)
    {
        foreach (GameObject attack in attacks)
        {
            attack.GetComponent<attack>().damage += level * 5;
        }
    }

    //increases the size of each attack
    void UpgradeAttackRange(int level)
    {
        foreach (GameObject attack in attacks)
        {
            attack.transform.localScale *= 1 + level / 2;
        }
    }
    //increases the speed at which digging occurs
    void UpgradeDigSpeed(int level)
    {
        digAmount += 1;
    }
    //increases Maximum Health
    void UpgradeHealth(int level)
    {
        healthController hc = GetComponent<healthController>();
        hc.MaxHealth += level * 20;
        hc.Heal(hc.MaxHealth);
    }

    //performs an attack
    public void attack()
    {
        //interupts digging
        isDigging = false;
        //starts an animation. 
        //which animation depends on attackNum
        anim.SetTrigger("attack");
        anim.SetInteger("attack num", attackNum);
        //interrupts movement
        moveButton.toggleButton = false;
        //progresses through the attack combo
        attackNum++;
    }
    //moves the player forward
    public void move()
    {
        //some things stop the player moving
        if (canMove)
        {
            //change animation to running
            anim.SetBool("isRunning", true);
            //sets the velocity of the player
            rb.velocity = new Vector3(transform.forward.x * speed, rb.velocity.y, transform.forward.z * speed);
            //interrupts digging
            isDigging = false;
        }
    }

    //moves player backwards
    public void backStep()
    {
        //only if you can move
        if (canMove)
        {
            //sends the player backwards
            rb.velocity = (-transform.forward * backstepSpeed);
        }
    }
   //performs a backstep
    public void startBackStep()
    {
        //can only backstep while on the ground and not already backstepping
        if (isGrounded && !isBackstepping)
        {
            //interrupts digging
            isDigging = false;
            //the player is now backstepping 
            isBackstepping = true;
            //plays the animation
            anim.SetTrigger("backstep");
            //starts the backstep cooldown
            StartCoroutine(stopBackstep());
        }
    }

    //cooldown for backstepping
    public IEnumerator stopBackstep()
    {
        yield return new WaitForSeconds(backstepTime);
        isBackstepping = false;
        //stop moving after backstepping
        stopMoveXZ();
    }

    //stops movement on the x and z axis. useful way of stopping movement without choppy falling.
    public void stopMoveXZ()
    {
        //changes animation to stop running
        anim.SetBool("isRunning", false);
        //sets the velocity to 0 on the x and z axis
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    //useful for stopping movement via animation event trigger
    public void disallowMoving()
    {
        canMove = false;
    }
    //useful for regaining movement via animation event trigger
    public void allowMoving()
    {
        canMove = true;
    }

    //performs a jump
    public void jump()
    {
        if (canJump)
        {
            //interrupts digging
            isDigging = false;
            //interrupts moving
            moveButton.toggleButton = false;
            //changes animation to jumping
            anim.SetTrigger("jump");
            //makes player jump up
            //(it says "+ transform.forward" but has no effect.)
            rb.velocity = (transform.up + transform.forward) * jumpHeight;
        }
    }

    //performs one iteraction of a dig. digs treasure.
    public void dig()
    {
        //changes animation to digging
        anim.SetBool("isDigging", true);
        //stops movement
        stopMoveXZ();
        //catch the error for if nothing is in the event
        if (digSomething != null)
        {
            //calls the digging event
            //has an effect on any treasure that is touching the player
            digSomething(digAmount);
        }
    }
    
    //allows time between shaking
    public IEnumerator shakeCoolDown()
    {
        yield return new WaitForSeconds(1f);
        canShake = true;
    }


    public void OnTriggerEnter(Collider other)
    {
        //if near a toilet
        if (other.tag == "toilet")
        {
            //interrupts movement
            moveButton.toggleButton = false;
            //allows player to sit on toilet
            nearToilet = true;
            //saves this as a respawn point
            nearestToilet = other.transform;
        }
        if(other.tag == "goldenObject")
        {
            //triggers event for collecting a quest item
            GoldenCollected(other.GetComponent<goldenObject>().id);
            //get rid of the object
            Destroy(other.gameObject);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "toilet")
        {
            //player can no longer sit on the toilet
            nearToilet = false;
        }
    }

    public void resetAttackCombo()
    {
        //sets the current attack to the first one
        attackNum = 0;
    }
    //turns on this attack hitbox via animation event trigger
    public void turnOnAttack(int attack)
    {
        attacks[attack].SetActive(true);
    }
    //turns off this attack hitbox via animation event trigger
    public void turnOffAttack(int attack)
    {
        attacks[attack].SetActive(false);
    }

    //turns off all attacks.
    //set the current attack to be the first attack
    public void resetAllAttacks()
    {
        foreach(GameObject attack in attacks)
        {
            attack.SetActive(false);
        }
        attackNum = 0;
        anim.SetInteger("attack num", attackNum);
    }


    //Getters



    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public bool GetIsDigging()
    {
        return isDigging;
    }

    public bool GetIsBackstepping()
    {
        return isBackstepping;
    }

    public Transform GetNearestToilet()
    {
        return nearestToilet;
    }

    public float GetGroundSpeed()
    {
        return groundSpeed;
    }

    public float GetTurnSpeed()
    {
        return turnSpeed;
    }

    public bool GetShook()
    {
        return shook;
    }
    //setters

    public void SetIsGrounded(bool boolSet)
    {
        isGrounded = boolSet;
    }

    public void SetIsMoving(bool boolSet)
    {
        isMoving = boolSet;
    }

    public void SetIsDigging(bool boolSet)
    {
        isDigging = boolSet;
    }

    public void SetIsBackstepping(bool boolSet)
    {
        isBackstepping = boolSet;
    }

    public void SetNearestToilet(Transform transformSet)
    {
        nearestToilet = transformSet;
    }

    public void SetGroundSpeed(float floatSet)
    {
        groundSpeed = floatSet;
    }
    
    public void SetTurnSpeed(float floatSet)
    {
        turnSpeed = floatSet;
    }

    public void SetShook(bool boolSet)
    {
        shook = boolSet;
    }
}
