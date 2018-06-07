using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.DistanceMeasure
{
    public class OnScreenMeasurement
    {
        public Measurement MeasureInfo { get; }
        public static List<Measurement> Measurements = new List<Measurement>();

        static int numLines = 0;
        static List<GameObject> lineRendererContainers = new List<GameObject>();

        Vector3 firstPosition, secondPosition;
        GameObject measurementsContainer;
        MeasureLabel label;

        public OnScreenMeasurement(Vector3 originPosition, Vector3 endPosition, GameObject container)
        {
            firstPosition = originPosition;
            secondPosition = endPosition;
            measurementsContainer = container;

            label = new MeasureLabel(firstPosition, secondPosition);
        }

        public void DisplayMeasureLabel()
        {
            CreateLineRenderer();
            label.CreateMeasurementLabel();
        }

        private void CreateLineRenderer()
        {
            GameObject container = CreateContainer();
            SetLinerenderer(container);
            float measuredDistance = Vector3.Distance(firstPosition, secondPosition);
            Measurement MeasureInfo = new Measurement(measuredDistance, container.name);
            Measurements.Add(MeasureInfo);
        }

        private GameObject CreateContainer()
        {
            GameObject container = new GameObject();
            container.transform.parent = measurementsContainer.transform;
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
            Vector3[] positions = { firstPosition, secondPosition };
            lr.SetPositions(positions);
        }

        
    }
}
