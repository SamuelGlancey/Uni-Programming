using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//responsible for the movement of the player 
//and the use of the ultimate ability.
public class movement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]private GameObject Bullet;
    [SerializeField]private GameObject AOEeffect;
    [SerializeField]private float speed;
    [SerializeField]private Transform rotatingShootPoint;
    [SerializeField]private Transform shootPoint;
    private bool canShoot;
    private float ultimateProgress;
    [SerializeField] private int ultCost;
    [SerializeField] private GameObject UIultimate;
    [SerializeField] private Transform canvas, pauseUI;
    private int bulletNumber;
    private int shotNumber;
    private List<GameObject> friends;
    private float bps;
    [SerializeField]private Text bpsText;




    // Start is called before the first frame update
    void Start()
    {
        friends = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        canShoot = true;
        GetComponent<SpriteRenderer>().color = gameManager.PlayerColor;
        
    }

    // Update is called once per frame
    void Update() 
    {
        GatherInput();
        pausingCheck();
        movePlayer();
        updateUltimateTokens();
        calculateBPS();
    }

    public IEnumerator rapidFire(float speedChange)
    {
        gameManager.shootInterval -= speedChange;
        yield return new WaitForSeconds(15);
        gameManager.shootInterval += speedChange;
    }
    public void ultimate()
    {
        GetComponent<ParticleSystem>().Play();
        Instantiate(AOEeffect, transform.position, transform.rotation);
    }

    public void calculateBPS()
    {
        bps = (1 / gameManager.shootInterval) * (friends.Count + 1);
        
        bpsText.text = $"{bps.ToString("F2")}b/s";
    }
    void DiscardFriend()
    {
        if (friends.Count - 1 >= 0)
        {
            shotNumber++;
            if (shotNumber > 5)
            {
                shotNumber = 0;
            }
            friends[friends.Count - 1].GetComponent<getFriend>().Set_shotNumber(shotNumber);
            friends[friends.Count - 1].GetComponent<getFriend>().fire();
            if(friends[friends.Count - 1].tag == "badFriend")
            {
                StartCoroutine(regainShootSpeed(friends[friends.Count - 1].GetComponent<GetBadFriend>().Get_shootSpeedPenalty()));
            }
            friends.RemoveAt(friends.Count - 1);
        }
    }

    void pausingCheck()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            gameManager.isPaused = !gameManager.isPaused;
        }
        if (gameManager.isPaused)
        {
            GetComponent<SpriteRenderer>().color = gameManager.PlayerColor;
            pauseUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        if (!gameManager.isPaused)
        {
            if (progress.progressPercent != 0)
            {
                Time.timeScale = 1f;
                pauseUI.gameObject.SetActive(false);
            }
        }
    }

    void GatherInput()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (canvas.childCount > 0)
            {
                Destroy(canvas.GetChild(0).gameObject);
                ultimate();
            }
        }

        if (Input.GetButtonDown("Fire3"))
        {
            DiscardFriend();
        }

    }
    public IEnumerator regainShootSpeed(float shootSpeedPenalty)
    {
        yield return new WaitForSeconds(5);
        gameManager.shootInterval -= shootSpeedPenalty;
    }
    void movePlayer()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        rb.velocity = (move * speed);
    }
    void updateUltimateTokens()
    {
        ultimateProgress += Time.deltaTime;
        if (ultimateProgress >= ultCost)
        {
            ultimateProgress -= ultCost;
            Instantiate(UIultimate, canvas);
        }
    }

    //Getters and Setters
    public int Get_bulletNumber()
    {
        return bulletNumber;
    }

    public void Set_bulletNumber(int setToInt)
    {
        bulletNumber = setToInt;
    }

    public List<GameObject> Get_friends()
    {
        return friends;
    }


} 