using TelemipAdapter.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TelemipAdapter.Models.Values
{
    public class Message
    {
        public Message(Sensor sensor)
        {
            Sensor = sensor;
        }
        public Message(SensorInfo info, IEnumerable<ISensorMeas> meas) :this(new Sensor(info, meas))
        {            
        }
        [JsonProperty(PropertyName = "sensor")]
        public Sensor Sensor { get; set; }
    }
}
