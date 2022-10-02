using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public GameObject scorePrefab;

    public GameObject scoreList;

    private void Start()
    {
        GetHighScores();
    }

    public async void GetHighScores()
    {
        var highScores = await WebRequests.GetHighScores();

        for (int i = 0; i < 10; i++)
        {
            // var scoreListItem = Instantiate(scorePrefab, scoreList.transform);
            // instantiate and get child components
            var scoreListItem = Instantiate(scorePrefab, scoreList.transform);
            scoreListItem.GetComponentsInChildren<TextMeshProUGUI>()[0].text =
                (i + 1).ToString();

            if (i + 1 > highScores.Count)
            {
                scoreListItem
                    .GetComponentsInChildren<TextMeshProUGUI>()[1]
                    .text = "";

                scoreListItem
                    .GetComponentsInChildren<TextMeshProUGUI>()[2]
                    .text = "";

                scoreListItem
                    .GetComponentsInChildren<TextMeshProUGUI>()[3]
                    .text = "";
            }
            else
            {
                scoreListItem
                    .GetComponentsInChildren<TextMeshProUGUI>()[1]
                    .text =
                    highScores[i].creatorAddress.Substring(0, 4) +
                    "..." +
                    highScores[i]
                        .creatorAddress
                        .Substring(highScores[i].creatorAddress.Length - 4);

                scoreListItem
                    .GetComponentsInChildren<TextMeshProUGUI>()[3]
                    .text = highScores[i].score.ToString();
            }
        }
    }
}
