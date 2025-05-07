using UnityEngine;

/// �÷��̾ Ʈ���� ������ �����ϸ� ���� Ʈ���� �����ϴ� Ʈ���� ��ũ��Ʈ
public class EndPointTrigger : MonoBehaviour
{
    // �� ���� Ʈ���ŵǵ��� ������ �÷���
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // �̹� Ʈ���ŵǾ����� �� �̻� �������� ����
        if (triggered) return;

        // �÷��̾ �ƴ� ��� ����
        if (!other.CompareTag("Player")) return;

        // Ʈ���� ����
        triggered = true;

        // TrackSpawner ��ũ��Ʈ ȣ���Ͽ� ���� Ʈ�� ����
        FindObjectOfType<TrackSpawner>().SpawnTrack();
    }
}
