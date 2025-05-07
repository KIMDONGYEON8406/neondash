using UnityEngine;
using UnityEngine.UI;

public class HighScoreUIManager : MonoBehaviour
{
    // 최고 점수를 표시할 Text (예: "Best Score: 1234m")
    public Text bestScoreText;

    // 이번 게임 점수를 표시할 Text (예: "This Run: 1100m")
    public Text currentRunText;

    /// <summary>
    /// 점수 UI를 업데이트하는 함수
    /// </summary>
    /// <param name="bestScore">저장된 최고 점수</param>
    /// <param name="currentScore">이번 게임의 최종 점수</param>
    public void UpdateScoreUI(int bestScore, int currentScore)
    {
        if (bestScoreText != null)
            bestScoreText.text = $"Best Score: {bestScore}m";

        if (currentRunText != null)
            currentRunText.text = $"This Run: {currentScore}m";
    }
}
