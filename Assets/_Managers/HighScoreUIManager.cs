using UnityEngine;
using UnityEngine.UI;

public class HighScoreUIManager : MonoBehaviour
{
    // �ְ� ������ ǥ���� Text (��: "Best Score: 1234m")
    public Text bestScoreText;

    // �̹� ���� ������ ǥ���� Text (��: "This Run: 1100m")
    public Text currentRunText;

    /// <summary>
    /// ���� UI�� ������Ʈ�ϴ� �Լ�
    /// </summary>
    /// <param name="bestScore">����� �ְ� ����</param>
    /// <param name="currentScore">�̹� ������ ���� ����</param>
    public void UpdateScoreUI(int bestScore, int currentScore)
    {
        if (bestScoreText != null)
            bestScoreText.text = $"Best Score: {bestScore}m";

        if (currentRunText != null)
            currentRunText.text = $"This Run: {currentScore}m";
    }
}
