using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Attached to the enemy object
//Inherits from the getFriend script
//overrides collect() to destroy the pathfinding scripts when collected
//overrides fire to create a lasting effect of slowed shootspeed for a short time instead of
//returning the shootspeed to normal immediately
public class GetBadFriend : getFriend
{
    // Start is called before the first frame update
    [SerializeField] private float survivalTime;
    private float currentTime;
    [SerializeField] private GameObject wall;

    void Update()
    {
        GetComponent<SpriteRenderer>().color = gameManager.PlayerColor;
        if (shot)
        {

            transform.position += splittingDirection * 50 * Time.deltaTime;
            GetComponent<shoot>().enabled = false;
            Destroy(gameObject, 2f);

        }

        currentTime += Time.deltaTime;
        if(currentTime >= survivalTime)
        {
            if (!collected)
            {
                Vector3Int intPos = new Vector3Int((int)Mathf.RoundToInt(transform.position.x), (int)Mathf.RoundToInt(transform.position.y));
                wall thisWall = Instantiate(wall,intPos, transform.rotation, transform.parent).GetComponent<wall>();
                thisWall.partOfThisMap = GameObject.FindWithTag("mapGen").GetComponent<mapgeneration>();
                thisWall.GetComponent<wall>().isAnEdge = thisWall.partOfThisMap.isAnEdge(intPos.x, intPos.y);
                thisWall.GetComponent<wall>().gridPosition = new Vector2Int(intPos.x, intPos.y);
                Destroy(GetComponent<findPath>().Get_thisNodeStorage());
                Destroy(this.gameObject);
                

            }
        }
    }
    public override void collect(Collision2D collision)
    {
        base.collect(collision);
        Destroy(GetComponent<moveThroughPath>());
        Destroy(GetComponent<findPath>().Get_thisNodeStorage());
        Destroy(GetComponent<findPath>());
    }
    public override void fire()
    {

        shot = true;
        transform.parent = null;
        //StartCoroutine(delayTheRegainingOfShootSpeed());
        Destroy(GetComponent<BoxCollider2D>());

    }
}
