using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ���� �޴����� ��� ���� �� ���� UI ����
/// - �� ��ư ������ �� �̵�
/// - ����/��ŷ �г� ���� �ݱ�
/// - ��ŷ ������ �ε� ó�� ����
/// </summary>
public class MainMenu : MonoBehaviour
{

    [Header("��� ���� ��ư��")]
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public Button RankingButton;

    [Header("UI �г�")]
    public GameObject settingsPanel;
    public GameObject rankingPanel;

    void Start()
    {

        // ��ư ��ȣ�ۿ� �����ϰ� ����
        if (easyButton != null) easyButton.interactable = true;
        if (normalButton != null) normalButton.interactable = true;
        if (hardButton != null) hardButton.interactable = true;
        if (RankingButton != null) RankingButton.interactable = true;

        // ���� �г��� ���� �� ����
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // �� ��� �� �ε� �Լ�
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

    // ���� â ����
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // ���� â �ݱ�
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // ��ŷ �г� ���
    public void RankingPanel()
    {
        if (rankingPanel != null)
            rankingPanel.SetActive(!rankingPanel.activeSelf);
    }

    // ��ŷ �г� ���� �ݱ�
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

