using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Nonce
{
    public int nonce;
}

public class GameId
{
    public string gameId;
}

public class Score
{
    public int score;

    public string creatorAddress;
}

public class GameEnd
{
    public string gameId;

    public int rank;
}

public class WebRequests : MonoBehaviour
{
    public static string baseUrl = "https://crypto-space-shooter.herokuapp.com";

    // public static string baseUrl = "http://localhost:5002";
    static ScoreKeeper scoreKeeper;

    public static async Task<bool> GetNonce()
    {
        var request =
            UnityEngine
                .Networking
                .UnityWebRequest
                .Get(baseUrl +
                "/account/nonce/" +
                PlayerPrefs.GetString("Account"));
        request
            .SetRequestHeader("Authorization",
            "Bearer " + PlayerPrefs.GetString("AuthToken"));
        await request.SendWebRequest();

        // if request was successful, get the nonce from the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler);

            // get the nonce from the response
            var response = request.downloadHandler.text;
            Debug.Log (response);
            int nonce = JsonUtility.FromJson<Nonce>(response).nonce;
            Debug.Log("Nonce: " + nonce);

            // set the nonce in the player prefs
            PlayerPrefs.SetInt("Nonce", nonce);
            return true;
        }
        else
        {
            Debug.Log(request.error);

            // return from the function
            return false;
        }
    }

    public static async Task<string> SignMessage()
    {
        string message =
            "Approve Signature on Crypto Space Invaders with nonce " +
            PlayerPrefs.GetInt("Nonce");
        string signature = await Web3GL.Sign(message);
        return signature;
    }

    public static async Task<string> StartGame()
    {
        await GetNonce();
        string signature = await SignMessage();

        // send web request with json web token in authorization header
        // sign the message with nonce
        // send web request with json web token in authorization header to get game id
        WWWForm form = new WWWForm();
        form.AddField("signature", signature);
        var request =
            UnityEngine
                .Networking
                .UnityWebRequest
                .Post(baseUrl + "/game/start", form);
        request
            .SetRequestHeader("Authorization",
            "Bearer " + PlayerPrefs.GetString("AuthToken"));

        await request.SendWebRequest();

        // if request was successful, get the game id from the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            // get the game id from the response
            var response = request.downloadHandler.text;
            var gameId = JsonUtility.FromJson<GameId>(response).gameId;

            // set the game id in the player prefs
            PlayerPrefs.SetString("GameId", gameId);
            return gameId;
        }
        else
        {
            Debug.Log(request.error);

            // return from the function
            return "";
        }
    }

    public static async Task<int> EndGame()
    {
        await GetNonce();

        scoreKeeper = FindObjectOfType<ScoreKeeper>();

        WWWForm form = new WWWForm();
        form.AddField("score", scoreKeeper.GetScore());
        form.AddField("gameId", PlayerPrefs.GetString("GameId"));
        var request =
            UnityEngine
                .Networking
                .UnityWebRequest
                .Post(baseUrl + "/game/end", form);
        request
            .SetRequestHeader("Authorization",
            "Bearer " + PlayerPrefs.GetString("AuthToken"));
        await request.SendWebRequest();

        // if request was successful, get the game id from the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            // get the game id from the response
            var response = request.downloadHandler.text;
            var rank = JsonUtility.FromJson<GameEnd>(response).rank;

            // set the rank in the player prefs
            PlayerPrefs.SetInt("Rank", rank);
            return rank;
        }
        else
        {
            Debug.Log(request.error);
            return 0;
        }
    }

    public static async Task<List<Score>> GetHighScores()
    {
        var request =
            UnityEngine.Networking.UnityWebRequest.Get(baseUrl + "/score");
        request
            .SetRequestHeader("Authorization",
            "Bearer " + PlayerPrefs.GetString("AuthToken"));
        await request.SendWebRequest();

        // if request was successful, get the game id from the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            // get the game id from the response
            var response = request.downloadHandler.text;
            Debug.Log (response);
            var list = JsonConvert.DeserializeObject<List<Score>>(response);
            return list;
        }
        else
        {
            Debug.Log(request.error);

            // return from the function
            return null;
        }
    }
}
