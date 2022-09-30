using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    float projectileSpeed = 10f;

    [SerializeField]
    float projectileLifetime = 5f;

    [SerializeField]
    float baseFiringRate = 0.2f;

    [Header("AI")]
    [SerializeField]
    float firingRateVariance = 0f;

    [SerializeField]
    float minimumFiringRate = 0.1f;

    [SerializeField]
    bool useAI;

    [HideInInspector]
    public bool isFiring;

    Coroutine firingCoroutine;

    AudioPlayer audioPlayer;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    IEnumerator Start()
    {
        if (useAI)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine (firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinously()
    {
        // prevent going over firing rate by spamming LMB
        yield return new WaitForSeconds(0.1f);
        while (isFiring)
        {
            GameObject instance =
                Instantiate(projectilePrefab,
                transform.position,
                Quaternion.identity);

            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }
            Destroy (instance, projectileLifetime);

            float timeToNextProjectile =
                Random
                    .Range(baseFiringRate - firingRateVariance,
                    baseFiringRate + firingRateVariance);
            timeToNextProjectile =
                Mathf
                    .Clamp(timeToNextProjectile,
                    minimumFiringRate,
                    float.MaxValue);

            audioPlayer.PlayShootingClip (useAI);
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
