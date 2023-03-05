using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthToken
{
    public string token;
}


#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;

    private string account;

    public static string baseUrl = "https://shooter-server-323.herokuapp.com";

    public void OnLogin()
    {
        Web3Connect();
        OnConnected();
    }

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
                .Post(baseUrl + "/auth", form);
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
            if (PlayerPrefs.GetString("AuthToken") != "")
            {
                Debug.Log("Login successful " + token);
                SceneManager
                    .LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private async void OnConnected()
    {
        account = ConnectAccount();
        while (account == "")
        {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        }

        // save account for next scene
        PlayerPrefs.SetString("Account", account);
        PlayerPrefs.SetString("AuthToken", "");

        string message =
            "Login/Regsiter to an account on Crypto Space Invaders";
        string signature = await Web3GL.Sign(message);

        StartCoroutine(GetAuthToken(account, signature));
        SetConnectAccount("");
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");

        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif
