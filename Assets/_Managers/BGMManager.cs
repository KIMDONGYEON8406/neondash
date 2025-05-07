using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 씬에 따라 배경음악을 변경하고,
/// 음소거 및 볼륨 조절 기능을 제공하는 BGM 전용 매니저 클래스
/// </summary>
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; // 싱글톤 인스턴스

    public AudioSource audioSource; // 실제 음악을 재생할 AudioSource

    // 각 씬에 대응되는 배경음악 클립
    public AudioClip mainMenuClip;
    public AudioClip easyClip;
    public AudioClip normalClip;
    public AudioClip hardClip;
    public AudioClip timeClip;

    // 음소거 상태에 따라 텍스트를 변경할 UI 텍스트
    public Text muteText;

    /// 싱글톤 패턴 적용 및 오브젝트 유지
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트가 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 매니저는 제거
            return;
        }
    }

    /// 처음에 음악이 지정되어 있다면 자동 재생
    private void Start()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    /// 씬이 로드될 때마다 이벤트 연결
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// 이벤트 연결 해제
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// 씬이 바뀌면 해당 씬에 맞는 음악으로 교체
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenuScene":
                ChangeMusic(mainMenuClip);
                break;
            case "TrackScene_Easy":
                ChangeMusic(easyClip);
                break;
            case "TrackScene_Normal":
                ChangeMusic(normalClip);
                break;
            case "TrackScene_Hard":
                ChangeMusic(hardClip);
                break;
            case "TrackScene_Time":
                ChangeMusic(timeClip);
                break;
        }
    }

    /// 현재 음악을 새로운 클립으로 변경하여 재생
    public void ChangeMusic(AudioClip newClip)
    {
        if (newClip == null || audioSource == null)
            return;

        if (audioSource.clip == newClip)
            return; // 이미 같은 음악이면 다시 재생하지 않음

        audioSource.Stop();
        audioSource.clip = newClip;

        // 음소거 상태가 아닐 때만 재생
        if (!audioSource.mute)
            audioSource.Play();
    }

    /// 배경음악 음소거 상태를 토글 (켜기/끄기)
    public void ToggleMute()
    {
        if (audioSource == null) return;

        audioSource.mute = !audioSource.mute;

        // 텍스트가 연결되어 있다면 상태에 따라 텍스트를 변경
        if (muteText != null)
        {
            muteText.text = audioSource.mute ? "음소거 해제" : "음소거";
        }
    }

    /// 볼륨 조절 슬라이더에 연결할 수 있는 함수
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
