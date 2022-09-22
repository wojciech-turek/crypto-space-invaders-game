using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    bool isPlayer;

    [SerializeField]
    int score = 50;

    [SerializeField]
    int health = 50;

    [SerializeField]
    ParticleSystem hitEffect;

    CameraShake cameraShake;

    [SerializeField]
    bool applyCameraShake;

    AudioPlayer audioPlayer;

    ScoreKeeper scoreKeeper;

    LevelManager levelManager;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    public int GetHealth()
    {
        return health;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            // Take some damage, tell damage dealer that it hit something and it should destroy itself
            damageDealer.Hit();
            TakeDamage(damageDealer.GetDamage());
            PlayHitEffect();
            ShakeCamera();
        }
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance =
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject,
            instance.main.duration + instance.main.startLifetime.constant);
            audioPlayer.PlayExplosionClip();
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (!isPlayer)
        {
            scoreKeeper.UpdateScore (score);
        }
        else
        {
            levelManager.LoadGameOver();
        }
        Destroy (gameObject);
    }

    void ShakeCamera()
    {
        if (cameraShake != null & applyCameraShake)
        {
            cameraShake.Play();
        }
    }
}
