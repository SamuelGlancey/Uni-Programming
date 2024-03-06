using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//an imploding barrel
//when shot by a bullet, it will spawn an area of squares
public class barrel : MonoBehaviour
{
    public mapgeneration mapGen;
    public int range;
    public GameObject black;
    public Transform blackBlocks;
    public Vector2Int gridPosition;
    // Start is called before the first frame update
    void Start()
    {
        blackBlocks = GameObject.Find("blackBlocks").transform;
        mapGen = GameObject.FindWithTag("mapGen").GetComponent<mapgeneration>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bullet")
        {
            Destroy(collision.gameObject);
            spawnBlocks();
            Destroy(this.gameObject);
        }
        if(collision.tag == "pof")
        {
            Invoke("spawnBlocks", 0.5f);
            Destroy(this.gameObject,0.6f);

        }
    }
    public void spawnBlocks()
    {
        for (int i = 0; i < mapGen.Get_width(); i++)
        {
            for (int j = 0; j < mapGen.Get_height(); j++)
            {
                if (i > gridPosition.x - range && i < gridPosition.x + range && j > gridPosition.y - range && j < gridPosition.y + range)
                {
                    var square = Instantiate(black, new Vector3(mapGen.transform.position.x + i, mapGen.transform.position.y + j), Quaternion.identity, blackBlocks);
                    var map = mapGen.Get_map();
                    map[i, j] = 1;
                    mapGen.Set_map(map);
                    square.GetComponent<wall>().isAnEdge = mapGen.isAnEdge(i, j);
                    square.GetComponent<wall>().gridPosition = new Vector2Int(i, j);
                    square.GetComponent<wall>().partOfThisMap = mapGen;
                }
            }
        }
    }
}
