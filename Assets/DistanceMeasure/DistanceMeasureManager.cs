using Assets.DistanceMeasure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMeasureManager : MonoBehaviour
{
    public Text distance;

    int numLines = 1;
    bool awaitingFirstInput = true;
    bool awaitingSecondInput = false;
    Vector3 firstTouchPosition;
    Vector3 secondTouchPosition;

    //TODO scriptObject za seznam meritev, da lahko dostopam iz več koncev
    List<GameObject> lineRendererContainers = new List<GameObject>();
    List<Measurement> measurements = new List<Measurement>();

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
        GameObject go = new GameObject();
        go.transform.parent = gameObject.transform;
        go.name = $"Measurement_{numLines++}";
        lineRendererContainers.Add(go);
        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.enabled = true;
        lr.positionCount = 2;
        lr.startWidth = 0.2f;
        lr.endWidth = lr.startWidth;
        Vector3[] positions = { firstTouchPosition, secondTouchPosition };
        lr.SetPositions(positions);
        float distance = Vector3.Distance(firstTouchPosition, secondTouchPosition);
        this.distance.text = distance.ToString();
        Measurement createdMeasure = new Measurement(distance, go.name);
        measurements.Add(createdMeasure);
    }
}
