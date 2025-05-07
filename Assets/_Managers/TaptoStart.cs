using UnityEngine;
using UnityEngine.UI;

public class TaptoStart : MonoBehaviour
{
    // 깜빡임 주기 설정 (한 방향으로 서서히 사라지거나 나타나는 데 걸리는 시간)
    public float blinkDuration = 1f;

    // Text 컴포넌트를 담을 변수
    private Text text;

    // 타이머: 시간을 누적해서 현재 상태 계산에 사용
    private float timer = 0f;

    // 알파값이 줄어드는 중인지 증가하는 중인지 판단하는 변수
    private bool fadingOut = true;

    void Start()
    {
        // 이 스크립트가 붙은 오브젝트의 Text 컴포넌트를 가져온다
        text = GetComponent<Text>();
    }

    void Update()
    {
        // 프레임마다 시간을 누적한다 (deltaTime: 지난 프레임 시간)
        timer += Time.deltaTime;

        // t는 현재 진행률 (0 ~ 1)
        float t = timer / blinkDuration;

        // 알파값을 계산해주는 코드
        if (fadingOut)
        {
            // 서서히 알파값을 1 → 0으로 줄인다
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetAlpha(alpha);

            // 완료되었으면 방향 전환
            if (t >= 1f)
            {
                fadingOut = false;
                timer = 0f;
            }
        }
        else
        {
            // 서서히 알파값을 0 → 1로 증가
            float alpha = Mathf.Lerp(0f, 1f, t);
            SetAlpha(alpha);

            // 완료되었으면 다시 방향 전환
            if (t >= 1f)
            {
                fadingOut = true;
                timer = 0f;
            }
        }
    }

    // Text의 알파값만 변경해주는 함수
    void SetAlpha(float alpha)
    {
        // 기존 색상을 가져온 후 알파만 수정해서 다시 넣기
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}
