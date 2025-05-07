using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱���

    [Header("UI ���")]
    public Text disText;                 // �Ÿ� �Ǵ� �ð� ǥ��
    public Text lifeText;                // ���� ǥ��
    public GameObject gameOverUI;        // ���� ���� UI �г�
    public GameObject gameClearUI;       // ���� Ŭ���� UI �г�
    public GameObject pauseUI;           // �Ͻ����� UI �г�
    public Text distanceResultText;      // ���â�� ǥ�õ� �Ÿ� �ؽ�Ʈ

    [Header("Ŭ���� �Ÿ� ����")]
    public Transform player;             // �÷��̾� ��ġ ����
    public float clearDistance = 10000f; // �Ϲ� ��� Ŭ���� ���� �Ÿ�

    private float distanceTraveled = 0f; // ������� �̵� �Ÿ�
    private float countdownTime = 60f;   // Time ��� Ÿ�̸� (1������ ����)
    private bool isTimeMode = false;     // Time ��� ����
    private int life = 3;                // �Ϲ� ���� ���� ��
    private bool isPaused = false;       // �Ͻ����� ����
    private bool gameEnded = false;      // ������ �������� ����

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        isTimeMode = sceneName == "TrackScene_Time";

        Time.timeScale = 1f;

        if (isTimeMode && lifeText != null)
        {
            lifeText.gameObject.SetActive(false); // Time ���� ���� UI ����
        }

        UpdateUI();

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameClearUI != null) gameClearUI.SetActive(false);
        if (pauseUI != null) pauseUI.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        if (isTimeMode)
        {
            countdownTime -= Time.deltaTime;
            if (countdownTime <= 0f)
            {
                countdownTime = 0f;
                GameOver(); // Ÿ�̸� ���� �� ���ӿ���
            }

            distanceTraveled = player.position.z;
            if (disText != null)
                disText.text = $"Time : {countdownTime:F1}s";
        }
        else
        {
            distanceTraveled = player.position.z;

            if (disText != null)
                disText.text = $"Distance: {Mathf.FloorToInt(distanceTraveled)} / {Mathf.FloorToInt(clearDistance)}m";

            if (distanceTraveled >= clearDistance)
            {
                GameClear(); // �Ϲ� ��� Ŭ���� ����
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TakeDamage()
    {
        FindObjectOfType<PlayerMovement>().ResetSpeed();

        if (isTimeMode) return;

        life--;
        UpdateUI();

        if (life <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (isTimeMode) return;

        if (lifeText != null)
            lifeText.text = new string('��', life);
    }

    void GameOver()
    {
        gameEnded = true;
        Time.timeScale = 0f;

        int score = Mathf.FloorToInt(distanceTraveled);
        var highScoreManager = FindObjectOfType<HighScoreManager>();
        var uiManager = FindObjectOfType<HighScoreUIManager>();

        if (highScoreManager != null)
        {
            // �ְ� ���� ���� �õ�
            bool isNewHighScore = highScoreManager.CheckAndSetHighScore(score);

            if (uiManager != null)
            {
                int bestScore = highScoreManager.GetHighScore();
                uiManager.UpdateScoreUI(bestScore, score);
            }
        }

        if (gameOverUI != null) gameOverUI.SetActive(true);
    }

    void GameClear()
    {
        gameEnded = true;
        Time.timeScale = 0f;
        if (gameClearUI != null)
            gameClearUI.SetActive(true);

        if (SceneManager.GetActiveScene().name == "TrackScene_Easy")
        {
            PlayerPrefs.SetInt("HardUnlocked", 1);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (pauseUI != null)
            pauseUI.SetActive(isPaused);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}


