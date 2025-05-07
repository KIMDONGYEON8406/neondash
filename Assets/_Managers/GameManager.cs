//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

///// <summary>
///// 게임 상태를 전반적으로 관리하는 클래스
///// - 생명, 거리 또는 시간 측정
///// - UI 업데이트
///// - 일시정지 및 씬 전환
///// </summary>
//public class GameManager : MonoBehaviour
//{
//    public static GameManager instance; // 싱글톤

//    [Header("UI 요소")]
//    public Text disText;                 // 거리 또는 시간 표시
//    public Text lifeText;                // 생명 표시
//    public GameObject gameOverUI;        // 게임 오버 UI 패널
//    public GameObject gameClearUI;       // 게임 클리어 UI 패널
//    public GameObject pauseUI;           // 일시정지 UI 패널
//    public Text distanceResultText;      // 결과창에 표시될 거리 텍스트

//    [Header("클리어 거리 설정")]
//    public Transform player;             // 플레이어 위치 참조
//    public float clearDistance = 10000f; // 일반 모드 클리어 기준 거리

//    // 내부 상태
//    private float distanceTraveled = 0f; // 현재까지 이동 거리
//    private float countdownTime = 10f;   // Time 모드 타이머
//    private bool isTimeMode = false;  // Time 모드 여부
//    private int life = 3;                // 일반 모드용 생명 수
//    private bool isPaused = false;       // 일시정지 여부
//    private bool gameEnded = false;      // 게임이 끝났는지 여부

//    void Awake()
//    {
//        // 싱글톤 인스턴스 설정
//        if (instance == null)
//            instance = this;
//        else
//            Destroy(gameObject);
//    }

//    void Start()
//    {
//        // 현재 씬이 Endless 모드인지 확인
//        string sceneName = SceneManager.GetActiveScene().name;
//        isTimeMode = sceneName == "TrackScene_Time";

//        // 초기 UI 및 상태 설정
//        Time.timeScale = 1f;

//        if (isTimeMode && lifeText != null)
//        {
//            lifeText.gameObject.SetActive(false); // Endless 모드는 생명 UI 비활성화
//        }

//        UpdateUI();

//        if (gameOverUI != null) gameOverUI.SetActive(false);
//        if (gameClearUI != null) gameClearUI.SetActive(false);
//        if (pauseUI != null) pauseUI.SetActive(false);
//    }

//    void Update()
//    {
//        if (gameEnded) return;

//        if (isTimeMode)
//        {
//            countdownTime -= Time.deltaTime;
//            if (countdownTime <= 0f)
//            {
//                countdownTime = 0f;
//                GameOver(); // 타이머 끝나면 게임오버
//            }

//            distanceTraveled = player.position.z;
//            if (disText != null)
//                disText.text = $"Time : {countdownTime:F1}s";
//        }
//        else
//        {
//            distanceTraveled = player.position.z;

//            if (disText != null)
//                disText.text = $"Distance: {Mathf.FloorToInt(distanceTraveled)} / {Mathf.FloorToInt(clearDistance)}m";

//            if (distanceTraveled >= clearDistance)
//            {
//                GameClear(); // 클리어 조건 달성
//            }
//        }

//        // ESC 눌렀을 때 일시정지 토글
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            TogglePause();
//        }
//    }

//    /// 데미지를 입었을 때 처리 (일반 모드 전용)
//    public void TakeDamage()
//    {
//        FindObjectOfType<PlayerMovement>().ResetSpeed();

//        if (isTimeMode) return;

//        life--;
//        UpdateUI();

//        if (life <= 0)
//        {
//            GameOver();
//        }
//    }

//    /// 생명 UI 텍스트 업데이트
//    void UpdateUI()
//    {
//        if (isTimeMode) return;

//        if (lifeText != null)
//            lifeText.text = new string('♥', life);
//    }

//    /// 게임 오버 처리
//    //void GameOver()
//    //{
//    //    gameEnded = true;
//    //    Time.timeScale = 0f;

//    //    if (gameOverUI != null) gameOverUI.SetActive(true);

//    //    if (isTimeMode && disText != null)
//    //    {
//    //        disText.text = $"기록: {Mathf.FloorToInt(distanceTraveled)}m";
//    //    }

//    //    if (distanceResultText != null)
//    //    {
//    //        distanceResultText.text = "DISTANCE : " + Mathf.FloorToInt(distanceTraveled) + "m";
//    //    }
//    //}

//    void GameOver()
//    {
//        gameEnded = true;
//        Time.timeScale = 0f;

//        int score = Mathf.FloorToInt(distanceTraveled);
//        var highScoreManager = FindObjectOfType<HighScoreManager>();

//        bool isNewHighScore = highScoreManager.CheckAndSetHighScore(score);

//        // UI에 최고 점수 업데이트
//        var uiManager = FindObjectOfType<YourUIManager>(); // 너의 UI 관리 스크립트
//        int bestScore = highScoreManager.GetHighScore();
//        uiManager.UpdateScoreUI(bestScore, score);

//        if (gameOverUI != null) gameOverUI.SetActive(true);
//    }


//    /// 일반 모드 클리어 처리
//    void GameClear()
//    {
//        gameEnded = true;
//        Time.timeScale = 0f;
//        if (gameClearUI != null)
//            gameClearUI.SetActive(true);

//        // Easy 모드 클리어 시 Hard 해금 처리
//        if (SceneManager.GetActiveScene().name == "TrackScene_Easy")
//        {
//            PlayerPrefs.SetInt("HardUnlocked", 1);
//        }
//    }

//    /// 현재 씬 재시작
//    public void RestartGame()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }

//    /// 일시정지/해제
//    public void TogglePause()
//    {
//        isPaused = !isPaused;
//        Time.timeScale = isPaused ? 0f : 1f;
//        if (pauseUI != null)
//            pauseUI.SetActive(isPaused);
//    }

//    /// 메인 메뉴 씬으로 이동
//    public void LoadMainMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene("MainMenuScene");
//    }

//    /// 게임 종료
//    public void QuitGame()
//    {
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
//    }
//}
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


