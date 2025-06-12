using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ʈ���� �ֱ������� �����ϰ� ������ Ʈ���� �����ϸ�,
/// Ư�� ���ǿ� ���� ��ֹ��� ��ġ�ϴ� ����� ����.
/// </summary>
public class TrackSpawner : MonoBehaviour
{
    [Header("Ʈ�� ����")]
    [SerializeField] private GameObject[] trackPrefab;       // �Ϲ� Ʈ�� ������ �迭
    [SerializeField] private GameObject safeTrackPrefab;     // ���� �� ������ ���� Ʈ��
    [SerializeField] private int safeTrackCount = 2;         // ���� �� ���� Ʈ�� ����
    [SerializeField] private float trackLength = 10f;        // Ʈ�� �ϳ��� ����
    [SerializeField] private int tracksOnScreen = 5;         // ���ÿ� ������ Ʈ�� ��

    [Header("������ ����")]
    [SerializeField] private GameObject obstaclePrefab;        // �Ϲ� ��ֹ� ������
    [SerializeField] private GameObject slideObstaclePrefab;   // �����̵� ��ֹ� ������

    private float spawnZ = 0f;                        // ���� Ʈ�� ���� ��ġ�� Z��
    private List<GameObject> spawnedTracks = new();   // ���� ������ Ʈ�� ����Ʈ

    // ��ֹ� ���� ����
    private float lastObstacleZ = -999f;      // ������ ��ֹ� ���� ��ġ Z��
    private float obstacleSpacing = 18f;      // ��ֹ� �ּ� ����
    private float spawnChance = 0.7f;         // ��ֹ� ���� Ȯ��

    private string currentScene;              // ���� Ȱ��ȭ�� �� �̸�

    void Start()
    {
        // ���� �� �̸� ����
        currentScene = SceneManager.GetActiveScene().name;

        // ���� �� ���� Ʈ�� ���� ����
        for (int i = 0; i < safeTrackCount; i++)
        {
            SpawnSafeTrack();
        }

        // ���� �Ϲ� Ʈ�� ����
        for (int i = 0; i < tracksOnScreen - safeTrackCount; i++)
        {
            SpawnTrack();
        }
    }

    // ���� Ʈ�� ���� (���� ���� ����)
    void SpawnSafeTrack()
    {
        GameObject track = Instantiate(safeTrackPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        spawnedTracks.Add(track);

        // ���� Ʈ���� �Ϲ� Ʈ������ ������ �˳��� ��
        spawnZ += trackLength + 80f;
    }

    // �Ϲ� Ʈ�� ����
    public void SpawnTrack()
    {
        // �������� �ϳ��� Ʈ�� ����
        GameObject selectedPrefab = trackPrefab[Random.Range(0, trackPrefab.Length)];

        GameObject track = Instantiate(selectedPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        spawnedTracks.Add(track);

        // �ش� ��ġ�� ��ֹ� ���� �õ�
        SpawnItem(spawnZ);

        // ���� Ʈ�� ���� ��ġ ����
        spawnZ += trackLength + 80f;

        // ������ Ʈ�� ����
        DeleteOldTrack();
    }

    // ��ֹ� ���� ���� üũ �� ��ġ
    void SpawnItem(float zPos)
    {
        // Endless ����� ��� ���� ���� ����
        if (currentScene == "TrackScene_Endless")
        {
            SpawnRawObstacle(zPos);
            return;
        }

        // ������ ��ֹ����� �Ÿ� üũ
        if (zPos - lastObstacleZ < obstacleSpacing)
            return;

        // Ȯ�� üũ
        if (Random.value > spawnChance)
            return;

        // ��ֹ� ����
        SpawnRawObstacle(zPos);

        // ������ ��ֹ� ��ġ ����
        lastObstacleZ = zPos;
    }

    // ��ֹ� ���� �� ��ġ ���� �� ����
    void SpawnRawObstacle(float zPos)
    {
        int rand = Random.Range(0, 2);           // ��ֹ� ���� (0: �����̵�, 1: �Ϲ�)
        int lane = Random.Range(0, 3);           // 0: ����, 1: �߾�, 2: ������

        float xPos = (lane - 1) * 3f;            // ���� ��ġ (X ��ǥ)
        float yGround = 6f;                      // Y ��ǥ ����
        Vector3 obstaclePos = new Vector3(xPos, yGround, zPos + 10f); // Z�� �ణ �տ�

        // ��ֹ� ������ ���� ����
        if (rand == 0)
            Instantiate(slideObstaclePrefab, obstaclePos, Quaternion.identity);
        else
            Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity);
    }

    // ������ Ʈ�� ���� (�޸� ����ȭ)
    public void DeleteOldTrack()
    {
        if (spawnedTracks.Count > tracksOnScreen)
        {
            Destroy(spawnedTracks[0]);             // �� �� Ʈ�� ����
            spawnedTracks.RemoveAt(0);             // ����Ʈ������ ����
        }
    }
}
