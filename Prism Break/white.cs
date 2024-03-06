using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class white : MonoBehaviour
{
    [SerializeField]private ParticleSystem ps;
    [SerializeField]private GameObject pickup;
    [SerializeField]private int pickupChance;
    [SerializeField]private mapgeneration mapGen;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
        mapGen = GameObject.FindWithTag("mapGen").GetComponent<mapgeneration>();
        //var rando = Random.Range(0,5000);


    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(gameManager.BackgroundColor.r -0.2f, gameManager.BackgroundColor.g - 0.2f, gameManager.BackgroundColor.b - 0.2f);
        ps.startColor = gameManager.BlockColor;
        
    }
}
