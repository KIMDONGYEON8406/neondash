using UnityEngine;

public class CameaFollow : MonoBehaviour
{
    // 따라갈 대상 (예: 플레이어)
    public Transform target;

    // 타겟과의 거리 및 높이를 설정하는 오프셋
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    // 따라가는 부드러움 정도 (작을수록 더 빠르게 따라감)
    public float followSmoothTime = 0.1f;

    // SmoothDamp 함수에서 내부적으로 사용하는 속도 저장용 변수
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // 타겟이 존재하지 않으면 동작하지 않음
        if (target == null) return;

        // 목표 위치 계산: 타겟 위치 + 오프셋
        Vector3 desiredPosition = target.position + offset;

        // 현재 위치에서 목표 위치로 부드럽게 이동
        // velocity는 내부적으로 SmoothDamp에서 계속 갱신됨
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followSmoothTime);
    }
}
