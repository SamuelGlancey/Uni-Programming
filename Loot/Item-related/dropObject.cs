using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropObject : MonoBehaviour
{
    public List<GameObject> drops = new List<GameObject>();
    public List<int> dropChances = new List<int>();
    public void OnDisable()
    {
        int i = 0;
        foreach (GameObject obj in drops)
        {
            int rando = Random.Range(0, 100);
            if (rando < dropChances[i])
            {
                Instantiate(obj, transform.position, Quaternion.identity, null);
            }
            i++;
        }
    }
}
