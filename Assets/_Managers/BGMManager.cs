using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ���� ���� ��������� �����ϰ�,
/// ���Ұ� �� ���� ���� ����� �����ϴ� BGM ���� �Ŵ��� Ŭ����
/// </summary>
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; // �̱��� �ν��Ͻ�

    public AudioSource audioSource; // ���� ������ ����� AudioSource

    // �� ���� �����Ǵ� ������� Ŭ��
    public AudioClip mainMenuClip;
    public AudioClip easyClip;
    public AudioClip normalClip;
    public AudioClip hardClip;
    public AudioClip timeClip;

    // ���Ұ� ���¿� ���� �ؽ�Ʈ�� ������ UI �ؽ�Ʈ
    public Text muteText;

    /// �̱��� ���� ���� �� ������Ʈ ����
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ�� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �Ŵ����� ����
            return;
        }
    }

    /// ó���� ������ �����Ǿ� �ִٸ� �ڵ� ���
    private void Start()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    /// ���� �ε�� ������ �̺�Ʈ ����
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// �̺�Ʈ ���� ����
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// ���� �ٲ�� �ش� ���� �´� �������� ��ü
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

    /// ���� ������ ���ο� Ŭ������ �����Ͽ� ���
    public void ChangeMusic(AudioClip newClip)
    {
        if (newClip == null || audioSource == null)
            return;

        if (audioSource.clip == newClip)
            return; // �̹� ���� �����̸� �ٽ� ������� ����

        audioSource.Stop();
        audioSource.clip = newClip;

        // ���Ұ� ���°� �ƴ� ���� ���
        if (!audioSource.mute)
            audioSource.Play();
    }

    /// ������� ���Ұ� ���¸� ��� (�ѱ�/����)
    public void ToggleMute()
    {
        if (audioSource == null) return;

        audioSource.mute = !audioSource.mute;

        // �ؽ�Ʈ�� ����Ǿ� �ִٸ� ���¿� ���� �ؽ�Ʈ�� ����
        if (muteText != null)
        {
            muteText.text = audioSource.mute ? "���Ұ� ����" : "���Ұ�";
        }
    }

    /// ���� ���� �����̴��� ������ �� �ִ� �Լ�
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
