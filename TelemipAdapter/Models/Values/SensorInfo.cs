using Newtonsoft.Json;
using System;

namespace TelemipAdapter.Models.Values
{
    public class SensorInfo
    {
        public SensorInfo(int per, int volt, int csq)
        {
            Per = per;
            Volt = GetVolt(volt);
            Csq = csq;
        }
        [JsonProperty(PropertyName = "per")]
        public int Per { get; set; }
        [JsonProperty(PropertyName = "volt")]
        public float Volt { get; set; }
        [JsonProperty(PropertyName = "csq")]
        public int Csq { get; set; }
        public static float GetVolt(int volt)
        {
            return (float)Math.Round((volt * 2.56 * 2) / 1024, 5);
        }
    }
}
