using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.DistanceMeasure
{
    public struct Measurement
    {
        public Measurement(float distance, string label)
        {
            Distance = distance;
            Label = label;
        }

        public float Distance { get; }
        public string Label { get; set; }
    }
}
