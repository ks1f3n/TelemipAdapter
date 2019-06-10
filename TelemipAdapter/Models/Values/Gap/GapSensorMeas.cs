using TelemipAdapter.Interfaces;
using Newtonsoft.Json;
using System;

namespace TelemipAdapter.Models.Values
{
    public class GapSensorMeas : ISensorMeas
    {
        public GapSensorMeas(int d, long t, int d0 = 0)
        {
            D = GetValue(d, d0);
            T = t;
        }
        [JsonProperty(PropertyName = "d")]
        public float D { get; set; }
        [JsonProperty(PropertyName = "t")]
        public long T { get; set; }

        public static float GetValue(int d, int d0)
        {
            return (float)Math.Round((float)(d - d0) / 1000, 3);
        }
    }
}
