using UnityEngine;

public class CameaFollow : MonoBehaviour
{
    // ���� ��� (��: �÷��̾�)
    public Transform target;

    // Ÿ�ٰ��� �Ÿ� �� ���̸� �����ϴ� ������
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    // ���󰡴� �ε巯�� ���� (�������� �� ������ ����)
    public float followSmoothTime = 0.1f;

    // SmoothDamp �Լ����� ���������� ����ϴ� �ӵ� ����� ����
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Ÿ���� �������� ������ �������� ����
        if (target == null) return;

        // ��ǥ ��ġ ���: Ÿ�� ��ġ + ������
        Vector3 desiredPosition = target.position + offset;

        // ���� ��ġ���� ��ǥ ��ġ�� �ε巴�� �̵�
        // velocity�� ���������� SmoothDamp���� ��� ���ŵ�
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followSmoothTime);
    }
}
