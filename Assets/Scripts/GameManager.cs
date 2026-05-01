using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    public GameObject player1, player2, ball;

    private Vector3 p1StartPos, p2StartPos, ballStartPos;
    private int p1Score = 0, p2Score = 0;

    void Awake() { instance = this; }

    void Start()
    {
        p1StartPos = player1.transform.position;
        p2StartPos = player2.transform.position;
        ballStartPos = ball.transform.position;
        UpdateScoreUI();
    }

    // 표준 득점 함수[cite: 5, 11]
    public void AddScore(int playerNumber)
    {
        if (playerNumber == 1) p1Score++;
        else p2Score++;

        UpdateScoreUI();
        ResetRound(); 
    }

    void UpdateScoreUI()
    {
        scoreText.text = p1Score + " : " + p2Score;
    }

    public void ResetRound()
    {
        // 모든 오브젝트 원위치 및 물리 초기화[cite: 11, 13]
        player1.transform.position = p1StartPos;
        player2.transform.position = p2StartPos;
        
        player1.GetComponent<PlayerController>().ResetPlayer();
        player2.GetComponent<PlayerController>().ResetPlayer();

        ball.GetComponent<BallPhysics>().ResetBallPhysics(ballStartPos);
    }
}