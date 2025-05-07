using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤

    [Header("UI 요소")]
    public Text disText;                 // 거리 또는 시간 표시
    public Text lifeText;                // 생명 표시
    public GameObject gameOverUI;        // 게임 오버 UI 패널
    public GameObject gameClearUI;       // 게임 클리어 UI 패널
    public GameObject pauseUI;           // 일시정지 UI 패널
    public Text distanceResultText;      // 결과창에 표시될 거리 텍스트

    [Header("클리어 거리 설정")]
    public Transform player;             // 플레이어 위치 참조
    public float clearDistance = 10000f; // 일반 모드 클리어 기준 거리

    private float distanceTraveled = 0f; // 현재까지 이동 거리
    private float countdownTime = 60f;   // Time 모드 타이머 (1분으로 수정)
    private bool isTimeMode = false;     // Time 모드 여부
    private int life = 3;                // 일반 모드용 생명 수
    private bool isPaused = false;       // 일시정지 여부
    private bool gameEnded = false;      // 게임이 끝났는지 여부

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
            lifeText.gameObject.SetActive(false); // Time 모드는 생명 UI 숨김
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
                GameOver(); // 타이머 종료 시 게임오버
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
                GameClear(); // 일반 모드 클리어 조건
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
            lifeText.text = new string('♥', life);
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
            // 최고 점수 갱신 시도
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


