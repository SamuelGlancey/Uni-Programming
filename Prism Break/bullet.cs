using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//the bullet moves until either it hits a block, edge of the map, or runs out of time.
// if the bullet is notDestructable, the bullet will keep going. this was meant for special abilities later
public class bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject white;
    public float bulletSpeed;
    public List<Vector3> rainbowColors;
    public int thisColor;
    [SerializeField] private float lifeTime;
    public TrailRenderer trail;
    public bool notDestructable;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("destroyBullet", lifeTime); //destroys bullet after set time
        resetStartingColour();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Wall")
        {
            if (!notDestructable) // if destructable, destroy
            {
                destroyBullet();
            }
            other.GetComponent<wall>().destroythis();
        }
        if(other.tag == "edgewall")
        {
            Destroy(this.gameObject);
        }
    }

    void destroyBullet()
    {
        Destroy(this.gameObject);
    }
    
    void move()
    {
        rb.velocity = transform.right * -bulletSpeed;
    }

    void resetStartingColour()
    {
        GetComponent<TrailRenderer>().startColor = new Color(rainbowColors[thisColor].x, rainbowColors[thisColor].y, rainbowColors[thisColor].z);
        GetComponent<SpriteRenderer>().color = new Color(rainbowColors[thisColor].x, rainbowColors[thisColor].y, rainbowColors[thisColor].z);
    }
}
