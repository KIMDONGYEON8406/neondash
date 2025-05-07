//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

///// <summary>
///// ���� ���¸� ���������� �����ϴ� Ŭ����
///// - ����, �Ÿ� �Ǵ� �ð� ����
///// - UI ������Ʈ
///// - �Ͻ����� �� �� ��ȯ
///// </summary>
//public class GameManager : MonoBehaviour
//{
//    public static GameManager instance; // �̱���

//    [Header("UI ���")]
//    public Text disText;                 // �Ÿ� �Ǵ� �ð� ǥ��
//    public Text lifeText;                // ���� ǥ��
//    public GameObject gameOverUI;        // ���� ���� UI �г�
//    public GameObject gameClearUI;       // ���� Ŭ���� UI �г�
//    public GameObject pauseUI;           // �Ͻ����� UI �г�
//    public Text distanceResultText;      // ���â�� ǥ�õ� �Ÿ� �ؽ�Ʈ

//    [Header("Ŭ���� �Ÿ� ����")]
//    public Transform player;             // �÷��̾� ��ġ ����
//    public float clearDistance = 10000f; // �Ϲ� ��� Ŭ���� ���� �Ÿ�

//    // ���� ����
//    private float distanceTraveled = 0f; // ������� �̵� �Ÿ�
//    private float countdownTime = 10f;   // Time ��� Ÿ�̸�
//    private bool isTimeMode = false;  // Time ��� ����
//    private int life = 3;                // �Ϲ� ���� ���� ��
//    private bool isPaused = false;       // �Ͻ����� ����
//    private bool gameEnded = false;      // ������ �������� ����

//    void Awake()
//    {
//        // �̱��� �ν��Ͻ� ����
//        if (instance == null)
//            instance = this;
//        else
//            Destroy(gameObject);
//    }

//    void Start()
//    {
//        // ���� ���� Endless ������� Ȯ��
//        string sceneName = SceneManager.GetActiveScene().name;
//        isTimeMode = sceneName == "TrackScene_Time";

//        // �ʱ� UI �� ���� ����
//        Time.timeScale = 1f;

//        if (isTimeMode && lifeText != null)
//        {
//            lifeText.gameObject.SetActive(false); // Endless ���� ���� UI ��Ȱ��ȭ
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
//                GameOver(); // Ÿ�̸� ������ ���ӿ���
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
//                GameClear(); // Ŭ���� ���� �޼�
//            }
//        }

//        // ESC ������ �� �Ͻ����� ���
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            TogglePause();
//        }
//    }

//    /// �������� �Ծ��� �� ó�� (�Ϲ� ��� ����)
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

//    /// ���� UI �ؽ�Ʈ ������Ʈ
//    void UpdateUI()
//    {
//        if (isTimeMode) return;

//        if (lifeText != null)
//            lifeText.text = new string('��', life);
//    }

//    /// ���� ���� ó��
//    //void GameOver()
//    //{
//    //    gameEnded = true;
//    //    Time.timeScale = 0f;

//    //    if (gameOverUI != null) gameOverUI.SetActive(true);

//    //    if (isTimeMode && disText != null)
//    //    {
//    //        disText.text = $"���: {Mathf.FloorToInt(distanceTraveled)}m";
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

//        // UI�� �ְ� ���� ������Ʈ
//        var uiManager = FindObjectOfType<YourUIManager>(); // ���� UI ���� ��ũ��Ʈ
//        int bestScore = highScoreManager.GetHighScore();
//        uiManager.UpdateScoreUI(bestScore, score);

//        if (gameOverUI != null) gameOverUI.SetActive(true);
//    }


//    /// �Ϲ� ��� Ŭ���� ó��
//    void GameClear()
//    {
//        gameEnded = true;
//        Time.timeScale = 0f;
//        if (gameClearUI != null)
//            gameClearUI.SetActive(true);

//        // Easy ��� Ŭ���� �� Hard �ر� ó��
//        if (SceneManager.GetActiveScene().name == "TrackScene_Easy")
//        {
//            PlayerPrefs.SetInt("HardUnlocked", 1);
//        }
//    }

//    /// ���� �� �����
//    public void RestartGame()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }

//    /// �Ͻ�����/����
//    public void TogglePause()
//    {
//        isPaused = !isPaused;
//        Time.timeScale = isPaused ? 0f : 1f;
//        if (pauseUI != null)
//            pauseUI.SetActive(isPaused);
//    }

//    /// ���� �޴� ������ �̵�
//    public void LoadMainMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene("MainMenuScene");
//    }

//    /// ���� ����
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


