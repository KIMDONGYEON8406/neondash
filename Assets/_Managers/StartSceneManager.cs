using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    // "Tap to Start" �ؽ�Ʈ ������Ʈ (�ʱ� ���� �� ǥ�õ�)
    public GameObject tapToStartText;

    // "Loading... 0%" ���� ǥ���� �ؽ�Ʈ ������Ʈ
    public Text loadingText;

    // ���� ���� üũ�� �÷���
    private bool hasStarted = false;

    // �ε� �ð� ������ Ÿ�̸�
    private float timer = 0f;

    // ��ü �ε� �ð� (�� �̵����� �ɸ��� �ð�)
    private float loadingTime = 2.5f;

    void Start()
    {
        // ������ �� �ε� �ؽ�Ʈ�� ���α�
        if (loadingText != null)
            loadingText.gameObject.SetActive(false);
    }

    void Update()
    {
        // ���� �������� �ʾҰ�, ��ġ�� Ŭ���� ������ ���
        if (!hasStarted && (IsTouchBegan() || Input.GetMouseButtonDown(0)))
        {
            hasStarted = true;

            // "Tap to Start" �ؽ�Ʈ ��Ȱ��ȭ
            if (tapToStartText != null)
                tapToStartText.SetActive(false);

            // �ε� �ؽ�Ʈ Ȱ��ȭ
            if (loadingText != null)
                loadingText.gameObject.SetActive(true);
        }

        // ���� �� Ÿ�̸� ���� ��
        if (hasStarted)
        {
            // �ð� ����
            timer += Time.deltaTime;

            // 0~1 ���� �ε� ���൵ ���
            float progress = Mathf.Clamp01(timer / loadingTime);

            // �ε� �ؽ�Ʈ�� �ۼ�Ʈ ǥ�� (������ ��ȯ)
            if (loadingText != null)
            {
                int percent = Mathf.RoundToInt(progress * 100f);
                loadingText.text = $"Loading... \n {percent}%";
            }

            // �ε��� �Ϸ�Ǹ� �� ��ȯ
            if (timer >= loadingTime)
            {
                LoadGameScene();
            }
        }
    }

    // ����� ��ġ ���� �Լ� (�� �� true ��ȯ)
    bool IsTouchBegan()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    // �ε� �Ϸ� �� ���� �޴� ������ �̵�
    void LoadGameScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
