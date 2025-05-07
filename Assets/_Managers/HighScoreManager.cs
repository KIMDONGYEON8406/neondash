using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    // 최고 점수 가져오기
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0); // 없으면 0
    }

    // 최고 점수 갱신 (더 높을 때만)
    public bool CheckAndSetHighScore(int newScore)
    {
        int currentHighScore = GetHighScore();
        if (newScore > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, newScore);
            PlayerPrefs.Save();
            Debug.Log($"새 최고 점수 기록됨: {newScore}");
            return true; // 최고 점수 갱신됨
        }
        return false; // 갱신 안 됨
    }
}
