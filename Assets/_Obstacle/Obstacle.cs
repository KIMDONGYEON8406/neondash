using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // 이 장애물이 슬라이드로 피할 수 있는 타입인지 여부
    public bool isSlideObstacle = false;

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 대상이 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            // 슬라이드 장애물인 경우
            if (isSlideObstacle)
            {
                // 플레이어의 슬라이드 상태 확인
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();


                // 슬라이드 중이면 무시하고 종료
                if (playerMovement != null && playerMovement.IsSliding())
                {
                    return;
                }

                // 슬라이드 안 하고 부딪혔으면 데미지 처리
                GameManager.instance.TakeDamage();
            }
            else
            {
                // 일반 장애물이므로 무조건 데미지 처리
                GameManager.instance.TakeDamage();
            }

            // 충돌 후 장애물 제거
            Destroy(gameObject);
        }
    }
}
