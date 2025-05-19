using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private ActionManager inputActions;
    private Rigidbody rb;
    private Animator animator;
    private Vector2 inputWalk;

    private bool rightDirection = true;
    public float moveSpeed = 3f;
    private bool walkState = false;

    private Clock walkTime;

    void Awake()
    {
        // inputActions 불러오기
        inputActions = new ActionManager();
    }
    void OnEnable()
    {
        // inputActions 활성화
        inputActions.playerAction.Enable();
    }
    void Start()
    {
        // 프레임 고정
        Application.targetFrameRate = 60;
        // Rigidbody와 Animator 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        walkTime = new Clock();
    }
    void OnDisable()
    {
        // inputActions 비활성화
        inputActions.playerAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // inputWalk에 inputAction으로 받은 입력값 저장
        inputWalk = inputActions.playerAction.walk.ReadValue<Vector2>();
        // magnitude로 벡터값 확인
        float inputMagnitude = inputWalk.magnitude;
        animator.SetFloat("playerWalkSpeed", inputMagnitude);

        if (inputMagnitude != 0)
        {
            if (!walkState)
            {
                walkTime.StartTime();
                walkState = true;
                animator.SetBool("playerWalkState", true);
            }
        }
            
        if (walkState)
        {
            if (inputMagnitude == 0)
            {
                if (walkTime.TimeUp(0.2f))
                {
                    walkState = false;
                    animator.SetBool("playerWalkState", false);
                }
            }
        }

        //좌우 반전
        Vector3 currentScale = transform.localScale;

        if (inputWalk.x > 0 && !rightDirection)
        {
            rightDirection = true;
            currentScale.x *= -1;
            transform.localScale = currentScale;
        }
        else if (inputWalk.x < 0 && rightDirection)
        {
            rightDirection = false;
            currentScale.x *= -1;
            transform.localScale = currentScale;
        }
    }
    void FixedUpdate()
    {

        if (rb != null)
        {

            Vector3 moveDirection = new Vector3(inputWalk.x, 0f, inputWalk.y);
            Vector3 targetVelocity = moveDirection.normalized * moveSpeed;
            targetVelocity.z *= 1.5f;

            // 현재 Rigidbody의 Y축 속도는 그대로 유지하면서 XZ 평면의 속도만 변경
            // (이렇게 하면 나중에 점프나 중력 같은 Y축 움직임을 추가했을 때 서로 간섭하지 않아.)
            // 만약 Y축 움직임이 전혀 없고 바닥에만 붙어 다닌다면 targetVelocity.y = 0f; 로 해도 돼.
            targetVelocity.y = rb.velocity.y;
            rb.velocity = targetVelocity;
        }
    }


}
public class Clock
{
    private float saveTime = 0f;

    public void StartTime()
    {
        saveTime = 0f;
    }

    public bool TimeUp(float aimTime)
    {
        saveTime += Time.deltaTime;

        if (saveTime >= aimTime)
        {
            return true;
        }
        return false;
    }
}

