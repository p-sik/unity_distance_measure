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

    //TODO scriptObject za seznam meritev, da lahko dostopam iz več koncev
    List<GameObject> lineRendererContainers = new List<GameObject>();
    List<Measurement> measurements = new List<Measurement>();

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
            CreateLineRenderer();
            CreateMeasurementLabel();
            isFirstInputInvalid = true;
            isSecondInputInvalid = false;
        }
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

    private void CreateLineRenderer()
    {
        GameObject container = CreateContainer();
        SetLinerenderer(container);
        float measuredDistance = Vector3.Distance(firstTouchPosition, secondTouchPosition);
        distance.text = measuredDistance.ToString();
        Measurement createdMeasure = new Measurement(measuredDistance, container.name);
        measurements.Add(createdMeasure);
    }

    private GameObject CreateContainer()
    {
        GameObject container = new GameObject();
        container.transform.parent = gameObject.transform;
        container.name = $"Measurement_{++numLines}";
        lineRendererContainers.Add(container);
        return container;
    }

    private void SetLinerenderer(GameObject containerObject)
    {
        LineRenderer lr = containerObject.AddComponent<LineRenderer>();
        lr.enabled = true;
        lr.positionCount = 2;
        lr.startWidth = 0.2f;
        lr.endWidth = lr.startWidth;
        Vector3[] positions = { firstTouchPosition, secondTouchPosition };
        lr.SetPositions(positions);
    }

    private void CreateMeasurementLabel()
    {
        GameObject labelObject = new GameObject("MeasureLabel");
        Vector3 labelPosition = Vector3.Lerp(firstTouchPosition, secondTouchPosition, 0.5f);
        labelObject.transform.position = labelPosition;
        Canvas labelCanvas = labelObject.AddComponent<Canvas>();
        labelCanvas.renderMode = RenderMode.WorldSpace;        

        GameObject textObject = new GameObject("TextLabel");
        textObject.transform.parent = labelObject.transform;
        textObject.transform.position = labelPosition;
        RectTransform rt = labelObject.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3);
        TextMeshProUGUI labelText = textObject.AddComponent<TextMeshProUGUI>();
        RectTransform textRt = labelText.GetComponent<RectTransform>();
        textRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 6);
        textRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3);
        labelText.text = measurements[measurements.Count - 1].Distance.ToString();
        labelText.fontSize = 1;
    }
}
