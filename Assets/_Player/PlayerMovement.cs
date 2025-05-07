using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // ===== 이동 설정값 =====
    [Header("이동 관련")]
    public float forwardSpeed = 15f;        // 현재 전진 속도
    public float laneDistance = 5f;         // 레인 간 거리 (좌우 이동 간격)
    public float laneChangeSpeed = 10f;     // 레인 변경 시 부드러운 이동 속도

    // ===== 속도 증가 관련 =====
    [Header("속도 증가 관련")]
    public float minSpeed = 15f;            // 최소 속도
    public float maxSpeed = 65f;            // 최대 속도
    public float speedIncreaseRate = 2f;    // 속도 증가량
    public float speedIncreaseInterval = 1f; // 속도 증가 주기 (초)

    // ===== 점프 / 슬라이드 설정 =====
    [Header("점프 / 슬라이드")]
    public float jumpForce = 25f;           // 점프 힘
    public float slideDuration = 1f;        // 슬라이드 지속 시간

    // ===== 내부 동작 변수 =====
    private int currentLane = 1;            // 현재 위치한 레인 (0: 좌, 1: 중, 2: 우)
    private Vector3 targetPosition;         // 목표 위치 (레인 변경 시 목적지)

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private Animator animator;

    private bool isGrounded = true;         // 땅에 있는지 여부
    private bool isSliding = false;         // 슬라이드 중인지 여부
    private bool isJumping = false;         // 점프 중인지 여부

    // ===== 모바일 입력 (스와이프) 관련 =====
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float swipeThreshold = 50f;     // 스와이프 인식 임계값

    void Start()
    {
        // 필수 컴포넌트 할당
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();

        // 시작 시 속도 초기화
        forwardSpeed = minSpeed;

        // 일정 시간마다 속도 증가시키는 코루틴 시작
        StartCoroutine(SpeedUpOverTime());
    }

    void Update()
    {
        // 모바일 입력 처리 (스와이프 방식)
        HandleTouchInput();

        // 앞으로 전진
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // ===== 키보드 입력 처리 (PC 전용) =====

        // 좌우 이동 키 입력 (좌: ←, 우: →)
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
            currentLane--;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
            currentLane++;

        // 목표 위치 계산 및 부드럽게 이동
        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);

        // 점프 키 입력 (스페이스바)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
        {
            Jump();
        }

        // 슬라이드 키 입력 (S키)
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && !isSliding)
        {
            Slide();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 바닥과 충돌 시 점프 가능 상태로 전환
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    // ===== 속도 증가 코루틴 =====
    IEnumerator SpeedUpOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);

            if (forwardSpeed < maxSpeed)
            {
                forwardSpeed += speedIncreaseRate;
                forwardSpeed = Mathf.Min(forwardSpeed, maxSpeed);
            }
        }
    }

    // 속도 초기화 (장애물 충돌 등에서 호출)
    public void ResetSpeed()
    {
        forwardSpeed = minSpeed;
    }

    // 슬라이드 상태 반환
    public bool IsSliding()
    {
        return isSliding;
    }

    // 점프 실행 (키보드/스와이프 공용)
    public void Jump()
    {
        if (isGrounded && !isJumping && !isSliding)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.ResetTrigger("Slide");
            animator.SetTrigger("Jump");

            isGrounded = false;
            isJumping = true;
        }
    }

    // 슬라이드 실행 (키보드/스와이프 공용)
    public void Slide()
    {
        if (isGrounded && !isSliding)
        {
            StartCoroutine(PerformSlide());
        }
    }

    // 슬라이드 처리 코루틴 (자세 및 콜라이더 수정 포함)
    private IEnumerator PerformSlide()
    {
        isSliding = true;

        animator.ResetTrigger("Jump");
        animator.SetTrigger("Slide");

        // 콜라이더 작게 (앉은 상태)
        capsule.height = 1f;
        capsule.center = new Vector3(0f, 0.5f, 0f);

        yield return new WaitForSeconds(slideDuration);

        // 콜라이더 원상복귀
        capsule.height = 2f;
        capsule.center = new Vector3(0f, 1f, 0f);

        isSliding = false;
    }

    // ===== 모바일 입력 처리 (스와이프) =====
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                if (swipeDirection.magnitude > swipeThreshold)
                {
                    float x = swipeDirection.x;
                    float y = swipeDirection.y;

                    // 수직 스와이프 판별
                    if (Mathf.Abs(x) < Mathf.Abs(y))
                    {
                        if (y > 0)
                            Jump();    // 위로 스와이프 → 점프
                        else
                            Slide();   // 아래로 스와이프 → 슬라이드
                    }
                }
            }
        }
    }

    // ===== UI 버튼 입력 처리 (모바일 전용) =====

    // 왼쪽 이동 버튼 클릭
    public void OnMoveLeftButton()
    {
        if (currentLane > 0)
        {
            currentLane--;
        }
    }

    // 오른쪽 이동 버튼 클릭
    public void OnMoveRightButton()
    {
        Debug.Log("오른쪽 버튼 눌림");
        if (currentLane < 2)
        {
            currentLane++;
        }
    }
}
