using Assets.DistanceMeasure;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMeasureManager : MonoBehaviour
{
    public Text distance;

    int numLines;
    bool isFirstInputInvalid = true;
    bool isSecondInputInvalid = false;
    Vector3 firstTouchPosition;
    Vector3 secondTouchPosition;

    private void Update()
    {

        if (isFirstInputInvalid && Input.GetMouseButtonUp(0))
        {
            firstTouchPosition = GetRaycastHitPosition(Input.mousePosition);
            isFirstInputInvalid = firstTouchPosition == Vector3.zero;
            isSecondInputInvalid = !isFirstInputInvalid;
        }
        else if (isSecondInputInvalid && Input.GetMouseButtonUp(0))
        {
            secondTouchPosition = GetRaycastHitPosition(Input.mousePosition);
            isSecondInputInvalid = secondTouchPosition == Vector3.zero;
        }
        else if (!isFirstInputInvalid && !isSecondInputInvalid)
        {
            OnScreenMeasurement measure = new OnScreenMeasurement(firstTouchPosition, secondTouchPosition, gameObject);
            measure.DisplayMeasureLabel();
            isFirstInputInvalid = true;
        }

        MeasureLabel.Canvases.ForEach(canvas => RotateTowardsCamera(canvas));
        MeasureLabel.TextLabels.ForEach(label => RotateTowardsCamera(label));
    }

    private Vector3 GetRaycastHitPosition(Vector3 screenPosition)
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

    private void RotateTowardsCamera(GameObject objectToRotate)
    {
        objectToRotate.transform.LookAt(Camera.main.transform);
        objectToRotate.transform.Rotate(0, 180, 0);
    }
}
