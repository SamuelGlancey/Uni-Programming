using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class moveImage : MonoBehaviour
{
    public Canvas canvas;
    public Camera cam;
    public Vector2 origin;
    public bool hovering;
    public GameObject mapParent;
    float initialDistance = 0;
    float initialScale;
    float currentScale;
    public Transform pinchOrigin;
    // Start is called before the first frame update
    private void Start()
    {
        currentScale = pinchOrigin.localScale.x;
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector2.zero;
    }
    public void hover()
    {
        hovering = true;
    }

    public void unhover()
    {
        hovering = false;
    }
    public void Update()
    {
        if (hovering)
        {
            if (Input.touches.Length == 1) {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    origin = Input.touches[0].position;
                }
                if (Input.touches.Length == 1)
                {
                    Vector3 difference = Input.touches[0].position - origin;
                    GetComponent<RectTransform>().position += difference;
                    origin = Input.touches[0].position;
                }
            }
            PinchCheck();
            pinchOrigin.localScale = new Vector3(currentScale, currentScale, currentScale);
        }

    }
    //PINCH TO ZOOM

    private void PinchCheck()
    {
        if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            // if one of the touches Ended or Canceled do nothing
            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled
               || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return;
            }

            // It is enough to check whether one of them began since we
            // already excluded the Ended and Canceled phase in the line before
            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                // track the initial values
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                pinchOrigin.position = (touchOne.position + touchZero.position) / 2;
                transform.parent = pinchOrigin;
                initialScale = currentScale;
            }
            // else now is any other case where touchZero and/or touchOne are in one of the states
            // of Stationary or Moved
            else
            {
                // otherwise get the current distance
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                // A little emergency brake ;)
                if (Mathf.Approximately(initialDistance, 0)) return;

                // get the scale factor of the current distance relative to the inital one
                var factor = currentDistance / initialDistance;

                // apply the scale
                // instead of a continuous addition rather always base the 
                // calculation on the initial and current value only
                currentScale = initialScale * factor;
            }
        }
        else
        {
            transform.parent = mapParent.transform;
        }
    }
}
