using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public delegate void bossEvent();

public class pirateBoss : MonoBehaviour
{
    public static event bossEvent bossDied;
    public Animator anim;
    public GameObject shovelHitbox;
    float attackCooldown;
    float time;
    bool deathTrigger = true;
    public GameObject skelly;
    healthController hc;
    PlayerController_ pc;
    public bool spawnMini = true;
    Vector3 startPos;
    Quaternion startRot;
    // Start is called before the first frame update
    void Start()
    {
        attackCooldown = Random.Range(1.5f, 4f);
        hc = GetComponent<healthController>();

    }
    public void OnEnable()
    {
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController_>();
        startPos = transform.position;
        startRot = transform.rotation;
        pc.died += reset;
    }

    public void OnDisable()
    {

        pc.died -= reset;
    }

    private void reset()
    {
        transform.position = startPos;
        transform.rotation = startRot;
        hc.gameObject.SetActive(true);
        hc.Heal(hc.MaxHealth);
    }
    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = pc.transform.position;
        if (hc.isDead)
        {
            anim.SetBool("isDead", true);
            if (deathTrigger)
            {
                bossDied();
                deathTrigger = false;
            }
        }
        else
        {
            lookAtPlayer();
            time += Time.deltaTime;
            if (time > attackCooldown)
            {
                attack();
                time = 0;
            }
        }

    }

    public void turnOnAttack()
    {
        shovelHitbox.SetActive(true);
    }
    public void turnOffAttack()
    {
        shovelHitbox.SetActive(false);
    }
    public void lookAtPlayer()
    {
        PlayerController_ pc = GameObject.FindWithTag("Player").GetComponent<PlayerController_>();
        Vector3 dir = pc.transform.position - transform.position;
        dir.y = 0;//This allows the object to only rotate on its y axis
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 50 * Time.deltaTime);
    }
    float attackCount;
    void attack()
    {
        attackCooldown = Random.Range(1.5f, 2.5f);
        attackCount++;
        if (hc.currentHealth < hc.MaxHealth/2)
        {
            if (spawnMini)
            {
                Instantiate(skelly, transform.position, transform.rotation);
                Instantiate(skelly, transform.position, transform.rotation);
                Instantiate(skelly, transform.position, transform.rotation);
                Instantiate(skelly, transform.position, transform.rotation);
                Instantiate(skelly, transform.position, transform.rotation);
                spawnMini = false;
            }
        }
        else
        {
            spawnMini = true;
        }
        anim.SetInteger("attackNum", Random.Range(0,7));
        anim.SetTrigger("attack");
    }
}
