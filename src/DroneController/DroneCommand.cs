using Newtonsoft.Json; // Importing the Newtonsoft.Json library to serialize objects to JSON format
using System.Text; // Importing the System.Text namespace to work with string encodings

namespace DroneController
{
    public class DroneCommand // Defining a public class called DroneCommand
    {
        // Defining three constant strings to be used as command names
        public const string START_FLIGHT_PLAN_CMD = "StartFlightPlan";
        public const string STOP_FLIGHT_PLAN_CMD = "StopFlightPlan";
        public const string STATUS_CMD = "Status";

        // Defining two nullable string properties that represent a command and its arguments
        public string? Command { get; set; }
        public string? Arguments { get; set; }

        // Defining a public method called Encode that returns a byte array
        public byte[] Encode()
        {
            // Serializing the current instance of the DroneCommand class to a JSON string using Newtonsoft.Json library
            var msg = JsonConvert.SerializeObject(this);

            // Encoding the JSON string as a UTF-8 byte array using System.Text namespace
            return Encoding.UTF8.GetBytes(msg);
        }
    }
}