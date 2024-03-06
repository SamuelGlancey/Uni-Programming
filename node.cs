using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//individual node which is used in the 
//pathfinding script.
//responsible for storing costs and decides whether the node is a wall
//also calculates costs
public class node : MonoBehaviour
{

    private float Gcost;
    private float Hcost;
    private float Fcost;
    private bool isWall;
    private bool isClosed;
    private bool isOpen;
    private Transform start;
    private mapgeneration mapGen;
    private findPath fromThisEnemy;
    // Start is called before the first frame update
    void Start()
    {
        start = fromThisEnemy.transform;
        mapGen = GameObject.FindWithTag("mapGen").GetComponent<mapgeneration>();
    }

    // Update is called once per frame
    void Update()
    {
        checkIfNodeIsAWall();
        //Debug.Log(mapGen);
    }

    public void checkNeighbours(List<GameObject> open)
    {
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if( transform.position.x + i < fromThisEnemy.Get_PathGrid().GetLength(0) && 
                    transform.position.x + i >= 0 && 
                    transform.position.y + j < fromThisEnemy.Get_PathGrid().GetLength(1) && 
                    transform.position.y + j >= 0 )
                {
                    if(fromThisEnemy.Get_PathGrid()[(int)Mathf.RoundToInt(transform.position.x) + i, (int)Mathf.RoundToInt(transform.position.y) + j] != null)
                    {
                         node square = fromThisEnemy.Get_PathGrid()[(int)Mathf.RoundToInt(transform.position.x) + i, (int)Mathf.RoundToInt(transform.position.y) + j].GetComponent<node>();
                         if (new Vector2(i, j) != new Vector2(0, 0) &&
                            !square.isWall &&
                            !square.isClosed &&
                            !(Mathf.Abs(i) == Mathf.Abs(j)))
                         {
                                if (square.isOpen)
                                {
                                    calculateAllCosts(square.Fcost);
                                }

                                if (!square.isOpen)
                                {
                                    open.Add(square.gameObject);
                                    square.transform.parent = transform;
                                    square.fromThisEnemy = fromThisEnemy;

                                }
                         }
                    }
                }
            }
        }
    }

    public void calculateAllCosts(float currentFcost)
    {
        Hcost = Vector3.Distance(fromThisEnemy.Get_EndSquare().transform.position, transform.position);
        Gcost = Vector3.Distance(transform.position, fromThisEnemy.Get_StartSquare().transform.position);
        Fcost = Gcost + Hcost;
        if (Fcost > currentFcost)
        {
            Fcost = currentFcost;
        }
    }
    public void checkIfNodeIsAWall()
    {
        if ((int)Mathf.RoundToInt(transform.position.x) > 0 && (int)Mathf.RoundToInt(transform.position.x) < mapGen.Get_width() && (int)Mathf.RoundToInt(transform.position.y) > 0 && (int)Mathf.RoundToInt(transform.position.y) < mapGen.Get_height())
        {
            if (mapGen.Get_map()[(int)Mathf.RoundToInt(transform.position.x), (int)Mathf.RoundToInt(transform.position.y)] == 1)
            {
                isWall = true;
            }
            else if (mapGen.Get_map()[(int)Mathf.RoundToInt(transform.position.x), (int)Mathf.RoundToInt(transform.position.y)] == 0)
            {
                isWall = false;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public int roundUP(float value)
    {
        int valueInt = 0;
        if (value > 0)
        {
            valueInt = 1;
        }
        if (value < 0)
        {
            valueInt = -1;
        }
        return valueInt;
    }

    public bool Get_isClosed()
    {
        return isClosed;
    }

    public void Set_isClosed(bool setToBool)
    {
        isClosed = setToBool;
    }

    public bool Get_isOpen()
    {
        return isOpen;
    }

    public void Set_isOpen(bool setToBool)
    {
        isOpen = setToBool;
    }
    public bool Get_isWall()
    {
        return isWall;
    }

    public void Set_isWall(bool setToBool)
    {
        isWall = setToBool;
    }
    public float Get_Fcost()
    {
        return Fcost;
    }

    public void Set_Fcost(float setToFloat)
    {
        Fcost = setToFloat;
    }
    public float Get_Hcost()
    {
        return Hcost;
    }

    public void Set_Hcost(float setToFloat)
    {
        Hcost = setToFloat;
    }
    public float Get_Gcost()
    {
        return Gcost;
    }

    public void Set_Gcost(float setToFloat)
    {
        Gcost = setToFloat;
    }

    public findPath Get_fromThisEnemy()
    {
        return fromThisEnemy;
    }

    public void Set_fromThisEnemy(findPath setTofindPath)
    {
        fromThisEnemy = setTofindPath;
    }
}
