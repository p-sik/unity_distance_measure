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
        Vector3 labelPosition = Vector3.Lerp(firstTouchPosition, secondTouchPosition, 0.5f);
        Quaternion labelRotation = Quaternion.FromToRotation(Vector3.right, secondTouchPosition - firstTouchPosition);
        GameObject labelObject = NewLabelCanvas(labelPosition, labelRotation);
        GameObject textObject = NewLabel(labelPosition, labelRotation, labelObject);
        TextMeshProUGUI labelText = NewLabelText(textObject);

        string distanceToDisplay = measurements[measurements.Count - 1].Distance.ToString();
        distanceToDisplay = distanceToDisplay.Substring(0, 5);
        labelText.text = distanceToDisplay;
        
    }

    private static TextMeshProUGUI NewLabelText(GameObject label)
    {
        TextMeshProUGUI labelText = label.AddComponent<TextMeshProUGUI>();
        labelText.fontSize = 1;

        RectTransform textRt = labelText.GetComponent<RectTransform>();
        SetPositionParameters(textRt);
        return labelText;
    }

    private static void SetPositionParameters(RectTransform textRt)
    {
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.pivot = Vector2.one * 0.5f;
        textRt.offsetMax = Vector2.zero;
        textRt.offsetMin = Vector2.zero;
    }

    private static GameObject NewLabel(Vector3 labelPosition, Quaternion labelRotation, GameObject parentCanvas)
    {
        GameObject textObject = new GameObject("TextLabel");
        textObject.transform.parent = parentCanvas.transform;
        textObject.transform.position = labelPosition;
        textObject.transform.rotation = labelRotation;
        return textObject;
    }

    private static GameObject NewLabelCanvas(Vector3 labelPosition, Quaternion labelRotation)
    {
        GameObject canvasLabel = new GameObject("MeasureLabel");
        canvasLabel.transform.position = labelPosition;
        canvasLabel.transform.rotation = labelRotation;
        RectTransform rt = canvasLabel.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3);
        Canvas labelCanvas = canvasLabel.AddComponent<Canvas>();
        labelCanvas.renderMode = RenderMode.WorldSpace;
        return canvasLabel;
    }
}
