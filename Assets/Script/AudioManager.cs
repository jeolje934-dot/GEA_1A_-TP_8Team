
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("오디오 재생 장치")]
    public AudioSource audioSource;

    [Header("효과음")]
    public AudioClip duckShootSound;
    public AudioClip enemyHitSound;
    public AudioClip levelUpSound;
    public AudioClip playerHitSound;
    public AudioClip gameOverSound;

    [Header("소리 크기")]
    [Range(0f, 1f)]
    public float shootVolume = 0.8f;

    [Range(0f, 1f)]
    public float otherVolume = 0.7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayDuckShoot()
    {
        PlaySound(duckShootSound, shootVolume);
    }

    public void PlayEnemyHit()
    {
        PlaySound(enemyHitSound, otherVolume);
    }

    public void PlayLevelUp()
    {
        PlaySound(levelUpSound, otherVolume);
    }

    public void PlayPlayerHit()
    {
        PlaySound(playerHitSound, otherVolume);
    }

    public void PlayGameOver()
    {
        PlaySound(gameOverSound, otherVolume);
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip, volume);
    }
}
