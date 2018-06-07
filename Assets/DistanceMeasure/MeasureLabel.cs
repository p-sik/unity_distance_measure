using System;
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

        public MeasureLabel(Vector3 beginPosition, Vector3 endPosition)
        {
            firstPosition = beginPosition;
            secondPosition = endPosition;
        }

        public void CreateMeasurementLabel()
        {
            Vector3 labelPosition = Vector3.Lerp(firstPosition, secondPosition, 0.5f);
            Quaternion labelRotation = Quaternion.FromToRotation(Vector3.right, secondPosition - firstPosition);
            GameObject labelObject = NewLabelCanvas(labelPosition, labelRotation);
            GameObject textObject = NewLabel(labelPosition, labelRotation, labelObject);
            TextMeshProUGUI labelText = NewLabelText(textObject);

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

        private GameObject NewLabel(Vector3 labelPosition, Quaternion labelRotation, GameObject parentCanvas)
        {
            GameObject textObject = new GameObject("TextLabel");
            textObject.transform.parent = parentCanvas.transform;
            textObject.transform.position = labelPosition;
            textObject.transform.rotation = labelRotation;
            return textObject;
        }

        private GameObject NewLabelCanvas(Vector3 labelPosition, Quaternion labelRotation)
        {
            GameObject canvasLabel = new GameObject($"MeasureLabel_{++numLines}");
            canvasLabel.transform.position = labelPosition;
            canvasLabel.transform.rotation = labelRotation;
            Canvas labelCanvas = canvasLabel.AddComponent<Canvas>();
            labelCanvas.renderMode = RenderMode.WorldSpace;
            RectTransform rt = canvasLabel.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3);
            return canvasLabel;
        }
    }
}
