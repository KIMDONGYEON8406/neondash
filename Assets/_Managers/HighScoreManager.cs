using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    // �ְ� ���� ��������
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0); // ������ 0
    }

    // �ְ� ���� ���� (�� ���� ����)
    public bool CheckAndSetHighScore(int newScore)
    {
        int currentHighScore = GetHighScore();
        if (newScore > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, newScore);
            PlayerPrefs.Save();
            Debug.Log($"�� �ְ� ���� ��ϵ�: {newScore}");
            return true; // �ְ� ���� ���ŵ�
        }
        return false; // ���� �� ��
    }
}
