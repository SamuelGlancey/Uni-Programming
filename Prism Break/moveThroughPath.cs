using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(findPath))]//if moveThroughPath is added to an object, findPath will also be added. 

//makes the enemy move through each coordinate in the list which was
//created by the pathfinding script.
public class moveThroughPath : MonoBehaviour
{
    private findPath fp;
    private int pathIndex = 0;
    private bool canIncrement;
    [SerializeField]private float enemySpeed;
    // Start is called before the first frame update
    void Start()
    {
        fp = GetComponent<findPath>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fp.Get_Path().Count > 0)
        {
            if (pathIndex <= fp.Get_Path().Count - 1)
            {
                Vector3 nextSquare = new Vector3(fp.Get_Path()[fp.Get_Path().Count - 1 - pathIndex].x, fp.Get_Path()[fp.Get_Path().Count - 1 - pathIndex].y);
                transform.position = Vector3.MoveTowards(transform.position, nextSquare, enemySpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, nextSquare) < 0.1f)
                {
                    pathIndex++;
                }
            }
            else
            {
                pathIndex = 0;
            }
        }
    }
}
