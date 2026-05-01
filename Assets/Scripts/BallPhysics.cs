using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public float gravity = 15f;
    public float horizontalLimit = 8.5f;
    public float topLimit = 4.5f;

    private bool isRoundActive = false;

    void Start()
    {
        // 게임 시작 시 중앙 상단에서 수직 낙하
        ResetBallPhysics(new Vector3(0, 4, 0)); 
    }

    void Update()
    {
        if (!isRoundActive) return;

        // 물리 연산[cite: 3, 9]
        velocity.y -= gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime);

        Vector3 pos = transform.position;

        // 화면 경계 제한 및 튕김[cite: 3, 9]
        if (pos.x <= -horizontalLimit || pos.x >= horizontalLimit)
        {
            velocity.x *= -1f;
            pos.x = Mathf.Clamp(pos.x, -horizontalLimit, horizontalLimit);
        }
        if (pos.y >= topLimit)
        {
            velocity.y = -Mathf.Abs(velocity.y);
            pos.y = topLimit;
        }
        transform.position = pos;
    }

    public void ResetBallPhysics(Vector3 startPos)
    {
        transform.position = startPos; 
        velocity = Vector2.zero;
        isRoundActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isRoundActive) return;

        // 1. Ground 오브젝트(태그: Player, 이름: Ground)에 닿았을 때
        if (collision.gameObject.name == "Ground")
        {
            isRoundActive = false;
            // X축 위치에 따라 득점자 결정[cite: 3, 9]
            int winner = (transform.position.x < 0) ? 2 : 1;
            GameManager.instance.AddScore(winner);
        }
        // 2. 플레이어나 네트에 닿았을 때 (일반 리바운드)[cite: 3, 9]
        else if (collision.CompareTag("Player") || collision.CompareTag("Net"))
        {
            float hitPoint = transform.position.x - collision.transform.position.x;
            velocity.x = hitPoint * 15f;
            velocity.y = 12f;
        }
    }
}