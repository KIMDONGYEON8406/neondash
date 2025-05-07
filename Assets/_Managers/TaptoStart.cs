using UnityEngine;
using UnityEngine.UI;

public class TaptoStart : MonoBehaviour
{
    // ������ �ֱ� ���� (�� �������� ������ ������ų� ��Ÿ���� �� �ɸ��� �ð�)
    public float blinkDuration = 1f;

    // Text ������Ʈ�� ���� ����
    private Text text;

    // Ÿ�̸�: �ð��� �����ؼ� ���� ���� ��꿡 ���
    private float timer = 0f;

    // ���İ��� �پ��� ������ �����ϴ� ������ �Ǵ��ϴ� ����
    private bool fadingOut = true;

    void Start()
    {
        // �� ��ũ��Ʈ�� ���� ������Ʈ�� Text ������Ʈ�� �����´�
        text = GetComponent<Text>();
    }

    void Update()
    {
        // �����Ӹ��� �ð��� �����Ѵ� (deltaTime: ���� ������ �ð�)
        timer += Time.deltaTime;

        // t�� ���� ����� (0 ~ 1)
        float t = timer / blinkDuration;

        // ���İ��� ������ִ� �ڵ�
        if (fadingOut)
        {
            // ������ ���İ��� 1 �� 0���� ���δ�
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetAlpha(alpha);

            // �Ϸ�Ǿ����� ���� ��ȯ
            if (t >= 1f)
            {
                fadingOut = false;
                timer = 0f;
            }
        }
        else
        {
            // ������ ���İ��� 0 �� 1�� ����
            float alpha = Mathf.Lerp(0f, 1f, t);
            SetAlpha(alpha);

            // �Ϸ�Ǿ����� �ٽ� ���� ��ȯ
            if (t >= 1f)
            {
                fadingOut = true;
                timer = 0f;
            }
        }
    }

    // Text�� ���İ��� �������ִ� �Լ�
    void SetAlpha(float alpha)
    {
        // ���� ������ ������ �� ���ĸ� �����ؼ� �ٽ� �ֱ�
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}
