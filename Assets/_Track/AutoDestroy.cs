using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // �÷��̾� Ʈ������ ����
    private Transform player;

    // �÷��̾�� �󸶳� �־����� �������� �����ϴ� �Ÿ�
    public float destroyDistance = 10f;

    void Start()
    {
        // �÷��̾ �������� ���� ���, �±׸� �̿��� �ڵ����� ã�Ƽ� �Ҵ�
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        // �÷��̾ ������ �������� ����
        if (player == null) return;

        // ���� ������Ʈ�� �÷��̾�� destroyDistance��ŭ �ڿ� ������ ����
        if (player.position.z - transform.position.z > destroyDistance)
        {
            Destroy(gameObject); // ���� ������Ʈ ����
        }
    }
}
