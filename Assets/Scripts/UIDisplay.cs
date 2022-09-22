using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    [Header("Score")]
    [SerializeField]
    TextMeshProUGUI scoreText;

    [Header("Health")]
    [SerializeField]
    Slider healthSlider;

    [SerializeField]
    Health playerHealth;

    ScoreKeeper scoreKeeper;

    private void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    private void Start()
    {
        healthSlider.maxValue = playerHealth.GetHealth();
    }

    void Update()
    {
        scoreText.text = scoreKeeper.GetScore().ToString("0000000000");
        healthSlider.value = playerHealth.GetHealth();
    }
}
