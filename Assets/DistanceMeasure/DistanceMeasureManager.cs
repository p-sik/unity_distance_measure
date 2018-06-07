using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMeasureManager : MonoBehaviour
{
    bool awaitingFirstInput = true;
    bool awaitingSecondInput = false;
    Vector3 firstTouchPosition;
    Vector3 secondTouchPosition;

    private void Start()
    {
        gameObject.AddComponent<LineRenderer>();
    }

    private void Update()
    {
        if (awaitingFirstInput && Input.GetMouseButtonUp(0))
        {
            firstTouchPosition = PerformRaycast(Input.mousePosition);
            awaitingFirstInput = firstTouchPosition == Vector3.zero;
            awaitingSecondInput = !awaitingFirstInput;
        }
        else if (awaitingSecondInput && Input.GetMouseButtonUp(0))
        {
            secondTouchPosition = PerformRaycast(Input.mousePosition);
            awaitingSecondInput = secondTouchPosition == Vector3.zero;
        }
        else if (!awaitingFirstInput && !awaitingSecondInput)
        {
            CreateLineRenderer();
            awaitingFirstInput = true;
            awaitingSecondInput = false;
        }
    }

    private Vector3 PerformRaycast(Vector3 screenPosition)
    {
        Ray rayFromDetected = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit rayHit;
        if (Physics.Raycast(rayFromDetected, out rayHit))
        {
            return rayHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void CreateLineRenderer()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.2f;
        lr.endWidth = lr.startWidth;
        Vector3[] positions = { firstTouchPosition, secondTouchPosition };
        lr.SetPositions(positions);
    }
}
