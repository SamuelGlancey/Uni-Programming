using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
        if(GameObject.FindGameObjectsWithTag("pickup").Length > 2)
        {
            Destroy(this.gameObject);
        }
        StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<movement>().StartCoroutine(collision.GetComponent<movement>().rapidFire(0.4f));
            Destroy(this.gameObject);
        }
    }
    public IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
