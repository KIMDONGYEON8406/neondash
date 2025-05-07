using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // �� ��ֹ��� �����̵�� ���� �� �ִ� Ÿ������ ����
    public bool isSlideObstacle = false;

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ����� �÷��̾����� Ȯ��
        if (other.CompareTag("Player"))
        {
            // �����̵� ��ֹ��� ���
            if (isSlideObstacle)
            {
                // �÷��̾��� �����̵� ���� Ȯ��
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();


                // �����̵� ���̸� �����ϰ� ����
                if (playerMovement != null && playerMovement.IsSliding())
                {
                    return;
                }

                // �����̵� �� �ϰ� �ε������� ������ ó��
                GameManager.instance.TakeDamage();
            }
            else
            {
                // �Ϲ� ��ֹ��̹Ƿ� ������ ������ ó��
                GameManager.instance.TakeDamage();
            }

            // �浹 �� ��ֹ� ����
            Destroy(gameObject);
        }
    }
}
