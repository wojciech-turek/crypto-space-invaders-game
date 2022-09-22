using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField]
    AudioClip enemyShootingClip;

    [SerializeField]
    AudioClip playerShootingClip;

    [SerializeField]
    [Range(0f, 1f)]
    float shootingVolume = 0.1f;

    [Header("Explosion")]
    [SerializeField]
    AudioClip explosionClip;

    [SerializeField]
    [Range(0f, 1f)]
    float explosionVolume = 1f;

    // way 2
    static AudioPlayer instance;

    private void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy (gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
    }

    public void PlayShootingClip(bool isAI)
    {
        if (enemyShootingClip != null && playerShootingClip != null)
        {
            PlayClip(isAI ? enemyShootingClip : playerShootingClip,
            shootingVolume);
        }
    }

    public void PlayExplosionClip()
    {
        if (explosionClip != null)
        {
            PlayClip (explosionClip, explosionVolume);
        }
    }

    void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint (clip, cameraPos, volume);
        }
    }
}
