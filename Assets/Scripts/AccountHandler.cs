using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AuthToken
{
    public string token;
}

public class AccountHandler : MonoBehaviour
{
    // send a web request using UnityWebRequest to the server for auth token
    public IEnumerator GetAuthToken(string account, string signature)
    {
        // create a form to send to the server
        WWWForm form = new WWWForm();
        form.AddField("address", account);

        // form.AddField("signature", signature);
        form.AddField("signature", signature);

        // send the request
        var request =
            UnityEngine
                .Networking
                .UnityWebRequest
                .Post("http://localhost:5002/auth", form);
        yield return request.SendWebRequest();

        // check if there was any error in the response
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            // TODO display error message to user
        }
        else
        {
            // parse response
            string authToken = request.downloadHandler.text;
            string token = JsonUtility.FromJson<AuthToken>(authToken).token;

            // save the auth token for next scene
            PlayerPrefs.SetString("AuthToken", token);
        }
    }
}
