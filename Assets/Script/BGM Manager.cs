using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    private AudioSource audioSource;

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            // 씬이 넘어가도 BGM이 끊기지 않게 유지하고 싶다면 주석 해제
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    // BGM을 멈추는 함수
    public void StopBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    public void Die()
    {
        // 이미 죽었거나 중복 호출 방지
        // ... 플레이어 사망 애니메이션 및 로직 ...

        // BGM 정지 함수 호출
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.StopBGM();
        }
    }

}