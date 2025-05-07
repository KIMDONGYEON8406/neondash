using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 메인 메뉴에서 모드 선택 및 설정 UI 관리
/// - 각 버튼 눌러서 씬 이동
/// - 설정/랭킹 패널 열고 닫기
/// - 랭킹 데이터 로드 처리 포함
/// </summary>
public class MainMenu : MonoBehaviour
{

    [Header("모드 선택 버튼들")]
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public Button RankingButton;

    [Header("UI 패널")]
    public GameObject settingsPanel;
    public GameObject rankingPanel;

    void Start()
    {

        // 버튼 상호작용 가능하게 설정
        if (easyButton != null) easyButton.interactable = true;
        if (normalButton != null) normalButton.interactable = true;
        if (hardButton != null) hardButton.interactable = true;
        if (RankingButton != null) RankingButton.interactable = true;

        // 설정 패널은 시작 시 숨김
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // 각 모드 씬 로딩 함수
    public void StartEasy()
    {
        SceneManager.LoadScene("TrackScene_Easy");
    }

    public void StartNormal()
    {
        SceneManager.LoadScene("TrackScene_Normal");
    }

    public void StartHard()
    {
        SceneManager.LoadScene("TrackScene_Hard");
    }

    public void StartRanking()
    {
        SceneManager.LoadScene("TrackScene_Time");
    }

    // 설정 창 열기
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // 설정 창 닫기
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // 랭킹 패널 토글
    public void RankingPanel()
    {
        if (rankingPanel != null)
            rankingPanel.SetActive(!rankingPanel.activeSelf);
    }

    // 랭킹 패널 강제 닫기
    public void CloseRankingPanel()
    {
        if (rankingPanel != null)
            rankingPanel.SetActive(false);
    }

    public void OpenRankingPanel()
    {
        rankingPanel.SetActive(true);
    }
}

