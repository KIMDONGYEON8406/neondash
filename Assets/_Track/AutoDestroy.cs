using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // 플레이어 트랜스폼 참조
    private Transform player;

    // 플레이어보다 얼마나 멀어지면 제거할지 설정하는 거리
    public float destroyDistance = 10f;

    void Start()
    {
        // 플레이어가 지정되지 않은 경우, 태그를 이용해 자동으로 찾아서 할당
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        // 플레이어가 없으면 동작하지 않음
        if (player == null) return;

        // 현재 오브젝트가 플레이어보다 destroyDistance만큼 뒤에 있으면 삭제
        if (player.position.z - transform.position.z > destroyDistance)
        {
            Destroy(gameObject); // 게임 오브젝트 제거
        }
    }
}
