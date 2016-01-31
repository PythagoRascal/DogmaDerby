using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

public class ControllerMessage
{
    public ControllerMessage(JToken data)
    {
        ReadData(data);
    }


    public string ActionName { get; set; }
    public bool IsPressed { get; set; }
    public Vector3 JoystickPosition { get; set; }


    private void ReadData(JToken data)
    {
        var reader = data.CreateReader();
        string prevProperty = null;

        var x = 0.0f;
        var y = 0.0f;

        while (reader.Read())
        {
            if (reader.Depth == 1 && reader.TokenType.ToString() == "PropertyName")
            {
                ActionName = (string)reader.Value;
            }
            else if (reader.TokenType.ToString() == "PropertyName")
            {
                prevProperty = reader.Value.ToString();
            }
            else if (prevProperty != null)
            {
                if (prevProperty == "pressed")
                {
                    IsPressed = (bool)reader.Value;
                }
                else if (prevProperty == "x")
                {
                    x = ReadFloat(reader);
                }
                else if (prevProperty == "y")
                {
                    y = ReadFloat(reader) * -1;
                }

                prevProperty = null;
            }

            JoystickPosition = new Vector3(x, y);
        }
    }

    private float ReadFloat(JsonReader reader)
    {
        if (reader.Value is Int64)
        {
            return Convert.ToSingle((Int64)reader.Value);
        }
        else
        {
            return Convert.ToSingle((double)reader.Value);
        }
    }
}
