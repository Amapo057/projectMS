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
    

    public float moveSpeed = 5f;

    void Awake()
    {
        inputActions = new ActionManager();
    }
    void OnEnable()
    {
        inputActions.playerAction.Enable();
    }
    void Start()
    {
        Application.targetFrameRate = 60;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }
    void OnDisable()
    {
        inputActions.playerAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        inputWalk = inputActions.playerAction.walk.ReadValue<Vector2>();
        animator.SetFloat("playerMoveSpeed", inputWalk.x);
    }
    void FixedUpdate()
    {
        // 9. Rigidbody를 사용한 물리 기반 이동은 FixedUpdate() 안에서 처리하는 것이
        //    물리 계산 주기와 맞아서 더 안정적이고 일관된 결과를 보여줘.
        if (rb != null)
        {
            // 읽어온 2D 입력(inputWalk.x는 키보드 A/D 또는 스틱 좌우, inputWalk.y는 키보드 W/S 또는 스틱 상하)을
            // 3D 공간에서의 X축(좌우) 및 Z축(앞뒤) 이동 방향으로 변환해.
            Vector3 moveDirection = new Vector3(inputWalk.x, 0f, inputWalk.y); // inputWalk의 Y값을 3D 공간의 Z값으로 사용

            // Rigidbody의 속도(velocity)를 직접 제어해서 움직이기
            // moveDirection.normalized는 이동 방향 벡터의 크기를 1로 만들어줘서, 대각선 이동 시 더 빨라지는 현상을 방지해.
            Vector3 targetVelocity = moveDirection.normalized * moveSpeed;

            // 현재 Rigidbody의 Y축 속도는 그대로 유지하면서 XZ 평면의 속도만 변경
            // (이렇게 하면 나중에 점프나 중력 같은 Y축 움직임을 추가했을 때 서로 간섭하지 않아.)
            // 만약 Y축 움직임이 전혀 없고 바닥에만 붙어 다닌다면 targetVelocity.y = 0f; 로 해도 돼.
            targetVelocity.y = rb.velocity.y;

            rb.velocity = targetVelocity;

            // ----- 주석 처리된 다른 이동 방식 (MovePosition) -----
            // 만약 MovePosition을 사용하고 싶다면 아래 주석을 풀고, 위의 velocity 설정 부분을 주석 처리해.
            // Vector3 targetPosition = rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
            // rb.MovePosition(targetPosition);
            // ----------------------------------------------------

            // (선택 사항) 캐릭터가 이동 방향을 바라보게 회전시키기
            // if (moveDirection.sqrMagnitude > 0.01f) // 아주 작은 움직임은 무시 (제자리 회전 방지)
            // {
            //     Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            //     rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
            // }
        }
    }
}
