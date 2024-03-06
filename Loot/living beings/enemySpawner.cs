using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnTimer;
    public float spawnDistance;
    public bool canSpawn;
    float time;
    bool isPause;
    // Start is called before the first frame update
    void Start()
    {
       canSpawn = true;
    }
    private void OnEnable()
    {
        GameManager.pause += OnPause; 
        GameManager.unPause += OnUnPause; 
    }
    private void OnDisable()
    {
        GameManager.pause -= OnPause;
        GameManager.unPause -= OnUnPause;
    }
    private void Update()
    {
        if (!isPause)
        {
            time += Time.deltaTime;
            
            if (time > spawnTimer)
            {
                spawn();
                time = 0;
            }
        }
    }
    //spawns enemy on a random point on circumference around player.
    void spawn()
    {
        if (canSpawn)
        {
            spawnTimer = Random.Range(15, 40);
            int angle = Random.Range(0, 360);
            float x = transform.position.x + spawnDistance * Mathf.Cos(angle);
            float z = transform.position.z + spawnDistance * Mathf.Sin(angle);
            Instantiate(enemy, new Vector3(x, transform.position.y, z), Quaternion.identity);
        }
    }

    void OnPause()
    {
        isPause = true;
    }
    
    void OnUnPause()
    {
        isPause = false;
    }
}
