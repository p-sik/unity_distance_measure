﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.DistanceMeasure
{
    public class MeasureLabel
    {
        static int numLines = 0;
        Vector3 firstPosition, secondPosition;
        List<Measurement> Measurements = OnScreenMeasurement.Measurements;

        public static List<GameObject> Canvases = new List<GameObject>();
        public static List<GameObject> TextLabels = new List<GameObject>();

        public MeasureLabel(Vector3 beginPosition, Vector3 endPosition)
        {
            firstPosition = beginPosition;
            secondPosition = endPosition;
        }

        public void CreateMeasurementLabel()
        {
            Vector3 labelPosition = Vector3.Lerp(firstPosition, secondPosition, 0.5f);
            GameObject labelObject = NewLabelCanvas(labelPosition);
            GameObject textObject = NewLabel(labelPosition, labelObject);
            TextMeshProUGUI labelText = NewLabelText(textObject);

            Canvases.Add(labelObject);
            TextLabels.Add(textObject);

            string distanceToDisplay = Measurements[Measurements.Count - 1].Distance.ToString();
            distanceToDisplay = distanceToDisplay.Substring(0, 5);
            labelText.text = distanceToDisplay;
        }

        private TextMeshProUGUI NewLabelText(GameObject label)
        {
            TextMeshProUGUI labelText = label.AddComponent<TextMeshProUGUI>();
            labelText.fontSize = 1;

            RectTransform textRt = labelText.GetComponent<RectTransform>();
            SetPositionParameters(textRt);
            return labelText;
        }

        private void SetPositionParameters(RectTransform textRt)
        {
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.pivot = Vector2.one * 0.5f;
            textRt.offsetMax = Vector2.zero;
            textRt.offsetMin = Vector2.zero;
        }

        private GameObject NewLabelCanvas(Vector3 labelPosition)
        {
            GameObject canvasLabel = new GameObject($"MeasureLabel_{++numLines}");
            canvasLabel.transform.position = labelPosition;
            Canvas labelCanvas = canvasLabel.AddComponent<Canvas>();
            labelCanvas.renderMode = RenderMode.WorldSpace;
            RectTransform rt = canvasLabel.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3);
            return canvasLabel;
        }

        private GameObject NewLabel(Vector3 labelPosition, GameObject parentCanvas)
        {
            GameObject textObject = new GameObject("TextLabel");
            textObject.transform.parent = parentCanvas.transform;
            textObject.transform.position = labelPosition;
            return textObject;
        }
    }
}
