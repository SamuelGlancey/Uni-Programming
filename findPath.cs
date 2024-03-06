using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//A* pathfinding used to create a list of coordinates 
//between the enemy and the player along the shortest route
public class findPath : MonoBehaviour
{

    private Transform End;
    [SerializeField] private GameObject node;
    private GameObject EndSquare;
    private GameObject StartSquare;
    private Vector3 startCoords;
    private List<GameObject> open;
    private List<GameObject> closed;
    private List<Vector2Int> Path;
    private GameObject targetSquare;
    private bool canDraw = true;
    private mapgeneration mapGen;
    private GameObject[,] PathGrid;
    private Transform player;
    [SerializeField] private int EnemyAttackRange;
    [SerializeField] private GameObject nodeStorage;
    private GameObject thisNodeStorage;
    [SerializeField] private float lifeTime;

    // Start is called before the first frame update
    void Start()
    {

        startingVariables();
        generateNodes();
        
    }

    // Update is called once per frame
    void Update()
    {
        checkIfInRange();
        if (canDraw)
        {
            canDraw = false;
            resetPath();
            drawPath();
        }
        
    }

    public void drawPath()
    {
        int count = 0;
        while(true)
        {
            count++;
            closed.Add(targetSquare);
            open.Remove(targetSquare);
            targetSquare.GetComponent<node>().Set_isClosed(true);
            targetSquare.GetComponent<node>().checkNeighbours(open);
            GameObject lowestOBJ = findLowestFcostObject();
            if (targetSquare == EndSquare || count > 350)
            {
                break;
            }
            if (lowestOBJ != null)
            {
                lowestOBJ.transform.parent = lowestOBJ.transform;
                targetSquare = lowestOBJ;
            }
        }
        drawFinalPath();
                
    }

    public void drawFinalPath()
    {
        Transform square = EndSquare.transform;
        if (square != null)
        {
            Path.Add(new Vector2Int((int)square.transform.position.x, (int)square.transform.position.y));
            while (square != transform.root)
            {
                if (square == null)
                {
                    break;
                }
                //square.GetComponent<SpriteRenderer>().color = Color.yellow;
                Path.Add(new Vector2Int((int)square.transform.position.x, (int)square.transform.position.y));
                square = square.parent;
            }
        }
    }
    public GameObject findLowestFcostObject()
    {
        float lowestF = 1000000;
        GameObject lowestOBJ = null;
        foreach (GameObject node in open)
        {
            node nodeSCR = node.GetComponent<node>();
            calculateCosts(nodeSCR);
            nodeSCR.Set_isOpen(true);
            if (nodeSCR.Get_Fcost() < lowestF)
            {
                lowestF = nodeSCR.Get_Fcost();
                lowestOBJ = node;
            }

        }
        return lowestOBJ;
    }
    public void resetPath()
    {
        
        open = new List<GameObject>();
        closed = new List<GameObject>();
        Path = new List<Vector2Int>();
        targetSquare = PathGrid[(int)Mathf.RoundToInt(transform.position.x), (int)Mathf.RoundToInt(transform.position.y)];

        EndSquare = PathGrid[(int)End.position.x, (int)End.position.y];
        StartSquare = PathGrid[(int)Mathf.RoundToInt(transform.position.x), (int)Mathf.RoundToInt(transform.position.y)];
        open.Add(targetSquare);

        for (int i = 0; i < PathGrid.GetLength(0); i++)
        {
            for (int j = 0; j < PathGrid.GetLength(1); j++)
            {
                if (PathGrid[i, j] != null)
                {
                    if (!PathGrid[i, j].GetComponent<node>().Get_isWall())
                    {
                        PathGrid[i, j].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                        PathGrid[i, j].GetComponent<node>().Set_isOpen(false);
                        PathGrid[i, j].GetComponent<node>().Set_isClosed(false);
                        PathGrid[i, j].GetComponent<node>().Set_Fcost(0);
                        PathGrid[i, j].GetComponent<node>().Set_Gcost(0);
                        PathGrid[i, j].GetComponent<node>().Set_Hcost(0);
                    }

                    PathGrid[i, j].transform.parent = thisNodeStorage.transform;
                }
            }
        }
    }

    public void calculateCosts(node thisNode)
    {
        if (EndSquare != null)
        {
            thisNode.Set_Hcost(calcCost(EndSquare, thisNode));
            thisNode.Set_Gcost(calcCost(StartSquare, thisNode));
            thisNode.Set_Fcost(thisNode.Get_Gcost() + thisNode.Get_Hcost());
        }
    }

    public float calcCost(GameObject endSquare, node thisNode)
    {
        Transform thisSquare = thisNode.transform;
        float cost = 0;
        int count = 0;
        while (thisSquare != endSquare.transform)
        {
            
            count++;
            if (count > 100)
            {
                break;
            }
            Vector2 distance = (endSquare.transform.position - thisSquare.position).normalized;
            Vector2Int direction = new Vector2Int(roundUP(distance.x), roundUP(distance.y));
            if (Mathf.Abs(direction.x) == Mathf.Abs(direction.y))
            {
                cost += 10f;
            }
            else
            {
                cost += 10f;
            }
            thisSquare = PathGrid[(int)thisSquare.position.x + direction.x, (int)thisSquare.position.y + direction.y].transform;
        }
        return cost;
    }

    public void canDrawTimer()
    {
        canDraw = true;
         
    }
    void generateNodes()
    {
        thisNodeStorage = Instantiate(nodeStorage, transform.position, transform.rotation, null);
        for (int i = 0; i < mapGen.Get_width(); i++)
        {
            for (int j = 0;  j < mapGen.Get_height(); j++)
            {
                node nd = Instantiate(node, new Vector3(i, j), transform.rotation,thisNodeStorage.transform).GetComponent<node>();
                PathGrid[i, j] = nd.gameObject;
                nd.Set_fromThisEnemy(this);
            }
        }
    }

    void checkIfInRange()
    {
        if (Vector3.Distance(transform.position, player.position) < EnemyAttackRange)
        {
            End = player;
            canDraw = true;
        }
        else
        {
            canDraw = false;
        }
    }

    public void startingVariables()
    {
        canDraw = false;
        player = GameObject.Find("Player").transform;
        End = player;
        startCoords = transform.position;
        mapGen = GameObject.FindWithTag("mapGen").GetComponent<mapgeneration>();
        open = new List<GameObject>();
        closed = new List<GameObject>();
        Path = new List<Vector2Int>();
        PathGrid = new GameObject[mapGen.Get_width(), mapGen.Get_height()];
    }
    public int roundUP(float value)
    {
        int valueInt = 0;
        if(value > 0)
        {
            valueInt = 1;
        }
        if(value < 0)
        {
            valueInt = -1;
        }
        return valueInt;
    }
    public List<Vector2Int> Get_Path()
    {
        return Path;
    }

    public GameObject Get_thisNodeStorage()
    {
        return thisNodeStorage;
    }
    public GameObject[,] Get_PathGrid()
    {
        return PathGrid;
    }

    public void Set_PathGrid(int x, int y, GameObject setToGameObject)
    {
        PathGrid[x, y] = setToGameObject;
    }

    public GameObject Get_EndSquare()
    {
        return EndSquare;
    }

    public void Set_EndSquare(GameObject setToGameObject)
    {
        EndSquare = setToGameObject;
    }
    public GameObject Get_StartSquare()
    {
        return StartSquare;
    }

    public void Set_StartSquare(GameObject setToGameObject)
    {
        StartSquare = setToGameObject;
    }
}
