using UnityEngine;
using System.Collections;
using System;

public class JsonSerializationOption : ISerializationOption
{
    public string ContentType => "application/json";


    public T Deserialize<T>(string text)
    {
        try
        {
            var result = JsonUtility.FromJson<T>(text);
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse response {text}. {ex.Message}");
            return default;
        }

    }
}

