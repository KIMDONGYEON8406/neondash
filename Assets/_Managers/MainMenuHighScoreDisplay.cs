using UnityEngine;
using UnityEngine.UI;

public class MainMenuHighScoreDisplay : MonoBehaviour
{
    public Text bestScoreText;

    void Start()
    {
        var highScoreManager = FindObjectOfType<HighScoreManager>();
        if (highScoreManager != null && bestScoreText != null)
        {
            int bestScore = highScoreManager.GetHighScore();
            bestScoreText.text = $": {bestScore}m";
        }
    }
}
