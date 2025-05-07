using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    // "Tap to Start" 텍스트 오브젝트 (초기 진입 시 표시됨)
    public GameObject tapToStartText;

    // "Loading... 0%" 등을 표시할 텍스트 오브젝트
    public Text loadingText;

    // 시작 여부 체크용 플래그
    private bool hasStarted = false;

    // 로딩 시간 누적용 타이머
    private float timer = 0f;

    // 전체 로딩 시간 (씬 이동까지 걸리는 시간)
    private float loadingTime = 2.5f;

    void Start()
    {
        // 시작할 때 로딩 텍스트는 꺼두기
        if (loadingText != null)
            loadingText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 아직 시작하지 않았고, 터치나 클릭이 감지된 경우
        if (!hasStarted && (IsTouchBegan() || Input.GetMouseButtonDown(0)))
        {
            hasStarted = true;

            // "Tap to Start" 텍스트 비활성화
            if (tapToStartText != null)
                tapToStartText.SetActive(false);

            // 로딩 텍스트 활성화
            if (loadingText != null)
                loadingText.gameObject.SetActive(true);
        }

        // 시작 후 타이머 진행 중
        if (hasStarted)
        {
            // 시간 누적
            timer += Time.deltaTime;

            // 0~1 사이 로딩 진행도 계산
            float progress = Mathf.Clamp01(timer / loadingTime);

            // 로딩 텍스트에 퍼센트 표시 (정수로 변환)
            if (loadingText != null)
            {
                int percent = Mathf.RoundToInt(progress * 100f);
                loadingText.text = $"Loading... \n {percent}%";
            }

            // 로딩이 완료되면 씬 전환
            if (timer >= loadingTime)
            {
                LoadGameScene();
            }
        }
    }

    // 모바일 터치 감지 함수 (탭 시 true 반환)
    bool IsTouchBegan()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    // 로딩 완료 시 메인 메뉴 씬으로 이동
    void LoadGameScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
