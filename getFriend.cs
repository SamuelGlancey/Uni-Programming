using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for collecting a friend and discarding that friend
//is the base class of GetBadFriend

public class getFriend : MonoBehaviour
{
    protected bool collected;
    protected bool shot;
    protected int shotNumber;
    [SerializeField]protected GameObject powerOfFriendship;
    protected Vector3 splittingDirection;
    private Vector2Int gridPosition;
    [SerializeField]protected float shootSpeedPenalty;
    // Start is called before the first frame update
    void Start()
    {
        while (splittingDirection.x == 0 && splittingDirection.y == 0)
        {
            splittingDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = gameManager.PlayerColor;
        if (shot)
        {

            transform.position += splittingDirection * 50 * Time.deltaTime;
            GetComponent<shoot>().enabled = false;
            Destroy(gameObject, 5f);
            
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collected)
            {
                collect(collision);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected)
        {
            if (collision.tag == "pof")
            {

                Destroy(this.gameObject);
            }
        }
        
    }
    virtual public void fire()
    {
        
        shot = true;
        transform.parent = null;
        gameManager.shootInterval -= shootSpeedPenalty;
        bullet BLT = Instantiate(powerOfFriendship, transform.position, transform.rotation, transform).GetComponent<bullet>();
        BLT.thisColor = shotNumber;
        Destroy(GetComponent<BoxCollider2D>());
    }

    public virtual void collect(Collision2D collision)
    {
        collected = true;
        GetComponent<shoot>().enabled = true;
        GetComponent<shoot>().Set_bulletNumber(collision.gameObject.GetComponent<movement>().Get_bulletNumber());
        GetComponent<shoot>().Set_canShoot(false);
        transform.parent = collision.transform;
        gameManager.shootInterval += shootSpeedPenalty;
        transform.parent.GetComponent<movement>().Get_friends().Add(this.gameObject); 
    }

    public float Get_shootSpeedPenalty()
    {
        return shootSpeedPenalty;
    }
    public int Get_shotNumber()
    {
        return shotNumber;
    }
    public void Set_shotNumber(int SetToInt)
    {
        shotNumber = SetToInt;
    }
    public Vector2Int Get_gridPosition()
    {
        return gridPosition;
    }

    public void Set_gridPosition(Vector2Int setToVector2Int)
    {
        gridPosition = setToVector2Int;
    }
}
