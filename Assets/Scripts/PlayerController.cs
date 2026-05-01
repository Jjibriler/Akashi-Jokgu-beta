using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isPlayer2 = false;
    public float speed = 8f;
    public float jumpForce = 12f; // 점프 힘
    public float gravity = 30f;   // 플레이어에게 적용될 중력
    public float groundY = -3.2f; // 바닥 높이 (실제 바닥 위치에 맞춰 조절하세요)

    private float velocityY = 0f; // 수직 속도 변수
    private bool isJumping = false;

    public float screenLimit = 8.5f;
    public float netLimit = 0.7f;

    // PlayerController 클래스 내부에 추가
public void ResetPlayer()
{
    velocityY = 0f;
    isJumping = false;
}

    void Update()
    {
        float moveX = 0f;

        // 1. 플레이어별 조작키 분리
        if (!isPlayer2) // Player 1: A, D로 이동 / W로 점프
        {
            if (Input.GetKey(KeyCode.A)) moveX = -1f;
            if (Input.GetKey(KeyCode.D)) moveX = 1f;
            
            if (Input.GetKeyDown(KeyCode.W) && !isJumping)
            {
                velocityY = jumpForce;
                isJumping = true;
            }
        }
        else // Player 2: 방향키로 이동 / 위쪽 방향키로 점프
        {
            if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

            if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
            {
                velocityY = jumpForce;
                isJumping = true;
            }
        }

        // 2. 수평 이동 적용
        transform.Translate(Vector2.right * moveX * speed * Time.deltaTime);

        // 3. 점프 및 중력 로직
        if (isJumping)
        {
            velocityY -= gravity * Time.deltaTime; // 아래로 당기는 중력 적용
            transform.Translate(Vector3.up * velocityY * Time.deltaTime);

            // 바닥에 착지했는지 확인
            if (transform.position.y <= groundY)
            {
                Vector3 pos = transform.position;
                pos.y = groundY;
                transform.position = pos;
                
                velocityY = 0f;
                isJumping = false;
            }
        }

        // 4. 영역 제한 (네트 및 화면 끝)[cite: 1]
        Vector3 currentPos = transform.position;
        if (!isPlayer2)
        {
            currentPos.x = Mathf.Clamp(currentPos.x, -screenLimit, -netLimit);
        }
        else
        {
            currentPos.x = Mathf.Clamp(currentPos.x, netLimit, screenLimit);
        }
        transform.position = currentPos;
    }
}