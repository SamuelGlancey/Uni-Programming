using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class skeleton : MonoBehaviour
{
    bool isLeft;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float chargeSpeed = 12;
    [SerializeField] private List<GameObject> attacks;
    bool canAttack;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerController_ pc;
    float time;
    [SerializeField] private float attackCooldown;
    [SerializeField] private healthController hc;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float aggroDistance = 20;
    bool isAggro;
    [SerializeField] private bool isSpawnedIn;
    EnemyStates tempState;
    public enum EnemyStates
    {
        RUNNING,
        ATTACKING,
        IDLE,
        NONE
    }
    EnemyStates currentState;
    public NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav.speed = moveSpeed;
        currentState = EnemyStates.IDLE;
    }

    //subscribe to events that effect the skeletons
    private void OnEnable()
    {
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController_>();
        //happens when player is not in gameplay mode
        GameManager.pause += OnPause;
        GameManager.unPause += OnUnPause;
        //if this is a skeleton that spawned in, it takes damage on death
        if (isSpawnedIn)
        {
            pc.died += deathPenalty;
            pirateBoss.bossDied += deathPenalty;
            pirateBoss.bossDied += deathPenalty;
        }
    }
    //unsubscribe to the events
    private void OnDisable()
    {
        GameManager.pause -= OnPause;
        GameManager.unPause -= OnUnPause;
        if (isSpawnedIn)
        {
            pc.died -= deathPenalty;
            pirateBoss.bossDied -= deathPenalty;
            pirateBoss.bossDied -= deathPenalty;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //start the frame by setting the navmesh destination to the player's position
        nav.destination = pc.transform.position;
        switch (currentState)
        {
            //while running
            case EnemyStates.RUNNING:
                //setting the animation to running
                anim.SetBool("isRunning", true);
                //set the speed to regular speed
                nav.speed = moveSpeed;
                if (!canAttack)
                {
                    //cooldown until next attack
                    waitToAttack();
                }
                //if you can attack and in range
                if (inAttackRange() && canAttack)
                {
                   //switch to attack state
                    currentState = EnemyStates.ATTACKING;
                }
                //if in range but cant attack
                if(inAttackRange() && !canAttack)
                {
                    //switch to idle state
                    setIdle();
                }
                break;
                //while attacking
            case EnemyStates.ATTACKING:
                if (canAttack)
                {
                    //performs an attack and switches to idle
                    startAttack();
                }
                break;
            case EnemyStates.IDLE:
                //rotate to face player
                lookAtPlayer();
                //stop running animation
                anim.SetBool("isRunning", false);
                //set destination to current position
                nav.destination = transform.position;
                if (!canAttack)
                {
                    //cooldown until next attack
                    waitToAttack();
                }
                //aggro is the range at which the player is noticed by the skeleton
                if (!inAttackRange() && isAggro)
                {
                    //switch to running state
                    currentState = EnemyStates.RUNNING;
                }
                else if (canAttack && isAggro)
                {
                    //switch to attacking state
                    currentState = EnemyStates.ATTACKING;
                }
                //checks if the player is in the aggro range of the player
                checkIfAggro();
                break;
            case EnemyStates.NONE:
                //the enemy is completely inactive
                nav.destination = transform.position;
                anim.SetBool("isRunning", false);
                break;
        }
        //checks if the enemy is dead
        if (hc.isDead)
        {
            die();
        }
        //checks if the enemy is within stopping distance. this stops the enemy from being directly ontop of the player
        if(Vector3.Distance(transform.position, pc.transform.position) < stoppingDistance)
        {
            //basically the same as being idle
            nav.destination = transform.position;
            anim.SetBool("isRunning", false);
        }

    }
    void die()
    {
        //plays the dying animation
        anim.SetTrigger("isDead");
        //makes the enemy inactive
        currentState = EnemyStates.NONE;
        //destroys object after animation finishes
        Destroy(gameObject, 1.5f);
    }

    //performs an attack
    void startAttack()
    {
        //enemy moves faster toward player
        nav.speed = chargeSpeed;
        //enemy alternates between doing left and right handed attacks
        isLeft = !isLeft;
        //triggers animation
        anim.SetBool("isLeft", isLeft);
        anim.SetTrigger("attack");
        canAttack = false;
    }

    public void checkIfAggro()
    {
        if (!isAggro)
        {
            //if the distance between the enemy and the player is less than a value
            if (Vector3.Distance(transform.position, pc.transform.position) < aggroDistance)
            {
                isAggro = true;
            }
        }
    }
    //cooldown till attack
    public void waitToAttack()
    {
        //increment time by seconds since last frame
        time += Time.deltaTime;
        //if time is bigger than a value
        if (time > attackCooldown)
        {
            //reset timer
            time = 0;
            //trigger an attack
            canAttack = true;
        }
    }
    public void deathPenalty()
    {
        // if this is a spawned in skeleton, enemy takes 35 damage when player dies
        hc.takeDamage(35);
    }
    public void lookAtPlayer()
    {
        PlayerController_ pc = GameObject.FindWithTag("Player").GetComponent<PlayerController_>();
        Vector3 dir = pc.transform.position - transform.position;
        dir.y = 0;//This allows the object to only rotate on its y axis
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 50 * Time.deltaTime);
    }
    //activates an attack. used in animation event trigger
    public void turnOnAttack(int attack)
    {
        attacks[attack].SetActive(true);
    }
    //deactivates an attack. used in animation event trigger
    public void turnOffAttack(int attack)
    {
        attacks[attack].SetActive(false);
    }

    //transitions enemy to idle state
    public void setIdle()
    {
        if (currentState != EnemyStates.NONE)
        {
            currentState = EnemyStates.IDLE;
        }
        
    }

    //checks if player is in the attack range of enemy
    bool inAttackRange()
    {
        return Vector3.Distance(transform.position, pc.transform.position) < attackRange;
    }

    //deactivates enemy when paused
    void OnPause()
    {
        currentState = EnemyStates.NONE;
    }
    //activates enemy when paused
    void OnUnPause()
    {
        currentState = EnemyStates.IDLE;
    }
}
