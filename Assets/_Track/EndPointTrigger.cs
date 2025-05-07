using UnityEngine;

/// 플레이어가 트리거 영역에 진입하면 다음 트랙을 생성하는 트리거 스크립트
public class EndPointTrigger : MonoBehaviour
{
    // 한 번만 트리거되도록 방지용 플래그
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 이미 트리거되었으면 더 이상 동작하지 않음
        if (triggered) return;

        // 플레이어가 아닌 경우 무시
        if (!other.CompareTag("Player")) return;

        // 트리거 실행
        triggered = true;

        // TrackSpawner 스크립트 호출하여 다음 트랙 생성
        FindObjectOfType<TrackSpawner>().SpawnTrack();
    }
}
