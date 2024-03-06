using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class pageTurner : MonoBehaviour
{
    public GameObject page1, page2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnPage()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }
}
