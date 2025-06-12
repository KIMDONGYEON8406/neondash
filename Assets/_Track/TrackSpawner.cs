using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 트랙을 주기적으로 생성하고 오래된 트랙은 제거하며,
/// 특정 조건에 따라 장애물도 배치하는 기능을 수행.
/// </summary>
public class TrackSpawner : MonoBehaviour
{
    [Header("트랙 설정")]
    [SerializeField] private GameObject[] trackPrefab;       // 일반 트랙 프리팹 배열
    [SerializeField] private GameObject safeTrackPrefab;     // 시작 시 생성할 안전 트랙
    [SerializeField] private int safeTrackCount = 2;         // 시작 시 안전 트랙 개수
    [SerializeField] private float trackLength = 10f;        // 트랙 하나의 길이
    [SerializeField] private int tracksOnScreen = 5;         // 동시에 유지할 트랙 수

    [Header("아이템 설정")]
    [SerializeField] private GameObject obstaclePrefab;        // 일반 장애물 프리팹
    [SerializeField] private GameObject slideObstaclePrefab;   // 슬라이드 장애물 프리팹

    private float spawnZ = 0f;                        // 다음 트랙 생성 위치의 Z값
    private List<GameObject> spawnedTracks = new();   // 현재 생성된 트랙 리스트

    // 장애물 관련 설정
    private float lastObstacleZ = -999f;      // 마지막 장애물 생성 위치 Z값
    private float obstacleSpacing = 18f;      // 장애물 최소 간격
    private float spawnChance = 0.7f;         // 장애물 생성 확률

    private string currentScene;              // 현재 활성화된 씬 이름

    void Start()
    {
        spawnZ += GetTrackLength(track);
        spawnZ += GetTrackLength(track);

    // Calculate length of newly spawned track
    private float GetTrackLength(GameObject track)
    {
        Renderer[] renderers = track.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return trackLength;
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds.size.z;
    }
        // 시작 시 안전 트랙 먼저 생성
        for (int i = 0; i < safeTrackCount; i++)
        {
            SpawnSafeTrack();
        }

        // 이후 일반 트랙 생성
        for (int i = 0; i < tracksOnScreen - safeTrackCount; i++)
        {
            SpawnTrack();
        }
    }

    // 안전 트랙 생성 (시작 지점 전용)
    void SpawnSafeTrack()
    {
        GameObject track = Instantiate(safeTrackPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        spawnedTracks.Add(track);

        // 안전 트랙은 일반 트랙보다 간격을 넉넉히 줌
        spawnZ += trackLength + 80f;
    }

    // 일반 트랙 생성
    public void SpawnTrack()
    {
        // 무작위로 하나의 트랙 선택
        GameObject selectedPrefab = trackPrefab[Random.Range(0, trackPrefab.Length)];

        GameObject track = Instantiate(selectedPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        spawnedTracks.Add(track);

        // 해당 위치에 장애물 생성 시도
        SpawnItem(spawnZ);

        // 다음 트랙 생성 위치 갱신
        spawnZ += trackLength + 80f;

        // 오래된 트랙 제거
        DeleteOldTrack();
    }

    // 장애물 생성 조건 체크 및 배치
    void SpawnItem(float zPos)
    {
        // Endless 모드일 경우 제한 없이 생성
        if (currentScene == "TrackScene_Endless")
        {
            SpawnRawObstacle(zPos);
            return;
        }

        // 마지막 장애물과의 거리 체크
        if (zPos - lastObstacleZ < obstacleSpacing)
            return;

        // 확률 체크
        if (Random.value > spawnChance)
            return;

        // 장애물 생성
        SpawnRawObstacle(zPos);

        // 마지막 장애물 위치 갱신
        lastObstacleZ = zPos;
    }

    // 장애물 종류 및 위치 지정 후 생성
    void SpawnRawObstacle(float zPos)
    {
        int rand = Random.Range(0, 2);           // 장애물 종류 (0: 슬라이드, 1: 일반)
        int lane = Random.Range(0, 3);           // 0: 왼쪽, 1: 중앙, 2: 오른쪽

        float xPos = (lane - 1) * 3f;            // 레인 위치 (X 좌표)
        float yGround = 6f;                      // Y 좌표 고정
        Vector3 obstaclePos = new Vector3(xPos, yGround, zPos + 10f); // Z는 약간 앞에

        // 장애물 종류에 따라 생성
        if (rand == 0)
            Instantiate(slideObstaclePrefab, obstaclePos, Quaternion.identity);
        else
            Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity);
    }

    // 오래된 트랙 제거 (메모리 최적화)
    public void DeleteOldTrack()
    {
        if (spawnedTracks.Count > tracksOnScreen)
        {
            Destroy(spawnedTracks[0]);             // 맨 앞 트랙 제거
            spawnedTracks.RemoveAt(0);             // 리스트에서도 제거
        }
    }
}
