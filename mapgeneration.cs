using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;


//Generates blocks on a grid 
//some of those blocks are walls, barrels, friends, enemies and edges
public class mapgeneration : MonoBehaviour
{
    [SerializeField]private int width;
    [SerializeField] private int height;
    [Range(0,200)]
    [SerializeField]private int circleRadius;
    [SerializeField]private string seed;
    [SerializeField]private bool useRandomSeed;
    [Range(0,100)]
    [SerializeField]private int randomFillPercent;
    [Range(0, 1000)]
    [SerializeField]private int friendSpawnRate;
    [Range(0, 1000)]
    [SerializeField]private int barrelSpawnRate;
    private int[,] map;
    private GameObject[,] grid;
    [SerializeField]private int refine;
    [SerializeField]private GameObject black, friend, barrel, genMap, edgeWall;
    [SerializeField]private Transform gridOBJ, whiteEmpty, blackEmpty;
    System.Random rando;


    void Awake()
    {

        if (useRandomSeed)
        {
            seed = DateTime.UtcNow.ToString();
        }
        rando = new System.Random(seed.GetHashCode());
        GenerateMap();
        draw(transform.position.x, transform.position.y);
        gameManager.numOfBlocks = transform.GetChild(0).GetChild(0).childCount;

    }
    void Start()
    {
        
    }
    void Update()
    {
        gameManager.numOfBlocks = gameManager.numOfBlocks = transform.GetChild(0).GetChild(0).childCount;
    }

    public void GenerateMap()
    {
        map = new int[width, height];
        grid = new GameObject[width, height];
        RandomFillMap();
        for (int i = 0; i < refine; i++)
        {
            SmoothMap();
            
        }
    }
    void RandomFillMap()
    {


        
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                
                if (x == 0|| x == width-1 || y == 0||  y == height - 1)
                {
                    map[x, y] = 1;
                    //Instantiate(black, new Vector3(x,y, 0f), transform.rotation);
                }

                map[x, y] = (rando.Next(0, 100) < randomFillPercent) ? 1 : 0;
                if ((((x - (width/2)) * (x - (width / 2))) + ((y - (height / 2)) * (y - (height / 2) )) < circleRadius))
                {
                    map[x, y] = 1;
                    //Instantiate(white, new Vector3(x, y, 0f), transform.rotation);
                }


            }
        }

    }
    public void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) 
                {
                    map[x, y] = 2; // its the edge of the map

                }
                else if (neighbourWallTiles > 4)
                {
                    map[x, y] = 1;
                    
                }
                else if (neighbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }
                
            }
        }
        
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for(int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += map[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
            }

        }
        return wallCount;
    }

    public bool isAnEdge(int x, int y)
    {
        int Neighbours = 0;
        if(x + 1 > width -1 || x -1 < 0 || y + 1 > height - 1 || y - 1 < 0)
        {
            return false;
        }
        if(map[x + 1, y] == 1)
        {
            Neighbours++;
        }
        if (map[x - 1, y] == 1)
        {
            Neighbours++;
        }
        if (map[x, y + 1] == 1)
        {
            Neighbours++;
        }
        if (map[x, y - 1] == 1)
        {
            Neighbours++;
        }
        if (Neighbours < 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void fillNotWall(int x, int y)
    {
        if (map[x, y] == 0)
        {
            grid[x, y] = null;
            GameObject block2 = (rando.Next(0, 1000) < barrelSpawnRate) ? barrel : null;
            if (block2 != null)
            {
                var square2 = Instantiate(block2, new Vector3(transform.position.x + x, transform.position.y + y, 0f), transform.rotation);
                square2.GetComponent<barrel>().gridPosition = new Vector2Int(x, y);
                grid[x, y] = square2;
            }
        }
    }

    void fillWall(int x, int y)
    {
        if (map[x, y] == 2)
        {
            Instantiate(edgeWall, new Vector3(transform.position.x + x, transform.position.y + y, 0f), transform.rotation, whiteEmpty);
        }
        GameObject block = (rando.Next(0, 1000) < friendSpawnRate) ? friend : black;
        if (map[x, y] == 1)
        {
            var square = Instantiate(block, new Vector3(transform.position.x + x, transform.position.y + y, 0f), transform.rotation, blackEmpty);
            if (square.GetComponent<wall>())
            {
                square.GetComponent<wall>().isAnEdge = isAnEdge(x, y);
                square.GetComponent<wall>().gridPosition = new Vector2Int(x, y);
                square.GetComponent<wall>().partOfThisMap = this;
            }
            if (square.GetComponent<getFriend>())
            {
                square.GetComponent<getFriend>().Set_gridPosition(new Vector2Int(x, y));
                map[x, y] = 0;
                transform.parent = whiteEmpty;
            }

            grid[x, y] = square;
        }
    }

    void draw(float posX, float posY)
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    grid[x, y] = null;
                    fillNotWall(x, y);
                    fillWall(x, y);
                }

            }
        }
    }

    //Getters and Setters
    public int Get_width()
    {
        return width;
    }
    public int Get_height()
    {
        return height;
    }

    public int[,] Get_map()
    {
        return map;
    }
    public void Set_map(int[,] setToArray)
    {
        map = setToArray;
    }
}
