using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawLines : MonoBehaviour
{
    public LineRenderer lr;
    public bool canDrawLines = true;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.positionCount = transform.childCount;
        for(int i = 0; i < transform.childCount; i++)
        {
            lr.SetPosition(i, transform.GetChild(i).position);
        }
        if (!canDrawLines)
        {
            lr.enabled = false;
        }
        else
        {
            lr.enabled = true;
        }
    }
}
