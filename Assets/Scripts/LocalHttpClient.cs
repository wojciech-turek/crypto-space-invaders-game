using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;

public class LocalHttpClient
{
    private readonly ISerializationOption _serializationOption;

    public LocalHttpClient(ISerializationOption serializationOption)
    {
        _serializationOption = serializationOption;
    }

    public async Task<TResultType> Get<TResultType>(string url) {

        try
        {

            var request = UnityWebRequest.Get(url);

            request.SetRequestHeader("Content-Type", _serializationOption.ContentType);

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();



            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed fetching");
            }

            var result = _serializationOption.Deserialize<TResultType>(request.downloadHandler.text);

            return result;
        }

        catch (Exception ex)
        {
            Debug.LogError(ex);
            return default;
        }
    }

}

