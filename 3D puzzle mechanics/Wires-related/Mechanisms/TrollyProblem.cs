using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollyProblem : MonoBehaviour
{
    [SerializeField] int trolleyPoint;
    [SerializeField] int direction = 1;
    [SerializeField] float moveSpeed;
    [SerializeField] bool isActive;
    [SerializeField] Transform trolleyPoints;
    [SerializeField] wireReciever wr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isActive = wr.hasRecievedPositiveSignal;
        trolleyPoints.GetComponent<drawLines>().canDrawLines = isActive;
        if (isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, trolleyPoints.GetChild(trolleyPoint).position, moveSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, trolleyPoints.GetChild(trolleyPoint).position) < 0.01)
            {
                trolleyPoint += direction;
                if (trolleyPoint < 0 || trolleyPoint == trolleyPoints.childCount)
                {
                    direction = -direction;
                    trolleyPoint += direction;
                }
            }
            
        }
    }
}
