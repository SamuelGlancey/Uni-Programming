using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/* Class description
 * A wall has a different sprite if is an edgeblock
 * an edgeblock is defined as being a block with less than 4 adjacent blocks
 */

public class wall : MonoBehaviour
{
    public bool isAnEdge;
    public Vector2Int gridPosition;
    public mapgeneration partOfThisMap;
    public Sprite wallSpr;
    public Sprite edgeSpr; //this sprite is dragged into the inspector
    public GameObject white;
    public bool canEnemySpawn;
    public GameObject badFriend;
    // Start is called before the first frame update
    void Start()
    {
        wallSpr = GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().color = gameManager.BlockColor;
        var map = partOfThisMap.Get_map();
        map[gridPosition.x, gridPosition.y] = 1;
        partOfThisMap.Set_map(map);
    }

    // Update is called once per frame
    void Update()
    {

        //isAnEdge is defined in the mapgeneration script
        GetComponent<SpriteRenderer>().color = gameManager.BlockColor;
        isAnEdge = partOfThisMap.isAnEdge(gridPosition.x, gridPosition.y);
        if (isAnEdge)
        {
            GetComponent<SpriteRenderer>().sprite = edgeSpr;
            if (canEnemySpawn)
            {
                checkIfCanSpawnEnemy();
            }
        }
        if (!isAnEdge)
        {
            GetComponent<SpriteRenderer>().sprite = wallSpr;
            canEnemySpawn = true;
        }
    }  

    public void checkIfCanSpawnEnemy()
    {
        canEnemySpawn = false;
        int rando = Random.Range(0, 100);
        if (rando == 1)
        {
            Instantiate(badFriend, transform.position, transform.rotation, transform.parent);
            Destroy(this.gameObject);
        }
    }
    public void destroythis()
    {
        Instantiate(white, transform.position, transform.rotation, transform.parent.parent.GetChild(1));
        var map = partOfThisMap.Get_map();
        map[gridPosition.x, gridPosition.y] = 0;
        partOfThisMap.Set_map(map);

        Destroy(this.gameObject);
    }
}
