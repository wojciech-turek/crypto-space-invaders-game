using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI rankText;

    void Start()
    {
        int rank = PlayerPrefs.GetInt("Rank");
        rankText.text = rank.ToString();
    }
}
